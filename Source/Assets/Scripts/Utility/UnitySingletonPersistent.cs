using UnityEngine;

namespace UtilityCode
{

    public class UnitySingletonPersistent<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static bool Quitting { get; private set; }
		
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError($"Instance of {typeof(T)} not found");
                }

                return instance;
            }
        }
		
        #region  Methods
		
        public virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            Quitting = true;
        }
        #endregion
    }
}