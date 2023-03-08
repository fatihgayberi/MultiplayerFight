using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(WonnasmithEditor.HelpBoxAttribute))]
public class HelpBoxAttributeDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        var helpBoxAttribute = attribute as WonnasmithEditor.HelpBoxAttribute;
        if (helpBoxAttribute == null) return base.GetHeight();
        var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
        if (helpBoxStyle == null) return base.GetHeight();
        return Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text), EditorGUIUtility.currentViewWidth) + 4);
    }

    public override void OnGUI(Rect position)
    {
        var helpBoxAttribute = attribute as WonnasmithEditor.HelpBoxAttribute;
        if (helpBoxAttribute == null) return;
        EditorGUI.HelpBox(position, helpBoxAttribute.text, GetMessageType(helpBoxAttribute.messageType));
    }

    private MessageType GetMessageType(WonnasmithEditor.HelpBoxMessageType helpBoxMessageType)
    {
        switch (helpBoxMessageType)
        {
            default:
            case WonnasmithEditor.HelpBoxMessageType.None: return MessageType.None;
            case WonnasmithEditor.HelpBoxMessageType.Info: return MessageType.Info;
            case WonnasmithEditor.HelpBoxMessageType.Warning: return MessageType.Warning;
            case WonnasmithEditor.HelpBoxMessageType.Error: return MessageType.Error;
        }
    }
}