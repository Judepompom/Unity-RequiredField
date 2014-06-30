using UnityEngine;
using System.Collections;

/// <summary>
/// Inherit from this base class if you want the RequiredField check to be
/// performed when starting the game in the editor.
/// 
/// If you have your own baseclass, you can simply call
/// RequiredField.CheckRequiredFields(this); in its Start() method instead of 
/// inheriting from BaseClass.
/// </summary>
public class BaseClass : MonoBehaviour {

	protected virtual void Start () {
		RequiredField.CheckRequiredFields(this);	
	}
}
