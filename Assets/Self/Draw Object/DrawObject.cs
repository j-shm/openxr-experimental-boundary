using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawObject : MonoBehaviour
{
    [SerializeField]
    private Material occul;
    private void Resize(GameObject obj,float amount, Vector3 direction)
    {
        obj.transform.position += direction * amount / 2;
        obj.transform.localScale += direction * amount; 
    }
    public void ResizeObject(GameObject obj, float amount, Vector3 direction)
    {
        var parent = obj.transform.parent;
        obj.transform.SetParent(null, true);
        if(direction == Vector3.up || direction == Vector3.down)
        {
            Resize(obj, obj.transform.localScale.y, Vector3.down);
        } else if(direction == Vector3.left || direction == Vector3.right)
        {
            Resize(obj, obj.transform.localScale.x, Vector3.left);
        } else
        {
            Resize(obj, obj.transform.localScale.z, Vector3.back);
        }
        Resize(obj, amount, direction);
        obj.transform.SetParent(parent, true);
    }
    public void ResizeObject(GameObject placedObject, Vector3 spawnPoint, Vector3 maxPoint, Vector3 addPoint)
    {
        //have to reset sizes before placing objects: if you dont then it'll be incorrect!
        Resize(placedObject, placedObject.transform.localScale.x, Vector3.left);
        Resize(placedObject, placedObject.transform.localScale.z, Vector3.back);
        Resize(placedObject, placedObject.transform.localScale.y, Vector3.down);

        Resize(placedObject, maxPoint.y - spawnPoint.y, Vector3.up);
        float xDiff = placedObject.transform.position.x - addPoint.x;
        float zDiff = placedObject.transform.position.z - addPoint.z;
        Resize(placedObject, xDiff, Vector3.left);
        Resize(placedObject, zDiff, Vector3.back);
    }
}
