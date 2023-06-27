namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Structure containing the command line options for building a Unity application from a <see cref="PlayerBuilder"/> object.
	/// </summary>

	internal struct BuildPlayerArgs
	{
		/// <summary>
		/// The name of the <see cref="PlayerBuilder"/> object asset to build.
		/// </summary>

		internal string Builder { get; set; }

		/// <summary>
		/// The build number to use in the build.
		/// </summary>

		internal uint Build { get; set; }

		/// <summary>
		/// The destination path to build the application to.
		/// </summary>

		internal string BuildPath { get; set; }

		/// <summary>
		/// Set to <c>true</c> if a clean build is requested.
		/// </summary>

		internal bool Clean { get; set; }

		/// <summary>
		/// Returns a string representation of the <see cref="BuildPlayerArgs"/>.
		/// </summary>
		/// <returns>A string representation of the <see cref="BuildPlayerArgs"/>.</returns>

		public override string ToString()
		{
			return $"[Builder = {Builder}, Build = {Build}, BuildPath = {(BuildPath != null ? $"\"{BuildPath}\"" : "null")}, Clean = {Clean}]";
		}

		/// <summary>
		/// Parses a set of command line arguments to initialize a new <see cref="BuildPlayerArgs"/>.
		/// </summary>
		/// <param name="commandLineArgs">The command line arguments to parse.</param>
		/// <returns>An initialized <see cref="BuildPlayerArgs"/>.</returns>

		internal static BuildPlayerArgs Parse(string[] commandLineArgs)
		{
			BuildPlayerArgs playerArgs = new BuildPlayerArgs();

			// First argument is executing application.

			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-clean")
				{
					playerArgs.Clean = true;
					continue;
				}

				int n = i + 1;

				if (n >= commandLineArgs.Length || commandLineArgs[n][0] == '-')
				{
					continue;
				}

				if (commandLineArgs[i] == "-builder")
				{
					playerArgs.Builder = commandLineArgs[++i];
					continue;
				}

				if (commandLineArgs[i] == "-build")
				{
					playerArgs.Build = uint.Parse(commandLineArgs[++i]);
					continue;
				}

				if (commandLineArgs[i] == "-buildPath")
				{
					playerArgs.BuildPath = commandLineArgs[++i];
					continue;
				}
			}

			return playerArgs;
		}
	}
}