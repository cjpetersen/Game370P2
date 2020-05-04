using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
	public string name;
	public string description;
	public List<int> allowedRooms;
	public bool moveable;
	public bool container;
	public bool open;
	public List<Item> inv;

	public Item() { }
	public Item(string name, string desc)
	{
		this.name = name;
		description = desc;
	}
}
