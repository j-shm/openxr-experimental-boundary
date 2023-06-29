using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{

    [Obsolete("This method is slow only use do not use on update!")]
    public void GetObjects()
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            objects.Add(transform.GetChild(i).gameObject);
        }
    }
    [Obsolete("This method is slow only use do not use on update!")]
    public void GetObjectsOfType(ObjectType.Object type)
    {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if(child.GetComponent<ObjectData>().objectType == type)
            {
                objects.Add(child);
            }
        }
    }

}
