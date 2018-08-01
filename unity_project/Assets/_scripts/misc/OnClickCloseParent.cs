using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnClickCloseParent : MonoBehaviour {

	private Button button = null;

	void Awake() 
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(CloseParent);
	}
	void CloseParent()
	{
		transform.parent.gameObject.SetActive(false);
	}
}
