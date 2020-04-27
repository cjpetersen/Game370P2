using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coords
{
	int x, z;

	public bool Compair(Coords c)
	{
		if (x == c.x && z == c.z)
			return true;
		else
			return false;
	}
	public bool Compair(int x, int z)
	{
		if (x == this.x && z == this.z)
			return true;
		else
			return false;
	}

	public void Set(int x, int z)
	{
		this.x = x;
		this.z = z;
	}
}
