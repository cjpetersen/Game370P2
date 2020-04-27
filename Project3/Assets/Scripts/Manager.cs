using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	public int distanceX, distanceZ, floors;
	public GameObject[,,] map;

	[Header("Text Control")]
	public Text output;
	public List<string> actions;
	public List<string> logToOutput;

	[Header("Player")]
	public GameObject currentRoom;
	public int currentFloor;

	public Dictionary<string, string> objects;
	public List<GameObject> roomTypes;

	void Start()
	{
		map = new GameObject[distanceX, distanceZ, floors];
		GenerateMap();
		DisplayRoomText();
		DisplayLoggedText();
	}

	public void AddToLog(string stringToAdd)
	{
		logToOutput.Add(stringToAdd + "\n");
	}

	public void DisplayLoggedText()
	{
		string logAsText = string.Join("\n", logToOutput.ToArray());
		output.text = logAsText;
		Debug.Log(currentRoom.GetComponent<Room>().locX + " | " + currentRoom.GetComponent<Room>().locZ);
	}

	public void TakeAction(List<string> input)
	{
		switch (input[0])
		{
			case "go":
				AttemptToChangeRooms(input[1]);
				break;
			case "examine":
				if (input.Count == 1 || input[1] == "room")
					DisplayRoomText();
				break;
			default:
				AddToLog("I don't understand that.");
				break;
		}
	}

	public void AttemptToChangeRooms(string direction)
	{
		GameObject nextRoom = null;
		Room room = currentRoom.GetComponent<Room>();
		switch (direction)
		{
			case "north":
				if (room.exits[0])
					nextRoom = map[room.locX + 1, room.locZ, room.floor];
				break;
			case "south":
				if (room.exits[1])
					nextRoom = map[room.locX - 1, room.locZ, room.floor];
				break;
			case "east":
				if (room.exits[2])
					nextRoom = map[room.locX, room.locZ + 1, room.floor];
				break;
			case "west":
				if (room.exits[3])
					nextRoom = map[room.locX, room.locZ - 1, room.floor];
				break;
			case "up":
				if(room.roomName == "stairs" && currentFloor < floors)
					nextRoom = map[room.locX, room.locZ, room.floor + 1];
				break;
			case "down":
				if (room.roomName == "stairs" && currentFloor > 1)
					nextRoom = map[room.locX, room.locZ, room.floor - 1];
				break;
		}
		if (nextRoom != null)
		{
			currentRoom = nextRoom;
			AddToLog("You head off to the " + direction);
			objects = nextRoom.GetComponent<Room>().objects;
			DisplayRoomText();
		}
		else
		{
			AddToLog("There is no path to the " + direction);
		}

	}

	public void DisplayRoomText()
	{
		string outputText = currentRoom.GetComponent<Room>().description + "\n";

		for(int i = 0; i < 4; i++)
		{
			if (currentRoom.GetComponent<Room>().exitDescs[i] != "")
				outputText += "There is " + currentRoom.GetComponent<Room>().exitDescs[i].ToLower() + "\n";
		}

		AddToLog(outputText);
	}

	public void GenerateMap()
	{
		float[] chance = new float[1] { 100f };
		Room room;
		Coords start = new Coords(), end = new Coords(), stairs = new Coords();
		int id = 1, endF;
		GameObject temp = null;

		//critical room locations
		start.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ));
		do { end.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ)); endF = Random.Range(0, floors); }
		while(endF != 0 || start.Compair(end));
		do { stairs.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ)); } while (stairs.Compair(start) || stairs.Compair(end));

		//room generation
		for (int f = 0; f < floors; f++)
		{
			for (int x = 0; x < distanceX; x++)
			{
				for (int z = 0; z < distanceZ; z++)
				{
					bool criticalRoom = false;
					if (start.Compair(x, z) && f == 0)
						criticalRoom = true;
					else if (end.Compair(x, z) && f == endF)
						criticalRoom = true;
					else if (stairs.Compair(x, z))
						criticalRoom = true;

					if (criticalRoom)
					{
						if (start.Compair(x, z) && f == 0)
						{
							temp = (GameObject)Instantiate(Resources.Load("room"));
							temp.name = "Start Room";
							room = temp.GetComponent<Room>();
							RoomDetails(room, id, "start", x, z, f, 0);
							currentRoom = temp;
						} //start room
						else if (end.Compair(x, z) && f == endF)
						{
							temp = (GameObject)Instantiate(Resources.Load("room"));
							temp.name = "End Room";
							room = temp.GetComponent<Room>();
							RoomDetails(room, id, "end", x, z, f, 1);
						} //end room
						else if (stairs.Compair(x, z))
						{
							temp = (GameObject)Instantiate(Resources.Load("room"));
							temp.name = "Stairwell";
							room = temp.GetComponent<Room>();
							RoomDetails(room, id, "stairs", x, z, f, 2);
						}
					} //make critical rooms
					else
					{
						float roll = Random.Range(0f, 100f);
						if (roll <= chance[0])
						{
							temp = (GameObject)Instantiate(Resources.Load("room"));
							temp.name = "Room" + id;
							room = temp.GetComponent<Room>();
							RoomDetails(room, id, "room", x, z, f, -5);
						}
					} //make filler rooms

					id++;
					map[x, z, f] = temp;
				}
			}
		}
	}

	public void RoomDetails(Room room, int id, string name, int x, int z, int f, int type)
	{
		room.ID = id;
		room.locX = x;
		room.locZ = z;
		room.floor = f;
		room.roomName = name;
		room.description = GenerateDescription(type);

		if (x == distanceX - 1)
			room.exits[0] = false;
		else if (x == 0)
			room.exits[1] = false;

		if (z == distanceZ - 1)
			room.exits[2] = false;
		else if (z == 0)
			room.exits[3] = false;

		for (int i = 0; i < 4; i++)
			if (room.exits[i])
				GenerateExit(room, i);
	}

	public void GenerateExit(Room room, int direction)
	{
		switch (direction)
		{
			case 0:
				if (room.locX != distanceX - 1)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the north";
				break;
			case 1:
				if (room.locX != 0)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the south";
				break;
			case 2:
				if (room.locZ != distanceZ - 1)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the east";
				break;
			case 3:
				if (room.locZ != 0)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the west";
				break;
		}
	}

	public string GenerateDescription(int roomType)
	{
		if (roomType == 0)
			return "This is the start, go find the end.";
		else if (roomType == 1)
			return "This is it, you found the end.";
		else if (roomType == 2)
			return "This is a stairwell.";
		else if (roomType == -1)
			return "It's a door ";
		else
			return "This is a generic room.";
	}
}