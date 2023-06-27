using UnityEngine;

namespace WondeluxeEditor.Build
{
	/// <summary>
	/// Denotes a field that should be grouped with platform information options in the inspector of a <see cref="PlayerBuilder"/> object.
	/// </summary>

	internal class PlatformInfoAttribute : PropertyAttribute
	{
		/// <summary>
		/// Order priority of the field to be displayed.
		/// </summary>

		internal int PropertyOrder { get; set; }

		/// <summary>
		/// Denotes a field that should be grouped with platform information options in the inspector of a <see cref="PlayerBuilder"/> object.
		/// </summary>
		/// <param name="propertyOrder">Order priority of the field to be displayed.</param>

		internal PlatformInfoAttribute(int propertyOrder = 0)
		{
			PropertyOrder = propertyOrder;
		}
	}
}