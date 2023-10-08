using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Object used to build Unity applications targeting Android.
	/// </summary>

	[CreateAssetMenu(menuName = "Wondeluxe/Build/iOS Builder", fileName = "IOSBuilder")]
	public class IOSBuilder : PlayerBuilder
	{
		[AppInfo(-1)]
		[SerializeField]
		[Tooltip("Unique identifier for the application bundle.")]
		private string bundleIdentifier;

		[BuildConfig]
		[SerializeField]
		[Tooltip("Symlink runtime libraries when generating the Xcode project (for faster iteration time).")]
		private bool symlinkSources = true;

		public override NamedBuildTarget NamedBuildTarget
		{
			get => NamedBuildTarget.iOS;
		}

		public override BuildTargetGroup BuildTargetGroup
		{
			get => BuildTargetGroup.iOS;
		}

		public override BuildTarget BuildTarget
		{
			get => BuildTarget.iOS;
		}

		public string BundleIdentifier
		{
			get => bundleIdentifier;
		}

		public bool SymlinkSources
		{
			get => symlinkSources;
		}

		internal override void SetBuildOptions(ref BuildOptions buildOptions)
		{
			base.SetBuildOptions(ref buildOptions);

			if (SymlinkSources)
			{
#if UNITY_2021_2_OR_NEWER
				buildOptions |= BuildOptions.SymlinkSources;
#else
				buildOptions |= BuildOptions.SymlinkLibraries;
#endif
			}
		}

		internal override void ApplyPlayerSettings()
		{
			base.ApplyPlayerSettings();
			PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, BundleIdentifier);
			PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)BuildArchitecture.ARM64);

			PlayerSettings.iOS.buildNumber = Build.ToString();

			// PlayerSettings.iOS.appleDeveloperTeamID = CurrentBuildSettings.TeamID;
			// PlayerSettings.iOS.iOSManualProvisioningProfileID = CurrentBuildSettings.ProvisioningUUID;
			// PlayerSettings.iOS.iOSManualProvisioningProfileType = CurrentBuildSettings.ProvisioningProfileType;
		}

		private void Reset()
		{
			platformName = "iOS";
			// fileExtension = "apk";
		}
	}
}