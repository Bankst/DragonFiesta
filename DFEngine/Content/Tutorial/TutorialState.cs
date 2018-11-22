namespace DFEngine.Content.Tutorial
{
	/// <summary>
	/// Represents the state of a character's tutorial instance.
	/// </summary>
	public enum TutorialState
	{
		TS_PROGRESS = 0,
		TS_DONE = 1,
		TS_SKIP = 2,
		TS_EXCEPTION = 3,
		TS_MAX = 4
	}
}
