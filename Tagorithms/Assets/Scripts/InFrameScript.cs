using UnityEngine;
using System.Collections;

public class InFrameScript : MonoBehaviour {

	public float moveSpeed = 0.1f;
	private Vector3 mousePos;

	void Start () {
	}
	
	void Update () {
		
		//clamp it within visible screen
		Vector3 viewPos = Camera.main.WorldToScreenPoint (this.transform.position);
		if (viewPos.x > Screen.width) {
			viewPos.x = 0.1f;
		}
		if (viewPos.x < 0) {
			viewPos.x = Screen.width-0.1f;
		}
		if (viewPos.y > Screen.height) {
			viewPos.y = 0.1f;
		}
		if (viewPos.y < 0) {
			viewPos.y = Screen.height-0.1f;
		}
		this.transform.position = Camera.main.ScreenToWorldPoint(viewPos);
	}
}
