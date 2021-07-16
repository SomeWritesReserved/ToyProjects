using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace HL1BspReader
{
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

		public static Quaternion GetRotationTo(this Vector3 v0, Vector3 dest)
		{
			// Based on Stan Melax's article in Game Programming Gems
			Quaternion q;
			Vector3 v1 = dest;
			v0.Normalize();
			v1.Normalize();

			Vector3 fallbackAxis = Vector3.Zero;

			float d = Vector3.Dot(v0, v1);
			// If dot == 1, vectors are the same
			if (d >= 1.0f)
			{
				return Quaternion.Identity;
			}
			if (d < (1e-6f - 1.0f))
			{
				if (fallbackAxis != Vector3.Zero)
				{
					// rotate 180 degrees about the fallback axis
					q = Quaternion.CreateFromAxisAngle(fallbackAxis, MathHelper.Pi);
				}
				else
				{
					// Generate an axis
					Vector3 axis = Vector3.Cross(Vector3.UnitX, v0);
					if (axis.IsZeroLength()) // pick another if colinear
						axis = Vector3.Cross(Vector3.UnitY, v0);
					axis.Normalize();
					q = Quaternion.CreateFromAxisAngle(axis, MathHelper.Pi);
				}
			}
			else
			{
				float s = (float)Math.Sqrt((1 + d) * 2);
				float invs = 1 / s;

				Vector3 c = Vector3.Cross(v0, v1);

				q.X = c.X * invs;
				q.Y = c.Y * invs;
				q.Z = c.Z * invs;
				q.W = s * 0.5f;
				q.Normalize();
			}
			return q;
		}

		public static bool IsZeroLength(this Vector3 vec)
		{
			float sqlen = (vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z);
			return (sqlen < (1e-06 * 1e-06));
		}

		#endregion Methods
	}
}
