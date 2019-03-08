using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoLGameController : MonoBehaviour
{
    #region Events..
    void Awake()
    {
        SceneManager.activeSceneChanged += SpawnCharacters;
    }


    void Start()
    {

    }

    void Update()
    {

    }
    #endregion Events..

    #region Methods..
    private void SpawnCharacters(Scene current, Scene next)
    {
        // Player
        Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Player.PlayerPrefab]);

        // Enemy
        if (next.name == Fields.Scenes.LordOfFunk)
        {
            Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Enemy.LordOfFunkPrefab]);
        }
    }
    #endregion Methods..
}
