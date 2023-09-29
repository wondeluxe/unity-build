using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor.Build
{
	[FilePath("UserSettings/WondeluxeBuildSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class BuildSettings : ScriptableSingleton<BuildSettings>
	{
		/// <summary>
		/// Base path where builds will be built to.
		/// </summary>

		public static string BuildPath
		{
			get => instance.buildPath;
		}

		/// <summary>
		/// Path where the Android keystore file/s are located for this project.
		/// </summary>

		public static string AndroidKeystoreFilePath
		{
			get => instance.androidKeystoreFilePath;
		}

		/// <summary>
		/// Path where the Android key password file/s are located for this project.
		/// </summary>

		public static string AndroidKeyPasswordFilePath
		{
			get => instance.androidKeyPasswordFilePath;
		}

		// Fields are internal so they may be accessed from BuildSettingsProvider.

		[SerializeField]
		[Tooltip("Base path where builds will be built to. Individual platforms will be built to paths relative to this folder as defined on their PlayerBuilder objects.")]
		internal string buildPath;

		[SerializeField]
		[Tooltip("Path where the Android keystore file/s are located for this project.")]
		internal string androidKeystoreFilePath;

		[SerializeField]
		[Tooltip("Path where the Android key password file/s are located for this project.")]
		internal string androidKeyPasswordFilePath;

		internal void SaveAsset()
		{
			Save(true);
		}
	}
}