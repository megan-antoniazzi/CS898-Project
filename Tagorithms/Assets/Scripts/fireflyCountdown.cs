using UnityEngine;
using System.Collections;

public class fireflyCountdown : MonoBehaviour {

	private string countdown;    
	private int showCountdown;

	void Start () {
		showCountdown  = 0;
		countdown  = ""; 
		StartCoroutine(getReady ());
	}


	// call this function to display countdown
	IEnumerator getReady ()    
	{
		showCountdown = 1;    

		countdown = "3";
		yield return new WaitForSeconds (1.0f);  

		countdown = "2";    
		yield return new WaitForSeconds (1.0f);

		countdown = "1";    
		yield return new WaitForSeconds (1.0f);

		countdown = ""; 
		showCountdown = 0;
		//enable scripts
		this.GetComponent<fireflyTimer> ().enabled = true;
		this.GetComponent<BoidsScript> ().enabled = true;
		GameObject pObject = GameObject.FindWithTag ("Player");
		if (pObject != null)
		{
			pObject.GetComponent <PlayerScript>().enabled = true;
			pObject.GetComponent <PlayerScript> ().type = this.GetComponent<BoidsScript> ().type;
		}
		if (pObject == null)
		{
			Debug.Log ("Cannot find 'Player' script");
		}
 
	}

	// GUI
	void OnGUI ()
	{
		if (showCountdown == 1)
		{    
			GUIStyle guiStyle = new GUIStyle ();
			guiStyle.fontSize = (int) (Screen.width * 0.2f);
			guiStyle.normal.textColor = new Color32 (141, 26, 0, 255);
			// display countdown     
			GUI.Label (new Rect (Screen.width / 2f-Screen.width * 0.05f, Screen.height/2f-Screen.width * 0.1f, 180f, 140f), countdown, guiStyle);
		}    
	}
}
