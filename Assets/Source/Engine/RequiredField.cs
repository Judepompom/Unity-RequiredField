using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System;

/// <summary>
/// Flag a field with this attribute to imply to the user that this field MUST
/// be filled in the editor in order for the game to work. If the field is left
/// empty, it will be displayed in red in the inspector with a warning message.
/// 
/// Empty fields marked as "RequiredField" will also stop the execution of the
/// application on Start().
/// 
/// IMPORTANT : this work only on class that inherit from BaseClass, or if you
/// call RequiredField.CheckRequiredFields(this) in your own baseclass Start()
/// method.
/// </summary>
public class RequiredFieldAttribute : PropertyAttribute {
	public RequiredFieldAttribute() {
	}
}

/// <summary>
/// 
/// </summary>
public static class RequiredField {

	/// <summary>
	/// 
	/// </summary>
	/// <param name="a_Component"></param>
	[Conditional("UNITY_EDITOR")]
	public static void CheckRequiredFields(Component a_Component) {
		// Retrieve the type of the current class.
		Type componentType = a_Component.GetType();

		// Retrieve all fields from the class, public and privates.
		FieldInfo[] fields = componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		for(int j = 0; j < fields.Length; j++) {
			FieldInfo currentField = fields[j];

			// Retrieve all attributes for the current field.
			object[] attributes = currentField.GetCustomAttributes(true);

			// Search for RequiredFieldAttribute object in the attributes list.
			for(int i = 0; i < attributes.Length; i++) {
				if(attributes[i].GetType() == typeof(RequiredFieldAttribute)) {

					// Check if the field marked by the attribute is null.
					if(currentField.GetValue(a_Component) == null) {
						string fieldName = ToEditorFormat(currentField.Name);
						string objectType = GetObjectType(a_Component);
						string gameObjectName = GetGameObjectName(a_Component);

						// Alert the user with an explicit message pointing to
						// the affected object.
						UnityEngine.Debug.LogError(gameObjectName + " : The Required field " + fieldName + " in component " + objectType + " is empty. Please, fill it with the appropriate component.", a_Component);

						// Break the application to force the user to fill the 
						// empty field.
						UnityEngine.Debug.Break();
					}
				}
			}
		}
	}

	/// <summary>
	/// Highlights a text for the console using rich text markers. Makes the
	/// text bold and sets color to white.
	/// </summary>
	private static string Highlight(string a_text) {
		return "<b><color=white>" + a_text + "</color></b>";
	}

	/// <summary>
	/// Transform the string to Editor Case : remove the standard "member
	/// variable" prefixes ("_", "_m") then convert it to Title Case. Also
	/// highlights the text.
	/// 
	/// Examples:
	///		_value => Value
	///		m_Value => Value
	/// </summary>
	public static string ToEditorFormat(string a_text) {
		string str = a_text;

		if(str[0] == '_') {
			str = str.Remove(0, 1);
		}
		else if(str.StartsWith("m_")) {
			str = str.Remove(0, 2);
		}

		str = str[0].ToString().ToUpper() + str.Remove(0, 1);
		return Highlight(str);
	}

	/// <summary>
	/// Retrieves the name of the game object to which is attached the
	/// component. Also highlights the resulting name.
	/// </summary>
	private static string GetGameObjectName(Component a_component) {
		return Highlight(a_component.gameObject.name);
	}

	/// <summary>
	/// Retrieves the type of the given object as a string and highlights it.
	/// </summary>
	private static string GetObjectType(UnityEngine.Object a_object) {
		return Highlight(a_object.GetType().Name);
	}
}