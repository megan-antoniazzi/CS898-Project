using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timerBar : MonoBehaviour {

	public float percent = 1.0f;

	void Start () {
	
	}

	void Update () {
		Image image = GetComponent<Image> ();
		image.fillAmount = percent;
	}
}
