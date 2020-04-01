using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gaurd : MonoBehaviour
{
    public NavMeshAgent move;

    [Header("Stats")]
    public int hunger;
    public int sleep;
    public int fatigue;
    public int exhaustion;

    [Header("Personality")]
    public string recreation;
    public int prisonerTrust;

    [Header("Room Information")]
    public bool roomCheck = false;
    public string cell;
    public string currentRoom;

    void Start()
    {
        #region Randomized starting stats
        recreation = Manager.m.recRooms[Random.Range(0, Manager.m.recRooms.Count)];
        hunger = Random.Range(50, 101);
        sleep = Random.Range(50, 101);
        fatigue = Random.Range(0, 51);
        exhaustion = Random.Range(0, 51);
        prisonerTrust = Random.Range(0, 101);
        #endregion
    }

    void Update()
    {
        if (roomCheck)
            RoomCheck();
    }

    public void RoomCheck()
    {
        int time = Manager.m.hour;

        roomCheck = false;
        Stats(-5, -5, 0, 0);

        switch (time)
        {
            case int t when (t <= 6 || t >= 22):
                if (currentRoom != cell)
                    move.SetDestination(GameObject.Find(cell).transform.position);
                Stats(0, 10, -10, -10);
                break;
            case 7:
                break;
            case int t when (t == 8 || t == 12 || t == 17):
                if (hunger < 50)
                {
                    move.SetDestination(GameObject.Find("Cafeteria").transform.position);
                    Stats(1, 50);
                    Debug.Log(name + " is going to go eat.");
                }
                else if (fatigue <= 50)
                {
                    move.SetDestination(GameObject.Find(recreation).transform.position);
                    Stats(3, 25);
                    Debug.Log(name + " is going to the " + recreation);
                }
                else
                {
                    move.SetDestination(GameObject.Find(cell).transform.position);
                    Stats(3, -25);
                    Debug.Log(name + " is going to rest in " + cell);
                }
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            case 16:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
            case 21:
                break;
        }
    }

    private void Desires(int desire)
    {
        /*Desire Chart
		 * 0 = 
		 * 1 = 
		 * 2 = 
		 * 3 = 
		 * 4 = 
		 * 5 = 
		 * 6 = 
		 * 7 = 
		 * 8 = 
		 * 9 = 
		 */
        switch (desire)
        {
            case 0:
                //code here
                break;
            case 1:
                //code here
                break;
            case 2:
                //code here
                break;
            case 3:
                //code here
                break;
            case 4:
                //code here
                break;
            case 5:
                //code here
                break;
            case 6:
                //code here
                break;
            case 7:
                //code here
                break;
            case 8:
                //code here
                break;
            case 9:
                //code here
                break;
        }
    }

    private void Stats(int hunger, int rest, int fatigue, int exhaustion)
    {
        this.hunger += hunger;
        this.sleep += rest;
        this.fatigue += fatigue;
        this.exhaustion += exhaustion;
    }

    private void Stats(int stat, int magnitude)
    {
        switch (stat)
        {
            case 1:
                hunger += magnitude;
                break;
            case 2:
                sleep += magnitude;
                break;
            case 3:
                fatigue += magnitude;
                break;
            case 4:
                exhaustion += magnitude;
                break;
        }
    }

    void Eyes()
    {
        void Eyes()
        {
            RaycastHit hit;
            float distance = 15f;
            float angle = 78;
            float segments = angle - 1;
            Vector3 startPos = transform.position + (Vector3.up * 2);
            Vector3 targetPos = new Vector3();
            float startAngle = -angle * .5f;
            float finishAngle = angle * .5f;
            float increment = angle / segments;

            for (float i = startAngle; i < finishAngle; i += increment)
            {
                targetPos = (Quaternion.Euler(0, i, 0) * transform.forward).normalized * distance;

                if (Physics.Raycast(startPos, targetPos, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "Guard")
                    {
                        //Do something to make them move away
                        Debug.Log("REEEE");
                    }
                }
                Debug.DrawRay(startPos, targetPos, Color.red);
            }
        }
    }
}
