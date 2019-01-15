using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetLibrary
{
    #region Properties..
    public static Dictionary<string, Sprite> UiAssets { get; private set; }

    public static Dictionary<string, Material> MaterialAssets { get; private set; }
    #endregion Properties..

    #region Methods..
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
    #endregion Methods..

}
