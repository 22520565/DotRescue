#nullable enable

namespace Game;

/// <summary>
/// The field is displayed as read-only in the Inspector if it's serialized by Unity.
/// </summary>
/// <remarks>
/// The field must be serialized by Unity for this attribute to work.
/// </remarks>
[System.AttributeUsage(System.AttributeTargets.Field)]
public sealed class ReadOnlyInInspectorAttribute : UnityEngine.PropertyAttribute
{
}