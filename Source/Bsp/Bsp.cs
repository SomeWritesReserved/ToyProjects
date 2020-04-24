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
						_Type = reader.ReadInt32(),
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

						_VertexAIndex = vertexAIndex,
						VertexA = vertices[vertexAIndex],

						_VertexBIndex = vertexBIndex,
						VertexB = vertices[vertexBIndex],
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
			}

			List<BspSurfaceEdge> surfaceEdges = new List<BspSurfaceEdge>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.SurfaceEdges].Data)))
			{
				int surfaceEdgeIndex = 0;
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					int edgeIndex = reader.ReadInt32();
					surfaceEdges.Add(new BspSurfaceEdge()
					{
						SurfaceEdgeIndex = surfaceEdgeIndex++,
						_EdgeIndex = edgeIndex,
						Edge = edges[Math.Abs(edgeIndex)],
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

						_PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						_Side = reader.ReadInt16(),

						_FirstSurfaceEdgeIndex = reader.ReadInt32(),
						_NumberOfSurfaceEdges = reader.ReadInt16(),

						_TextureIndex = reader.ReadInt16(),
						LightStyles = reader.ReadInt32(),
						LightmapOffset = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspFace face in faces)
				{
					face.Edges = surfaceEdges.Skip(face._FirstSurfaceEdgeIndex).Take(face._NumberOfSurfaceEdges)
						.Select((surfaceEdge) => surfaceEdge.Edge)
						.ToArray();
				}
			}

			List<ushort> marksurfaces = new List<ushort>();
			using (BinaryReader reader = new BinaryReader(new MemoryStream(lumps[LumpType.Marksurfaces].Data)))
			{
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{
					marksurfaces.Add(reader.ReadUInt16());
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
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

						_FirstMarksurfaceIndex = reader.ReadUInt16(),
						_NumberOfMarksurfaces = reader.ReadUInt16(),

						AmbientLevel = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspLeaf leaf in leafs)
				{
					leaf.Faces = marksurfaces.Skip(leaf._FirstMarksurfaceIndex).Take(leaf._NumberOfMarksurfaces)
						.Select((marksurface) => faces[marksurface])
						.ToArray();
				}
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

						_PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						_ChildAIndex = reader.ReadInt16(),
						_ChildBIndex = reader.ReadInt16(),

						BoundsMinX = reader.ReadInt16(),
						BoundsMinY = reader.ReadInt16(),
						BoundsMinZ = reader.ReadInt16(),
						BoundsMaxX = reader.ReadInt16(),
						BoundsMaxY = reader.ReadInt16(),
						BoundsMaxZ = reader.ReadInt16(),

						_FirstFaceIndex = reader.ReadUInt16(),
						_NumberOfFaces = reader.ReadUInt16(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspNode node in nodes)
				{
					if (node._ChildAIndex > 0)
					{
						node.ChildANode = nodes[node._ChildAIndex];
					}
					else
					{
						node.ChildALeaf = leafs[-1 - node._ChildAIndex];
					}
					if (node._ChildBIndex > 0)
					{
						node.ChildBNode = nodes[node._ChildBIndex];
					}
					else
					{
						node.ChildBLeaf = leafs[-1 - node._ChildBIndex];
					}
					node.Faces = faces.Skip(node._FirstFaceIndex).Take(node._NumberOfFaces).ToArray();
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

						_PlaneIndex = planeIndex,
						Plane = planes[planeIndex],

						_ChildAIndex = reader.ReadInt16(),
						_ChildBIndex = reader.ReadInt16(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspClipnode clipnode in clipnodes)
				{
					if (clipnode._ChildAIndex > 0)
					{
						clipnode.ChildAClipnode = clipnodes[clipnode._ChildAIndex];
					}
					else
					{
						clipnode.ChildAContents = (Contents)clipnode._ChildAIndex;
					}
					if (clipnode._ChildBIndex > 0)
					{
						clipnode.ChildBClipnode = clipnodes[clipnode._ChildBIndex];
					}
					else
					{
						clipnode.ChildBContents = (Contents)clipnode._ChildBIndex;
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

						_Clipnode0Index = reader.ReadInt32(),
						_Clipnode1Index = reader.ReadInt32(),
						_Clipnode2Index = reader.ReadInt32(),
						_Clipnode3Index = reader.ReadInt32(),

						VisLeafs = reader.ReadInt32(),

						_FirstFaceIndex = reader.ReadInt32(),
						_NumberOfFaces = reader.ReadInt32(),
					});
				}
				if (reader.BaseStream.Position != reader.BaseStream.Length) { throw new InvalidDataException("Didn't read all"); }
				foreach (BspModel model in models)
				{
					model.Clipnode0 = clipnodes[model._Clipnode0Index];
					model.Clipnode1 = clipnodes[model._Clipnode1Index];
					model.Clipnode2 = clipnodes[model._Clipnode2Index];
					model.Clipnode3 = clipnodes[model._Clipnode3Index];
					model.Faces = faces.Skip(model._FirstFaceIndex).Take(model._NumberOfFaces).ToArray();
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
