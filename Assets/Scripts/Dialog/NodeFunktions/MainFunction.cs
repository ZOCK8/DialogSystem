
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class MainFunction : MonoBehaviour
{
    public VisualFunctions visualFunctions;
    public GraphView graphView;

    public void SetVisualFunctions(VisualFunctions visual)
    {
        this.visualFunctions = visual;
    }


    public Node CreateNewNode(string Title, Rect rect, NodeTypes nodeTypes)
    {
        NormalNode NewNode = new NormalNode
        {
            name = nodeTypes.ToString(),
            title = Title,
            nodeID = System.Guid.NewGuid().ToString()
        };

        NewNode.userData = NewNode.nodeID;
        visualFunctions.AddDropDownField(NewNode, Title + "DropDown");
        NewNode.SetPosition(rect);
        graphView.AddElement(NewNode);
        Nodes node = new Nodes
        {
            node = NewNode,
            nodeType = nodeTypes
        };
        DialogSaver.instance.nodes.Add(node);
        NewNode.RefreshPorts();
        NewNode.RefreshExpandedState();
        return NewNode;
    }

    public void RemoveExtensionElement(Node node, String ElementName = "DropDownField")
    {
        List<VisualElement> visualElements = new List<VisualElement>();
        foreach (var element in node.extensionContainer.Children())
        {
            if (element.name == ElementName)
            {
                if (node.extensionContainer.Contains(element))
                {
                    visualElements.Add(element);
                }
            }
        }
        foreach (VisualElement visualElement in visualElements)
        {
            node.extensionContainer.Remove(visualElement);
        }
    }

    public void RemoveExtensionElements(Node node, String Exclude = "DropDownField")
    {
        List<VisualElement> visualElements = new List<VisualElement>();
        foreach (var element in node.extensionContainer.Children())
        {
            if (element.name != Exclude)
            {
                if (node.extensionContainer.Contains(element))
                {
                    visualElements.Add(element);
                }
            }
        }
        foreach (VisualElement visualElement in visualElements)
        {
            node.extensionContainer.Remove(visualElement);
        }
    }
    public void RemovePorts(Node node, UnityEditor.Experimental.GraphView.Direction direction, String Exclude = "DropDownField")
    {
        VisualElement container = direction == UnityEditor.Experimental.GraphView.Direction.Input
    ? node.inputContainer
    : node.outputContainer;
        List<Port> Ports = new List<Port>();
        foreach (Port ports in container.Children())
        {
            if (ports.name != Exclude)
            {
                if (container.Contains(ports))
                {
                    Ports.Add(ports);
                }
            }
        }
        foreach (Port port in Ports)
        {
            container.Remove(port);
        }
    }
    public NodeTypes CheckDropDown(Node node)
    {
        if (node != null)
        {
            switch (node.name)
            {
                case "Start":
                    StartNote startNote = new StartNote();
                    startNote.SetDialogNode(node);
                    return NodeTypes.Start;
                case "Dialog":
                    OneSidedDialogNode dialogNode = new OneSidedDialogNode();
                    dialogNode.SetDialogNode(node);
                    return NodeTypes.Dialog;
                case "MultiDialog":
                    MultipleChoiceNode multipleChoice = new MultipleChoiceNode();
                    multipleChoice.SetDialogNode(node);
                    return NodeTypes.MultiDialog;
                case "Audio":
                    AudioNode audioNode = new AudioNode();
                    audioNode.SetDialogNode(node);
                    return NodeTypes.Audio;
                case "Action":
                    return NodeTypes.Action;
                default:
                    return NodeTypes.nothing;
            }
        }
        else
        {
            Debug.LogWarning("There is no dropdownField");
            return NodeTypes.Start;
        }
    }

    public ValueTypes GetValueType(VisualElement visualElement)
    {
        switch (visualElement.GetType())
        {
            case Type t when t == typeof(Unity.AppUI.UI.TextField):
                return ValueTypes.String;
            case Type t when t == typeof(Unity.AppUI.UI.FloatField):
                return ValueTypes.Float;
            case Type t when t == typeof(Unity.AppUI.UI.ColorField):
                return ValueTypes.Color;
            case Type t when t == typeof(Unity.AppUI.UI.Vector3Field):
                return ValueTypes.Vector3;
            default:
                return ValueTypes.nothing;
        }
    }

    /// <summary>
    /// Converts a Node To node data 
    /// for saving
    /// </summary>
    /// <param name="Node To Dialog Node Data"></param>
    /// <returns></returns>

    public List<DialogNodeData> NodeToDialogNodeData(List<Node> node)
    {
        List<DialogNodeData> dialogNodeDatas = new List<DialogNodeData>();

        for (int i = 0; i < node.Count; i++)
        {
            List<FieldSaveData> fieldSave = new List<FieldSaveData>();
            foreach (var field in node[i].extensionContainer.Children().OfType<Unity.AppUI.UI.TextField>())
            {
                fieldSave.Add(new FieldSaveData
                {
                    name = field.name,
                    Value = field.value,
                    type = GetValueType(field)
                });
            }

            List<DropDownFieldData> dropDownFieldData = new List<DropDownFieldData>();
            foreach (var dropdownField in node[i].extensionContainer.Children().OfType<DropdownField>())
            {
                dropDownFieldData.Add(new DropDownFieldData
                {
                    Value = dropdownField.value,
                    name = dropdownField.name,
                    DropDownChoices = dropdownField.choices
                });
            }

            List<ObjectSaveData> objectSaveDatas = new List<ObjectSaveData>();
            foreach (var objectField in node[i].extensionContainer.Children().OfType<ObjectField>())
            {
                objectSaveDatas.Add(new ObjectSaveData
                {
                    name = objectField.name,
                    Value = objectField.value,
                    type = objectField.GetType()
                });
            }

            dialogNodeDatas.Add(new DialogNodeData
            {
                NodeName = node[i].title,
                nodeTypes = CheckDropDown(node[i]),
                PosX = node[i].GetPosition().x,
                PosY = node[i].GetPosition().y,
                height = node[i].GetPosition().height,
                width = node[i].GetPosition().width,
                fields = fieldSave,
                dropDownFields = dropDownFieldData,
                objectSaveDatas = objectSaveDatas,
                NodeID = node[i].viewDataKey,
                ConnectedNodes = new List<DialogNodeData>()
            });
        }

        for (int i = 0; i < node.Count; i++)
        {
            foreach (var port in node[i].outputContainer.Children().OfType<Port>())
            {
                foreach (var edge in port.connections)
                {
                    int connectedIndex = node.IndexOf(edge.input.node);
                    if (connectedIndex != -1)
                        dialogNodeDatas[i].ConnectedNodes.Add(dialogNodeDatas[connectedIndex]);
                }
            }
        }

        return dialogNodeDatas;
    }

    public List<Node> DialogNodeDataToNode(List<DialogNodeData> nodeDatas)
    {
        List<Node> nodes = new List<Node>();
        foreach (var node in nodeDatas)
        {
            if (nodes.Any(n => n.viewDataKey == node.NodeID))
            {
                continue;
            }
            Rect rect = new Rect
            {
                width = node.width,
                height = node.height,
                x = node.PosX,
                y = node.PosY
            };

            Node NewNode = CreateNewNode(node.NodeName, rect, node.nodeTypes);
            NewNode.viewDataKey = node.NodeID;
            DropdownField dropdownField = visualFunctions.AddDropDownField(NewNode, "DropDown");
            dropdownField.value = node.nodeTypes.ToString();

            foreach (FieldSaveData field in node.fields)
            {

                visualFunctions.AddText(NewNode, field.name, field.Value, field.type);
            }

            foreach (DropDownFieldData dropDownField in node.dropDownFields)
            {
                DropdownField dropdown = visualFunctions.AddDropDownField(NewNode, "DropDown");
                dropdown.value = dropDownField.Value;
                dropdown.choices = dropDownField.DropDownChoices;
            }

            foreach (ObjectSaveData objectSave in node.objectSaveDatas)
            {
                visualFunctions.AddObjectField(NewNode, objectSave.name, objectSave.type, objectSave.Value);
            }
            CheckDropDown(NewNode);
            nodes.Add(NewNode);
        }
        return nodes;
    }

    public List<EdgeSaveData> EdgeToEdgeSaveData(List<Edge> edges)
    {
        if (edges.Count > 0)
        {

            List<EdgeSaveData> edgeSaveDatas = new List<EdgeSaveData>();
            foreach (Edge edge in edges)
            {

                EdgeSaveData saveData = new EdgeSaveData
                {
                    outputNodeID =  edge.output.node.userData as string ?? "X",
                    outputPortName = edge.output.name,
                    inputPortName = edge.input.name,
                    inputNodeID =  edge.input.node.userData as string ?? "X",
                };
                edgeSaveDatas.Add(saveData);
            }
            if (edgeSaveDatas.Count < 1)
            {
                Debug.LogWarning("There was no save data created");
                return null;
            }
            return edgeSaveDatas;
        }
        else
        {
            Debug.LogWarning("There was no save data created");
            return null;
        }
    }



}