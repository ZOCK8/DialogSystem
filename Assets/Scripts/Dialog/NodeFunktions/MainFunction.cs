
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MainFunction : MonoBehaviour
{
    public VisualFunctions visualFunctions;
    public GraphView graphView;

    public void SetVisualFunctions(VisualFunctions visual)
    {
        this.visualFunctions = visual;
    }

    public Node CreateNewNode(String Title, Rect rect, NodeTypes nodeTypes)
    {
        Node NewNode = new Node
        {
            title = Title
        };
        visualFunctions.AddDropDownField(NewNode);
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
        VisualElement container = direction == Direction.Input
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
    public void CheckDropDown(DropdownField dropdownField, Node node)
    {
        switch (dropdownField.value)
        {
            case "Start":
                StartNote startNote = new StartNote();
                startNote.SetDialogNode(node);
                break;
            case "Dialog":
                OneSidedDialogNode dialogNode = new OneSidedDialogNode();
                dialogNode.SetDialogNode(node);
                break;
            case "MultiDialog":
                MultipleChoiceNode multipleChoice = new MultipleChoiceNode();
                multipleChoice.SetDialogNode(node);
                break;
            case "Audio":
                AudioNode audioNode = new AudioNode();
                audioNode.SetDialogNode(node);
                break;
            case "Action":
                break;
        }
    }
}