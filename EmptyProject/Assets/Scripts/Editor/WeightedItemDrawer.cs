using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(WeightedItem))]
public class WeightedItemDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect itemRect = new Rect(position.x + 40, position.y, position.width - 40, position.height);
        Rect weightRect = new Rect(position.x, position.y, 30, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("weight"), GUIContent.none);
        EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("item"), GUIContent.none);
       

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
