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
		public int PlaneIndex { get; set; }

		public float NormalX { get; set; }
		public float NormalY { get; set; }
		public float NormalZ { get; set; }
		public float Distance { get; set; }
		public int _Type { get; set; }   // Maybe represents which axis this is most aligned with?

		public override string ToString()
		{
			return $"Plane {PlaneIndex} = ({NormalX}, {NormalY}, {NormalZ}) x {Distance}";
		}
	}

	public class BspVertex
	{
		public int VertexIndex { get; set; }

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public override string ToString()
		{
			return $"Vertex {VertexIndex} ({X}, {Y}, {Z})";
		}
	}

	public class BspEdge
	{
		public int EdgeIndex { get; set; }

		public short _VertexAIndex { get; set; }
		public BspVertex VertexA { get; set; }

		public short _VertexBIndex { get; set; }
		public BspVertex VertexB { get; set; }

		public override string ToString()
		{
			return $"Edge {EdgeIndex}";
		}
	}

	public class BspSurfaceEdge
	{
		public int SurfaceEdgeIndex { get; set; }

		public int _EdgeIndex { get; set; }
		public BspEdge Edge { get; set; }
		public bool IsReversed => this._EdgeIndex < 0;

		public override string ToString()
		{
			return $"SurfaceEdge {SurfaceEdgeIndex}";
		}
	}

	public class BspFace
	{
		public int FaceIndex { get; set; }

		public int _PlaneIndex { get; set; }
		public BspPlane Plane { get; set; }

		public short _Side { get; set; }
		public bool IsFlippedFromPlane => this._Side != 0;

		public int _FirstSurfaceEdgeIndex { get; set; }
		public short _NumberOfSurfaceEdges { get; set; }
		public BspEdge[] Edges { get; set; }

		public short _TextureIndex { get; set; }

		public int LightStyles { get; set; }
		public int LightmapOffset { get; set; }

		public override string ToString()
		{
			return $"Face {this.FaceIndex}";
		}
	}

	public class BspLeaf
	{
		public int LeafIndex { get; set; }

		public Contents Contents { get; set; }
		public int Visibility { get; set; }

		public short BoundsMinX { get; set; }
		public short BoundsMinY { get; set; }
		public short BoundsMinZ { get; set; }
		public short BoundsMaxX { get; set; }
		public short BoundsMaxY { get; set; }
		public short BoundsMaxZ { get; set; }

		public ushort _FirstMarksurfaceIndex { get; set; }
		public ushort _NumberOfMarksurfaces { get; set; }
		public BspFace[] Faces { get; set; }

		public int AmbientLevel { get; set; }

		public int Debug_PointedToByNodeCount { get; set; }

		public override string ToString()
		{
			return $"Leaf {LeafIndex} ({Contents})";
		}
	}

	public class BspNode
	{
		public int NodeIndex { get; set; }

		public int _PlaneIndex { get; set; }
		public BspPlane Plane { get; set; }

		public short _ChildAIndex { get; set; }
		public BspNode ChildANode { get; set; }
		public BspLeaf ChildALeaf { get; set; }
		public short _ChildBIndex { get; set; }
		public BspNode ChildBNode { get; set; }
		public BspLeaf ChildBLeaf { get; set; }

		public short BoundsMinX { get; set; }
		public short BoundsMinY { get; set; }
		public short BoundsMinZ { get; set; }
		public short BoundsMaxX { get; set; }
		public short BoundsMaxY { get; set; }
		public short BoundsMaxZ { get; set; }

		public ushort _FirstFaceIndex { get; set; }
		public ushort _NumberOfFaces { get; set; }
		public BspFace[] Faces { get; set; }

		public int Debug_PointedToByNodeCount { get; set; }

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
		public int ClipnodeIndex { get; set; }

		public int _PlaneIndex { get; set; }
		public BspPlane Plane { get; set; }

		public short _ChildAIndex { get; set; }
		public BspClipnode ChildAClipnode { get; set; }
		public Contents ChildAContents { get; set; }
		public short _ChildBIndex { get; set; }
		public BspClipnode ChildBClipnode { get; set; }
		public Contents ChildBContents { get; set; }

		public int Debug_PointedToByClipnodeCount { get; set; }

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
		public int ModelIndex { get; set; }

		public float BoundsMinX { get; set; }
		public float BoundsMinY { get; set; }
		public float BoundsMinZ { get; set; }
		public float BoundsMaxX { get; set; }
		public float BoundsMaxY { get; set; }
		public float BoundsMaxZ { get; set; }

		public float OriginX { get; set; }
		public float OriginY { get; set; }
		public float OriginZ { get; set; }

		public int _Clipnode0Index { get; set; }
		public BspClipnode Clipnode0 { get; set; }
		public int _Clipnode1Index { get; set; }
		public BspClipnode Clipnode1 { get; set; }
		public int _Clipnode2Index { get; set; }
		public BspClipnode Clipnode2 { get; set; }
		public int _Clipnode3Index { get; set; }
		public BspClipnode Clipnode3 { get; set; }

		public int VisLeafs { get; set; }

		public int _FirstFaceIndex { get; set; }
		public int _NumberOfFaces { get; set; }
		public BspFace[] Faces { get; set; }

		public override string ToString()
		{
			return $"Model {ModelIndex}";
		}
	}
}
