using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class playButton : MonoBehaviour {

	public void onClick(){
		SceneManager.LoadScene ("ControlStart");
	}
}
