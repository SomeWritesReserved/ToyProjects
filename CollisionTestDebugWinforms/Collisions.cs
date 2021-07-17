using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace CollisionTest
{
	public static class Helpers
	{
		#region Methods

		public static PointF ToPointF(this Vector2 vector)
		{
			return new PointF(vector.X, vector.Y);
		}

		public static BoundingBox2D Translate(this BoundingBox2D box, Vector2 translation)
		{
			return new BoundingBox2D() { Min = box.Min + translation, Max = box.Max + translation };
		}

		public static RectangleF ToRectangleF(this BoundingBox2D box)
		{
			return new RectangleF(box.Min.X, box.Min.Y, box.Max.X - box.Min.X, box.Max.Y - box.Min.Y);
		}

		public static Vector2 GetCenter(this BoundingBox2D box)
		{
			return box.Min + (box.Max - box.Min) * 0.5f;
		}

		public static BoundingBox2D GetRelativeBox(this BoundingBox2D box)
		{
			return box.Translate(-box.GetCenter());
		}

		public static BoundingBox2D Expand(this BoundingBox2D box, BoundingBox2D other)
		{
			return new BoundingBox2D()
			{
				Min = box.GetCenter() + box.GetRelativeBox().Min + other.GetRelativeBox().Min,
				Max = box.GetCenter() + box.GetRelativeBox().Max + other.GetRelativeBox().Max,
			};
		}

		public static Vector2 GetExtents(this BoundingBox2D box)
		{
			return box.Max - box.Min;
		}

		public static RectangleF ToCenteredCircle(this BoundingBox2D box)
		{
			return new RectangleF(box.GetCenter().X - 2, box.GetCenter().Y - 2, 4, 4);
		}

		public static bool CheckCollideSwept(Vector2 positionA, Vector2 extentsA, Vector2 velocityA,
			Vector2 positionB, Vector2 extentsB, Vector2 velocityB,
			out float firstCollideTime, out Vector2 normal, out Vector2 edgeDir)
		{
			normal = Vector2.Zero;
			edgeDir = Vector2.Zero;

			if (Helpers.CheckOverlap(positionA, extentsA, positionB, extentsB))
			{
				firstCollideTime = 0.0f;
				return true;
			}

			// Treat 'a' as stationary and 'b' as the only one moving
			Vector2 relativeVelocity = velocityB - velocityA;

			firstCollideTime = 0.0f;
			float lastCollideTime = 1.0f;

			if (relativeVelocity.X < 0.0f)
			{
				if ((positionB.X + extentsB.X * 0.5f) < (positionA.X - extentsA.X * 0.5f)) { return false; }
				if ((positionA.X + extentsA.X * 0.5f) < (positionB.X - extentsB.X * 0.5f))
				{
					float thisCollideTime = ((positionA.X + extentsA.X * 0.5f) - (positionB.X - extentsB.X * 0.5f)) / relativeVelocity.X;
					if (thisCollideTime > firstCollideTime)
					{
						normal = Vector2.UnitX;
						edgeDir = Vector2.UnitY;
						firstCollideTime = thisCollideTime;
					}
				}
				if ((positionB.X + extentsB.X * 0.5f) > (positionA.X - extentsA.X * 0.5f)) { lastCollideTime = Math.Min(((positionA.X - extentsA.X * 0.5f) - (positionB.X + extentsB.X * 0.5f)) / relativeVelocity.X, lastCollideTime); }
			}
			if (relativeVelocity.X > 0.0f)
			{
				if ((positionB.X - extentsB.X * 0.5f) > (positionA.X + extentsA.X * 0.5f)) { return false; }
				if ((positionB.X + extentsB.X * 0.5f) < (positionA.X - extentsA.X * 0.5f))
				{
					float thisCollideTime = ((positionA.X - extentsA.X * 0.5f) - (positionB.X + extentsB.X * 0.5f)) / relativeVelocity.X;
					if (thisCollideTime > firstCollideTime)
					{
						normal = -Vector2.UnitX;
						edgeDir = Vector2.UnitY;
						firstCollideTime = thisCollideTime;
					}
				}
				if ((positionA.X + extentsA.X * 0.5f) > (positionB.X - extentsB.X * 0.5f)) { lastCollideTime = Math.Min(((positionA.X + extentsA.X * 0.5f) - (positionB.X - extentsB.X * 0.5f)) / relativeVelocity.X, lastCollideTime); }
			}
			if (firstCollideTime > lastCollideTime) { return false; }

			if (relativeVelocity.Y < 0.0f)
			{
				if ((positionB.Y + extentsB.Y * 0.5f) < (positionA.Y - extentsA.Y * 0.5f)) { return false; }
				if ((positionA.Y + extentsA.Y * 0.5f) < (positionB.Y - extentsB.Y * 0.5f))
				{
					float thisCollideTime = ((positionA.Y + extentsA.Y * 0.5f) - (positionB.Y - extentsB.Y * 0.5f)) / relativeVelocity.Y;
					if (thisCollideTime > firstCollideTime)
					{
						normal = Vector2.UnitY;
						edgeDir = Vector2.UnitX;
						firstCollideTime = thisCollideTime;
					}
				}
				if ((positionB.Y + extentsB.Y * 0.5f) > (positionA.Y - extentsA.Y * 0.5f)) { lastCollideTime = Math.Min(((positionA.Y - extentsA.Y * 0.5f) - (positionB.Y + extentsB.Y * 0.5f)) / relativeVelocity.Y, lastCollideTime); }
			}
			if (relativeVelocity.Y > 0.0f)
			{
				if ((positionB.Y - extentsB.Y * 0.5f) > (positionA.Y + extentsA.Y * 0.5f)) { return false; }
				if ((positionB.Y + extentsB.Y * 0.5f) < (positionA.Y - extentsA.Y * 0.5f))
				{
					float thisCollidTime = ((positionA.Y - extentsA.Y * 0.5f) - (positionB.Y + extentsB.Y * 0.5f)) / relativeVelocity.Y;
					if (thisCollidTime > firstCollideTime)
					{
						normal = -Vector2.UnitY;
						edgeDir = Vector2.UnitX;
						firstCollideTime = thisCollidTime;
					}
				}
				if ((positionA.Y + extentsA.Y * 0.5f) > (positionB.Y - extentsB.Y * 0.5f)) { lastCollideTime = Math.Min(((positionA.Y + extentsA.Y * 0.5f) - (positionB.Y - extentsB.Y * 0.5f)) / relativeVelocity.Y, lastCollideTime); }
			}
			if (firstCollideTime > lastCollideTime) { return false; }

			return true;
		}

		public static bool CheckOverlap(Vector2 positionA, Vector2 extentsA,
			Vector2 positionB, Vector2 extentsB)
		{
			if ((positionA.X + extentsA.X * 0.5f) < (positionB.X - extentsB.X * 0.5f) || (positionA.X - extentsA.X * 0.5f) > (positionB.X + extentsB.X * 0.5f)) { return false; }
			if ((positionA.Y + extentsA.Y * 0.5f) < (positionB.Y - extentsB.Y * 0.5f) || (positionA.Y - extentsA.Y * 0.5f) > (positionB.Y + extentsB.Y * 0.5f)) { return false; }
			return true;
		}

		public static bool CheckCollideRay(BoundingBox2D b, Vector2 origin, Vector2 direction, out float firstCollideTime)
		{
			firstCollideTime = float.NegativeInfinity;
			float lastCollideTime = float.PositiveInfinity;

			Ray ray = new Ray()
			{
				Origin = origin,
				Direction = direction,
			};

			if (ray.Direction.X != 0.0)
			{
				float txMin = (b.Min.X - ray.Origin.X) / ray.Direction.X;
				float txMax = (b.Max.X - ray.Origin.X) / ray.Direction.X;

				firstCollideTime = Math.Max(firstCollideTime, Math.Min(txMin, txMax));
				lastCollideTime = Math.Min(lastCollideTime, Math.Max(txMin, txMax));
			}

			if (ray.Direction.Y != 0.0)
			{
				float tyMin = (b.Min.Y - ray.Origin.Y) / ray.Direction.Y;
				float tyMax = (b.Max.Y - ray.Origin.Y) / ray.Direction.Y;

				firstCollideTime = Math.Max(firstCollideTime, Math.Min(tyMin, tyMax));
				lastCollideTime = Math.Min(lastCollideTime, Math.Max(tyMin, tyMax));
			}

			return lastCollideTime > Math.Max(firstCollideTime, 0.0f) && firstCollideTime <= 1.0f;
		}

		#endregion Methods

		#region Nested Types

		private struct Ray
		{
			public Vector2 Origin;
			public Vector2 Direction;
		}

		#endregion Nested Types
	}

	public static class SAT
	{
		#region Methods


		public static bool DoesPenetrateWithDepth(BoundingBox2D a, BoundingBox2D b, out Vector2 minimumTranslationVector, out float penetrationDepth)
		{
			minimumTranslationVector = Vector2.Zero;
			penetrationDepth = 0.0f;

			// Minimum Translation Vector
			// ==========================
			float mtvDistance = float.MaxValue;             // Set current minimum distance (max float value so next value is always less)
			Vector2 mtvAxis = new Vector2();                // Axis along which to travel with the minimum distance

			// Axes of potential separation
			// ============================
			// - Each shape must be projected on these axes to test for intersection:
			//          
			// (1, 0, 0)                    A0 (= B0) [X Axis]
			// (0, 1, 0)                    A1 (= B1) [Y Axis]
			// (0, 0, 1)                    A1 (= B2) [Z Axis]

			// [X Axis]
			if (!testAxisStatic(Vector2.UnitX, a.Min.X, a.Max.X, b.Min.X, b.Max.X, ref mtvAxis, ref mtvDistance))
				return false;

			// [Y Axis]
			if (!testAxisStatic(Vector2.UnitY, a.Min.Y, a.Max.Y, b.Min.Y, b.Max.Y, ref mtvAxis, ref mtvDistance))
				return false;

			// Calculate Minimum Translation Vector (MTV) [normal * penetration]
			minimumTranslationVector = Vector2.Normalize(mtvAxis);

			// Multiply the penetration depth by itself plus a small increment
			// When the penetration is resolved using the MTV, it will no longer intersect
			penetrationDepth = (float)Math.Sqrt(mtvDistance) * 1.001f;

			return true;
		}

		private static bool testAxisStatic(Vector2 axis, float minA, float maxA, float minB, float maxB, ref Vector2 mtvAxis, ref float mtvDistance)
		{
			// Separating Axis Theorem
			// =======================
			// - Two convex shapes only overlap if they overlap on all axes of separation
			// - In order to create accurate responses we need to find the collision vector (Minimum Translation Vector)   
			// - The collision vector is made from a vector and a scalar, 
			//   - The vector value is the axis associated with the smallest penetration
			//   - The scalar value is the smallest penetration value
			// - Find if the two boxes intersect along a single axis
			// - Compute the intersection interval for that axis
			// - Keep the smallest intersection/penetration value
			float axisLengthSquared = Vector2.Dot(axis, axis);

			// If the axis is degenerate then ignore
			if (axisLengthSquared < 1.0e-8f)
				return true;

			// Calculate the two possible overlap ranges
			// Either we overlap on the left or the right sides
			float d0 = (maxB - minA);   // 'Left' side
			float d1 = (maxA - minB);   // 'Right' side

			// Intervals do not overlap, so no intersection
			if (d0 <= 0.0f || d1 <= 0.0f)
				return false;

			// Find out if we overlap on the 'right' or 'left' of the object.
			float overlap = (d0 < d1) ? d0 : -d1;

			// The mtd vector for that axis
			Vector2 sep = axis * (overlap / axisLengthSquared);

			// The mtd vector length squared
			float sepLengthSquared = Vector2.Dot(sep, sep);

			// If that vector is smaller than our computed Minimum Translation Distance use that vector as our current MTV distance
			if (sepLengthSquared < mtvDistance)
			{
				mtvDistance = sepLengthSquared;
				mtvAxis = sep;
			}

			return true;
		}

		#endregion Methods
	}
}
