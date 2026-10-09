using UnityEngine;

namespace _Project.Code.AidenStuff
{
    // Generic Singleton base class to ensure a single instance of a MonoBehaviour-derived class
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isQuitting;

        protected virtual bool PersistBetweenScenes => true;

        public static T Instance
        {
            get
            {
                // During shutdown, never create anything. Return whatever reference we
                // already have so existing code like Instance.Event -= Handler keeps working.
                if (_isQuitting) return _instance;

                // Look for one in the scene before making one. Without this step,
                // asking for a manager early (ManagerSpawner does it in Awake)
                // builds a bare GameObject, and the properly configured manager
                // sitting in the scene then finds _instance taken and destroys
                // itself. You lose whatever was set on it in the inspector: the
                // AudioSource on AudioManager, the clue total on ClueTracker.
                // Which one wins depends on script execution order, so it breaks
                // intermittently and blames the wrong thing.
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }

                if (_instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>(); // Awake handles DontDestroyOnLoad
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _isQuitting = false;

            if (_instance == null)
            {
                _instance = this as T;
                if (PersistBetweenScenes) DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}