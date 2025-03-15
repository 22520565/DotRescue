#nullable enable

namespace Game;

using System;
using UnityEngine;

/// <summary>
/// Logs a warning in the Unity console if the `LayerMask` field is set to `Nothing` or `Everything`.
/// </summary>
/// <remarks>
/// The field must be serialized by Unity for this attribute to work,
/// as it only verifies the field in the Inspector via its custom drawer
/// when the GameObject or Unity hierarchy changes.
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class LayerMaskIsNothingOrEverythingWarningAttribute : PropertyAttribute
{
}
