namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Encapsulates a method that contains actions to run after a Player is built.
	/// </summary>
	/// <param name="playerPath">The output path for the build.</param>

	public delegate void PostBuildProcess(string playerPath);
}