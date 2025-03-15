#if UNITY_EDITOR

#nullable enable

namespace Game;

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LayerMaskIsNothingOrEverythingWarningAttribute))]
public sealed class LayerMaskIsNothingOrEverythingWarningDrawer : PropertyDrawer
{
    private bool hierarchyChanged = false;

    public LayerMaskIsNothingOrEverythingWarningDrawer()
    {
        // Register an event that triggers when the hierarchy changes (e.g., component added/removed)
        EditorApplication.hierarchyChanged += () => this.hierarchyChanged = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var fieldInfo = this.fieldInfo;
        var fieldType = fieldInfo.FieldType;
        var targetObject = property.serializedObject.targetObject;

        if (fieldType != typeof(LayerMask))
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, true);
            if (EditorGUI.EndChangeCheck() || this.hierarchyChanged)
            {
                Debug.LogError($"`{nameof(LayerMaskIsNothingOrEverythingWarningAttribute)}` is not valid for field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}` because it's not a {typeof(LayerMask).FullName}!");
                this.hierarchyChanged = false;
            }
            return;
        }

        EditorGUI.BeginChangeCheck();
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, UnityEditorInternal.InternalEditorUtility.layers);
        if (!EditorGUI.EndChangeCheck())
        {
            return;
        }

        property.intValue = EditorGUI.MaskField(position, label, property.intValue, UnityEditorInternal.InternalEditorUtility.layers);
        if (property.intValue == 0) // Nothing
        {
            Debug.LogWarning($"LayerMask '{property.name}' is 'Nothing' which is invalid!");
        }
        else if (property.intValue == ~0) // Everything
        {
            Debug.LogWarning($"LayerMask '{property.name}' is 'Everything' which is invalid!");
        }
        else
        {
            // LayerMask is valid! I guess...
        }
    }
}

#endif
