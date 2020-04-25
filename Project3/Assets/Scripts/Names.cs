using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Names : MonoBehaviour
{
	[Header("Basic Generator")]
	public List<string> firstNames;
	public List<string> lastNames;

	[Header("Advanced Generator")]
	public bool male = false;
	public string firstName;
	public string lastName;

	[Header("Timer")]
	public float timerLength;
	public float timer;

	void Start()
	{
		//Gender
		if (Random.Range(0, 2) == 1)
			male = true;

		//Generate the given name
		switch (Random.Range(0, 4))
		{
			case 0:
				firstName = "A";
				break;
			case 1:
				firstName = "E";
				break;
			case 2:
				firstName = "I";
				break;
			case 3:
				firstName = "O";
				break;
		}

		//Generate the surname
		switch (Random.Range(0, 4))
		{
			case 0:
				lastName = "A";
				break;
			case 1:
				lastName = "E";
				break;
			case 2:
				lastName = "I";
				break;
			case 3:
				lastName = "O";
				break;
		}

		if (male)
			lastName += "sson";
		else
			lastName += "sdottir";
	}

	// Update is called once per frame
	void Update()
	{
		if(timer <= 0)
		{
			char middleInitial = (char)(Random.Range(0, 26) + 65);
			Debug.Log(firstNames[Random.Range(0, firstNames.Count)] + " " + middleInitial + ". " + lastNames[Random.Range(0, lastNames.Count)]);
			timer = timerLength;
		}
	}
}
