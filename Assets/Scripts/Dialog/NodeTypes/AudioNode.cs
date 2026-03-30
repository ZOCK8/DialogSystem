using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioNode : DialogueGraphView
{
    public List<String> Paths;
    public List<AudioSource> audioSources;

    void Start()
    {
        LoadAudioFiles();
    }

    void LoadAudioFiles()
    {
        foreach (string Path in Paths)
        {
            audioSources = Resources.LoadAll<AudioSource>(Path).ToList();
        }
    }

    public void SetDialogNode(Node node)
    {
        string DropdownID = Guid.NewGuid().ToString();

        mainFunction.RemoveExtensionElements(node);
        mainFunction.RemovePorts(node, Direction.Input);
        mainFunction.RemovePorts(node, Direction.Output);
        node.name = NodeTypes.Audio.ToString();
        visualFunctions.AddPort(node, Direction.Input, "In");
        visualFunctions.AddPort(node, Direction.Output, "Out");

        visualFunctions.AddText(node, DropdownID, "", ValueTypes.Vector3);
        List<string> Options = new List<string> { "true", "false" };
        DropdownField UsePositionField = visualFunctions.AddDropDownField(node, "DropdownUsePosition", Options);
        UsePositionField.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue == "false")
            {
                mainFunction.RemoveExtensionElement(node, DropdownID);
            }
            else
            {
                visualFunctions.AddText(node, DropdownID, "", ValueTypes.Vector3);
            }
        });
        visualFunctions.AddObjectField(node, "AudioField", typeof(AudioClip));
    }

    public void PlayNode(DialogNodeData node)
    {
        if (node.nodeTypes == NodeTypes.Audio)
        {
            if (node.objectSaveDatas != null)
            {
                DropDownFieldData dropdownBool = node.dropDownFields.Find(DropdownField => DropdownField.name == "DropdownUsePosition");
                UnityEngine.Object AudioObject = node.objectSaveDatas[0].Value;
                if (AudioObject is AudioClip audioClip)
                {
                    if (dropdownBool != null)
                    {
                        GameObject NewAudioObject = new GameObject();
                        if (dropdownBool.Value == "true")
                        {
                            string Position = node.fields.Find(InputField => InputField.type == ValueTypes.Vector3).ToString();
                            var Pos = Position.Split(",");
                            Vector3 AudioVector3 = new Vector3
                            {
                                x = float.Parse(Pos[0]),
                                y = float.Parse(Pos[1]),
                                z = float.Parse(Pos[0])
                            };
                            NewAudioObject.transform.position = AudioVector3;
                        }
                        AudioSource audioSource = NewAudioObject.AddComponent<AudioSource>();
                        audioSource.clip = audioClip;
                    }
                }
            }
        }
    }
}