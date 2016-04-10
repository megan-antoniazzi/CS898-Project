using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timerBar : MonoBehaviour {

	public float percent = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Image image = GetComponent<Image> ();
		image.fillAmount = percent;
	}
}
