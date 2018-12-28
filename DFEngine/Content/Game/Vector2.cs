using System;
using DFEngine.Network;
using DFEngine.Utils;

namespace DFEngine.Content.Game
{
	/// <summary>
	/// Representation of 2D vectors and points.
	/// </summary>
	public class Vector2
	{
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="Vector2"/> class.
		/// </summary>
		public Vector2()
		{
			X = 0.0D;
			Y = 0.0D;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Vector2"/> class.
		/// </summary>
		/// <param name="x">The X component of the vector.</param>
		/// <param name="y">The Y component of the vector.</param>
		public Vector2(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Vector2"/> class.
		/// </summary>
		/// <param name="vector">A vector to copy components from.</param>
		public Vector2(Vector2 vector)
		{
			X = vector.X;
			Y = vector.Y;
		}

		public double DistanceTo(Vector2 target)
		{
			return DistanceTo(target.X, target.Y);
		}

		public double DistanceTo(double x, double y)
		{
			return Math.Sqrt(Math.Pow(x - X, 2.0) + Math.Pow(y - Y, 2.0));
		}

		public static double Distance(Vector2 v1, Vector2 v2)
		{
			return Math.Sqrt(Math.Pow(v2.X - v1.X, 2.0) + Math.Pow(v2.Y - v1.Y, 2.0));
		}

		/// <summary>
		/// Determines the angle between two vectors.
		/// </summary>
		/// <param name="target">The target vector.</param>
		/// <returns>The angle between the vectors.</returns>
		public double AngleTo(Vector2 target)
		{
			return AngleTo(target.X, target.Y);
		}

		/// <summary>
		/// Determines the angle between the vector and the coordinates.
		/// </summary>
		/// <param name="targetX">The x-coordinate.</param>
		/// <param name="targetY">The y-coordinate</param>
		/// <returns>The agnel between the vectors.</returns>
		public double AngleTo(double targetX, double targetY)
		{
			return Angle(this, new Vector2(targetX, targetY), true);
		}

		public static double Angle(Vector2 v1, Vector2 v2, bool clamp = true)
		{
			var num = Math.Atan2(v2.Y - v1.Y, v2.X - v1.X) * Mathd.Rad2Deg;
			if (clamp && num < 0.0)
				num += 360.0;
			return num;
		}

		
		public static Vector2 MoveTowards(Vector2 current, Vector2 target, double maxDistanceDelta)
		{
			var vector2 = target - current;
			var num = vector2.Magnitude();
			return num <= maxDistanceDelta || Math.Abs(num) < 0.0 ? target : current + vector2 / num * maxDistanceDelta;
		}

		public static double Fangle(Vector2 v1, Vector2 v2)
		{
			var num = Math.Atan2(-v2.Y + v1.Y, v2.X - v1.X) * Mathd.Rad2Deg + 90.0;
			if (num < 0.0)
				num += 360.0;
			return num;
		}

		public static Vector2 Point(Vector2 center, double distanceFromCenter, double angle)
		{
			var num = angle * Mathd.Deg2Rad;
			return new Vector2((int)(center.X + distanceFromCenter * Math.Cos(num)), (int)(center.Y + distanceFromCenter * Math.Sin(num)));
		}

		public double Magnitude()
		{
			return Math.Sqrt(X * X + Y * Y);
		}

		public static Vector2 Normalize(Vector2 v)
		{
			v.X /= Convert.ToInt32(v.Magnitude());
			v.Y /= Convert.ToInt32(v.Magnitude());
			return v;
		}

		public Vector2 Normalize()
		{
			var num = Magnitude();
			var vector2 = new Vector2(this);
			return num <= 1E-05 ? new Vector2(0.0, 0.0) : vector2 / num;
		}

		public static double Dot(Vector2 v1, Vector2 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		/// <summary>
		/// Determines whether or not the position is in the radius of the specified position.
		/// </summary>
		/// <param name="centerX">The center x-coordinate of the second position.</param>
		/// <param name="centerY">The center y-coordinate of the second position.</param>
		/// <param name="radius">The radius of the circle getting checked.</param>
		/// <returns>True if this position is in the circle.</returns>
		public bool IsInCircle(double centerX, double centerY, int radius)
		{
			return Distance(this, new Vector2(centerX, centerY)) <= radius;
		}

		public static void Copy(Vector2 v1, Vector2 v2)
		{
			if (v2 == null)
			{
				v2 = new Vector2(v1);
			}
			else
			{
				v2.X = v1.X;
				v2.Y = v1.Y;
			}
		}

		public static Vector2 operator /(Vector2 a, double d)
		{
			return new Vector2(Convert.ToInt32(a.X / d), Convert.ToInt32(a.Y / d));
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		public static Vector2 operator *(Vector2 a, double m)
		{
			return new Vector2(Convert.ToInt32(a.X * m), Convert.ToInt32(a.Y * m));
		}
	}
}
