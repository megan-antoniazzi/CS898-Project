using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class controlEnd : MonoBehaviour {

	public void onClick(){
		SceneManager.LoadScene ("FlockStart");
	}
}
