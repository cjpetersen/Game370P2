using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDescription : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public string GenerateDescription(int roomType)
	{
		if (roomType == 1)
			return "You walk into a cold cellar with wet floors that reek of a putrid stench";
		else if (roomType == 2)
			return "As you walk in you see a pentagram on the floor and the sense that something evil is lurking in this room.";
		else if (roomType == 3)
			return "You are now in a large living room that looks to be untouched for a very long time.";
		else if (roomType == 4)
			return "You are standing in the entrance to this grand haunted house. For many years this place has been untouched due to the mass amount of dissaperances and stories told." +
				"You must find the attic to unlock the mysteries that are lurking here.";
		else if (roomType == 5)
			return "You are now in a large corridor that gives you a feeling of uneasiness as you try and decide where to go next.";
		else if (roomType == 6)
			return "You walk into a kitchen that smells foul from the dishes and food that have been rotting for who knows how long.";
		else if (roomType == 7)
			return "You walk into a nice sitting room with a large amount of dust covering the furniture.";
		else if (roomType == 8)
			return "You walk out into a balcony. A cold breeze hits your face as you stare off into the dark shadows of the surounding land.";
		else if (roomType == 9)
			return "You are now in what looks to be a study. Lots of books and papers lay around. Who knows what secrets may be hidden here";
		else if (roomType == 10)
			return "As you walk into the attic you feel your body start to tense. This has been your destination the whole time but will the answers give you closure to the mystery or lead to your demise.";
		else if (roomType == 11)
			return "You walk into a very dark, tight corridor that gives you a sense of uneasiness.";
		else if (roomType == 12)
			return "You walk into a small bedroom. You wonder who this bedroom might have belonged to and if the person knew anything behind the haunting here.";
		else if (roomType == 13)
			return "You are now standing in a stairwell with stairs going up and down. Not knowing what lies ahead in either direction makes you a little nervous.";
		else if (roomType == -1)
			return "a door ";
		else
			return "This is a generic room.";
	}
}
