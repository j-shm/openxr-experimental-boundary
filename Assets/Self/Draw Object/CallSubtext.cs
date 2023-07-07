using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallSubtext : MonoBehaviour
{
    private TextMeshProUGUI subtext;
    private void Start()
    {
        subtext = this.GetComponent<TextMeshProUGUI>();
    }
    public void Call(string text)
    {
        StartCoroutine(CallHandler(text));
    }
    private IEnumerator CallHandler(string text)
    {
        subtext.text = text;
        yield return new WaitForSeconds(1f);
        subtext.text = "";
    }
}
