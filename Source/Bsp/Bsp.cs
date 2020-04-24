using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL1BspReader
{
	public class Bsp
	{
		#region Constructors

		public Bsp(BspRawData rawData)
		{
			this.Raw = rawData;
			this.RootNode = this.Raw.AllNodes.Single((node) => node.Debug_PointedToByNodeCount == 0);
		}

		#endregion Constructors

		#region Properties

		public BspRawData Raw { get; }

		#endregion Properties

		#region Methods

		public IReadOnlyList<BspModel> Models => this.Raw.AllModels;

		public BspNode RootNode { get; }

		#endregion Methods
	}

	public class BspRawData
	{
		#region Constructors

		public BspRawData(IEnumerable<BspPlane> planes, IEnumerable<BspNode> nodes, IEnumerable<BspLeaf> leafs, IEnumerable<BspClipnode> clipnodes, IEnumerable<BspModel> models)
		{
			this.AllPlanes = planes.ToList().AsReadOnly();
			this.AllNodes = nodes.ToList().AsReadOnly();
			this.AllLeafs = leafs.ToList().AsReadOnly();
			this.AllClipnodes = clipnodes.ToList().AsReadOnly();
			this.AllModels = models.ToList().AsReadOnly();
		}

		#endregion Constructors

		#region Properties

		public IReadOnlyList<BspPlane> AllPlanes { get; }

		public IReadOnlyList<BspNode> AllNodes { get; }

		public IReadOnlyList<BspLeaf> AllLeafs { get; }

		public IReadOnlyList<BspClipnode> AllClipnodes { get; }

		public IReadOnlyList<BspModel> AllModels { get; }

		#endregion Properties
	}

	public static class BspReader
	{
		#region Methods

		public static Bsp ReadFromFile(string filePath)
		{
			const int numberOfLumps = 15;
			Dictionary<LumpType, BspLump> lumps = new Dictionary<LumpType, BspLump>();
			using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
			{
				int version = reader.ReadInt32();
				foreach (int lumpIndex in Enumerable.Range(0, numberOfLumps))
				{
					int filePosition = reader.ReadInt32();
					int dataLength = reader.ReadInt32();
					long currentPosition = reader.BaseStream.Position;
					reader.BaseStream.Seek(filePosition, SeekOrigin.Begin);
					byte[] lumpData = reader.ReadBytes(dataLength);
					reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
					lumps.Add((LumpType)lumpIndex, new BspLump() { LumpType = (LumpType)lumpIndex, Data = lumpData });
				}
			}

			List<BspPlane> planes = new List<BspPlane>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Planes].Data)))
			{
				int planeIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					planes.Add(new BspPlane()
					{
						PlaneIndex = planeIndex++,
						NormalX = reader.ReadSingle(),
						NormalY = reader.ReadSingle(),
						NormalZ = reader.ReadSingle(),
						Distance = reader.ReadSingle(),
						Type = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
			}

			List<BspVertex> vertices = new List<BspVertex>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Vertices].Data)))
			{
				int vertexIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					vertices.Add(new BspVertex()
					{
						VertexIndex = vertexIndex++,

						X = reader.ReadSingle(),
						Y = reader.ReadSingle(),
						Z = reader.ReadSingle(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
			}

			List<BspEdge> edges = new List<BspEdge>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Edges].Data)))
			{
				int edgeIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					short vertexAIndex = reader.ReadInt16();
					short vertexBIndex = reader.ReadInt16();
					edges.Add(new BspEdge()
					{
						EdgeIndex = edgeIndex++,

						VertexAIndex = vertexAIndex,
						VertexA = vertices[vertexAIndex],

						VertexBIndex = vertexBIndex,
						VertexB = vertices[vertexBIndex],
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
			}

			List<BspFace> faces = new List<BspFace>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Faces].Data)))
			{
				int faceIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					int planeIndex = reader.ReadInt16();
					faces.Add(new BspFace()
					{
						FaceIndex = faceIndex++,

						PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						Side = reader.ReadInt16(),

						FirstEdgeIndex = reader.ReadInt32(),
						NumberOfEdges = reader.ReadInt16(),

						TextureIndex = reader.ReadInt16(),
						LightStyles = reader.ReadInt32(),
						LightmapOffset = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspFace face in faces)
				{
					face.Edges = edges.Skip(face.FirstEdgeIndex).Take(face.NumberOfEdges).ToArray();
				}
			}

			List<BspLeaf> leafs = new List<BspLeaf>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Leafs].Data)))
			{
				int leafIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					leafs.Add(new BspLeaf()
					{
						LeafIndex = leafIndex++,

						Contents = (Contents)reader.ReadInt32(),
						Visibility = reader.ReadInt32(),

						BoundsMinX = reader.ReadInt16(),
						BoundsMinY = reader.ReadInt16(),
						BoundsMinZ = reader.ReadInt16(),
						BoundsMaxX = reader.ReadInt16(),
						BoundsMaxY = reader.ReadInt16(),
						BoundsMaxZ = reader.ReadInt16(),

						FirstMarkSufaceIndex = reader.ReadUInt16(),
						NumberOfMarkSufaces = reader.ReadUInt16(),

						AmbientLevel = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
			}

			List<BspNode> nodes = new List<BspNode>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Nodes].Data)))
			{
				int nodeIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					int planeIndex = reader.ReadInt32();
					nodes.Add(new BspNode()
					{
						NodeIndex = nodeIndex++,

						PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						ChildAIndex = reader.ReadInt16(),
						ChildBIndex = reader.ReadInt16(),

						BoundsMinX = reader.ReadInt16(),
						BoundsMinY = reader.ReadInt16(),
						BoundsMinZ = reader.ReadInt16(),
						BoundsMaxX = reader.ReadInt16(),
						BoundsMaxY = reader.ReadInt16(),
						BoundsMaxZ = reader.ReadInt16(),

						FirstFaceIndex = reader.ReadUInt16(),
						NumberOfFaces = reader.ReadUInt16(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspNode node in nodes)
				{
					if (node.ChildAIndex > 0)
					{
						node.ChildANode = nodes[node.ChildAIndex];
					}
					else
					{
						node.ChildALeaf = leafs[-1 - node.ChildAIndex];
					}
					if (node.ChildBIndex > 0)
					{
						node.ChildBNode = nodes[node.ChildBIndex];
					}
					else
					{
						node.ChildBLeaf = leafs[-1 - node.ChildBIndex];
					}
					node.Faces = faces.Skip(node.FirstFaceIndex).Take(node.NumberOfFaces).ToArray();
				}
			}

			List<BspClipnode> clipnodes = new List<BspClipnode>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Clipnodes].Data)))
			{
				int clipnodeIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					int planeIndex = reader.ReadInt32();
					clipnodes.Add(new BspClipnode()
					{
						ClipnodeIndex = clipnodeIndex++,

						PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						ChildAIndex = reader.ReadInt16(),
						ChildBIndex = reader.ReadInt16(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspClipnode clipnode in clipnodes)
				{
					if (clipnode.ChildAIndex > 0)
					{
						clipnode.ChildAClipnode = clipnodes[clipnode.ChildAIndex];
					}
					else
					{
						clipnode.ChildAContents = (Contents)clipnode.ChildAIndex;
					}
					if (clipnode.ChildBIndex > 0)
					{
						clipnode.ChildBClipnode = clipnodes[clipnode.ChildBIndex];
					}
					else
					{
						clipnode.ChildBContents = (Contents)clipnode.ChildBIndex;
					}
				}
			}

			List<BspModel> models = new List<BspModel>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Model].Data)))
			{
				int modelIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					models.Add(new BspModel()
					{
						ModelIndex = modelIndex++,

						BoundsMinX = reader.ReadSingle(),
						BoundsMinY = reader.ReadSingle(),
						BoundsMinZ = reader.ReadSingle(),
						BoundsMaxX = reader.ReadSingle(),
						BoundsMaxY = reader.ReadSingle(),
						BoundsMaxZ = reader.ReadSingle(),

						OriginX = reader.ReadSingle(),
						OriginY = reader.ReadSingle(),
						OriginZ = reader.ReadSingle(),

						Clipnode0Index = reader.ReadInt32(),
						Clipnode1Index = reader.ReadInt32(),
						Clipnode2Index = reader.ReadInt32(),
						Clipnode3Index = reader.ReadInt32(),

						VisLeafs = reader.ReadInt32(),

						FirstFaceIndex = reader.ReadInt32(),
						NumberOfFaces = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspModel model in models)
				{
					model.Clipnode0 = clipnodes[model.Clipnode0Index];
					model.Clipnode1 = clipnodes[model.Clipnode1Index];
					model.Clipnode2 = clipnodes[model.Clipnode2Index];
					model.Clipnode3 = clipnodes[model.Clipnode3Index];
					model.Faces = faces.Skip(model.FirstFaceIndex).Take(model.NumberOfFaces).ToArray();
				}
			}

			// Debug calculations
			{
				// How many times a node is pointed to by some other node
				foreach (BspNode node in nodes)
				{
					foreach (BspNode otherNode in nodes)
					{
						if (node == otherNode) { continue; }
						if (otherNode.ChildANode == node) { node.Debug_PointedToByNodeCount++; }
						if (otherNode.ChildBNode == node) { node.Debug_PointedToByNodeCount++; }
					}
				}
				// How many times a leaf is pointed to by some node
				foreach (BspLeaf leaf in leafs)
				{
					foreach (BspNode node in nodes)
					{
						if (node.ChildALeaf == leaf) { leaf.Debug_PointedToByNodeCount++; }
						if (node.ChildBLeaf == leaf) { leaf.Debug_PointedToByNodeCount++; }
					}
				}
				// How many times a clipnode is pointed to by some other clipnode
				foreach (BspClipnode clipnode in clipnodes)
				{
					foreach (BspClipnode otherClipnode in clipnodes)
					{
						if (clipnode == otherClipnode) { continue; }
						if (otherClipnode.ChildAClipnode == clipnode) { clipnode.Debug_PointedToByClipnodeCount++; }
						if (otherClipnode.ChildBClipnode == clipnode) { clipnode.Debug_PointedToByClipnodeCount++; }
					}
				}
			}

			return new Bsp(new BspRawData(planes, nodes, leafs, clipnodes, models));
		}

		#endregion Methods
	}
}
