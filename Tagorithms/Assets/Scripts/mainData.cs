using UnityEngine;
using System.Collections;

public class mainData : MonoBehaviour {

	public int scoreControl;
	public int scoreFlock;
	public int scoreSwarm;
	public int scoreFirefly;

	private static bool spawned = false;
	void Awake()
	{
		if(spawned == false)
		{
			spawned = true;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject); //This deletes new gameObjects that are being created
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
