using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class <c>LoadObjects</c> is used for loading a json of objects into the scene.
/// </summary>
public class LoadObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject cornerToPlace;
    private GameObject corner;
    [SerializeField]
    private GameObject objectToPlace;
    [SerializeField]
    private Material occul;
    [SerializeField]
    private bool useOccul;
    /// <summary>
    /// This method is the entry for the <c>LoadObjects</c> class it calls 
    /// other methods that are needed to load and place objects
    /// <param name="save">the save file.</param>
    /// <param name="isFullFilePath">includes the directory or not.</param>
    /// <example>
    /// For example:
    /// <code>
    /// Load("C:/save.json",true)
    /// </code>
    /// Will load the file path C:/save.json
    /// </example>
    /// <returns>
    /// Bool of success
    /// </returns>
    /// </summary>
    public bool Load(string save = null, bool isFullFilePath = true)
    {
        if(save == null || save == "")
        {
            save = "roomscaleObjects.json";
            isFullFilePath = false;
        }
        string fullFilePath = CheckSaveExists(save, isFullFilePath);
        if (fullFilePath != "")
        {
            if(Import(fullFilePath))
            {
                return true;
            }
            Debug.LogError($"error importing! For: {save}");
            return false;
        }
        Debug.LogError($"no save found! For: {save}");
        return false;

    }
    [Obsolete("DO NOT USE!! USE Load()")]
    public void DoLoad()
    {
        Load(null);
    }
    
    public void DoLoadQuest() {
        Load("/sdcard/documents/boundarysaves/roomscaleObjects.json");
    }
    public delegate void OnLoad(GameObject corner);
    public OnLoad onLoad;
    private string CheckSaveExists(string saveName = null, bool isFullFilePath = false)
    {
        if (saveName == null)
        {
            saveName = "roomscaleObjects.json";
        }
        string savePath = "";
        if(!isFullFilePath)
        {
            savePath = $"{Application.persistentDataPath}/saves/";
        }
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
                        objPlaced.transform.eulerAngles = ArrayToVector(objRot);
                        objPlaced.GetComponent<ObjectData>().placed = true;
                        objPlaced.GetComponent<Collider>().enabled = true;
                        if(useOccul && occul != null)
                        {
                            objPlaced.GetComponent<Renderer>().material = occul;
                        }
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
