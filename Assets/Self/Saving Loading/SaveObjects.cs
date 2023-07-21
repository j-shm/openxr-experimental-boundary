using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveObjects : MonoBehaviour
{
    public void Save()
    {
        Debug.Log("saving....");
        //get all the objects attached to this corner and serialise them for saving.
        List<SerialObject> objects = new List<SerialObject>();
        foreach(Transform child in this.gameObject.transform)
        {
            objects.Add(ObjectToSerialObject(child.gameObject));
        }
        //add them to the object that holds them all
        SerialObjects objectList = new SerialObjects(objects.ToArray(), VectorToArray(transform.position));

        string serialisedObjectList = JsonConvert.SerializeObject(objectList);
        string savePath = GetSavePath();
        if (savePath != "")
        {
            File.WriteAllText(GetSavePath(), serialisedObjectList);
        }
    }

    private string GetSavePath(string saveName = null,bool overwrite = true)
    {
        if(saveName == null)
        {
            saveName = "roomscaleObjects.json";
        }
        string savePath = "/sdcard/documents/boundarysaves/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (File.Exists(savePath + saveName))
        {
            if(!overwrite)
            {
                return "";
            }
            File.Delete(savePath + saveName);
        }
        return savePath + saveName;
    }
    private SerialObject ObjectToSerialObject(GameObject obj)
    {
        SerialObject serialObj = new SerialObject(
            VectorToArray(obj.transform.localPosition), //in relation to corner
            VectorToArray(obj.transform.rotation.eulerAngles),
            VectorToArray(obj.transform.localScale)
            );

        return serialObj;
    }

    //you can't serialise a vector3 so conversion
    private float[] VectorToArray(Vector3 vec)
    {
        float[] vecArray = { vec.x, vec.y, vec.z };
        return vecArray;
    }

}
