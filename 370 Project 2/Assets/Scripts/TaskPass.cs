using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPass : MonoBehaviour
{
    public string task;
    
    void OnTriggerEnter(Collider obj)
    {
        if (obj.GetComponent(task.GetType()) == null)
            obj.gameObject.AddComponent(task.GetType());
        else
            Destroy(obj.gameObject.GetComponent(task.GetType()));
    }
}
