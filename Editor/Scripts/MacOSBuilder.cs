using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using Wondeluxe;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Object used to build Unity applications targeting Mac OS.
	/// </summary>

	[CreateAssetMenu(menuName = "Wondeluxe/Build/Mac OS Builder", fileName = "MacOSBuilder")]
	public sealed class MacOSBuilder : PlayerBuilder
	{
		[SerializeField]
		[Group("AppInfo")]
		[Order(-1)]
		[Tooltip("Unique identifier for the application bundle.")]
		private string bundleIdentifier;

		public override NamedBuildTarget NamedBuildTarget
		{
			get => NamedBuildTarget.Standalone;
		}

		public override BuildTargetGroup BuildTargetGroup
		{
			get => BuildTargetGroup.Standalone;
		}

		public override BuildTarget BuildTarget
		{
			get => BuildTarget.StandaloneOSX;
		}

		public string BundleIdentifier
		{
			get => bundleIdentifier;
		}

		protected internal override void ApplyPlayerSettings()
		{
			base.ApplyPlayerSettings();
			PlayerSettings.SetApplicationIdentifier(NamedBuildTarget, BundleIdentifier);
			PlayerSettings.macOS.buildNumber = Build.ToString();
		}

		private void Reset()
		{
			platformName = "Mac OS";
			fileExtension = "app";
		}
	}
}