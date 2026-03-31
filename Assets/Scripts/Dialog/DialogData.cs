using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
[System.Serializable]
public class AllNodes
{
    public List<DialogNodeData> dialogNodeDatas = new List<DialogNodeData>();
    public List<EdgeSaveData> edgeSaveDatas = new List<EdgeSaveData>();

}
/// <summary>
/// Can Only Be used for One Sided Dialogs
/// and Multiple Choice Dialogs but not 
/// other node types
/// </summary>
[System.Serializable]
public class DialogNodeData
{
    public string NodeName;
    public NodeTypes nodeTypes;
    public float PosX, PosY;
    public float width, height;
    public List<FieldSaveData> fields = new List<FieldSaveData>();
    public List<DropDownFieldData> dropDownFields = new List<DropDownFieldData>();
    public List<ObjectSaveData> objectSaveDatas = new List<ObjectSaveData>();
    public string NodeID;
    public List<DialogNodeData> ConnectedNodes;
}


/// <summary>
/// For InputFields
/// (int, string float)
/// </summary>
[System.Serializable]
public class FieldSaveData
{
    public string name;
    public string Value;
    public ValueTypes type;
}
[System.Serializable]
public class ObjectSaveData
{
    public string name;
    public UnityEngine.Object Value;
    public Type type;
}
[System.Serializable]
public class DropDownFieldData
{
    public string Value; ////////////////////////////////////////////
    public string name;
    public List<string> DropDownChoices = new List<string>();
}
/// <summary>
/// Saves The edges 
/// the lines that connect 
/// the nodes
/// </summary>
[System.Serializable]
public class EdgeSaveData
{
    public string outputNodeID;
    public string outputPortName;
    public string inputNodeID;
    public string inputPortName;
}