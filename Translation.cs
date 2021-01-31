using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class InputJson
{
    public int code;
    public string text;
}

public class Tr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string text;
    public Text translatedText;
    InputJson inputJson;

    //コンソールが指定のオブジェクトの上に入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(translatedText == null){
            translatedText = transform.root.transform.Find("Menu").Find("TranslatedText").GetComponent<Text>();
        }
        text = GetComponent<Text>().text;
        StartCoroutine (Translate (text));
        translatedText.enabled = true;
    }

    //コンソールが指定のオブジェクトの上からでた時
    public void OnPointerExit(PointerEventData eventData)
    {
        translatedText.text = "";
        translatedText.enabled = false;
    }

    public IEnumerator Translate(string t){
        //string url = $"https://script.google.com/macros/s/AKfycbzZtvOvf14TaMdRIYzocRcf3mktzGgXvlFvyczo/exec?text={t}&source=en&target=ja";
        string url = $"https://script.google.com/macros/s/AKfycbwgSD4RDUUQ6he_RBNWjB5QoSdwwOgJAJyhked-VHexWcF6I1A6/exec?text={t}&source=en&target=ja";
        var www = new WWW (url);
        yield return www;
        inputJson = JsonUtility.FromJson<InputJson>(www.text);
        translatedText.text = inputJson.text;
    }
}
