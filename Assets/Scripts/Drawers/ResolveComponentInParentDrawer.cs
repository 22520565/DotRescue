#if UNITY_EDITOR

#nullable enable

namespace Game;

using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ResolveComponentInParentAttribute))]
public class ResolveComponentInParentDrawer : PropertyDrawer
{
    private bool hierarchyChanged = false;

    public ResolveComponentInParentDrawer()
    {
        // Register an event that triggers when the hierarchy changes (e.g., component added/removed)
        EditorApplication.hierarchyChanged += () => this.hierarchyChanged = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Start checking for changes in the Inspector
        EditorGUI.BeginChangeCheck();

        if (EditorGUI.EndChangeCheck() || this.hierarchyChanged)
        {
            this.ResolveComponentInParentLogic(property);
            this.hierarchyChanged = false; // Reset flag
        }

        // Display field on Inspector as read only
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }

    private void ResolveComponentInParentLogic(SerializedProperty property)
    {
        var targetObject = property.serializedObject.targetObject;

        // Only works with fields that their type is Object (Component, GameObject, ScriptableObject, ...)
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            Debug.LogError($"`{nameof(ResolveComponentInParentAttribute)}` is not valid for field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}`!");
            return;
        }

        if (this.attribute is not ResolveComponentInParentAttribute attributeInstance)
        {
            Debug.LogError($"`{nameof(ResolveComponentInParentAttribute)}` cannot be found in field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}`!");
            return;
        }

        if (targetObject is not MonoBehaviour monoBehaviour)
        {
            Debug.LogError($"`{nameof(ResolveComponentInParentAttribute)}` is not valid for field `{this.fieldInfo.FieldType}` in Object `{targetObject.name}`!");
            return;
        }

        var gameObject = monoBehaviour.gameObject;
        var fieldInfo = this.fieldInfo;
        var fieldType = fieldInfo.FieldType;
        var fieldName = fieldInfo.Name;

        // Get the target parent name from the attribute
        var targetParentName = attributeInstance.TargetParentName;
        var isIncludingInactive = attributeInstance.IsIncludingInactive;
        var targetParentNameNotBlank = !string.IsNullOrWhiteSpace(targetParentName);

        var foundComponents = gameObject.GetComponentsInParent(fieldType, isIncludingInactive);
        if (targetParentNameNotBlank)
        {
            foundComponents = foundComponents.Where(component => component.name == targetParentName).ToArray();
        }

        Component? componentToAssign = null;
        if (foundComponents.Length <= 0)
        {
            Debug.LogError($"Failed to assign a `{fieldType.FullName}` component to `{fieldName}` in `{monoBehaviour.GetType().FullName}` " +
                $"because no instances were found on `{monoBehaviour.name}` or its parent {(targetParentNameNotBlank ? $"`{targetParentName}`" : string.Empty)} ({(isIncludingInactive ? "including" : "excluding")} inactive GameObjects). " +
                "The field remains null.");
        }
        else if (foundComponents.Length > 1)
        {
            Debug.LogError($"Failed to assign a `{fieldType.FullName}` component to `{fieldName}` in `{monoBehaviour.GetType().FullName}` " +
                $"because multiple instances were found on `{monoBehaviour.name}` or its parent {(targetParentNameNotBlank ? $"`{targetParentName}`" : string.Empty)} ({(isIncludingInactive ? "including" : "excluding")} inactive GameObjects). " +
                "The field remains null.");
        }
        else
        {
            // Assign the found component
            componentToAssign = foundComponents[0];
        }

        property.objectReferenceValue = componentToAssign;
        property.serializedObject.ApplyModifiedProperties();
    }
}

#endif
