using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
	public int distanceX, distanceZ, floors;
	public GameObject[,,] map;
	public string[,,] visualMap;
	public List<Item> items;
	public List<Item> itemGen;

	[Header("Text Control")]
	public Text output;
	public List<string> actions;
	public List<string> logToOutput;

	[Header("Player")]
	public GameObject currentRoom;
	public List<Item> inventory;

	GameObject roomGen;

	void Start()
	{
		map = new GameObject[distanceX, distanceZ, floors];
		visualMap = new string[distanceX, distanceZ, floors];
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
		//Debug.Log(currentRoom.GetComponent<Room>().coords.x + " | " + currentRoom.GetComponent<Room>().coords.z + " | " + currentRoom.GetComponent<Room>().coords.f);
	}

	public void TakeAction(List<string> input)
	{
		bool found = false;

		switch (input[0])
		{
			case "go":
				AttemptToChangeRooms(input[1]);
				break;
			case "examine":
				found = false;
				if (input.Count == 1 || input[1] == "room")
					DisplayRoomText();
				else
				{
					for (int i = 0; i < items.Count; i++)
					{
						if (input[1] == items[i].name.ToLower())
						{
							AddToLog(items[i].description);
							i = items.Count;
							found = true;
						}
						else if (items[i].container && items[i].open)
						{
							for (int j = 0; j < items[i].inv.Count; j++)
							{
								if (items[i].inv[j].name.ToLower() == input[1])
								{
									found = true;
									AddToLog(items[i].inv[j].description);
								}
							}
						}
					}
					for (int i = 0; i < inventory.Count; i++)
					{
						if (input[1] == inventory[i].name.ToLower())
						{
							AddToLog(inventory[i].description);
							i = items.Count;
							found = true;
						}
					}

					if (!found)
						AddToLog("No such item exists.");
				}
				break;
			case "map":
				DisplayMap(currentRoom.GetComponent<Room>().coords.f);
				break;
			case "get":
				found = false;
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i].name.ToLower() == input[1])
					{
						found = true;
						if (items[i].moveable)
						{
							inventory.Add(items[i]);
							AddToLog(items[i].name + " aquired");
							items.RemoveAt(i);
						}
						else
							AddToLog("You can't move that.");
					}
					else if(items[i].container && items[i].open)
					{
						for(int j = 0; j < items[i].inv.Count; j++)
						{
							if(items[i].inv[j].name.ToLower() == input[1])
							{
								found = true;
								if (items[i].moveable)
								{
									inventory.Add(items[i]);
									AddToLog(items[i].name + " aquired");
									items.RemoveAt(i);
								}
								else
									AddToLog("You can't move that.");
							}
						}
					}
				}

				if (!found)
					AddToLog("There is no such item here.");
				break;
			case "drop":
				found = false;
				for (int i = 0; i < inventory.Count; i++)
				{
					if (inventory[i].name.ToLower() == input[1])
					{
						found = true;
						//items.Add(inventory[i]);
						currentRoom.GetComponent<Room>().items.Add(inventory[i]);
						AddToLog(inventory[i].name + " dropped");
						inventory.RemoveAt(i);
					}
				}

				if (!found)
					AddToLog("You don't have any such item");
					DisplayLoggedText();
				break;
			case "inv":
				string outputText = "Your inventory contains ";
				if (inventory.Count != 0)
				{
					for (int i = 0; i < inventory.Count; i++)
					{
						if (i != inventory.Count - 1 || inventory.Count == 1)
							outputText += inventory[i].name + ", ";
						else
							outputText += "and " + inventory[i].name + ".";
					}
				}
				else
					outputText += "nothing.";
				AddToLog(outputText);
				break;
			case "open":
				found = false;
				for(int i = 0; i < items.Count; i++)
				{
					if(items[i].name.ToLower() == input[1])
					{
						found = true;
						if (items[i].container)
						{
							if (!items[i].open)
							{
								items[i].open = true;
								AddToLog("You open the " + items[i].name.ToLower());
							}
							else
								AddToLog("That container is already open.");
						}
						else
							AddToLog("That isn't a container");
					}
				}
				
				if (!found)
					AddToLog("There is no such container here.");
				break;
			case "close":
				found = false;
				for(int i = 0; i < items.Count; i++)
				{
					if (items[i].name.ToLower() == input[1])
					{
						found = true;
						if (items[i].container)
						{
							if (items[i].open)
							{
								items[i].open = false;
								AddToLog("You close the " + items[i].name.ToLower());
							}
							else
								AddToLog("That container is already closed.");
						}
						else
							AddToLog("That isn't a container");
					}
				}

				if (!found)
					AddToLog("There is no such container here.");
				break;
			default:
				AddToLog("I don't understand that.");
				break;
		}

		DisplayLoggedText();
	}

	public void AttemptToChangeRooms(string direction)
	{
		GameObject nextRoom = null;
		Room room = currentRoom.GetComponent<Room>();
		switch (direction)
		{
			case "north":
				if (room.exits[0])
					nextRoom = map[room.coords.x + 1, room.coords.z, room.coords.f];
				break;
			case "south":
				if (room.exits[1])
					nextRoom = map[room.coords.x - 1, room.coords.z, room.coords.f];
				break;
			case "east":
				if (room.exits[2])
					nextRoom = map[room.coords.x, room.coords.z + 1, room.coords.f];
				break;
			case "west":
				if (room.exits[3])
					nextRoom = map[room.coords.x, room.coords.z - 1, room.coords.f];
				break;
			case "up":
				if (room.roomName == "stairs" && currentRoom.GetComponent<Room>().coords.f < floors)
					nextRoom = map[room.coords.x, room.coords.z, room.coords.f + 1];
				break;
			case "down":
				if (room.roomName == "stairs" && currentRoom.GetComponent<Room>().coords.f > 0)
					nextRoom = map[room.coords.x, room.coords.z, room.coords.f - 1];
				break;
		}
		if (nextRoom != null)
		{
			currentRoom = nextRoom;
			AddToLog("You head off to the " + direction);
			items = nextRoom.GetComponent<Room>().items;
			DisplayRoomText();
		}
		else
		{
			AddToLog("There is no path to the " + direction);
		}

	}

	public void DisplayRoomText()
	{
		string outputText = currentRoom.GetComponent<Room>().roomName.ToUpper() + "\n";
		outputText += currentRoom.GetComponent<Room>().description + "\n";

		if (items.Count > 0)
		{
			outputText += "The room contains ";
			for (int i = 0; i < items.Count; i++)
			{
				if (i != items.Count - 1 || items.Count == 1)
					outputText += items[i].name + ", ";
				else
					outputText += "and " + items[i].name + ".\n";
			}

			for (int i = 0; i < items.Count; i++)
			{
				if(items[i].container && items[i].open)
				{
					outputText += "There is a ";
					for(int j = 0; j < items[i].inv.Count; j++)
					{
						if (j != items[i].inv.Count || items[i].inv.Count == 1)
							outputText += items[i].inv[j].name + ", ";
						else
							outputText += "and " + items[i].inv[j].name;
					}
					outputText += " in the " + items[i].name + ".\n";
				}
			}
		}

		for (int i = 0; i < 4; i++)
		{
			if (currentRoom.GetComponent<Room>().exitDescs[i] != "")
				outputText += "There is " + currentRoom.GetComponent<Room>().exitDescs[i].ToLower() + "\n";
		}

		AddToLog(outputText);
	}

	public void DisplayMap(int floor)
	{
		string mapRow = "";
		for (int i = distanceX - 1; i >= 0; i--)
		{
			for (int j = 0; j < distanceZ; j++)
			{
				mapRow += visualMap[i, j, floor];
			}
			mapRow += "\n";
		}
		AddToLog(mapRow);
		DisplayLoggedText();
	}

	public void GenerateMap()
	{
		string floorType = "basement";
		float[] floorChance = new float[5] { 20f, 40f, 60f, 80f, 100f };
		Coords start = new Coords(), end = new Coords(), stairs = new Coords();
		int id = 1;

		//critical room locations
		start.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ), 1);
		end.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ), floors - 1);
		do { stairs.Set(Random.Range(0, distanceX), Random.Range(0, distanceZ)); } while (stairs.CompairIgnoreFloor(start) || stairs.CompairIgnoreFloor(end));

		//room generation
		for (int f = 0; f < floors; f++)
		{
			for (int x = 0; x < distanceX; x++)
			{
				for (int z = 0; z < distanceZ; z++)
				{
					Coords coords = new Coords(x, z, f);
					bool criticalRoom = false;
					if (start.Compair(x, z, f))
						criticalRoom = true;
					else if (end.Compair(x, z, f))
						criticalRoom = true;
					else if (stairs.Compair(x, z))
						criticalRoom = true;

					if (criticalRoom)
					{
						if (start.Compair(x, z, f))
						{
							RoomDetails(id, "entrance porch", coords, 4);
							visualMap[x, z, f] = "Ep ";
						} //start room
						else if (end.Compair(x, z, f))
						{
							RoomDetails(id, "attic", coords, 10);
							visualMap[x, z, f] = "At ";
						} //end room
						else if (stairs.Compair(x, z))
						{
							RoomDetails(id, "stairs", coords, 13);
							visualMap[x, z, f] = "Sr ";
						}
					} //make critical rooms
					else
					{
						float roll = Random.Range(0f, 100f);
						switch (floorType)
						{
							case "basement":
								if (roll <= floorChance[0])
								{
									RoomDetails(id, "cellar", coords, 1);
									visualMap[x, z, f] = "Cr ";
								}
								else if (roll > floorChance[0] && roll <= floorChance[1])
								{
									RoomDetails(id, "ritual room", coords, 2);
									visualMap[x, z, f] = "Rr ";
								}
								else if (roll > floorChance[1] && roll <= floorChance[2])
								{
									RoomDetails(id, "living room", coords, 3);
									visualMap[x, z, f] = "Lr ";
								}
								else if (roll > floorChance[2] && roll <= floorChance[3])
								{
									RoomDetails(id, "side corridor", coords, 11);
									visualMap[x, z, f] = "Sc ";
								}
								else if (roll > floorChance[3] && roll <= floorChance[4])
								{
									RoomDetails(id, "bedroom", coords, 12);
									visualMap[x, z, f] = "Br ";
								}
								break;
							case "main":
								if (roll <= floorChance[0])
								{
									RoomDetails(id, "living room", coords, 3);
									visualMap[x, z, f] = "Lr ";
								}
								else if (roll > floorChance[0] && roll <= floorChance[1])
								{
									RoomDetails(id, "main hall", coords, 5);
									visualMap[x, z, f] = "Mh ";
								}
								else if (roll > floorChance[1] && roll <= floorChance[2])
								{
									RoomDetails(id, "kitchen", coords, 6);
									visualMap[x, z, f] = "Kt ";
								}
								else if (roll > floorChance[2] && roll <= floorChance[3])
								{
									RoomDetails(id, "parlor", coords, 7);
									visualMap[x, z, f] = "Pl ";
								}
								else if (roll > floorChance[3] && roll <= floorChance[4])
								{
									RoomDetails(id, "side corridor", coords, 11);
									visualMap[x, z, f] = "Sc ";
								}
								else if (roll > floorChance[4] && roll <= floorChance[5])
								{
									RoomDetails(id, "bedroom", coords, 12);
									visualMap[x, z, f] = "Br ";
								}
								break;
							case "upper":
								if (roll <= floorChance[0])
								{
									RoomDetails(id, "parlor", coords, 7);
									visualMap[x, z, f] = "Pl ";
								}
								else if (roll > floorChance[0] && roll <= floorChance[1])
								{
									RoomDetails(id, "balcony", coords, 8);
									visualMap[x, z, f] = "Bl ";
								}
								else if (roll > floorChance[1] && roll <= floorChance[2])
								{
									RoomDetails(id, "study", coords, 9);
									visualMap[x, z, f] = "St ";
								}
								else if (roll > floorChance[2] && roll <= floorChance[3])
								{
									RoomDetails(id, "side corridor", coords, 11);
									visualMap[x, z, f] = "Sc ";
								}
								else if (roll > floorChance[3] && roll <= floorChance[4])
								{
									RoomDetails(id, "bedroom", coords, 12);
									visualMap[x, z, f] = "Br ";
								}
								break;
						}
					} //make filler rooms

					id++;
					map[x, z, f] = roomGen;
				}
			}

			if (floorType == "basement")
			{
				floorType = "main";
				floorChance = new float[6] { 20f, 40f, 60f, 80f, 90f, 100f };
			}
			else if (floorType == "main")
			{
				floorType = "upper";
				floorChance = new float[5] { 20f, 40f, 60f, 80f, 100f };
			}
		}
	}

	public void RoomDetails(int id, string name, Coords coords, int type)
	{
		roomGen = (GameObject)Instantiate(Resources.Load("room"));
		roomGen.name = name + " " + id;
		Room room = roomGen.GetComponent<Room>();
		room.ID = id;
		room.type = type;
		room.coords = coords;
		room.roomName = name;
		room.description = GenerateDescription(type);

		if (type == 4)
		{
			currentRoom = roomGen;
			items = room.items;
			room.items.Add(itemGen[0]);
			room.items[0].inv.Add(itemGen[18]);
		}
		GenerateItems(room, room.items);

		if (coords.x == distanceX - 1)
			room.exits[0] = false;
		else if (coords.x == 0)
			room.exits[1] = false;

		if (coords.z == distanceZ - 1)
			room.exits[2] = false;
		else if (coords.z == 0)
			room.exits[3] = false;

		for (int i = 0; i < 4; i++)
			if (room.exits[i])
				GenerateExit(room, i);
	}

	public void GenerateItems(Room room, List<Item> items)
	{
		int amount = Random.Range(0, 2);
		for (int i = 0; i < amount; i++)
		{
			int item = Random.Range(0, itemGen.Count);
			if (itemGen[item].allowedRooms.Contains(room.type))
				items.Add(itemGen[item]);
			else
				i--;

			if (items[i].container)
				GenerateItems(room, room.items[i].inv);
		}
	}

	public void GenerateExit(Room room, int direction)
	{
		switch (direction)
		{
			case 0:
				if (room.coords.x != distanceX - 1)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the north";
				break;
			case 1:
				if (room.coords.x != 0)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the south";
				break;
			case 2:
				if (room.coords.z != distanceZ - 1)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the east";
				break;
			case 3:
				if (room.coords.z != 0)
					room.exitDescs[direction] = GenerateDescription(-1) + "to the west";
				break;
		}
	}

	public string GenerateDescription(int roomType)
	{

		string[] randomAdj = new string[10]
		{
			"creaky",
			"creepy",
			"old",
			"broken",
			"wooden",
			"metal",
			"large",
			"demonic looking",
			"rotten",
			"red",
		};
		string randomArrayString = randomAdj[Random.Range(0, randomAdj.Length)];

		if (roomType == 1)
			return "You walk into a cold cellar with wet floors that reek of a putrid stench";
		else if (roomType == 2)
			return "As you walk in you see a pentagram on the floor and the sense that something evil is lurking in this room.";
		else if (roomType == 3)
			return "You are now in a large living room that looks to be untouched for a very long time.";
		else if (roomType == 4)
			return "You are standing in the entrance to this grand haunted house.\nFor many years this place has been untouched due to the mass amount of dissaperances and stories told.\n" +
				"You must find the attic to unlock the mysteries that are lurking here.";
		else if (roomType == 5)
			return "You are now in a large corridor that gives you a feeling of uneasiness as you try and decide where to go next.";
		else if (roomType == 6)
			return "You walk into a kitchen that smells foul from the dishes and food that have been rotting for who knows how long.";
		else if (roomType == 7)
			return "You walk into a nice sitting room with a large amount of dust covering the furniture.";
		else if (roomType == 8)
			return "You walk out into a balcony.\nA cold breeze hits your face as you stare off into the dark shadows of the surounding land.";
		else if (roomType == 9)
			return "You are now in what looks to be a study.\nLots of books and papers lay around.\nWho knows what secrets may be hidden here";
		else if (roomType == 10)
			return "As you walk into the attic you feel your body start to tense.\nThis has been your destination the whole time but will the answers give you closure to the mystery or lead to your demise.";
		else if (roomType == 11)
			return "You walk into a very dark, tight corridor that gives you a sense of uneasiness.";
		else if (roomType == 12)
			return "You walk into a small bedroom.\nYou wonder who this bedroom might have belonged to and if the person knew anything behind the haunting here.";
		else if (roomType == 13)
			return "You are now standing in a stairwell with stairs going up and down.\nNot knowing what lies ahead in either direction makes you a little nervous.";
		else if (roomType == -1)
			return "a " + randomArrayString + " door ";
		else
			return "This is a generic room.";
	}
}