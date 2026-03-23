using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EdgeConnectorListener : IEdgeConnectorListener
{
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