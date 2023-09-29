using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace WondeluxeEditor.Build
{
	public class BuildSettingsProvider : SettingsProvider
	{
		private readonly SerializedObject serializedObject;

		public BuildSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
		{
			serializedObject = new SerializedObject(BuildSettings.instance);
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
		}

		public override void OnGUI(string searchContext)
		{
			HideFlags defaultHideFlags = BuildSettings.instance.hideFlags;
			float defaultLabelWidth = EditorGUIUtility.labelWidth;

			BuildSettings.instance.hideFlags = HideFlags.None;

			SerializedProperty serializedProperty = serializedObject.GetIterator();

			// Skip of the script reference.
			serializedProperty.NextVisible(true);

			EditorGUILayout.Space();
			EditorGUI.indentLevel++;
			EditorGUIUtility.labelWidth *= 1.5f;

			while (serializedProperty.NextVisible(true))
			{
				EditorGUILayout.PropertyField(serializedProperty);
			}

			EditorGUIUtility.labelWidth = defaultLabelWidth;
			EditorGUI.indentLevel--;

			// Unity docs use WithoutUndo version of method, just following their example, don't know why undo should be disabled.
			serializedObject.ApplyModifiedPropertiesWithoutUndo();

			BuildSettings.instance.SaveAsset();
			BuildSettings.instance.hideFlags = defaultHideFlags;
		}

		[SettingsProvider]
		public static SettingsProvider Register()
		{
			return new BuildSettingsProvider("Wondeluxe/Build", SettingsScope.User, GetSearchKeywordsFromGUIContentProperties<BuildSettings>());
		}
	}
}