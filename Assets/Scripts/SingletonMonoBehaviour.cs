#nullable enable

namespace Game;

using UnityDebug = UnityEngine.Debug;
using UnityDisallowMultipleComponent = UnityEngine.DisallowMultipleComponent;
using UnityMonoBehaviour = UnityEngine.MonoBehaviour;
using UnityObject = UnityEngine.Object;

[UnityDisallowMultipleComponent]
public abstract class SingletonMonoBehaviour<T> : UnityMonoBehaviour
    where T : SingletonMonoBehaviour<T>
{
    public static T Instance { get; private set; } = null!;

    protected abstract T LocalInstance { get; }

    protected void Awake()
    {
        if (SingletonMonoBehaviour<T>.Instance != null)
        {
            var newInstanceTypeName = typeof(T).FullName;
            var currentInstanceGameObject = SingletonMonoBehaviour<T>.Instance.gameObject;
            var currentInstanceGameObjectName = currentInstanceGameObject.name;
            var newInstanceGameObject = this.LocalInstance.gameObject;
            var newInstanceGameObjectName = newInstanceGameObject.name;

            UnityDebug.LogAssertion($"Multiple instances of {newInstanceTypeName} detected! " +
                $"{newInstanceTypeName} is a singleton and should have only one instance. " +
                $"The first instance is assigned to \"{currentInstanceGameObjectName}\". " +
                $"Attempted to assign \"{newInstanceGameObject}\" as a new instance. " +
                $"The component on \"{newInstanceGameObjectName}\" will now be destroyed.");
            UnityObject.Destroy(this.LocalInstance);

            return;
        }

        SingletonMonoBehaviour<T>.Instance = this.LocalInstance;
        UnityObject.DontDestroyOnLoad(SingletonMonoBehaviour<T>.Instance);

        this.OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
}
