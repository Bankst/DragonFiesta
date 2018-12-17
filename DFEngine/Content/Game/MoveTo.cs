namespace DFEngine.Content.Game
{
	public class MoveTo
	{
		public Vector2 From { get; set; }

		public Vector2 To { get; set; }

		public MoveTo(Vector2 from, Vector2 to)
		{
			From = from;
			To = to;
		}
	}
}
