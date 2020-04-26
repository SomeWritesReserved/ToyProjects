using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HL1BspReader
{
	public class BspRender
	{
		#region Methods

		public static void RenderBspModel<TEffect>(GraphicsDevice graphicsDevice, TEffect effect, BspModel bspModel)
			where TEffect : Effect, IEffectMatrices
		{
			foreach (BspFace face in bspModel.Faces)
			{
				List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();

				Vector3 normal = new Vector3(face.Plane.NormalX, face.Plane.NormalY, face.Plane.NormalZ).ToDirectXVector();
				bool isFirstEdge = true;
				foreach (BspEdge edge in face.Edges)
				{
					var vertA = new VertexPositionNormalTexture(new Vector3(edge.VertexA.X, edge.VertexA.Y, edge.VertexA.Z).ToDirectXVector(), normal, Vector2.Zero);
					var vertB = new VertexPositionNormalTexture(new Vector3(edge.VertexB.X, edge.VertexB.Y, edge.VertexB.Z).ToDirectXVector(), normal, Vector2.Zero);
					if (isFirstEdge)
					{
						// We only use the first vertex on the first edge, all following edges will have their first vertex match the previous edge's last vertex
						vertices.Add(vertA);
					}
					vertices.Add(vertB);
					isFirstEdge = false;
				}

				// Triangle are created with the past two verts and the first vert
				List<VertexPositionNormalTexture> triangleVerts = new List<VertexPositionNormalTexture>();
				for (int i = 1; i < vertices.Count; i++)
				{
					triangleVerts.Add(vertices[0]);
					triangleVerts.Add(vertices[i - 1]);
					triangleVerts.Add(vertices[i]);
				}
				VertexPositionNormalTexture[] renderVertices = triangleVerts.ToArray();

				effect.World = Matrix.Identity;
				effect.CurrentTechnique.Passes[0].Apply();
				graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, renderVertices, 0, renderVertices.Length / 3);
			}
		}

		#endregion Methods
	}

	public static class VectorHelpers
	{
		#region Methods

		/// <summary>
		/// Converts an outside vector into a vector that DirectX expects (Y = up).
		/// </summary>
		public static Vector3 ToDirectXVector(this Vector3 vector)
		{
			return new Vector3(vector.X, vector.Z, -vector.Y);
		}

		#endregion Methods
	}
}
