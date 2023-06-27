using UnityEngine;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Denotes a field that should be grouped with application information options in the inspector of a <see cref="PlayerBuilder"/> object.
	/// </summary>

	internal class AppInfoAttribute : PropertyAttribute
	{
		/// <summary>
		/// Order priority of the field to be displayed.
		/// </summary>

		internal int PropertyOrder { get; private set; }

		/// <summary>
		/// Denotes a field that should be grouped with application information options in the inspector of a <see cref="PlayerBuilder"/> object.
		/// </summary>
		/// <param name="propertyOrder">Order priority of the field to be displayed.</param>

		internal AppInfoAttribute(int propertyOrder = 0)
		{
			PropertyOrder = propertyOrder;
		}
	}
}