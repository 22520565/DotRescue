#nullable enable

namespace Game;

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
            UnityObject.Destroy(this.LocalInstance.gameObject);

            return;
        }

        SingletonMonoBehaviour<T>.Instance = this.LocalInstance;
        UnityObject.DontDestroyOnLoad(SingletonMonoBehaviour<T>.Instance.gameObject);

        this.OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
}
