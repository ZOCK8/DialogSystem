using System;
using System.Collections.Generic;
using UnityEngine;

public class translation : MonoBehaviour
{
    public List<String> LanguagePaths;
    public TranslationData translationData;
    private string CurrentDialogPath;
    void Start()
    {
        for (int i = 0; i < LanguagePaths.Count; i++)
        {
            string[] CurrentDialogPaths  = LanguagePaths[i].Split(':');
            if (CurrentDialogPaths[0] == translationData.CurrentLanguage)
            {
                CurrentDialogPath = CurrentDialogPaths[1];
            }
        }

        TextAsset LangugeJSON = Resources.Load<TextAsset>(CurrentDialogPath);
        Data daten = JsonUtility.FromJson<Data>(LangugeJSON.text);
        Debug.Log(daten.Dialog_Tutorial_Welcome);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
[CreateAssetMenu(fileName = "TranslationData", menuName = "Save/TranslationData")]
public class TranslationData : ScriptableObject
{
    public string CurrentLanguage;    
}
[System.Serializable]
public class Data
{
    public string Dialog_Tutorial_Welcome;
}
