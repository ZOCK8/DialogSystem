
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StartNote : DialogueGraphView
{
    public void SetDialogNode(Node node)
    {
        mainFunction.RemoveExtensionElements(node);
        mainFunction.RemovePorts(node, Direction.Input);
        mainFunction.RemovePorts(node, Direction.Output);

        var Port = visualFunctions.AddPort(node, Direction.Input, "Start");
        node.title = "Start";

    }
}