using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class LoadObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject cornerToPlace;
    private GameObject corner;
    [SerializeField]
    private GameObject objectToPlace;
    public void Load(string save = null)
    {
        if(save == null)
        {
            save = "roomscaleObjects.json";
        }
        string fullFilePath = CheckSaveExists(save);
        if (fullFilePath != "")
        {
            if(!Import(fullFilePath))
            {
                Debug.Log("loading failed :(");
            }
            return;
        }
        Debug.LogError($"no save found! For: {save}");

    }
    public delegate void OnLoad(GameObject corner);
    public OnLoad onLoad;
    private string CheckSaveExists(string saveName = null)
    {
        if (saveName == null)
        {
            saveName = "roomscaleObjects.json";
        }
        string savePath = Application.persistentDataPath + "/saves/";
        if (File.Exists(savePath + saveName))
        {
            return savePath + saveName;
        }
        return "";
    }
    private bool Import(string file)
    {
        if (System.IO.File.Exists(file))
        {
            try
            {

                //get all the objects in the file
                JObject o1 = JObject.Parse(File.ReadAllText(file));
                
                float[] cornerPos = ((JArray)o1["position"]).ToObject<float[]>();
                corner = Instantiate(cornerToPlace,ArrayToVector(cornerPos),Quaternion.identity);

                var objs = o1["objects"];

                onLoad?.Invoke(corner);
                //import each object
                foreach (var obj in objs)
                {
                    float[] objPos = ((JArray)obj["position"]).ToObject<float[]>();
                    float[] objRot = ((JArray)obj["rotation"]).ToObject<float[]>();
                    float[] objScale = ((JArray)obj["scale"]).ToObject<float[]>();
                    if (objPos != null && objRot != null && objScale != null)
                    {
                        GameObject objPlaced = Instantiate(objectToPlace, corner.transform);
                        objPlaced.transform.localScale = ArrayToVector(objScale);
                        objPlaced.transform.localPosition = ArrayToVector(objPos);
                        objPlaced.GetComponent<ObjectData>().placed = true;
                        objPlaced.GetComponent<Collider>().enabled = true;
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return false;
    }
    private Vector3 ArrayToVector(float[] vecArray)
    {
        return new Vector3(vecArray[0], vecArray[1], vecArray[2]);
    }
}
