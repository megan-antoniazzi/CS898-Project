using UnityEngine;
using System.Collections;

public class BoidsScript : MonoBehaviour {

	public int type = 0;
	GameObject[] boids;
	private Vector3 mousePos;
	public float controlSpeed = 2f;
	public float flockSpeed = 2f;
	public float neighbourRadius = 70;
	public float sepRadius = 20;
	public float aWeight = 0.1f;
	public float cWeight = 0.1f;
	public float sWeight = 0.1f;
	public float dWeight = 0.1f;

	//flocking
	private Vector3 dir = new Vector3 (0, 0, 0);
	private int neighbours = 0;
	private int sepN = 0;
	private Vector3 alignV = new Vector3 (0, 0, 0);
	private Vector3 cohesionV = new Vector3 (0, 0, 0);
	private Vector3 posN = new Vector3 (0, 0, 0);
	private Vector3 separationV = new Vector3 (0, 0, 0);
	private Vector3 distN = new Vector3 (0, 0, 0);
	private float dist = 0;
	private Vector3 iLoc = new Vector3 (0, 0, 0);
	private Vector3 jLoc = new Vector3 (0, 0, 0);
	private Vector3 jRot = new Vector3 (0, 0, 0);
	float a1 = 0f;
	float a2 = 0f;
	float angle = 0f;


	//pso
	//create a array of best know positions
	float[,] bestPos; //x,y,dist
	int closest = 0;

	//firefly
	float[,] dists;

	//player
	PlayerScript player;

