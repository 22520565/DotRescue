#nullable enable

namespace Game;

using System;
using UnityEngine;

/// <summary>
/// Automatically assigns a component to the field, similar to `GetComponent()`.
/// If no matching component is found or multiple components of the field's type exist on the GameObject,
/// the field is null and an error is logged.
/// </summary>
/// <remarks>
/// The field must be serialized by Unity for this attribute to work,
/// as it only verifies and updates the field in the Inspector via its custom drawer
/// when the GameObject or Unity hierarchy changes.
/// The field is displayed as read-only in the Inspector.
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public sealed class ResolveComponentAttribute : PropertyAttribute
{
}
