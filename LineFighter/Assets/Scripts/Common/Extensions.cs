using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions 
{
    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();

        SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<T>(true)));

        return results;
    }
}
