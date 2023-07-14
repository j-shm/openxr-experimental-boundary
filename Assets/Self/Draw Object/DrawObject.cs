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
}
