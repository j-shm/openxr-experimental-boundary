using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveObjects : MonoBehaviour
{
    public void Save()
    {
        List<SerialObject> objects = new List<SerialObject>();
        foreach(Transform child in this.gameObject.transform)
        {
            objects.Add(ObjectToSerialObject(child.gameObject));
        }
        SerialObjects objectList = new SerialObjects(objects.ToArray(), VectorToArray(transform.position));
        string serialisedObjectList = JsonConvert.SerializeObject(objectList);
        string savePath = GetSavePath();
        if (savePath != "")
        {
            File.WriteAllText(GetSavePath(), serialisedObjectList);
        }
    }

    private string GetSavePath(string saveName = null)
    {
        saveName = "roomscaleObjects.json";
        string savePath = Application.persistentDataPath + "/saves/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (File.Exists(savePath + saveName))
        {
            File.Delete(savePath + saveName);
        }
        return savePath + saveName;
    }
    private SerialObject ObjectToSerialObject(GameObject obj)
    {
        SerialObject serialObj = new SerialObject(
            VectorToArray(obj.transform.localPosition),
            VectorToArray(obj.transform.rotation.eulerAngles),
            VectorToArray(obj.transform.localScale)
            );

        return serialObj;
    }

    private float[] VectorToArray(Vector3 vec)
    {
        float[] vecArray = { vec.x, vec.y, vec.z };
        return vecArray;
    }

}
