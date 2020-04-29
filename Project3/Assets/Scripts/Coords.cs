using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Coords
{
	public int x, z, f = 0;

	public bool Compair(Coords c)
	{
		if (x == c.x && z == c.z && f == c.f)
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
	public bool Compair(int x, int z, int f)
	{
		if (x == this.x && z == this.z && f == this.f)
			return true;
		else
			return false;
	}

	public bool CompairIgnoreFloor(Coords c)
	{
		if (x == c.x && z == c.z)
			return true;
		else
			return false;
	}

	public void Set(int x, int z)
	{
		this.x = x;
		this.z = z;
	}
	public void Set(int x, int z, int f)
	{
		this.x = x;
		this.z = z;
		this.f = f;
	}
}
