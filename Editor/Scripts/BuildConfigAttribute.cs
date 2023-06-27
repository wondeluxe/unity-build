using UnityEngine;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Denotes a field that should be grouped with build configuration options in the inspector of a <see cref="PlayerBuilder"/> object.
	/// </summary>

	internal class BuildConfigAttribute : PropertyAttribute
	{
		/// <summary>
		/// Order priority of the field to be displayed.
		/// </summary>

		internal int PropertyOrder { get; set; }

		/// <summary>
		/// Denotes a field that should be grouped with build configuration options in the inspector of a <see cref="PlayerBuilder"/> object.
		/// </summary>
		/// <param name="propertyOrder">Order priority of the field to be displayed.</param>

		internal BuildConfigAttribute(int propertyOrder = 0)
		{
			PropertyOrder = propertyOrder;
		}
	}
}