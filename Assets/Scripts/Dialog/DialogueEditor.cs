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
        toolbar.Add(loadButton);
        toolbar.Add(saveButton);
        rootVisualElement.Add(toolbar);

        var dialogueGraph = new DialogueGraphView();
        dialogueGraph.style.flexGrow = 1;
        rootVisualElement.Add(dialogueGraph);
    }
}