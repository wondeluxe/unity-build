using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Object used to build Unity applications targeting Android.
	/// </summary>

	[CreateAssetMenu(menuName = "Wondeluxe/Build/Android Builder", fileName = "AndroidBuilder")]
	public class AndroidBuilder : PlayerBuilder
	{
		[AppInfo(-1)]
		[SerializeField]
		[Tooltip("Unique identifier for the application bundle.")]
		private string packageName;

		[AppInfo(1)]
		[SerializeField]
		[Tooltip("Path to the keystore file. Relative to Android Keystore File Path (set in UserSettings > Wondeluxe > Build). If supplied, the password should be given in the Keystore File (if required).")]
		private string keystoreFile;

		[AppInfo(1)]
		[SerializeField]
		[Tooltip("Alias name of the key to use. If supplied, the password should be given in the Key Password File (if required).")]
		private string keyAlias;

		[AppInfo(1)]
		[SerializeField]
		[Tooltip("Path to the key password file. Relative to Android Key Password File Path (set in UserSettings > Wondeluxe > Build).")]
		private string keyPasswordFile;

		[BuildConfig]
		[SerializeField]
		[Tooltip("CPU architecture.")]
		private AndroidArchitecture architecture = AndroidArchitecture.ARMv7;

		public override NamedBuildTarget NamedBuildTarget
		{
			get => NamedBuildTarget.Android;
		}

		public override BuildTargetGroup BuildTargetGroup
		{
			get => BuildTargetGroup.Android;
		}

		public override BuildTarget BuildTarget
		{
			get => BuildTarget.Android;
		}

		public string BundleIdentifier
		{
			get => packageName;
		}

		public string KeystoreFile
		{
			get => string.IsNullOrWhiteSpace(keystoreFile) ? null : Path.Combine(BuildSettings.AndroidKeystoreFilePath, keystoreFile);
		}

		public string KeyAlias
		{
			get => keyAlias;
		}

		public string KeyPasswordFile
		{
			get => string.IsNullOrWhiteSpace(keyPasswordFile) ? null : Path.Combine(BuildSettings.AndroidKeyPasswordFilePath, keyPasswordFile);
		}

		public AndroidArchitecture Architecture
		{
			get => architecture;
		}

		internal override void ApplyPlayerSettings()
		{
			base.ApplyPlayerSettings();
			PlayerSettings.SetApplicationIdentifier(NamedBuildTarget, BundleIdentifier);
			
			PlayerSettings.Android.bundleVersionCode = (int)Build;

			if (!string.IsNullOrWhiteSpace(keystoreFile))
			{
				PlayerSettings.Android.useCustomKeystore = true;
				PlayerSettings.Android.keystoreName = Path.Combine(BuildSettings.AndroidKeystoreFilePath, keystoreFile);
			}

			if (!string.IsNullOrWhiteSpace(keyAlias))
			{
				PlayerSettings.Android.keyaliasName = keyAlias;
			}

			if (!string.IsNullOrWhiteSpace(keyPasswordFile))
			{
				string keyPasswordFilePath = Path.Combine(BuildSettings.AndroidKeyPasswordFilePath, keyPasswordFile);

				using (StreamReader reader = new StreamReader(keyPasswordFilePath))
				{
					// First line of the key password file will be the password for the keystore.
					PlayerSettings.Android.keystorePass = reader.ReadLine();

					// Subsequent lines will contain passwords for key aliases in the format `key: password`.

					Regex regex = new Regex(@"\s*([^:\s]+)\s*:\s*([^\n\r]+)");

					while (reader.Peek() > -1)
					{
						Match match = regex.Match(reader.ReadLine());

						if (match.Success && match.Groups[1].Value == keyAlias)
						{
							PlayerSettings.Android.keyaliasPass = match.Groups[2].Value;
							break;
						}
					}
				}
			}

			PlayerSettings.Android.targetArchitectures = Architecture;
		}

		private void Reset()
		{
			platformName = "Android";
			fileExtension = "apk";
		}
	}
}