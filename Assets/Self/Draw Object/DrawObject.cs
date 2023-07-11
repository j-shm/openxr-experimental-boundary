using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class DrawObject : MonoBehaviour
{
    [SerializeField]
    private Material occul;
    private void Resize(GameObject obj,float amount, Vector3 direction)
    {
        obj.transform.position += direction * amount / 2;
        obj.transform.localScale += direction * amount; 
    }

    private void ResetResize(GameObject obj)
    {
        Resize(obj, obj.transform.localScale.x, Vector3.left);
        Resize(obj, obj.transform.localScale.z, Vector3.back);
        Resize(obj, obj.transform.localScale.y, Vector3.down);
    }
    public void ResizeObject(GameObject obj, float amount, Vector3 direction)
    {
        Resize(obj, amount, direction);
    }
    // Deprecated API.
    public void ResizeObject(GameObject placedObject, Vector3 spawnPoint, Vector3 maxPoint, Vector3 addPoint)
    {
        //have to reset sizes before placing objects: if you dont then it'll be incorrect!
        ResetResize(placedObject);

        Resize(placedObject, maxPoint.y - spawnPoint.y, Vector3.up);
        
        float xDiff = placedObject.transform.position.x - addPoint.x;
        float zDiff = placedObject.transform.position.z - addPoint.z;
        Resize(placedObject, xDiff, Vector3.left);
        Resize(placedObject, zDiff, Vector3.back);
    }
    
    public void ResizeObject(GameObject placedObject, Vector3 spawnPoint, Vector3 maxPoint,Vector3 pivotPoint , Vector3 addPoint)
    {
        //have to reset sizes before placing objects: if you dont then it'll be incorrect!
        ResetResize(placedObject);
        
        Resize(placedObject, maxPoint.y - spawnPoint.y, Vector3.up);
        
        transform.rotation = Quaternion.LookRotation (pivotPoint - transform.position);
        transform.eulerAngles = Vector3.Scale(transform.eulerAngles, Vector3.forward + Vector3.right);
        float xDiff = placedObject.transform.position.x - addPoint.x;
        float zDiff = placedObject.transform.position.z - pivotPoint.z;
        Resize(placedObject, xDiff, Vector3.left);
        Resize(placedObject, zDiff, Vector3.back);
    }
    
}
