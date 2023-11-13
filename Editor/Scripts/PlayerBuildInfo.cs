namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Object containing information about a build.
	/// </summary>

	internal class PlayerBuildInfo
	{
		/// <summary>
		/// Build version.
		/// </summary>

		internal string Version { get; set; }

		/// <summary>
		/// Build number.
		/// </summary>

		internal uint Build { get; set; }

		/// <summary>
		/// Callback to run after the build completes.
		/// </summary>

		internal PostBuildProcess PostProcess { get; set; }

		/// <summary>
		/// Initializes a new instance of <see cref="PlayerBuildInfo"/>.
		/// </summary>
		/// <param name="version">Build version.</param>
		/// <param name="build">Build number.</param>
		/// <param name="postProcess">Callback to run after the build completes.</param>

		internal PlayerBuildInfo(string version, uint build, PostBuildProcess postProcess)
		{
			Version = version;
			Build = build;
			PostProcess = postProcess;
		}
	}
}