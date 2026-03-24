using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class EdgeConnectorListener : IEdgeConnectorListener
{
    private NodeSearchProvider searchTree = new NodeSearchProvider();
    public void OnDrop(GraphView graphView, Edge edge)
    {
        edge.input.Connect(edge);
        edge.output.Connect(edge);

        graphView.AddElement(edge);
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        
    }
}