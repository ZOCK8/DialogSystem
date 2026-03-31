using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogSaver
{
    private static DialogSaver _instance;
    private AllNodes allNodes = new AllNodes();
    public List<UnityEditor.Experimental.GraphView.Edge> allEdges = new List<UnityEditor.Experimental.GraphView.Edge>();
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
        if (dialogueGraphView != null)
        {
            allEdges = dialogueGraphView.edges
                .Where(e => e.input != null && e.output != null
                         && e.input.node != null && e.output.node != null)
                .ToList();

            Debug.Log($"Gefilterte Edges: {allEdges.Count}");
        }
        else
        {
            Debug.LogError("There is no DialogGraph");
        }

        List<Node> NodeList = new List<Node>();
        foreach (Nodes x in nodes)
        {
            NodeList.Add(x.node);
        }
        allNodes.dialogNodeDatas = mainFunction.NodeToDialogNodeData(NodeList);
        allNodes.edgeSaveDatas = mainFunction.EdgeToEdgeSaveData(allEdges);
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