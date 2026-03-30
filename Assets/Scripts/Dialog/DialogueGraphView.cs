using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueGraphView : GraphView
{
    public MainFunction mainFunction = new MainFunction();
    public VisualFunctions visualFunctions = new VisualFunctions();
    private Vector2 _lastMousePosition;
    private NodeSearchProvider _searchProvider;
    public DialogueGraphView()
    {
        GridBackground background = new GridBackground();
        Insert(0, background);
        background.StretchToParentSize();
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentZoomer());
        mainFunction.graphView = this;

        mainFunction.SetVisualFunctions(visualFunctions);
        visualFunctions.SetMainFunction(mainFunction);

        DialogSaver.instance.mainFunction = mainFunction;
        DialogSaver.instance.visualFunctions = visualFunctions;
        DialogSaver.instance.dialogueGraphView = this;
        var searchProvider = ScriptableObject.CreateInstance<NodeSearchProvider>();
        searchProvider.graphView = this;

        _searchProvider = ScriptableObject.CreateInstance<NodeSearchProvider>();
        _searchProvider.graphView = this;

        RegisterCallback<MouseMoveEvent>(evt =>
        {
            _lastMousePosition = contentViewContainer.WorldToLocal(evt.mousePosition);
        });

        nodeCreationRequest = context =>
        {
            _searchProvider.mousePosition = _lastMousePosition;
            Debug.Log("Gespeicherte Position: " + _lastMousePosition);
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchProvider);
        };
    }

    public void ClearAll()
    {
        graphElements.ForEach(elem =>
        {

            RemoveElement(elem);

        });
    }



    public override System.Collections.Generic.List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new System.Collections.Generic.List<Port>();
        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
}
[System.Serializable]
public class Nodes : MonoBehaviour
{
    public NodeTypes nodeType;
    public Node node;
}