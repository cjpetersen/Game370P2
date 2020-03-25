using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	[HideInInspector]
	public static Manager m { get; private set; }

	public List<string> recRooms;
	public List<GameObject> rooms;
	public List<GameObject> prisoners;
	public List<string> names;

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
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Prisoner");
		for (int i = 0; i < temp.Length; i++)
		{
			temp[i].GetComponent<Prisoner>().cell = "Cell " + (i + 1).ToString();
			temp[i].name = names[Random.Range(0, names.Count)];
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
			foreach (GameObject prisoner in prisoners)
				prisoner.GetComponent<Prisoner>().roomCheck = true;
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
