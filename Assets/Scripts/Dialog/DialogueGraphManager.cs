using UnityEngine;

public class DialogueGraphManager : MonoBehaviour
{
    void Start()
    {
        Play("Assets/Resources/DialogNodes.dat");
    }
    public void Play(string ResourcesPath)
    {
        AllNodes nodes = DialogSaver.instance.LoadData(ResourcesPath);

        DialogNodeData StartNode = nodes.dialogNodeDatas.Find(DialogNodeData => DialogNodeData.NodeName == "Start");
        if (StartNode != null)
        {
            if (StartNode.ConnectedNodes[0] != null)
            {
                PlayNode(StartNode.ConnectedNodes[0]);
            }
            else
            {
                Debug.LogWarning("There is no connected node to start node cant play: " + ResourcesPath);
            }
                    
        }
        else
        {
            Debug.LogWarning("There is no given Start Node for: " + ResourcesPath);
        }
    }

    void PlayNode(DialogNodeData nodes)
    {
        switch (nodes.nodeTypes)
        {
            case NodeTypes.Dialog:
                OneSidedDialogNode oneSidedDialog = new OneSidedDialogNode();
                // oneSidedDialog.Play()
                break;
            case NodeTypes.MultiDialog:
                break;
            case NodeTypes.Audio:
                AudioNode audioNode = new AudioNode();
                audioNode.PlayNode(nodes);
                break;
            case NodeTypes.Action:
                break;
        }
    }
}

public enum NodeTypes
{
    Start,
    Dialog,
    MultiDialog,
    Audio,
    Action,
    nothing
}

public enum ValueTypes
{
    String,
    Float,
    Color,
    Vector3, 
    nothing

}