#if UNITY_EDITOR

#nullable enable

namespace Game;

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ResolveComponentAttribute))]
public sealed class ResolveComponentDrawer : PropertyDrawer
{
    private bool hierarchyChanged = false;

    public ResolveComponentDrawer()
    {
        // Register an event that triggers when the hierarchy changes (e.g., component added/removed)
        EditorApplication.hierarchyChanged += () => this.hierarchyChanged = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Start checking for changes in the Inspector
        EditorGUI.BeginChangeCheck();

        // Display field on Inspector as read only
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;

        if (EditorGUI.EndChangeCheck() || this.hierarchyChanged)
        {
            this.ResolveComponentLogic(property);
            this.hierarchyChanged = false; // Reset flag
        }
    }

    private void ResolveComponentLogic(SerializedProperty property)
    {
        var targetObject = property.serializedObject.targetObject;

        // Only works with fields that their type is Object (Component, GameObject, ScriptableObject, ...)
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            Debug.LogError($"`{nameof(ResolveComponentAttribute)}` is not valid for field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}`!");
            return;
        }

        if (targetObject is not MonoBehaviour monoBehaviour)
        {
            Debug.LogError($"`{nameof(ResolveComponentAttribute)}` is not valid for field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}`!");
            return;
        }

        var gameObject = monoBehaviour.gameObject;
        var fieldInfo = this.fieldInfo;
        var fieldType = fieldInfo.FieldType;
        var fieldName = fieldInfo.Name;

        Component? componentToAssign = null;

        var foundComponents = gameObject.GetComponents(fieldType);
        if (foundComponents.Length <= 0)
        {
            Debug.LogError($"Failed to assign a `{fieldType.FullName}` component to the field `{fieldName}` in MonoBehaviour `{monoBehaviour.GetType().FullName}` " +
                $"because no instances were found on GameObject `{monoBehaviour.name}`. The field has been left null.");
        }
        else if (foundComponents.Length > 1)
        {
            Debug.LogError($"Failed to assign a `{fieldType.FullName}` component to the field `{fieldName}` in MonoBehaviour `{monoBehaviour.GetType().FullName}` " +
                $"because multiple instances were found on GameObject `{monoBehaviour.name}`. The field has been left null.");
        }
        else
        {
            componentToAssign = foundComponents[0];
        }

        property.objectReferenceValue = componentToAssign;
        property.serializedObject.ApplyModifiedProperties();
    }
}

#endif
