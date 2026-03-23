using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogSaver
{
    private static DialogSaver _instance;
    private AllNodes allNodes = new AllNodes();
    public MainFunction mainFunction;
    public VisualFunctions visualFunctions = new VisualFunctions();
    public static DialogSaver instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = new DialogSaver();
            }
            return _instance;
        }
    }
    public List<Nodes> nodes = new List<Nodes>();
    public void SaveGraph()
    {
        allNodes = new AllNodes();
        Debug.Log(nodes.Count);

        foreach (var Node in nodes)
        {
            var SaveData = new DialogNodeData
            {
                height = Node.node.GetPosition().height,
                width = Node.node.GetPosition().width,
                PosX = Node.node.GetPosition().x,
                PosY = Node.node.GetPosition().y,

                NodeName = Node.node.title,

                nodeTypes = Node.nodeType
            };

            /// Gets the fields
            foreach (var field in Node.node.extensionContainer.Children())
            {

                if (field is UnityEngine.UIElements.FloatField floatField)
                {
                    FieldSaveData fieldSave = new FieldSaveData
                    {
                        Value = floatField.value.ToString(),
                        type = ValueTypes.Float,
                        name = field.name
                    };

                    SaveData.fields.Add(fieldSave);
                }
                else if (field is UnityEngine.UIElements.TextField textField)
                {
                    FieldSaveData fieldSave = new FieldSaveData
                    {
                        Value = textField.value,
                        type = ValueTypes.String,
                        name = field.name
                    };

                    SaveData.fields.Add(fieldSave);
                }
                else if (field is ColorField colorField)
                {
                    FieldSaveData fieldSave = new FieldSaveData
                    {
                        Value = colorField.value.ToString(),
                        type = ValueTypes.Color,
                        name = field.name
                    };

                    SaveData.fields.Add(fieldSave);
                }

                else if (field is DropdownField DropdownField)
                {
                    DropDownFieldData dropDownFieldData = new DropDownFieldData
                    {
                        name = field.name
                    };
                    List<string> Choices = new List<string>();
                    foreach (var dropdown in DropdownField.choices)
                    {
                        Choices.Add(dropdown);
                    }

                    dropDownFieldData.DropDownChoices = Choices;
                    SaveData.dropDownFields.Add(dropDownFieldData);

                }
            }
            allNodes.dialogNodeDatas.Add(SaveData);
        }

        WriteData(allNodes, "DialogNodes.dat");
    }

    public void LoadData()
    {
        nodes.Clear();
        allNodes = new AllNodes();
        string FullPath = Path.Combine(Application.persistentDataPath, "DialogNodes.dat");
        if (File.Exists(FullPath))
        {
            string JsonContent = File.ReadAllText(FullPath);
            JsonUtility.FromJsonOverwrite(JsonContent, allNodes);

            foreach (var node in allNodes.dialogNodeDatas)
            {
                Rect rect = new Rect
                {
                    width = node.width,
                    height = node.height,
                    x = node.PosX,
                    y = node.PosY
                };

                Node NewNode = mainFunction.CreateNewNode(node.NodeName, rect, node.nodeTypes);

                DropdownField dropdownField = visualFunctions.AddDropDownField(NewNode);
                dropdownField.value = node.nodeTypes.ToString();

                foreach (FieldSaveData field in node.fields)
                {

                    visualFunctions.AddText(NewNode, field.name, field.Value, field.type);
                }

                foreach (DropDownFieldData dropDownField in node.dropDownFields)
                {
                    DropdownField dropdown = visualFunctions.AddDropDownField(NewNode);
                    dropdown.value = dropDownField.Value;
                    dropdown.choices = dropDownField.DropDownChoices;
                }


                switch (node.nodeTypes)
                {
                    case NodeTypes.Dialog:
                        OneSidedDialogNode dialogNode = new OneSidedDialogNode();
                        var Element = dialogNode.SetDialogNode(NewNode);
                        break;
                    case NodeTypes.MultiDialog:
                        MultipleChoiceNode multipleChoice = new MultipleChoiceNode();
                        multipleChoice.SetDialogNode(NewNode);
                        break;
                    case NodeTypes.Audio:
                        break;
                    case NodeTypes.Action:
                        break;
                }
            }


        }

        else
        {
            Debug.LogWarning("there is no saved data");
        }
    }
    void WriteData(object data, string FileName)
    {

        string JsonFile = JsonUtility.ToJson(data, true);


        /// <summary>
        /// Combines the PersistentDataPath with the file name
        /// to ensure cross-platform compatibility
        /// </summary>
        string FullPath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);

        File.WriteAllText(FullPath, JsonFile);
        Debug.Log($"Saved {FileName}");
    }
}