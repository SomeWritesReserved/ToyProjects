using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL1BspReader
{
	public class BspLump
	{
		public LumpType LumpType;
		public byte[] Data;
	}

	public class BspPlane
	{
		public int PlaneIndex;

		public float NormalX;
		public float NormalY;
		public float NormalZ;
		public float Distance;
		internal int Type;   // Maybe represents which axis this is most aligned with?

		public override string ToString()
		{
			return $"Plane {PlaneIndex} = ({NormalX}, {NormalY}, {NormalZ}) x {Distance}";
		}
	}

	public class BspVertex
	{
		public int VertexIndex;

		public float X;
		public float Y;
		public float Z;

		public override string ToString()
		{
			return $"Vertex {VertexIndex} ({X}, {Y}, {Z})";
		}
	}

	public class BspEdge
	{
		public int EdgeIndex;

		internal short VertexAIndex;
		public BspVertex VertexA;

		internal short VertexBIndex;
		public BspVertex VertexB;

		public override string ToString()
		{
			return $"Edge {EdgeIndex}";
		}
	}

	public class BspFace
	{
		public int FaceIndex;

		internal int PlaneIndex;
		public BspPlane Plane;

		internal short Side;
		public bool IsFlippedFromPlane => this.Side != 0;

		internal int FirstEdgeIndex;
		internal short NumberOfEdges;
		public BspEdge[] Edges;

		internal short TextureIndex;

		public int LightStyles;
		public int LightmapOffset;

		public override string ToString()
		{
			return $"Face {this.FaceIndex}";
		}
	}

	public class BspLeaf
	{
		public int LeafIndex;

		public Contents Contents;
		public int Visibility;

		public short BoundsMinX;
		public short BoundsMinY;
		public short BoundsMinZ;
		public short BoundsMaxX;
		public short BoundsMaxY;
		public short BoundsMaxZ;

		internal ushort FirstMarkSufaceIndex;
		internal ushort NumberOfMarkSufaces;

		public int AmbientLevel;

		public int Debug_PointedToByNodeCount;

		public override string ToString()
		{
			return $"Leaf {LeafIndex} ({Contents})";
		}
	}

	public class BspNode
	{
		public int NodeIndex;

		internal int PlaneIndex;
		public BspPlane Plane;

		internal short ChildAIndex;
		public BspNode ChildANode;
		public BspLeaf ChildALeaf;
		internal short ChildBIndex;
		public BspNode ChildBNode;
		public BspLeaf ChildBLeaf;

		public short BoundsMinX;
		public short BoundsMinY;
		public short BoundsMinZ;
		public short BoundsMaxX;
		public short BoundsMaxY;
		public short BoundsMaxZ;

		internal ushort FirstFaceIndex;
		internal ushort NumberOfFaces;
		public BspFace[] Faces;

		public int Debug_PointedToByNodeCount;

		public override string ToString()
		{
			string childA = this.ChildANode != null ? $"Node {this.ChildANode.NodeIndex}" : this.ChildALeaf.ToString();
			string childb = this.ChildBNode != null ? $"Node {this.ChildBNode.NodeIndex}" : this.ChildBLeaf.ToString();
			//return $"Node {NodeIndex} = {childA} : {childb}";
			return $"Node {NodeIndex}";
		}
	}

	public class BspClipnode
	{
		public int ClipnodeIndex;

		internal int PlaneIndex;
		public BspPlane Plane;

		internal short ChildAIndex;
		public BspClipnode ChildAClipnode;
		public Contents ChildAContents;
		internal short ChildBIndex;
		public BspClipnode ChildBClipnode;
		public Contents ChildBContents;

		public int Debug_PointedToByClipnodeCount;

		public override string ToString()
		{
			string childA = this.ChildAClipnode != null ? $"Clipnode {this.ChildAClipnode.ClipnodeIndex}" : this.ChildAContents.ToString();
			string childb = this.ChildBClipnode != null ? $"Clipnode {this.ChildBClipnode.ClipnodeIndex}" : this.ChildBContents.ToString();
			//return $"Clipnode {ClipnodeIndex} = {childA} : {childb}";
			return $"Clipnode {ClipnodeIndex}";
		}
	}

	public class BspModel
	{
		public int ModelIndex;

		public float BoundsMinX;
		public float BoundsMinY;
		public float BoundsMinZ;
		public float BoundsMaxX;
		public float BoundsMaxY;
		public float BoundsMaxZ;

		public float OriginX;
		public float OriginY;
		public float OriginZ;

		internal int Clipnode0Index;
		public BspClipnode Clipnode0;
		internal int Clipnode1Index;
		public BspClipnode Clipnode1;
		internal int Clipnode2Index;
		public BspClipnode Clipnode2;
		internal int Clipnode3Index;
		public BspClipnode Clipnode3;

		public int VisLeafs;

		internal int FirstFaceIndex;
		internal int NumberOfFaces;
		public BspFace[] Faces;

		public override string ToString()
		{
			return $"Model {ModelIndex}";
		}
	}
}
