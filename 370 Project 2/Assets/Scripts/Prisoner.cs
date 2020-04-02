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

	[Header("Personality")]
	public string rec1;
	public string rec2;
	public string rec3;
	public string labor;
	public int guardTrust;

	[Header("Desires")]
	public float foodDesire;
	public float restDesire;
	public float rec1Desire;
	public float rec2Desire;
	public float rec3Desire;

	[Header("Room Information")]
	public bool checkSchedule = false;
	public string cell;
	public string currentRoom;

	void Start()
	{
		#region Randomized starting stats
		rec1 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)];
		do { rec2 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (rec2 == rec1);
		do { rec3 = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (rec3 == rec1 || rec3 == rec2);
		labor = Manager.m.labors[Random.Range(0, Manager.m.labors.Count)];
		hunger = Random.Range(50, 101);
		sleep = Random.Range(50, 101);
		fatigue = Random.Range(0, 51);
		guardTrust = Random.Range(0, 101);
		#endregion

		//Eyes();
	}

	void Update()
	{
		if (checkSchedule)
			CheckSchedule();
	}

	public void CheckSchedule()
	{
		checkSchedule = false;
		Stats(-5, -5, 0);

		switch (Manager.m.hour)
		{
			case int t when (t <= 6 || t >= 22):
				if (currentRoom != cell)
					move.SetDestination(GameObject.Find(cell).transform.position);
				Stats(0, 10, -10);
				break;
			case int t when (t == 7 || t == 12 || t == 17):
				if (Desires(0) >= Desires(1) && Desires(0) >= Desires(2))
				{
					move.SetDestination(GameObject.Find("Cafeteria").transform.position);
					Stats(1, 50);
					Debug.Log(name + " is going to go eat.");
				}
				else if (Desires(1) >= Desires(2))
					Activity(ChooseRecreation());
				else
					Rest();
				break;
			case int t when (t >= 8 && t <= 11):
				if (Desires(1) > Desires(2))
					Activity(ChooseRecreation());
				else
					Rest();
				break;
			case int t when (t >= 13 && t <= 16):
				if (Desires(3) > Desires(2))
					Activity(labor);
				else
					Rest();
				break;
			case int t when (t >= 18 && t <= 21):
				if (Desires(1) > Desires(3) && Desires(1) > Desires(2))
					Activity(ChooseRecreation());
				else if (Desires(3) > Desires(2))
					Activity(labor);
				else
					Rest();
				break;
		}
	}

	private void Rest()
	{
		move.SetDestination(GameObject.Find(cell).transform.position);
		Stats(3, -25);
		Debug.Log(name + " is going to rest in " + cell);
	}

	private void Activity(string activity)
	{

		move.SetDestination(GameObject.Find(activity).transform.position);
		Stats(3, 25);
		Debug.Log(name + " is going to " + activity);
	}

	private float Desires(int desire)
	{
		/*Desire Chart
		 * 0 = Food
		 * 1 = Recreation
		 * 2 = Sleep/Rest
		 * 3 = Labor
		 */

		float total = 0;
		switch (desire)
		{
			case 0:
				//based on hunger, fatigue
				break;
			case 1:
				//based on fatigue, rest
				break;
			case 2:
				//based on fatigue, rest
				break;
			case 3:
				//based on fatigue
				break;
		}

		return total;
	}

	private string ChooseRecreation()
	{
		if (rec1Desire > rec2Desire && rec1Desire > rec3Desire)
			return rec1;
		else if (rec2Desire > rec3Desire)
			return rec2;
		else
			return rec3;
	}

	private void Stats(int hunger, int rest, int fatigue)
	{
		this.hunger += hunger;
		this.sleep += rest;
		this.fatigue += fatigue;

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
	}

	void Eyes()
	{
		RaycastHit hit;
		float distance = 15f;
		float angle = 78;
		float segments = angle - 1;
		Vector3 startPos = transform.position + (Vector3.up * 2);
		Vector3 targetPos;
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
