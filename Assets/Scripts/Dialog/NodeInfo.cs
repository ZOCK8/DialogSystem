using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NormalNode : Node
{
    public string nodeID;

    public NormalNode()
    {
        nodeID = System.Guid.NewGuid().ToString();
    }
}