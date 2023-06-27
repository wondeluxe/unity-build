using System;
using UnityEditor;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Provides the interface to building a Unity application using a <see cref="PlayerBuilder"/> object from the command line.
	/// </summary>

	public static class PlayerBuilderCLI
	{
		/// <summary>
		/// Build a Unity application using a <see cref="PlayerBuilder"/> object specified via command line arguments.
		/// </summary>

		public static void BuildPlayer()
		{
			// Example expected result of GetCommandLineArgs:
			// "/Applications/Unity/Hub/Editor/2020.3.48f1/Unity.app/Contents/MacOS/Unity",
			// "-nographics",
			// "-batchmode",
			// "-quit",
			// "-buildTarget",
			// "OSXUniversal",
			// "-executeMethod",
			// "WondeluxeEditor.Build.PlayerBuilderCLI.BuildPlayer",
			// "-builder",
			// "MacOSBuilder",
			// "-build",
			// "41",
			// "-buildPath",
			// "/Users/Charlie/Projects/Awesome App/Build/Mac OS"

			BuildPlayer(BuildPlayerArgs.Parse(Environment.GetCommandLineArgs()));
		}

		/// <summary>
		/// Build a Unity application using a specified set of arguments.
		/// </summary>
		/// <param name="args">The arguments to build the Unity application with.</param>

		private static void BuildPlayer(BuildPlayerArgs args)
		{
			//Debug.Log($"args = {args}");

			string builderPath = AssetDatabaseExtensions.FindAssetPath("t:ScriptableObject", args.Builder);

			PlayerBuilder builder = AssetDatabase.LoadAssetAtPath<PlayerBuilder>(builderPath);

			if (builder == null)
			{
				throw new Exception($"Builder \"{args.Builder}\" not found.");
			}

			builder.Build = args.Build;
			builder.ApplyBuildSettings();
			builder.ApplyPlayerSettings();
			builder.UpdateAppScript();

			// Not sure if refresh is needed during build step.
			// Code will be automatically recompiled anyway?

			EditorUtility.SetDirty(builder);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			BuildPlayerOptions buildPlayerOptions = builder.GetBuildPlayerOptions(args.Clean);
			buildPlayerOptions.locationPathName = PlayerBuilderUtility.ConstructPlayerPathName(args.BuildPath, builder.FileName, builder.FileExtension, builder.PlatformName, builder.Build);

			PlayerBuilder.BuildPlayer(buildPlayerOptions, builder.Version, builder.Build);
		}
	}
}