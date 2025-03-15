#nullable enable

namespace Game;

using System;
using UnityEngine;

/// <summary>
/// Automatically assigns a component of the GameObject's children to the field, similar to `GetComponentInChildren()`.
/// If no matching component is found or multiple components of the field's type exist on the GameObject or its children,
/// the field remains null and an error is logged.
/// Optionally, specify a non-blank `targetChildName` to restrict the search to a specific child GameObject by name.
/// Additionally, set `isIncludingInactive` to `true` to include inactive GameObjects to the search (default is `false`).
/// </summary>
/// <remarks>
/// The field must be serialized by Unity for this attribute to work,
/// as it only verifies and updates the field in the Inspector via its custom drawer
/// when the GameObject or Unity hierarchy changes.
/// The field is displayed as read-only in the Inspector.
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public sealed class ResolveComponentInChildrenAttribute : PropertyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInChildrenAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its children, excluding inactive ones by default.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    public ResolveComponentInChildrenAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInChildrenAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its children, excluding inactive ones by default.
    /// Restricts the search to specific GameObjects by name.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="targetChildName">The name of the GameObject or its children to search within. Leave blank to search all.</param>
    public ResolveComponentInChildrenAttribute(string targetChildName)
    {
        this.TargetChildName = targetChildName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInChildrenAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its children.
    /// Specifies whether to include inactive GameObjects in the search.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="isIncludingInactive">If `true`, includes inactive GameObjects in the search. If `false`, ignores them.</param>
    public ResolveComponentInChildrenAttribute(bool isIncludingInactive)
    {
        this.IsIncludingInactive = isIncludingInactive;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInChildrenAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its children.
    /// Allows restricting the search to specific GameObjects by name and specifying whether to include inactive GameObjects.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="targetChildName"> The name of the GameObject or its children to search within. Leave blank to search all.</param>
    /// <param name="isIncludingInactive"> If `true`, includes inactive GameObjects in the search. If `false`, ignores them.</param>
    public ResolveComponentInChildrenAttribute(string targetChildName, bool isIncludingInactive)
    {
        this.TargetChildName = targetChildName;
        this.IsIncludingInactive = isIncludingInactive;
    }

    public string? TargetChildName { get; } = null;

    public bool IsIncludingInactive { get; } = false;
}
