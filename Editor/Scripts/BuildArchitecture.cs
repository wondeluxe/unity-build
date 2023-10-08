namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Architecture associated with a build target when building a player. Values map to the integer values expected by
	/// <see href="https://docs.unity3d.com/ScriptReference/PlayerSettings.SetArchitecture.html">PlayerSettings.SetArchitecture</see>.
	/// </summary>

	public enum BuildArchitecture
	{
		None = 0,
		ARM64 = 1,
		Universal = 2
	}
}