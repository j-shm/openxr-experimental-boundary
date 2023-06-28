using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndStartSave : MonoBehaviour
{
    public void Find()
    {
        GameObject.FindGameObjectWithTag("Corner").GetComponent<SaveObjects>().Save();
    }
}
