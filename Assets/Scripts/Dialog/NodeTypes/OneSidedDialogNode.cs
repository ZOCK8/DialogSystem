
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class OneSidedDialogNode : DialogueGraphView
{
    public List<VisualElement> SetDialogNode(Node node)
    {
        mainFunction.RemoveExtensionElements(node);
        node.name = NodeTypes.Dialog.ToString();
        List<VisualElement> visualElements = new List<VisualElement>();

            visualElements.Add(visualFunctions.AddText(node, "Time", "0.2", ValueTypes.Float));
            visualElements.Add(visualFunctions.AddText(node, "DialogText", "Enter", ValueTypes.String));
            visualElements.Add(visualFunctions.AddPort(node, Direction.Input, "In"));
            visualElements.Add(visualFunctions.AddPort(node, Direction.Output, "Out"));
        
        return visualElements;
    }
}