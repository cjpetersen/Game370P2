using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	[HideInInspector]
	public static Manager m { get; private set; }

	public List<string> recRooms;
	public List<string> labors;
	public List<GameObject> prisoners;
	public List<string> firstNames;
	public List<string> lastNames;

	[Header("Time Cycle")]
	public float hourLength;
	public float timer;
	public int hour;
	public int day;
	public int month;
	public int year;

	void Awake()
	{
		if (m == null)
		{
			m = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		//can be moved to prisoner start
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Prisoner");
		for (int i = 0; i < temp.Length; i++)
		{
			if(i < 9)
				temp[i].GetComponent<Prisoner>().cell = "Cell0" + (i + 1).ToString();
			else
				temp[i].GetComponent<Prisoner>().cell = "Cell" + (i + 1).ToString();
			temp[i].name = firstNames[Random.Range(0, firstNames.Count)];
			temp[i].name += " " + (char)(Random.Range(0, 26) + 65) + ". ";
			temp[i].name += lastNames[Random.Range(0, lastNames.Count)];
			prisoners.Add(temp[i]);
		}
	}

	void Update()
	{
		#region Time Cycle
		if (timer >= 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			timer = hourLength;
			hour++;
			foreach (GameObject prisoner in prisoners) //see aobut moving this to prisoner script
				prisoner.GetComponent<Prisoner>().checkSchedule = true;
			//Debug.Log("Checking Rooms");
			if (hour == 25)
			{
				hour -= 24;
				day++;
				if (day == 31)
				{
					day -= 30;
					month++;
					if (month == 13)
					{
						month -= 12;
						year++;
					}
				}
			}
		}
		#endregion
	}
}
