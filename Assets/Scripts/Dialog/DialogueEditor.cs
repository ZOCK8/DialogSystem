using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditor : EditorWindow
{
    public static DialogueEditor instance {get; private set;}
    [MenuItem("Window/Dialogue Editor")]
    public static void OpenWindow() => GetWindow<DialogueEditor>("Dialogue Editor");

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {

        var dialogueGraph = new DialogueGraphView();

        var toolbar = new Toolbar();

        var saveButton = new ToolbarButton(() =>
        {
            DialogSaver.instance.SaveGraph();
        })
        {
            text = "Speichern"
        };


        var loadButton = new ToolbarButton(() =>
        {
            DialogSaver.instance.LoadData("Assets/Resources/DialogNodes.dat");
        })
        {
            text = "Load"
        };

        DialogSaver.instance.dialogueGraphView = dialogueGraph;
        DialogSaver.instance.mainFunction = dialogueGraph.mainFunction;
        DialogSaver.instance.visualFunctions = dialogueGraph.visualFunctions;

        toolbar.Add(loadButton);
        toolbar.Add(saveButton);
        rootVisualElement.Add(toolbar);

        dialogueGraph.style.flexGrow = 1;
        rootVisualElement.Add(dialogueGraph);
    }
}