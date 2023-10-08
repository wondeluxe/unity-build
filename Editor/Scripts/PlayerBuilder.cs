using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Base builder object for performing configured builds. Should be extended for each supported platform.
	/// </summary>

	public abstract class PlayerBuilder : ScriptableObject
	{
		#region Internal fields

		[PlatformInfo]
		[SerializeField]
		[Tooltip("Platform name of the player. Used for constructing the direcotry name where the built player will be output.")]
		protected string platformName;

		[BuildOutput]
		[SerializeField]
		[Tooltip("Destination path relative to Build Path (set in UserSettings > Wondeluxe > Build) where the built player will be output.")]
		protected string filePath;

		[BuildOutput]
		[SerializeField]
		[Tooltip("File name to use for the output application file.")]
		protected string fileName;

		[BuildOutput]
		[SerializeField]
		[Tooltip("File extension to apply to the output application file. May be left blank.")]
		protected string fileExtension;

		[AppInfo]
		[SerializeField]
		[Tooltip("Application version. Typically a <a href=\"https://semver.org/\">Semantic Version</a>.")]
		protected string version;

		[AppInfo]
		[SerializeField]
		[Tooltip("Application build number. Should be incremented on each build.")]
		protected uint build;

		[BuildConfig]
		[SerializeField]
		[Tooltip("Build scripting framework.")]
		protected ScriptingImplementation scriptingImplementation = ScriptingImplementation.IL2CPP;// Warn if IL2CPP not supported.

		[BuildConfig]
		[SerializeField]
		[Tooltip("Compiler configuration used when compiling generated C++ code.")]
		protected Il2CppCompilerConfiguration il2CppCompilerConfiguration = Il2CppCompilerConfiguration.Release;// Only show if scriptingImplementation is IL2CPP.

		[BuildConfig(1)]
		[SerializeField]
		[Tooltip("Enable the <a href=\"https://docs.unity3d.com/ScriptReference/BuildOptions.StrictMode.html\">StrictMode</a> build option.")]
		protected bool strictMode;

		[BuildConfig(1)]
		[SerializeField]
		[Tooltip("Automatically run the built player.")]
		protected bool autoRun;

		[BuildContent]
		[SerializeField]
		[Tooltip("A C# source file where the app version and build numbers will be written to. File must contain fields or constants named Version and Build.")]
		protected TextAsset appScript;

		[BuildContent]
		[SerializeField]
		[Tooltip("Scenes to be included in the build.")]
		protected SceneAsset[] scenes;

		#endregion

		#region Public API

		/// <summary>
		/// Target build platform name. Used to configure <see cref="PlayerSettings"/>.
		/// </summary>

		public abstract NamedBuildTarget NamedBuildTarget { get; }

		/// <summary>
		/// Target build group. Used to configure <see cref="BuildPlayerOptions"/>.
		/// </summary>

		public abstract BuildTargetGroup BuildTargetGroup { get; }

		/// <summary>
		/// Target build platform. Used to configure <see cref="BuildPlayerOptions"/>.
		/// </summary>

		public abstract BuildTarget BuildTarget { get; }

		/// <summary>
		/// Platform name of the player. Used for constructing the direcotry name where the built player will be output.
		/// </summary>

		public string PlatformName
		{
			get => platformName;
		}

		/// <summary>
		/// Destination path relative to the project folder where the player will be built.
		/// </summary>

		public string FilePath
		{
			get => Path.Combine(BuildSettings.BuildPath, filePath);
		}

		/// <summary>
		/// File name to use for the output application file.
		/// </summary>

		public string FileName
		{
			get => fileName;
		}

		/// <summary>
		/// File extension to apply to the output application file. May be left blank.
		/// </summary>

		public string FileExtension
		{
			get => fileExtension;
		}

		/// <summary>
		/// Application version. Typically a <see href="https://semver.org/">Semantic Version</see>.
		/// </summary>

		public string Version
		{
			get => version;
			set => version = value;
		}

		/// <summary>
		/// Application build number. Should be incremented on each build.
		/// </summary>

		public uint Build
		{
			get => build;
			set => build = value;
		}

		/// <summary>
		/// Destination file path name defining where the application will be built.
		/// </summary>

		public string PlayerPathName
		{
			get => PlayerBuilderUtility.ConstructPlayerPathName(FilePath, FileName, FileExtension, PlatformName, Build);
		}

		/// <summary>
		/// Backend scripting implementation. Used to configure <see cref="PlayerSettings"/>. Note that <see cref="ScriptingImplementation.IL2CPP"/> is only applied if it's supported by the client's environment.
		/// If <see cref="ScriptingImplementation.IL2CPP"/> is not supported, <see cref="ScriptingImplementation.Mono2x"/> will be applied by default.
		/// </summary>

		public ScriptingImplementation ScriptingImplementation
		{
			get => scriptingImplementation;
		}

		/// <summary>
		/// C++ compiler configuration used when compiling generated C++ code (IL2CPP). Used to configure <see cref="PlayerSettings"/>.
		/// </summary>

		public Il2CppCompilerConfiguration Il2CppCompilerConfiguration
		{
			get => il2CppCompilerConfiguration;
		}

		/// <summary>
		/// Should the <see cref="https://docs.unity3d.com/ScriptReference/BuildOptions.StrictMode.html">StrictMode</see> build option be enabled?
		/// </summary>

		public bool StrictMode
		{
			get => strictMode;
		}

		/// <summary>
		/// Automatically run the built player.
		/// </summary>

		public bool AutoRun
		{
			get => autoRun;
		}

		/// <summary>
		/// A C# source file where the app version and build numbers will be written to. File must contain fields (or constants) named <c>Version</c> and <c>Build</c>.
		/// </summary>

		public TextAsset AppScript
		{
			get => appScript;
		}

		/// <summary>
		/// The asset paths to the scenes to be included in the build.
		/// </summary>

		public string[] Scenes
		{
			get
			{
				if (scenes == null)
				{
					return null;
				}

				string[] paths = new string[scenes.Length];

				for (int i = 0; i < scenes.Length; i++)
				{
					paths[i] = AssetDatabase.GetAssetPath(scenes[i]);
				}

				return paths;
			}
		}

		/// <summary>
		/// Returns configured <see cref="BuildPlayerOptions"/> used to build the player with.
		/// </summary>
		/// <param name="clean">Set to <c>true</c> if the <see cref="BuildOptions.CleanBuildCache"/> build option should be applied.</param>
		/// <returns>Configured <see cref="BuildPlayerOptions"/>.</returns>

		public BuildPlayerOptions GetBuildPlayerOptions(bool clean)
		{
			BuildOptions buildOptions = BuildOptions.None;

			SetBuildOptions(ref buildOptions);

			if (clean)
			{
				buildOptions |= BuildOptions.CleanBuildCache;
			}

			return new BuildPlayerOptions
			{
				targetGroup = BuildTargetGroup,
				target = BuildTarget,
				scenes = Scenes,
				options = buildOptions,
				locationPathName = PlayerPathName
			};
		}

		/// <summary>
		/// Builds the player.
		/// </summary>
		/// <param name="clean"></param>

		public void BuildPlayer(bool clean = false)
		{
			ApplyBuildSettings();
			ApplyPlayerSettings();
			UpdateAppScript();

			// Not sure if refresh is needed during build step.
			// Code will be automatically recompiled anyway?

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			// Code continues while refresh is underway.
			// May need to implement an async refresh in AssetDatabaseExtensions?
			// See https://docs.unity3d.com/ScriptReference/EditorApplication.html

			BuildPlayer(GetBuildPlayerOptions(clean), Version, Build);
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Evaluates if <see cref="ScriptingImplementation.IL2CPP"/> can be used in the client's development environment.
		/// </summary>

		private bool Il2CppSupported
		{
#if UNITY_EDITOR_OSX
			get => (BuildTarget == BuildTarget.StandaloneOSX || BuildTarget == BuildTarget.iOS || BuildTarget == BuildTarget.Android);
#elif UNITY_EDITOR_WIN
			get => (BuildTarget == BuildTarget.StandaloneWindows64 || BuildTarget == BuildTarget.StandaloneWindows || BuildTarget == BuildTarget.Android);
#else
			get => false;
#endif
		}

		internal virtual void SetBuildOptions(ref BuildOptions buildOptions)
		{
			if (StrictMode)
			{
				buildOptions |= BuildOptions.StrictMode;
			}

			if (AutoRun)
			{
				buildOptions |= BuildOptions.AutoRunPlayer;
			}
		}

		internal void ApplyBuildSettings()
		{
			if (EditorUserBuildSettings.activeBuildTarget != BuildTarget)
			{
				if (EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup, BuildTarget))
				{
					EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup;
				}
			}
		}

		internal virtual void ApplyPlayerSettings()
		{
			if (ScriptingImplementation != ScriptingImplementation.IL2CPP)
			{
				PlayerSettings.SetScriptingBackend(NamedBuildTarget, ScriptingImplementation);
			}
			else if (Il2CppSupported)
			{
				PlayerSettings.SetScriptingBackend(NamedBuildTarget, ScriptingImplementation.IL2CPP);
				PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget, Il2CppCompilerConfiguration);
			}
			else
			{
				if (ScriptingImplementation == ScriptingImplementation.IL2CPP)
				{
					Debug.LogWarning("Scripting Implementation is set to IL2CPP, but is not supported on the current platform.");
				}

				PlayerSettings.SetScriptingBackend(NamedBuildTarget, ScriptingImplementation.Mono2x);
			}

			PlayerSettings.bundleVersion = Version;
		}

		internal void UpdateAppScript()
		{
			if (AppScript != null)
			{
				string assetPath = AssetDatabase.GetAssetPath(AppScript);

				// Application.dataPath includes Assets folder, GetDirectoryName strips this.

				string projectPath = Path.GetDirectoryName(Application.dataPath);
				string scriptPath = Path.Combine(projectPath, assetPath);

				string content = File.ReadAllText(scriptPath);

				Match versionMatch = Regex.Match(content, "(\\s+Version\\s+=\\s+\")(.*)(\"\\s*;)");
				Match buildMatch = Regex.Match(content, "(\\s+Build\\s+=\\s+)(\\d+)(\\s*;)");

				List<string> errors = new List<string>();

				if (!versionMatch.Success)
				{
					errors.Add("Version field (or constant) not found.");
				}

				if (!buildMatch.Success)
				{
					errors.Add($"Build field (or constant) not found.");
				}

				if (errors.Count > 0)
				{
					throw new Exception($"Failed to update App Script \"{assetPath}\". {string.Join(" ", errors)}");
				}

				// First group (index 0) is full match.

				content = Regex.Replace(content, versionMatch.Value, $"{versionMatch.Groups[1]}{Version}{versionMatch.Groups[3]}");
				content = Regex.Replace(content, buildMatch.Value, $"{buildMatch.Groups[1]}{Build}{buildMatch.Groups[3]}");

				File.WriteAllText(scriptPath, content);
			}
		}

		#endregion

		#region Static fields

		/// <summary>
		/// Build information for the currently running build. Used by post-build processes.
		/// </summary>

		private static PlayerBuildInfo currentBuildInfo;

		#endregion

		#region Static API

		/// <summary>
		/// Build a Unity application.
		/// </summary>
		/// <param name="buildPlayerOptions">The <see cref="BuildPlayerOptions"/> used to build the application.</param>
		/// <param name="version">Build version.</param>
		/// <param name="build">Build number.</param>

		public static void BuildPlayer(BuildPlayerOptions buildPlayerOptions, string version, uint build)
		{
			currentBuildInfo = new PlayerBuildInfo(version, build);

			BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

			currentBuildInfo = null;

			if (buildReport.summary.result == BuildResult.Succeeded)
			{
				Debug.Log($"Build succeeded: \"{buildReport.summary.outputPath}\". Duration: {PlayerBuilderUtility.GetBuildTime(buildReport)}.");
			}
			else
			{
				throw new Exception($"Build failed with {buildReport.summary.totalErrors} errors. Duration: {PlayerBuilderUtility.GetBuildTime(buildReport)}.\n{PlayerBuilderUtility.GetBuildErrors(buildReport)}");
			}
		}

		#endregion

		#region Static methods

		[PostProcessBuild(0)]
		private static void OnPostProcessBuild(BuildTarget target, string playerPath)
		{
			// TODO Could add InfoFile property to PlayerBuilder?
			// Could be templatable thing where the client can determine the format?

			if (currentBuildInfo != null)
			{
				string infoPath = Path.Combine(Path.GetDirectoryName(playerPath), "VersionInfo.txt");
				string infoContent = $"{currentBuildInfo.Version}\n{currentBuildInfo.Build}";

				File.WriteAllText(infoPath, infoContent);
			}
		}

		#endregion
	}
}