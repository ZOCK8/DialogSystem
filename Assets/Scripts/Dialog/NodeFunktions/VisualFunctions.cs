using System;
using System.Linq;
using Unity.AppUI.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
                Field = new ColorField();
                Field.AddToClassList("unity-color-field");
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

        else if (Field is ColorField colorFiled)
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

    public DropdownField AddDropDownField(Node node)
    {
        DropdownField dropdownField = new DropdownField
        {
            value = "Chose",
            choices = Enum.GetNames(typeof(NodeTypes)).ToList(),
            name = "DropDownField"
        };
        dropdownField.RegisterValueChangedCallback(evt => { mainFunction.CheckDropDown(dropdownField, node); });
        node.extensionContainer.Add(dropdownField);
        return dropdownField;
    }
}