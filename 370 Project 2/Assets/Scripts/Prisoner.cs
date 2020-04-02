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
	public string[] recreation = new string[3];
	public string labor;
	public int guardTrust;

	[Header("Room Information")]
	public bool checkSchedule = false;
	public string cell;
	public string currentRoom;

	void Start()
	{
		#region Randomized starting stats
		recreation[0] = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)];
		do { recreation[1] = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (recreation[1] == recreation[0]);
		do { recreation[2] = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)]; } while (recreation[2] == recreation[0] || recreation[2] == recreation[1]);
		labor = Manager.m.labors[Random.Range(0, Manager.m.labors.Count)];
		hunger = Random.Range(50, 101);
		sleep = Random.Range(50, 101);
		fatigue = Random.Range(0, 51);
		guardTrust = Random.Range(0, 101);
		#endregion
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
					Activity(recreation[Random.Range(0, recreation.Length)]);
				else
					Rest();
				break;
			case int t when (t >= 8 && t <= 11):
				if (Desires(1) > Desires(2))
					Activity(recreation[Random.Range(0, recreation.Length)]);
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
					Activity(recreation[Random.Range(0, recreation.Length)]);
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
				total = Mathf.Exp(5 * ((hunger / 100) - (.25f * (fatigue / 100))) - 5);
				break;
			case 1:
				total = 0 - ((((sleep / 100) - (fatigue / 100)) - 1) ^ 2) + 1;
				break;
			case 2:
				total = 0 - ((.5f * Mathf.Atan(50 * (((sleep / 100) - (fatigue / 100)) - .25f))) / (Mathf.PI / 2)) + .5f;
				break;
			case 3:
				total = (((fatigue / 100) - 1) ^ 2) + .5f;
				break;
		}

		return total;
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
}
