using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour {
	mainData data;

	public void onClick(){
		GameObject o = GameObject.FindWithTag ("Main");

		if (o != null)
		{
			data = o.GetComponent <mainData>();
		}

		if (data == null)
		{
			Debug.Log ("Cannot find 'mainData' script");
		}
		data.reset ();
		SceneManager.LoadScene ("Menu");
	}
}
