using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private List<GameObject> objects = new List<GameObject>();
    private void OnTransformChildrenChanged()
    {
        objects = GetObjectsFromChildren();
    }
    private List<GameObject> GetObjectsFromChildren()
    {
        List<GameObject> transformChildren = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++) {
            transformChildren.Add(transform.GetChild(i).gameObject);
        }
        return transformChildren;
    }
    private List<GameObject> GetObjects()
    {
        return objects;
    }

    public void GetObjectsOfType(ObjectType.Object type)
    {
        List<GameObject> objectsOfType = new List<GameObject>();
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i].GetComponent<ObjectData>().objectType == type)
            {
                objectsOfType.Add(objects[i]);
            }
        }
        return objectsOfType;
    }

}
