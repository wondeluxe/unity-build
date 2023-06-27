using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor.Build
{
	[CustomEditor(typeof(PlayerBuilder), true)]
	public class PlayerBuilderEditor : Editor
	{
		private PlayerBuilder Builder => (target as PlayerBuilder);

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// First property is Base.
			SerializedProperty serializedProperty = serializedObject.GetIterator();

			// Second property is Script reference.
			serializedProperty.NextVisible(true);

			GUI.enabled = false;
			EditorGUILayout.PropertyField(serializedProperty, true);
			GUI.enabled = true;

			List<PropertyOrderInfo> infos = new List<PropertyOrderInfo>();

			while (serializedProperty.NextVisible(false))
			{
				infos.Add(GetPropertyOrderInfo(serializedProperty.Copy(), infos.Count));
			}

			infos.Sort(PropertyOrderInfo.Compare);

			for (int i = 0; i < infos.Count; i++)
			{
				EditorGUILayout.PropertyField(infos[i].Property, true);
			}

			serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button("Build Player"))
			{
				Builder.BuildPlayer(false);
			}

			if (GUILayout.Button("Clean Build Player"))
			{
				Builder.BuildPlayer(true);
			}
		}

		private static PropertyOrderInfo GetPropertyOrderInfo(SerializedProperty serializedProperty, int defaultOrder)
		{
			PlatformInfoAttribute platformInfoAttribute = serializedProperty.GetAttribute<PlatformInfoAttribute>(true);

			if (platformInfoAttribute != null)
			{
				return new PropertyOrderInfo(serializedProperty, -5, platformInfoAttribute.PropertyOrder, defaultOrder);
			}

			BuildOutputAttribute buildOutputAttribute = serializedProperty.GetAttribute<BuildOutputAttribute>(true);

			if (buildOutputAttribute != null)
			{
				return new PropertyOrderInfo(serializedProperty, -4, buildOutputAttribute.PropertyOrder, defaultOrder);
			}

			AppInfoAttribute appInfoAttribute = serializedProperty.GetAttribute<AppInfoAttribute>(true);

			if (appInfoAttribute != null)
			{
				return new PropertyOrderInfo(serializedProperty, -3, appInfoAttribute.PropertyOrder, defaultOrder);
			}

			BuildConfigAttribute buildConfigAttribute = serializedProperty.GetAttribute<BuildConfigAttribute>(true);

			if (buildConfigAttribute != null)
			{
				return new PropertyOrderInfo(serializedProperty, -2, buildConfigAttribute.PropertyOrder, defaultOrder);
			}

			BuildContentAttribute buildContentAttribute = serializedProperty.GetAttribute<BuildContentAttribute>(true);

			if (buildContentAttribute != null)
			{
				return new PropertyOrderInfo(serializedProperty, -1, buildContentAttribute.PropertyOrder, defaultOrder);
			}

			return new PropertyOrderInfo(serializedProperty, 0, 0, defaultOrder);
		}

		private class PropertyOrderInfo
		{
			public SerializedProperty Property;
			public int Precedence;
			public int PropertyOrder;
			public int DefaultOrder;

			public PropertyOrderInfo(SerializedProperty property, int precedence, int propertyOrder, int defaultOrder)
			{
				Property = property;
				Precedence = precedence;
				PropertyOrder = propertyOrder;
				DefaultOrder = defaultOrder;
			}

			public override string ToString()
			{
				return $"[Property = \"{Property.displayName}\", Precedence = {Precedence}, PropertyOrder = {PropertyOrder}, DefaultOrder = {DefaultOrder}]";
			}

			public static int Compare(PropertyOrderInfo info1, PropertyOrderInfo info2)
			{
				if (info1.Precedence < info2.Precedence)
				{
					return -1;
				}

				if (info1.Precedence > info2.Precedence)
				{
					return 1;
				}

				if (info1.PropertyOrder < info2.PropertyOrder)
				{
					return -1;
				}

				if (info1.PropertyOrder > info2.PropertyOrder)
				{
					return 1;
				}

				if (info1.DefaultOrder < info2.DefaultOrder)
				{
					return -1;
				}

				if (info1.DefaultOrder > info2.DefaultOrder)
				{
					return 1;
				}

				return 0;
			}
		}
	}
}