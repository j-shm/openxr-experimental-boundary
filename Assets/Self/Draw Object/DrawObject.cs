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
    private void Resize(GameObject obj,float amount, Vector3 direction)
    {
        obj.transform.position += direction * amount / 2;
        obj.transform.localScale += direction * amount; 
    }

    //these two functions are heavily taxing: destroying objects constantly is very bad!
    //TODO: refactor these two functions to be more efficient 
    public void FirstResize(GameObject placedObject,float heightAmount,GameObject pivotPoint) {

        var tempPoint = new GameObject("temp pivot point"); 
        Resize(placedObject, heightAmount, Vector3.up);
        placedObject.transform.LookAt(pivotPoint.transform); 
        placedObject.transform.eulerAngles = Vector3.Scale(placedObject.transform.eulerAngles, Vector3.up);
        tempPoint.transform.position = placedObject.transform.position;
        tempPoint.transform.rotation = placedObject.transform.rotation;
        pivotPoint.transform.SetParent(tempPoint.transform,true);
        var amt =  pivotPoint.transform.localPosition.z;
        pivotPoint.transform.SetParent(null,true);
        placedObject.transform.Translate(Vector3.forward*amt/2);
        placedObject.transform.localScale += Vector3.forward*amt;
        Destroy(tempPoint);
    }
    public void SecondResize(GameObject placedObject,GameObject addPoint) {
        var tempPoint = new GameObject("temp pivot point"); 
        tempPoint.transform.position = placedObject.transform.position;
        tempPoint.transform.rotation = placedObject.transform.rotation;
        addPoint.transform.SetParent(tempPoint.transform,true);
        var amt =  addPoint.transform.localPosition.x;
        addPoint.transform.SetParent(null,true);
        placedObject.transform.Translate(Vector3.right*amt/2);
        placedObject.transform.localScale += Vector3.right*amt;
        Destroy(tempPoint);
    }
    public void ModifyResize(GameObject placedObject,GameObject pivotPoint, Vector3 dir) {
        dir = GetAbsoluteDirection(dir);
        /*Debug.Log(dir);
        GameObject pivot = new GameObject("Pivot Offset");
        pivot.transform.position = placedObject.transform.position;
        pivot.transform.rotation = placedObject.transform.rotation;
        pivotPoint.transform.SetParent(pivot.transform, true);
        var amt =  (FloatFromVector(pivotPoint.transform.localPosition,dir));
        pivotPoint.transform.SetParent(null, true);
        if(Mathf.Abs(amt) < 0.01) {
            return;
        }
        Debug.Log(amt + " " + dir);
        placedObject.transform.Translate(dir*amt/2);
        placedObject.transform.localScale = MakeVectorValueZero(placedObject.transform.localScale,dir) + dir*amt;
        Destroy(pivot);*/

        var tempPoint = new GameObject("temp pivot point"); 
        tempPoint.transform.position = placedObject.transform.position;
        tempPoint.transform.rotation = placedObject.transform.rotation;
        pivotPoint.transform.SetParent(tempPoint.transform,true);
        var amt =  pivotPoint.transform.localPosition.x;
        pivotPoint.transform.SetParent(null,true);
        placedObject.transform.Translate(dir*amt/2);
        placedObject.transform.localScale += dir*amt;
        Destroy(tempPoint);
    }
    private Vector3 GetAbsoluteDirection(Vector3 direction)
    {
        return new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
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
    private Vector3 MakeVectorValueZero(Vector3 vector, Vector3 dir) {
        if(dir.x != 0) {
            return new Vector3(0, vector.y,vector.z);
        } else if(dir.y != 0) {
            return new Vector3(vector.x, 0, vector.z);
        } else {
            return new Vector3(vector.x, vector.y, 0);
        }
    }
}
