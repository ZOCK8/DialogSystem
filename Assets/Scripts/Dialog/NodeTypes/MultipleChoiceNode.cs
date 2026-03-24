
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MultipleChoiceNode : DialogueGraphView
{
    public void SetDialogNode(Node node)
    {
        mainFunction.RemoveExtensionElements(node);
        mainFunction.RemovePorts(node, Direction.Input);
        mainFunction.RemovePorts(node, Direction.Output);
        node.name = NodeTypes.Dialog.ToString();
        visualFunctions.AddText(node, "Time", "0.8", ValueTypes.Float);
        visualFunctions.AddText(node, "DialogText", "DialogText", ValueTypes.String);
        visualFunctions.AddText(node, "Color", "#ac242422", ValueTypes.Color);
        visualFunctions.AddText(node, "DialogText", "DialogText", ValueTypes.String);
        visualFunctions.AddText(node, "DialogText", "DialogText", ValueTypes.String);
        visualFunctions.AddPort(node, Direction.Input, "In");
        visualFunctions.AddPort(node, Direction.Output, "Out");

    }
}