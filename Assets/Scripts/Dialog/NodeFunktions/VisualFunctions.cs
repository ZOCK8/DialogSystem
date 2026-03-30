using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.UI;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VisualFunctions : MonoBehaviour
{
    public MainFunction mainFunction;

    public void SetMainFunction(MainFunction main)
    {
        this.mainFunction = main;
    }
    public UnityEngine.UIElements.VisualElement AddText(Node node, string Name, string DefaultValue, ValueTypes type)
    {
        UnityEngine.UIElements.VisualElement Field;
        switch (type)
        {
            case ValueTypes.String:
                Field = new UnityEngine.UIElements.TextField();
                break;
            case ValueTypes.Float:
                Field = new UnityEngine.UIElements.FloatField();
                break;
            case ValueTypes.Color:
                Field = new Unity.AppUI.UI.ColorField();
                Field.AddToClassList("unity-color-field");
                break;
            case ValueTypes.Vector3:
                Field = new Unity.AppUI.UI.Vector3Field();
                break;
            default:
                Field = new UnityEngine.UIElements.TextField();
                break;

        }
        if (Field is UnityEngine.UIElements.FloatField floatField)
        {
            floatField.value = float.Parse(DefaultValue);
        }
        else if (Field is UnityEngine.UIElements.TextField textFiled)
        {
            textFiled.value = DefaultValue;
        }

        else if (Field is Unity.AppUI.UI.ColorField colorFiled)
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString(DefaultValue, out Color color))
            {
                colorFiled.value = Color.black;
                colorFiled.style.minWidth = 100;
                colorFiled.hdr = true;
            }
        }
        Field.name = Name;
        node.extensionContainer.Add(Field);
        node.RefreshPorts();
        node.RefreshExpandedState();
        return Field;
    }
    public Port AddPort(Node node, UnityEditor.Experimental.GraphView.Direction direction, string PortName)
    {
        var listener = new EdgeConnectorListener();
        var connector = new EdgeConnector<UnityEditor.Experimental.GraphView.Edge>(listener);

        Port port = Port.Create<UnityEditor.Experimental.GraphView.Edge>(
            Orientation.Horizontal,
            direction,
            Port.Capacity.Multi,
            typeof(float)
        );


        port.RemoveManipulator(port.edgeConnector);
        port.AddManipulator(connector);
        port.name = PortName;
        port.portName = PortName;
        if (direction == UnityEditor.Experimental.GraphView.Direction.Input)
        {
            node.inputContainer.Add(port);
        }
        else if (direction == UnityEditor.Experimental.GraphView.Direction.Output)
        {
            node.outputContainer.Add(port);
        }
        node.RefreshPorts();
        node.RefreshExpandedState();
        return port;
    }

    public ObjectField AddObjectField(Node node, string name, Type objectType, UnityEngine.Object defaultValue = null)
    {
        var audioField = new ObjectField("Audio Clip")
        {
            objectType = objectType,
            allowSceneObjects = false
        };

        audioField.RegisterValueChangedCallback(evt =>
        {
            var clip = evt.newValue as AudioClip;
            Debug.Log("Selected clip: " + clip);
        });
        node.extensionContainer.Add(audioField);
        return audioField;
    }

    public DropdownField AddDropDownField(Node node, string name, List<string> Options = null)
    {
        DropdownField dropdownField = new DropdownField
        {
            name = name,
            choices = Options ?? Enum.GetNames(typeof(NodeTypes)).ToList(),
            value = "Chose"
        };

        dropdownField.RegisterValueChangedCallback(evt =>
        {
            mainFunction.CheckDropDown(node);
        });

        node.extensionContainer.Add(dropdownField);
        return dropdownField;
    }

    public ValueTypes GetValueType(VisualElement visualElement)
    {
        switch (visualElement)
        {
            case Unity.AppUI.UI.TextField:
                return ValueTypes.String;

            case Unity.AppUI.UI.FloatField:
                return ValueTypes.Float;

            case Unity.AppUI.UI.ColorField:
                return ValueTypes.Color;

            case Unity.AppUI.UI.Vector3Field:
                return ValueTypes.Vector3;
            default:
             return ValueTypes.nothing;
        }

    }
}