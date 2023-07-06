using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>ObjectHolder</c> is a class for easily fetching objects attached to this corner.
/// </summary>
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
    /// <summary>
    /// This method returns a list of all objects
    /// </summary>
    /// <returns>
    /// List of objects
    /// </returns>
    public List<GameObject> GetObjects()
    {
        return objects;
    }
    /// <summary>
    /// This method returns a list of type specificed in paramater
    /// </summary>
    /// <param name="type">type of objects to return</param>
    /// <returns>
    /// List of objects of type specificed in paramater
    /// </returns>
    public List<GameObject> GetObjects(ObjectType.Object type)
    {
        return GetObjectsOfType(type);
    }
    /// <summary>
    /// This method returns a list of type specificed in paramater
    /// </summary>
    /// <param name="type">type of objects to return</param>
    /// <returns>
    /// List of objects of type specificed in paramater
    /// </returns>
    public List<GameObject> GetObjectsOfType(ObjectType.Object type)
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
