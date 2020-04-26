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
			List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
			foreach (BspFace face in bspModel.Faces)
			{
				Vector3 normal = new Vector3(face.Plane.NormalX, face.Plane.NormalY, face.Plane.NormalZ).ToDirectXVector();
				foreach (BspEdge edge in face.Edges)
				{
					vertices.Add(new VertexPositionNormalTexture(new Vector3(edge.VertexA.X, edge.VertexA.Y, edge.VertexA.Z).ToDirectXVector(), normal, Vector2.Zero));
					vertices.Add(new VertexPositionNormalTexture(new Vector3(edge.VertexB.X, edge.VertexB.Y, edge.VertexB.Z).ToDirectXVector(), normal, Vector2.Zero));
				}
			}
			VertexPositionNormalTexture[] renderVertices = vertices.ToArray();

			float avgX = renderVertices.Select((v) => v.Position.X).Average();
			float avgY = renderVertices.Select((v) => v.Position.Y).Average();
			float avgZ = renderVertices.Select((v) => v.Position.Z).Average();

			effect.World = Matrix.Identity;
			effect.CurrentTechnique.Passes[0].Apply();
			graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, renderVertices, 0, renderVertices.Length / 3);
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
