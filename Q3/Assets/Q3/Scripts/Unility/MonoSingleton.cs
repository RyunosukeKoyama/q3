using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool _dontDestroyOnLoad = default;

    private static T _instance;

    public static T I
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();
            if (_instance != null) return _instance;

            var obj = new GameObject(typeof(T).Name);
            _instance = obj.AddComponent<T>();

            return _instance;
        }
    }

    // 継承先でも呼ぶこと
    protected virtual void Awake()
    {
        if (!Application.isEditor && _dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

public class NonMonoSingleton<T> where T : class, new()
{
    private static T _instance;
    public static T I => _instance ??= new T();

    public NonMonoSingleton()
    {
    }
}
