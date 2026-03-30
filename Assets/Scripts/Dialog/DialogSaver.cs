using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogSaver
{
    private static DialogSaver _instance;
    private AllNodes allNodes = new AllNodes();
    public MainFunction mainFunction;
    public DialogueGraphView dialogueGraphView;
    public VisualFunctions visualFunctions = new VisualFunctions();
    public static DialogSaver instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = new DialogSaver();
            }
            return _instance;
        }
    }
    public List<Nodes> nodes = new List<Nodes>();



    public void SaveGraph()
    {
        allNodes = new AllNodes();
        Debug.Log(nodes.Count);

        List<Node> NodeList = new List<Node>();
        foreach (Nodes x in nodes)
        {
            NodeList.Add(x.node);
        }
        allNodes.dialogNodeDatas = mainFunction.NodeToDialogNodeData(NodeList);
        WriteData(allNodes, "Assets/Resources/DialogNodes.dat", true);
    }

    public AllNodes LoadData(string FilePath)
    {
        dialogueGraphView.ClearAll();
        nodes.Clear();
        allNodes = new AllNodes();

        string FullPath = FilePath;
        if (File.Exists(FullPath))
        {
            string JsonContent = File.ReadAllText(FullPath);
            JsonUtility.FromJsonOverwrite(JsonContent, allNodes);
            mainFunction.DialogNodeDataToNode(allNodes.dialogNodeDatas);            
            return allNodes;
        }

        else
        {
            Debug.LogWarning("there is no saved data");
            return null;
        }
    }
    void WriteData(object data, string FilePath, bool SaveInResources = false)
    {

        string JsonFile = JsonUtility.ToJson(data, true);


        /// <summary>
        /// Combines the PersistentDataPath with the file name
        /// to ensure cross-platform compatibility
        /// </summary>

        string FullPath = FilePath;

        File.WriteAllText(FullPath, JsonFile);
        if (SaveInResources)
        {
            AssetDatabase.Refresh();
        }
        Debug.Log($"Saved {FilePath}");
    }
}