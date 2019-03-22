using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AssetLibrary
{
    #region Properties..
    public static Dictionary<string, Sprite> UiAssets { get; private set; }

    public static Dictionary<string, Material> MaterialAssets { get; private set; }

    public static Dictionary<string, GameObject> CommonPrefabAssets { get; private set; }

    public static Dictionary<string, GameObject> EnemyPrefabAssets { get; private set; }

    public static Dictionary<string, GameObject> PlayerPrefabAssets { get; private set; }
    #endregion Properties..

    #region Methods..
    public static void LoadBaseAssets()
    {
        LoadMaterialAssets();
        LoadAllPrefabAssets();
        LoadUiAssets();
    }

    public static void LoadMaterialAssets()
    {
        MaterialAssets = new Dictionary<string, Material>();
        var materials = Resources.LoadAll<Material>(Fields.AssetPaths.Materials);

        foreach (Material material in materials)
        {
            MaterialAssets[material.name] = material;
        }
    }

    public static void LoadUiAssets()
    {
        UiAssets = new Dictionary<string, Sprite>();
        var assets = Resources.LoadAll<Sprite>(Fields.AssetPaths.Ui);

        foreach (Sprite sprite in assets)
        {
            UiAssets[sprite.name] = sprite;
        }
    }

    public static void LoadAllPrefabAssets()
    {
        CommonPrefabAssets = new Dictionary<string, GameObject>();
        EnemyPrefabAssets = new Dictionary<string, GameObject>();
        PlayerPrefabAssets = new Dictionary<string, GameObject>();

        var commonPrefabAssets = Resources.LoadAll<GameObject>(Fields.AssetPaths.PrefabsCommon);
        var enemyPrefabAssets = Resources.LoadAll<GameObject>(Fields.AssetPaths.PrefabsEnemy);
        var playerPrefabAssets = Resources.LoadAll<GameObject>(Fields.AssetPaths.PrefabsPlayer);

        // Common
        foreach (GameObject prefab in commonPrefabAssets)
        {
            CommonPrefabAssets[prefab.name] = prefab;
        }

        // Enemy
        foreach (GameObject prefab in enemyPrefabAssets)
        {
            EnemyPrefabAssets[prefab.name] = prefab;
        }

        // Player
        foreach (GameObject prefab in playerPrefabAssets)
        {
            PlayerPrefabAssets[prefab.name] = prefab;
        }
    }
    #endregion Methods..
}
