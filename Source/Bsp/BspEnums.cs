using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL1BspReader
{
	public enum LumpType
	{
		Entities = 0,
		Planes = 1,
		Textures = 2,
		Vertices = 3,
		Visibility = 4,
		Nodes = 5,
		TextureInfo = 6,
		Faces = 7,
		Lighting = 8,
		Clipnodes = 9,
		Leafs = 10,
		MarkSurfaces = 11,
		Edges = 12,
		SurfaceEdges = 13,
		Model = 14,
	}

	public enum Contents
	{
		Empty = -1,
		Solid = -2,
		Water = -3,
		Slime = -4,
		Lava = -5,
		Sky = -6,
		Origin = -7,
		Clip = -8,
		Current0Degrees = -9,
		Current90Degrees = -10,
		Current180Degrees = -11,
		Current270Degrees = -12,
		CurrentUp = -13,
		CurrentDown = -14,
		Transparent = -15,
	}
}
