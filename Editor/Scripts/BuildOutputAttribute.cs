using UnityEngine;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Denotes a field that should be grouped with build output options in the inspector of a <see cref="PlayerBuilder"/> object.
	/// </summary>

	internal class BuildOutputAttribute : PropertyAttribute
	{
		/// <summary>
		/// Order priority of the field to be displayed.
		/// </summary>

		internal int PropertyOrder { get; set; }

		/// <summary>
		/// Denotes a field that should be grouped with build output options in the inspector of a <see cref="PlayerBuilder"/> object.
		/// </summary>
		/// <param name="propertyOrder">Order priority of the field to be displayed.</param>

		internal BuildOutputAttribute(int propertyOrder = 0)
		{
			PropertyOrder = propertyOrder;
		}
	}
}