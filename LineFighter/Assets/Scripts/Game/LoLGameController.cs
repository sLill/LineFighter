using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoLGameController : MonoBehaviour
{
    #region Events..
    void Awake()
    {
        SceneManager.activeSceneChanged += SpawnPlayer;
    }


    void Start()
    {

    }

    void Update()
    {

    }
    #endregion Events..

    #region Methods..
    private void SpawnPlayer(Scene current, Scene next)
    {
        Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.PlayerPrefab]);
    }
    #endregion Methods..
}
