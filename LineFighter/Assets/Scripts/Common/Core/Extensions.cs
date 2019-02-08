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

    public static float GetDistance(Vector3 positionOne, Vector3 positionTwo)
    {
        return Mathf.Sqrt(Mathf.Pow((positionOne.x - positionTwo.x), 2) + Mathf.Pow((positionOne.y - positionTwo.y), 2));
    }
}
