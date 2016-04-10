using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class endScores : MonoBehaviour {

	private mainData data;

	// Use this for initialization
	void Start () {
		GameObject dataObject = GameObject.FindWithTag ("Main");
		if (dataObject != null)
		{
			data = dataObject.GetComponent <mainData>();
		}
		if (data == null)
		{
			Debug.Log ("Cannot find 'mainData' script");
		}

		gameObject.GetComponent<Text> ().text = "Thanks for playing Tag-o-rithms!\n\nYou scored:\n\nBlue\t\t\t\t\t\t\t" + data.scoreControl + "\nGreen\t\t\t\t\t\t" + data.scoreFlock + "\nYellow\t\t\t\t\t\t" + data.scoreSwarm + "\nRed\t\t\t\t\t\t\t" + data.scoreFirefly;
	}
}
