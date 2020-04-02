using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Prisoner : MonoBehaviour
{
	public NavMeshAgent move;

	[Header("Stats")]
	public int hunger;
	public int sleep;
	public int fatigue;
	public int exhaustion;

	[Header("Personality")]
	public string rec1;
	public string rec2;
	public string rec3;
	public int guardTrust;

	[Header("Desires")]
	public float foodDesire;
	public float rec1Desire;
	public float rec2Desire;
	public float rec3Desire;
	public float restDesire;

	[Header("Room Information")]
	public bool roomCheck = false;
	public string cell;
	public string currentRoom;

	void Start()
	{
		#region Randomized starting stats
		rec1 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)];
		do { rec2 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (rec2 == rec1);
		do { rec3 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (rec3 == rec1 || rec3 == rec2);
		hunger = Random.Range(50, 101);
		sleep = Random.Range(50, 101);
		fatigue = Random.Range(0, 51);
		exhaustion = Random.Range(0, 51);
		guardTrust = Random.Range(0, 101);
		#endregion

		//Eyes();
	}

	void Update()
	{
		if (roomCheck)
			RoomCheck();
	}

	public void RoomCheck()
	{
		roomCheck = false;
		Stats(-5, -5, 0, 0);

		switch (Manager.m.hour)
		{
			case int t when (t <= 6 || t >= 22):
				if (currentRoom != cell)
					move.SetDestination(GameObject.Find(cell).transform.position);
				Stats(0, 10, -10, -10);
				break;
			case int t when (t == 7 || t == 12 || t == 17):
				if (hunger < 50)
				{
					move.SetDestination(GameObject.Find("Cafeteria").transform.position);
					Stats(1, 50);
					Debug.Log(name + " is going to go eat.");
				}
				else if (fatigue <= 50)
				{
					move.SetDestination(GameObject.Find(rec1).transform.position);
					Stats(3, 25);
					Debug.Log(name + " is going to the " + rec1);
				}
				else
				{
					move.SetDestination(GameObject.Find(cell).transform.position);
					Stats(3, -25);
					Debug.Log(name + " is going to rest in " + cell);
				}
				break;
			case int t when (t >= 8 && t <= 11):
				//recreational time
				break;
			case int t when (t >= 13 && t <= 16):
				//labor time
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
		}
	}

	private float Desires(int desire)
	{
		/*Desire Chart
		 * 0 = Food
		 * 1 = Recreation
		 * 2 = Sleep/Rest
		 * 3 = 
		 * 4 = 
		 * 5 = 
		 * 6 = 
		 * 7 = 
		 * 8 = 
		 * 9 = 
		 */

		float total = 0;
		switch (desire)
		{
			case 0:
				//based on hunger, rest
				break;
			case 1:
				//based on fatigue, exhaustion
				break;
			case 2:
				//based on fatigue, exhaution, rest
				break;
			case 3:
				//code here
				break;
			case 4:
				//code here
				break;
			case 5:
				//code here
				break;
			case 6:
				//code here
				break;
			case 7:
				//code here
				break;
			case 8:
				//code here
				break;
			case 9:
				//code here
				break;
		}

		return total;
	}

	private void Stats(int hunger, int rest, int fatigue, int exhaustion)
	{
		this.hunger += hunger;
		this.sleep += rest;
		this.fatigue += fatigue;
		this.exhaustion += exhaustion;

		StatMinMax();
	}

	private void Stats(int stat, int magnitude)
	{
		switch (stat)
		{
			case 1:
				hunger += magnitude;
				break;
			case 2:
				sleep += magnitude;
				break;
			case 3:
				fatigue += magnitude;
				break;
			case 4:
				exhaustion += magnitude;
				break;
		}

		StatMinMax();
	}

	private void StatMinMax()
	{
		if (hunger > 100)
			hunger = 100;
		else if (hunger < 0)
			hunger = 0;

		if (sleep > 100)
			sleep = 100;
		else if (sleep < 0)
			sleep = 0;

		if (fatigue > 100)
			fatigue = 100;
		else if (fatigue < 0)
			fatigue = 0;

		if (exhaustion > 100)
			exhaustion = 100;
		else if (exhaustion < 0)
			exhaustion = 0;
	}

	void Eyes()
	{
		RaycastHit hit;
		float distance = 15f;
		float angle = 78;
		float segments = angle - 1;
		Vector3 startPos = transform.position + (Vector3.up * 2);
		Vector3 targetPos = new Vector3();
		float startAngle = -angle * .5f;
		float finishAngle = angle * .5f;
		float increment = angle / segments;

		for (float i = startAngle; i < finishAngle; i += increment)
		{
			targetPos = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * distance;

			if (Physics.Raycast(startPos, targetPos, out hit, distance))
			{
				if (hit.collider.gameObject.tag == "Guard")
				{
					//Do something to make them move away
					Debug.Log("REEEE");
				}
			}
			Debug.DrawRay(startPos, targetPos, Color.red);
		}
	}
}
