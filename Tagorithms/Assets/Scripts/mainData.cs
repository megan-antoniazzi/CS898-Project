using UnityEngine;
using System.Collections;

public class mainData : MonoBehaviour {

	public int scoreControl;
	public int scoreFlock;
	public int scoreSwarm;
	public int scoreFirefly;

	//Needs to be static.
	private static bool spawned = false;
	void Awake()
	{
		if(spawned == false)
		{
			spawned = true;
			DontDestroyOnLoad(gameObject);
			//Code...
		}
		else
		{
			DestroyImmediate(gameObject); //This deletes the new object/s that you
			// mentioned were being created
		}
	}

	public void reset()
	{
		scoreControl = 0;
		scoreFlock = 0;
		scoreSwarm = 0;
		scoreFirefly = 0;
	}

	// Use this for initialization
	void Start () {
		reset ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateControl(int num)
	{
		scoreControl = num;
	}

	public void UpdateFlock(int num)
	{
		scoreFlock = num;
	}

	public void UpdateSwarm(int num)
	{
		scoreSwarm = num;
	}

	public void UpdateFirefly(int num)
	{
		scoreFirefly = num;
	}
}
