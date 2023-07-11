using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class DrawObject : MonoBehaviour
{
    [SerializeField]
    private Material occul;

    private Vector3 fowardRight = new Vector3(1,0,1);
    private void Resize(GameObject obj,float amount, Vector3 direction,bool localPos = false)
    {
        if (localPos)
        {
            obj.transform.localPosition += direction * amount / 2;
        }
        else
        {
            obj.transform.position += direction * amount / 2;
        }
        
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
        float xDiff = placedObject.transform.position.x - addPoint.x;
        float zDiff = placedObject.transform.position.z - pivotPoint.z;
        Resize(placedObject, xDiff, Vector3.left);
        Resize(placedObject, zDiff, Vector3.back);
    }

    public void ResizeFinalSideOfObject(GameObject placedObject,Vector3 point,Vector3 dir)
    {
        dir = -dir;
        Resize(placedObject, placedObject.transform.localScale.x, dir,true);
        float dirDiff = FloatFromVector(placedObject.transform.position,dir) - FloatFromVector(point,dir);
        Resize(placedObject, dirDiff, dir,true);

    }
    public void ResizeToPivotAndHeightOfObject(GameObject placedObject,Vector3 spawnPoint,Vector3 heightPoint,Vector3 pivotPoint)
    {
        Resize(placedObject, heightPoint.y - spawnPoint.y, Vector3.up);
        
        Resize(placedObject, placedObject.transform.position.z - pivotPoint.z, Vector3.back);
    }
    private float FloatFromVector(Vector3 point, Vector3 direction)
    {
        Vector3 multipliedPoint = Vector3.Scale(point , direction);
        if(multipliedPoint.x != 0)
        {
            return point.x;
        }
        if (multipliedPoint.y != 0)
        {
            return point.y;
        }
        if (multipliedPoint.z != 0)
        {
            return point.z;
        }
        return 0;
    }
}
