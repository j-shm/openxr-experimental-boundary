using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UserInterfaceHandler : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private string[] headerText;
    [SerializeField]
    private string[] modalText;
    private int stage = 0;
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI headerTextComp;
    [SerializeField]
    private TextMeshProUGUI modalTextComp;
    [SerializeField]
    private TMP_Dropdown dropDown;
    [SerializeField]
    private GameObject[] buttons;
    private ObjectType.Object objType;

    private void Start()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach(ObjectType.Object obj in (ObjectType.Object[])ObjectType.Object.GetValues(typeof(ObjectType.Object)))
        {
            options.Add(new TMP_Dropdown.OptionData(obj.ToString()));
        }
        dropDown.options = options;
        UpdateType();
        ResetStage(0);
    }
    public void NextStage()
    {
        if(stage + 1 < headerText.Length)
        {
            stage++;
            SetText();
        }
    }
    public ObjectType.Object GetObjectType()
    {
        return objType;
    }
    public void UpdateType()
    {
        objType = (ObjectType.Object)Enum.Parse(typeof(ObjectType.Object), dropDown.options[dropDown.value].text);
    }
    public void ResetStage(int stage = 1)
    {
        this.stage = stage;
        SetText();
    }
    private void SetButtonStates()
    {
        if (stage == 1)
        {
            dropDown.gameObject.SetActive(true);
            foreach(GameObject button in buttons)
            {
                button.SetActive(true);
            }
        }
        else
        {
            dropDown.gameObject.SetActive(false);
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
        }
    }
    private void SetText()
    {
        headerTextComp.text = headerText[stage];
        modalTextComp.text = modalText[stage];
        SetButtonStates();
    }
}
