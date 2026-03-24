using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
        mainFunction.RemoveExtensionElements(node);
        mainFunction.RemovePorts(node, Direction.Input);
        mainFunction.RemovePorts(node, Direction.Output);
        node.name = NodeTypes.Audio.ToString();
        visualFunctions.AddPort(node, Direction.Input, "In");
        visualFunctions.AddPort(node, Direction.Output, "Out");
        visualFunctions.AddAudioField(node);
    }
}