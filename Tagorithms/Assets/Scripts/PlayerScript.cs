using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private Vector3 mousePos;
	private ScoreScript scoreScript;
	private mainData data;
	public int type;

	private int touching = 1;
	private float touchId = 1.1f;

	void Start () {
		GameObject scoreObject = GameObject.FindWithTag ("Score");
		if (scoreObject != null)
		{
			scoreScript = scoreObject.GetComponent <ScoreScript>();
		}
		if (scoreScript == null)
		{
			Debug.Log ("Cannot find 'ScoreScript' script");
		}

		GameObject dataObject = GameObject.FindWithTag ("Main");
		if (dataObject != null)
		{
			data = dataObject.GetComponent <mainData>();
		}
		if (data == null)
		{
			Debug.Log ("Cannot find 'mainData' script");
		}

		//disable to lessen lag
		QualitySettings.vSyncCount = 0;
	}

	void Update () {
		//is the player touhing the screen
		if (Input.touchCount > 0) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);

			//check is same finger is still touching. if not, set touching to false. 
			if (Input.GetTouch (0).fingerId != touchId) {
				touching = 0;
			}

			//if new touch, check it is on top of player icon. if so touching becomes true ad fingerID recorded.
			Vector2 diff = new Vector2 (Mathf.Abs (mousePos.x - this.transform.position.x), Mathf.Abs (mousePos.y - this.transform.position.y));
			if (diff.x < 0.5f && diff.y < 0.5f) {
				touching = 1;
				touchId = Input.GetTouch (0).fingerId;
			}

			//only move if player if touching is true
			if (touching == 1) {
				transform.position = Vector3.MoveTowards(transform.position, mousePos, 1);
				transform.position = new Vector3(mousePos.x,mousePos.y,0f);
				//clamp it within visible screen
				float size = 15f;
				Vector3 viewPos = Camera.main.WorldToScreenPoint (this.transform.position);
				if (viewPos.x > Screen.width) {
					viewPos.x = Screen.width - size;
				}
				if (viewPos.x < 0) {
					viewPos.x = size;
				}
				if (viewPos.y > Screen.height) {
					viewPos.y = Screen.height - size;
				}
				if (viewPos.y < 0) {
					viewPos.y = size;
				}
				this.transform.position = Camera.main.ScreenToWorldPoint (viewPos);
			}
		} else {
			touchId = 1.1f;
		}


	}

	void OnTriggerEnter2D(Collider2D coll) {
		//reset the boid to a random location
		Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0.0F, Screen.width), Random.Range (0.0F, Screen.height), 0));
		pos.z = 0f;
		coll.transform.position = pos;
		coll.GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-1.0F, 1.0F), Random.Range (-1.0F, 1.0F), Random.Range (0.0F, 1.0F));

		scoreScript.UpdateScore ();

		switch (type) {
		case 0:
			data.UpdateControl (scoreScript.score);
			break;
		case 1:
			data.UpdateFlock (scoreScript.score);
			break;
		case 2:
			data.UpdateSwarm (scoreScript.score);
			break;
		case 3:
			data.UpdateFirefly (scoreScript.score);
			break;
		}
	}
}
