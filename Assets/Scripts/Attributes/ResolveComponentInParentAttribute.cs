#nullable enable

namespace Game;

using System;
using UnityEngine;

/// <summary>
/// Automatically assigns a component of the GameObject's parent to the field, similar to `GetComponentInParent()`.
/// If no matching component is found or multiple components of the field's type exist on the GameObject or its parent,
/// the field is null and an error is logged.
///
/// Optionally, specify a non-blank `targetParentName` to restrict the search to a specific parent GameObject by name.
/// Additionally, set `isIncludingInactive` to `true` to include inactive GameObjects to the search (default is `false`).
/// </summary>
/// <remarks>
/// The field must be serialized by Unity for this attribute to work,
/// as it only verifies and updates the field in the Inspector via its custom drawer
/// when the GameObject or Unity hierarchy changes.
/// The field is displayed as read-only in the Inspector.
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public sealed class ResolveComponentInParentAttribute : PropertyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInParentAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its parent, excluding inactive ones by default.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    public ResolveComponentInParentAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInParentAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its parent, excluding inactive ones by default.
    /// Restricts the search to specific GameObjects by name.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="targetParentName">The name of the GameObject or its parent to search within. Leave blank to search all.</param>
    public ResolveComponentInParentAttribute(string targetParentName)
    {
        this.TargetParentName = targetParentName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInParentAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its parent.
    /// Specifies whether to include inactive GameObjects in the search.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="isIncludingInactive">If `true`, includes inactive GameObjects in the search. If `false`, ignores them.</param>
    public ResolveComponentInParentAttribute(bool isIncludingInactive)
    {
        this.IsIncludingInactive = isIncludingInactive;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResolveComponentInParentAttribute"/> class
    /// to automatically assign a component from the GameObject or any of its parent.
    /// Allows restricting the search to specific GameObjects by name and specifying whether to include inactive GameObjects.
    /// </summary>
    /// <remarks>
    /// The field must be serialized by Unity for this attribute to work.
    /// </remarks>
    /// <param name="targetParentName"> The name of the GameObject or its parent to search within. Leave blank to search all.</param>
    /// <param name="isIncludingInactive"> If `true`, includes inactive GameObjects in the search. If `false`, ignores them.</param>
    public ResolveComponentInParentAttribute(string targetParentName, bool isIncludingInactive)
    {
        this.TargetParentName = targetParentName;
        this.IsIncludingInactive = isIncludingInactive;
    }

    public string? TargetParentName { get; } = null;

    public bool IsIncludingInactive { get; } = false;
}
