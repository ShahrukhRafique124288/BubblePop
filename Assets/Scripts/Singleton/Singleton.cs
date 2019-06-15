using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    private static object _lock = new object();
    
    
    public static T Instance
    {
        get
        {   
            lock(_lock)
            {
                if (_instance == null)
                {
                    _instance = (T) FindObjectOfType(typeof(T));
                    if ( _instance != null && FindObjectsOfType(typeof(T)).Length > 1 )
                    {
                        return null;
                    }
                    
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        //singleton.hideFlags = HideFlags.HideInHierarchy;
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) "+ typeof(T).ToString();
                    } 
                }
                
                return _instance;
            }
        }
    }
}