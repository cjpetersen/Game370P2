using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public int ID;
	public string description;
	public string roomName;
	public Coords coords;
	public string[] exitDescs = new string[4] { "", "", "", "" };
	//public bool[] exits = new bool[4] { false, false, false, false };
	public bool[] exits = new bool[4] { true, true, true, true };
	public List<GameObject> exitRooms = new List<GameObject> { null, null, null, null};
	public Dictionary<string, string> objects;
	public List<string> objectDisplay;
}