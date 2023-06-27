using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Object used to build Unity applications targeting Windows.
	/// </summary>

	[CreateAssetMenu(menuName = "Wondeluxe/Build/Windows Builder", fileName = "WindowsBuilder")]
	public sealed class WindowsBuilder : PlayerBuilder
	{
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
			get => BuildTarget.StandaloneWindows64;
		}

		private void Reset()
		{
			platformName = "Windows";
			fileExtension = "exe";
		}
	}
}