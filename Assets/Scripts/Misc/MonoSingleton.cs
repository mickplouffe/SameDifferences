using UnityEngine;

/// <summary>
/// Mono singleton Class. Extend this class to make singleton component.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log(typeof(T).ToString() + " is NULL.");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this as T;
        }
        Init();
    }


    public virtual void Init() { }

    private void OnApplicationQuit()
    {
        _instance = null;
    }
}
