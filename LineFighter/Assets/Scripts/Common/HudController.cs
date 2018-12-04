using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    private GameObject _hud;
    private GameObject _fpsCounter;
    private GameObject _pencilSprite;
    private GameObject _eraserSprite;

    public DrawType DrawMode { get; private set; }

    public enum DrawType
    {
        Draw,
        Erase
    }

    // Use this for initialization
    void Start ()
    {
        InitializeHud();
    }

    // Update is called once per frame
    void Update()
    {
        // Set Draw/Erase UI
        if (Input.GetKey(KeyCode.Q))
        {
            DrawMode = DrawType.Draw;
            _pencilSprite.SetActive(true);
            _eraserSprite.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            DrawMode = DrawType.Erase;
            _pencilSprite.SetActive(false);
            _eraserSprite.SetActive(true);
        }
    }

    private void InitializeHud()
    {
        _hud = GameObject.Find(Fields.GameObjects.HUD);
        _pencilSprite = GameObject.Find(Fields.GameObjects.PencilSprite);
        _eraserSprite = GameObject.Find(Fields.GameObjects.EraserSprite);
        _fpsCounter = GameObject.Find(Fields.GameObjects.FpsCounter);

        _eraserSprite.SetActive(false);
        DrawMode = DrawType.Draw;

        if (!HudSettings.FpsCounterActive)
        {
            _fpsCounter.SetActive(false);
        }
    }
}

