namespace DFEngine.Utils
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
	}
}
