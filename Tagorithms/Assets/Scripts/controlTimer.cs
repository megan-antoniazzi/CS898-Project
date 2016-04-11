using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Timers;

public class controlTimer : MonoBehaviour {

	System.Timers.Timer LeTimer;
	float timeLeft = 60f;
	private timerBar barScript;

	void elapsed(object sender, ElapsedEventArgs e) {
		//decrease time left (working in 1/5 of a second)
		timeLeft = timeLeft - 0.2f;

		//update time bar
		barScript.percent = timeLeft/60f;
	}

	void Start () {

		GameObject timerObject = GameObject.FindWithTag ("TimerBar");
		if (timerObject != null)
		{
			barScript = timerObject.GetComponent <timerBar>();
		}
		if (timerObject == null)
		{
			Debug.Log ("Cannot find 'barScript' script");
		}

		//Initialize timer with 1/5 second intervals
		LeTimer = new System.Timers.Timer (200);
		LeTimer.Elapsed += new ElapsedEventHandler(elapsed);
		
		LeTimer.Start();
	}
	

	// Update is called once per frame
	void Update () {
		if (timeLeft <= 0) {
			//end screen
			SceneManager.LoadScene ("ControlEnd");
		}
		
	}
}
