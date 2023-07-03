using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModifyOrDelete : MonoBehaviour
{
    private Text infoText;
    [SerializeField]
    private GameObject ModifyRay;
    [SerializeField]
    private GameObject DeleteRay;
    private void Start()
    {
        infoText = this.GetComponentInChildren<Text>();
        DeleteRay.SetActive(false);
        Change();
    }

    public void Change()
    {
        ModifyRay.SetActive(!ModifyRay.activeSelf);
        DeleteRay.SetActive(!DeleteRay.activeSelf);
        infoText.text = ModifyRay.activeSelf ? "In Modify Mode" : "In Delete Mode";
    }
}
