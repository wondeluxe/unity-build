using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build.Reporting;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Utility/helper methods for Unity builds.
	/// </summary>

	public static class PlayerBuilderUtility
	{
		/// <summary>
		/// Constructs a string file path name defining where a Unity application will be built.
		/// </summary>
		/// <param name="filePath">Base destination path where the player will be built.</param>
		/// <param name="fileName">File name to use for the output application file.</param>
		/// <param name="fileExtension">File extension to apply to the output application file. May be blank (empty) or null.</param>
		/// <param name="platformName">Platform name of the player.</param>
		/// <param name="build">Application build number.</param>
		/// <returns>A string path.</returns>

		public static string ConstructPlayerPathName(string filePath, string fileName, string fileExtension, string platformName, uint build)
		{
			string condensedFileName = Regex.Replace(fileName, @"\s+", "");
			string condensedPlatformName = Regex.Replace(platformName, @"\s+", "");
			string playerDirectoryName = $"{condensedFileName}.{condensedPlatformName}.{build:D4}";
			string playerFileName = string.IsNullOrWhiteSpace(fileExtension) ? fileName : $"{fileName}.{fileExtension}";

			return Path.Combine(filePath, playerDirectoryName, playerFileName);
		}

		/// <summary>
		/// Returns a string representation of the time span taken for a build.
		/// </summary>
		/// <param name="buildReport">The <see cref="BuildReport"/> to get the build time from.</param>
		/// <returns>A string representation of a time span.</returns>

		public static string GetBuildTime(BuildReport buildReport)
		{
			TimeSpan totalTime = buildReport.summary.totalTime;

			int hours = totalTime.Hours;
			int minutes = totalTime.Minutes;
			int seconds = (totalTime.Milliseconds > 0) ? (totalTime.Seconds + 1) : totalTime.Seconds;

			List<string> timeComponents = new List<string>();

			if (hours > 0)
			{
				timeComponents.Add($"{hours} {(hours > 1 ? "hours" : "hour")}");
			}

			if (minutes > 0)
			{
				timeComponents.Add($"{minutes} {(minutes > 1 ? "minutes" : "minute")}");
			}

			if (seconds > 0)
			{
				timeComponents.Add($"{seconds} {(seconds > 1 ? "seconds" : "second")}");
			}

			return string.Join(" ", timeComponents);
		}

		/// <summary>
		/// Returns a string log of the errors encountered during a build.
		/// </summary>
		/// <param name="buildReport">The <see cref="BuildReport"/> to get the errors from.</param>
		/// <returns>A string log of errors.</returns>

		public static string GetBuildErrors(BuildReport buildReport)
		{
			string[] errors = new string[buildReport.summary.totalErrors];
			int index = 0;

			for (int s = 0; s < buildReport.steps.Length; s++)
			{
				BuildStep step = buildReport.steps[s];

				for (int m = 0; m < step.messages.Length; m++)
				{
					BuildStepMessage message = step.messages[m];

					if (message.type == LogType.Error)
					{
						errors[index++] = message.content;

						if (index == errors.Length)
						{
							return string.Join("\n", errors);
						}
					}
				}
			}

			return null;
		}
	}
}