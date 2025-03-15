#nullable enable

namespace Game;

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
public sealed class ReadOnlyInInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
