using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;


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
        var amt =  addPoint.transform.localPosition.x - placedObject.GetComponent<Collider>().bounds.extents.x;
        addPoint.transform.SetParent(null,true);
        placedObject.transform.Translate(Vector3.right*amt/2);
        placedObject.transform.localScale += Vector3.right*amt;
        Destroy(tempPoint);
    }
    public void ModifyResize(GameObject placedObject,GameObject pivotPoint, Vector3 dir) {
        //dir = GetAbsoluteDirection(dir);
        
        var tempPoint = new GameObject("temp pivot point"); 
        tempPoint.transform.position = placedObject.transform.position;
        tempPoint.transform.rotation = placedObject.transform.rotation;
        pivotPoint.transform.SetParent(tempPoint.transform,true);
        var amt =  Mathf.Abs(FloatFromVector(pivotPoint.transform.localPosition ,dir)) - FloatFromVector(placedObject.GetComponent<Collider>().bounds.extents,dir);
        
        //prevent the scale from switching
        if (Mathf.Sign(FloatFromVector(pivotPoint.transform.localPosition, dir)) 
            != Mathf.Sign(FloatFromVector(dir,dir)) 
            || (Mathf.Abs(FloatFromVector(placedObject.transform.localScale,dir)) < 0.01 && Mathf.Sign(amt) == -1f)
           )
        {
            pivotPoint.transform.SetParent(null,true);
            Destroy(tempPoint);
            return;
        }
        

        Debug.Log(FloatFromVector(pivotPoint.transform.localPosition, dir));
        pivotPoint.transform.SetParent(null,true);
        
        Vector3 amtToTranslate = dir*amt/2;
        Vector3 amtToScale = dir*amt;
        if (Mathf.Sign(FloatFromVector(dir, dir)) == -1)
        {
            if (Mathf.Sign(amt) != -1)
            {
                amtToTranslate = dir*amt/2;
                amtToScale = GetAbsoluteDirection(dir*amt);
            }
            else
            {
                amtToTranslate = GetAbsoluteDirection(dir*amt/2);
                amtToScale = dir*Mathf.Abs(amt); 
            }
        }

        placedObject.transform.Translate(amtToTranslate);
        placedObject.transform.localScale += amtToScale;


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
