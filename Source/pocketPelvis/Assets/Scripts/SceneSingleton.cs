﻿using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton for the scene.
/// e.g. public class MyClassName : SceneSingleton<MyClassName> {}
/// The lifetime of this singleton is the duration that the scene is active.
/// When a scene is loaded, the singleton is destroyed.
/// </summary>
public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[SceneSingleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (SceneSingleton)";
                    }
                }

                return m_Instance;
            }
        }
    }



    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
}