	void Start () {
		//player
		GameObject pObject = GameObject.FindWithTag ("Player");
		if (pObject != null)
		{
			player = pObject.GetComponent <PlayerScript>();
		}
		if (pObject == null)
		{
			Debug.Log ("Cannot find 'Player' script");
		}

		boids = GameObject.FindGameObjectsWithTag ("FlockBoid");

		bestPos = new float[boids.Length,3]; //x,y,dist
		dists = new float[boids.Length,3]; //x,y,dist
		//set an initial position and velocity for each boid
		Vector3 pos = new Vector3(0f,0f,0f);
		for (int i = 0; i < boids.Length; i++) {
			pos = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0.0F, Screen.width), Random.Range (0.0F, Screen.height), 0));
			pos.z = 0f;
			boids [i].transform.position = pos;
			boids [i].GetComponent<Rigidbody2D> ().velocity = new Vector3 (Random.Range (-1.0F, 1.0F), Random.Range (-1.0F, 1.0F), 0);

			if (type == 2) { //swarming
				//set initial bestPos for each boid to their current pos
				bestPos [i, 0] = pos.x;
				bestPos [i, 1] = pos.y;
				bestPos[i,2] = Vector3.Distance (mousePos,boids[i].transform.position);
				//if the dist we just calculated is better than any previous dist, that boid becomes closest
				if (bestPos [i, 2] < bestPos [closest, 2]) {
					closest = i;
				}
			}

			if (type == 3) { //fireflies
				//set initial bestPos for each boid to their current pos
				dists [i, 0] = pos.x;
				dists [i, 1] = pos.y;
				dists[i,2] = Vector3.Distance (mousePos,boids[i].transform.position);
			}

		}
	}



	// Update is called once per frame
	void Update () {
		mousePos = player.transform.position;

		switch (type) {
		case 0: //control
			for (int i = 0; i < boids.Length; i++) {
				//float dist = Vector3.Distance (boids [i].transform.position, mousePos);
				dir = Vector3.Normalize (mousePos - boids [i].transform.position);
				Vector3 v = boids [i].GetComponent<Rigidbody2D> ().velocity;
				boids [i].GetComponent<Rigidbody2D> ().velocity = 6*Vector3.Normalize(0.4f*v + 0.6f*dir);
				//rotation
				angle = Mathf.Atan2(boids[i].GetComponent<Rigidbody2D> ().velocity.y, boids [i].GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg;
				boids[i].transform.rotation = Quaternion.AngleAxis(angle-180, Vector3.forward);
			}
			break;
		case 1: //flocking
			for (int i = 0; i < boids.Length; i++) {
				neighbours = 0;
				alignV = new Vector3 (0f, 0f, 0f);
				posN = new Vector3 (0f, 0f, 0f);
				sepN = 0;
				distN = new Vector3 (0f, 0f, 0f);
				Vector3 iRot = Vector3.Normalize (mousePos - iLoc);
				for (int j = 0; j < boids.Length; j++) {
					if (j != i) { //if not our current boid
						//check if its close enought to be a neightbour
						iLoc = Camera.main.WorldToScreenPoint (boids [i].transform.position);
						jLoc = Camera.main.WorldToScreenPoint (boids [j].transform.position);

						a1 = boids [i].transform.rotation.eulerAngles.z;
						jRot = Vector3.Normalize (jLoc - iLoc);
						a2 = Vector2.Angle (new Vector2 (jRot.y, jRot.x),new Vector2 (iRot.y, iRot.x));
						if (a1 < 0) {a1 = 360 + a1;}
						if (a2 < 0) {a2 = 360 + a2;}
						if (Mathf.Abs (Mathf.DeltaAngle (a1, a2)) < 60) {
							dist = Vector3.Distance (iLoc, jLoc);
							if (dist < neighbourRadius) {
								neighbours++;

								//alignment
								Vector3 aVel = boids [j].GetComponent<Rigidbody2D> ().velocity;
								alignV = alignV + aVel;

								//add neighbours position to posN
								posN.x = posN.x + jLoc.x;
								posN.y = posN.y + jLoc.y;
							}

							if (dist < sepRadius) {
								sepN++;
								distN = distN + (Vector3.Normalize (iLoc - jLoc));
							}
						}
					}

					if (neighbours > 0) {
						alignV = Vector3.Normalize (new Vector3(alignV.x / neighbours, alignV.y / neighbours, 0f));

						//cohesion divide and normalize
						posN.x = posN.x / neighbours;
						posN.y = posN.y / neighbours;
						cohesionV = Vector3.Normalize (posN - iLoc);
						cohesionV.z = 0f;
					}

					if(sepN > 0){
						//separation divide and normalize
						separationV = Vector3.Normalize (new Vector3(distN.x / sepN, distN.y / sepN, 0f));
					}

					//direction to player
					dir = Vector3.Normalize (mousePos - boids[i].transform.position);

					//new velocity
					Vector3 newVel = Vector3.Normalize(alignV*aWeight + cohesionV*cWeight + separationV*sWeight + dir*dWeight);

					Vector3 v = boids [i].GetComponent<Rigidbody2D> ().velocity;
					boids [i].GetComponent<Rigidbody2D> ().velocity = 6*Vector3.Normalize(0.4f*v + 0.6f*newVel);

					//rotation
					angle = Mathf.Atan2(boids[i].GetComponent<Rigidbody2D> ().velocity.y, boids [i].GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg;
					boids[i].transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
				}
			}
			break;
		case 2: //swarming
			//update all the distances in bestPos
			for (int i = 0; i < boids.Length; i++) {
				bestPos [i, 2] = Vector3.Distance (mousePos, new Vector3 (bestPos [i, 0], bestPos [i, 1], 0f));
			}

			//check for new bestPos, and/or new closest
			for (int i = 0; i < boids.Length; i++) {
				//best objective function = boid who is closest to player

				//calculate current dist to mouse - and last dist to mouse
				float curDist = Vector3.Distance (mousePos, boids [i].transform.position);

				//check if better than previous best distance
				if (curDist < bestPos [i, 2]) {
					//and if so, replace bestPos
					bestPos [i, 0] = boids [i].transform.position.x;
					bestPos [i, 1] = boids [i].transform.position.y;
					bestPos [i, 2] = curDist;
				}

				//check if bestPos[i] is better than global best and if so, update
				if (bestPos [i, 2] < bestPos [closest, 2]) {
					closest = i;
				}
			}

			//update the velocities of each particle
			for (int i = 0; i < boids.Length; i++) {
				Vector3 v = boids [i].GetComponent<Rigidbody2D> ().velocity;
				Vector3 vecPbest = new Vector3 (bestPos [i, 0] - boids [i].transform.position.x, bestPos [i, 1] - boids [i].transform.position.y, 0f);
				Vector3 vecGbest = new Vector3 (bestPos [closest, 0] - boids [i].transform.position.x, bestPos [closest, 1] - boids [i].transform.position.y, 0f);
				boids [i].GetComponent<Rigidbody2D> ().velocity = 6*Vector3.Normalize(0.4f*v + 0.6f*(2 * Random.Range (0.0F, 1.0F) * vecPbest + 2 * Random.Range (0.0F, 1.0F) * vecGbest));

				//rotation
				angle = Mathf.Atan2(boids[i].GetComponent<Rigidbody2D> ().velocity.y, boids [i].GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg;
				boids[i].transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);

			}

			break;
		case 3: //fireflies
			//update all the distances in dists based on current posistion
			for (int i = 0; i < boids.Length; i++) {
				dists [i, 0] = boids [i].transform.position.x;
				dists [i, 1] = boids [i].transform.position.y;
				dists [i, 2] = Vector3.Distance (mousePos, boids [i].transform.position);
			}

			for (int i = 0; i < boids.Length; i++) {
				posN = new Vector3 (0, 0, 0);
				neighbours = 0;
				for (int j = 0; j < boids.Length; j++) {
					if (j != i) { //if not our current boid
						//check if its close enought to be a neightbour
						iLoc = Camera.main.WorldToScreenPoint (boids [i].transform.position);
						jLoc = Camera.main.WorldToScreenPoint (boids [j].transform.position);
						dist = Vector3.Distance (iLoc, jLoc);

						if (dist < neighbourRadius) {
							//add neighbours position to posN
							//is j closer to the player than i?
							if (dists [j,2] < dists [i,2]) {
								posN = posN + jLoc;
								neighbours++;
							}
						}
					}
				}
				Vector3 newVel;
				if (neighbours > 0) {
					//cohesion divide and normalize
					posN.x = posN.x / neighbours;
					posN.y = posN.y / neighbours;
					newVel = Vector3.Normalize (posN - iLoc);
					newVel.z = 0;
				} else {
					newVel = new Vector3 (Random.Range (-1.0F, 1.0F), Random.Range (-1.0F, 1.0F), 0);
				}

				//set boid's new velocity
				Vector3 v = boids [i].GetComponent<Rigidbody2D> ().velocity;
				boids [i].GetComponent<Rigidbody2D> ().velocity = 6*Vector3.Normalize(0.4f*v + 0.6f*newVel);

				//rotation
				angle = Mathf.Atan2(boids[i].GetComponent<Rigidbody2D> ().velocity.y, boids [i].GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg;
				boids[i].transform.rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
			}
			break;
		}
	}

}
