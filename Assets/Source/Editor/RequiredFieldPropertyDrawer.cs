using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
[CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
public class RequiredFieldPropertyDrawer : PropertyDrawer {

	/// <summary>
	/// Computes the drawer height. If the object marked by the attribute is
	/// null, then double the height so there's room for the error message.
	/// </summary>
	public override float GetPropertyHeight(SerializedProperty a_property, GUIContent a_label) {
		float height = base.GetPropertyHeight(a_property, a_label);

		if(a_property.objectReferenceValue == null) {
			return height * 2.0f;
		}
		else {
			return height;
		}
	}

	/// <summary>
	/// Show the RequiredField inspector. Changes depending on the field being
	/// empty or not.
	/// </summary>
	public override void OnGUI(Rect a_position, SerializedProperty a_property, GUIContent a_label) {
		// Split the widget position rectangle horizontally.
		Rect bottom = new Rect();
		Rect top = new Rect();
		SplitRect(a_position, ref top, ref bottom);

		// Save the default GUI color for later.
		Color defaultColor = GUI.color;

		// If the object pointed by the property is null, then show the error
		// message, and set the GUI color to red to display the PropertyField in
		// red.
		if(a_property.objectReferenceValue == null) {
			EditorGUI.HelpBox(top, "The field below is required and can't be empty.", MessageType.Error);
			GUI.color = Color.red;
		}

		// Draw the default property field, this drawer does not alter the GUI.
		if(a_property.objectReferenceValue == null) {
			EditorGUI.PropertyField(bottom, a_property, a_label);
		}
		else {
			EditorGUI.PropertyField(a_position, a_property, a_label);
		}

		// Restore the original colors.
		GUI.color = defaultColor;
	}

	/// <summary>
	/// Splits the rectangle horizontally in two equal rectangles.
	/// </summary>
	private static void SplitRect(Rect a_rect, ref Rect a_top, ref Rect a_bottom) {
		float halfHeight = a_rect.height / 2.0f;
		a_bottom = new Rect(a_rect.xMin, a_rect.yMin + halfHeight, a_rect.width, halfHeight);
		a_top = new Rect(a_rect.xMin, a_rect.yMin, a_rect.width, halfHeight);
	}
}
