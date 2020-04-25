using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	[Header("Text Control")]
	public Text output;
	public List<string> actions;
	public List<string> logToOutput;

	[Header("Player")]
	public GameObject currentRoom;

	public Dictionary<string, string> objects;
	public List<GameObject> roomTypes;

	public List<GameObject> roomList;
	public GameObject[,,] map = new GameObject[10,10,3];

	void Start()
	{
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

		switch (direction)
		{
			case "north":
				nextRoom = currentRoom.GetComponent<Room>().exitRooms[0];
				break;
			case "south":
				nextRoom = currentRoom.GetComponent<Room>().exitRooms[1];
				break;
			case "east":
				nextRoom = currentRoom.GetComponent<Room>().exitRooms[2];
				break;
			case "west":
				nextRoom = currentRoom.GetComponent<Room>().exitRooms[3];
				break;
		}
		if (nextRoom != null && currentRoom.GetComponent<Room>().exitRooms.Contains(nextRoom))
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
				outputText += "There is " + currentRoom.GetComponent<Room>().exitDescs[i].ToLower();
		}

		AddToLog(outputText);
	}

	public void GenerateMap()
	{
		bool done = false, started = false;
		float[] chance = new float[1] { 100f };
		Room prevprevRoom = null, prevRoom = null, room;
		int id = 1, startX, startZ, endX, endZ;
		GameObject temp;

		#region Free Range Generation
		//temp = (GameObject)Instantiate(Resources.Load("room"));
		//temp.name = "Start Room";
		//roomList.Add(temp);
		//room = temp.GetComponent<Room>();
		//room.ID = id;
		//room.name = "start";
		//room.description = GenerateDescription(0);
		//currentRoom = temp;

		//while (!done) 
		//// for(int i = 0; i < 5; i++)
		//{
		//	float rand = Random.Range(1f, 100f);
		//	if (rand <= chance[0])
		//	{
		//		temp = (GameObject)Instantiate(Resources.Load("room"));
		//		temp.name = "Room" + id;
		//		roomList.Add(temp);
		//		if (prevRoom != null)
		//			prevprevRoom = prevRoom;
		//		prevRoom = room;
		//		room = temp.GetComponent<Room>();
		//		room.name = "room";
		//		chance[0] -= 1f;
		//	}
		//	else
		//	{
		//		done = true;
		//	}

		//	id++;
		//	room.ID = id;
		//	if (prevRoom != null)
		//		GenerateExit(prevprevRoom, prevRoom, room);
		//}

		//temp = (GameObject)Instantiate(Resources.Load("room"));
		//temp.name = "End Room";
		//roomList.Add(temp);
		//prevprevRoom = prevRoom;
		//prevRoom = room;
		//room = temp.GetComponent<Room>();
		//room.name = "end";
		//room.description = GenerateDescription(2);
		//room.ID = id++;
		//GenerateExit(prevprevRoom, prevRoom, room);
		#endregion
		#region Caged Generation
		//critical room generation
		startX = Random.Range(0, 5); startZ = Random.Range(0, 5);
		do { endX = Random.Range(0, 5); endZ = Random.Range(0, 5); } while(endX == startX && endZ == startZ);

		//filler room generation
		for(int i = 0; i < 5; i++)
		{
			for(int j = 0; j < 5; j++)
			{
				bool criticalRoom = false;
				if (i == startX && j == startZ)
					criticalRoom = true;
				else if (i == endX && j == endZ)
					criticalRoom = true;

				if (criticalRoom)
				{
					if(i == startX && j == startZ)
					{
						temp = (GameObject)Instantiate(Resources.Load("room"));
						temp.name = "Start Room";
						roomList.Add(temp);
						room = temp.GetComponent<Room>();
						room.ID = id;
						room.roomName = "start";
						room.description = GenerateDescription(0);
						currentRoom = temp;
					}
					else
					{
						//make end
					}
				}
				else
				{
					//make room

					//float rand = Random.Range(0f, 100f);
					//switch (rand)
					//{
					//	case float c when (c >= 0 && c <= chance[0]):
					//		break;
					//}
				}

				id++;
			}
		}
		#endregion
	}

	public void GenerateExit(Room prevprevRoom, Room prevRoom, Room room)
	{
		//5% chance to make an offshoot room
		if (Random.Range(0, 10) == 0)
		{
			prevRoom = prevprevRoom;
		}

		//Pick a direction to go that isn't taken yet
		int direction;
		do { direction = Random.Range(0, 4); } while (prevRoom.exitRooms[direction] != null);

		switch (direction)
		{
			case 0:
				prevRoom.exitRooms[0] = room.gameObject;
				prevRoom.exitDescs[0] = GenerateDescription(-1) + "to the north.";
				room.exitRooms[1] = prevRoom.gameObject;
				room.exitDescs[1] = GenerateDescription(-1) + "to the south.";
				break;
			case 1:
				prevRoom.exitRooms[1] = room.gameObject;
				prevRoom.exitDescs[1] = GenerateDescription(-1) + "to the south.";
				room.exitRooms[0] = prevRoom.gameObject;
				room.exitDescs[0] = GenerateDescription(-1) + "to the north.";
				break;
			case 2:
				prevRoom.exitRooms[2] = room.gameObject;
				prevRoom.exitDescs[2] = GenerateDescription(-1) + "to the east.";
				room.exitRooms[3] = prevRoom.gameObject;
				room.exitDescs[3] = GenerateDescription(-1) + "to the west.";
				break;
			case 3:
				prevRoom.exitRooms[3] = room.gameObject;
				prevRoom.exitDescs[3] = GenerateDescription(-1) + "to the west.";
				room.exitRooms[2] = prevRoom.gameObject;
				room.exitDescs[2] = GenerateDescription(-1) + "to the east.";
				break;
		}
	}

	public string GenerateDescription(int roomType)
	{
		if (roomType == 0)
			return "This is the start, go find the end.";
		else if (roomType == 2)
			return "This is it, you found the end.";
		else if (roomType == -1)
			return "It's a door ";
		else
			return "This is a generic room.";
	}
}