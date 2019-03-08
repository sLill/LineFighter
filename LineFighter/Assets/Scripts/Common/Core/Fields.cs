using UnityEngine;

public static class Fields
{
    #region Animator
    public static class Animator
    {
        public const string Airborne = "Airborne";

        public const string Moving = "Moving";

        public const string Speed = "Speed";
    }
    #endregion Animator

    #region Assets
    public static class Assets
    {
        #region Materials
        public static class Materials
        {
            public const string EraseCursorSmall = "Erase_Cursor_Small";

            public const string LineMaterialBase = "LineMaterial_Base";
        } 
        #endregion Materials

        #region Prefabs
        public static class Prefabs
        {
            #region Common
            public static class Common
            {
                public const string Bullet = "Bullet";

                public const string ColliderPrefab = "Collider";

                public const string LineObjectPrefab = "LineObject";

                public const string TrailPrefab = "TrailPrefab";
            }
            #endregion Common

            #region Enemy
            public static class Enemy
            {
                public const string LordOfFunkPrefab = "LordOfFunk";
            }
            #endregion Enemy

            #region Player
            public static class Player
            {
                public const string PlayerPrefab = "Player";

                public const string PlayerLinesPrefab = "PlayerLines";
            }
            #endregion Player

            #region UI
            public static class UI
            {
                public const string SettingsMenuPrefab = "SettingsPanel";
            }
            #endregion UI
        } 
        #endregion Prefabs
    }
    #endregion Assets

    #region AssetPaths
    public static class AssetPaths
    {
        public const string Ui = "Artwork/UI";

        public const string Materials = "Materials";

        public const string Prefabs = "Prefabs";

        public const string PrefabsCommon = "Prefabs/Common";

        public const string PrefabsEnemy = "Prefabs/Enemy";

        public const string PrefabsPlayer = "Prefabs/Player";

        public const string PrefabsUI = "Prefabs/UI";
    }
    #endregion AssetPaths

    #region GameObjects
    public static class GameObjects
    {
        public const string AboutButton = "AboutButton";

        public const string ApplyButton = "ApplyButton";

        public const string CancelButton = "CancelButton";

        public const string EraserGauge = "EraserGauge";

        public const string EraserGaugeContainer = "EraserGaugeContainer";

        public const string EraserSprite = "EraserSprite";

        public const string EraserCursor = "EraserCursor";

        public const string ExitButton = "ExitButton";

        public const string EventSystem = "EventSystem";

        public const string FpsCounter = "FpsCounter";

        public const string Game = "Game";

        public const string HUD = "HUD";

        public const string LineObject = "LineObject";

        public const string LordsOfLineButton = "LordsOfLineButton";

        public const string MainCamera = "MainCamera";

        public const string MultiplayerButton = "MultiplayerButton";

        public const string PencilGauge = "PencilGauge";

        public const string PencilGaugeContainer = "PencilGaugeContainer";

        public const string PencilSprite = "PencilSprite";

        public const string Platform = "Platform";

        public const string Player = "Player";

        public const string PlayerLines = "PlayerLines";

        public const string Projectiles = "Projectiles";

        public const string SavedText = "SavedText";

        public const string SettingsPanel = "SettingsPanel";

        public const string SettingsButton = "SettingsButton";
    }
    #endregion GameObjects

    #region Scenes
    public static class Scenes
    {
        public const string LordOfFunk = "LoL_LordOfFunk";

        public const string SceneTemplate = "SceneTemplate";
    }
    #endregion Scenes

    #region Settings
    public static class Settings
    {
        // Display
        public const string FrameRateCap = "FrameRateCap";

        public const string VSyncEnabled = "VSyncEnabled";

        // Hud
        public const string FpsCounterActive = "FpsCounterActive";

        // Keyboard
        public const string DrawModeKey = "DrawModeKey";
    }
    #endregion Settings

    #region Tags
    public static class Tags
    {
        public const string Enemy = "Enemy";

        public const string LineObject = "LineObject";

        public const string LineObjectCollider = "LineObjectCollider";

        public const string NetworkLobbyPlayer = "NetworkLobbyPlayer";

        public const string Player = "Player";

        public const string PlayerLines = "PlayerLines";

        public const string EnemyProjectile = "EnemyProjectile";

        public const string PlayerProjectile = "PlayerProjectile";

        public const string DrawFuel = "DrawFuel";

        public const string EraseFuel = "EraseFuel";
    } 
    #endregion Tags
}
