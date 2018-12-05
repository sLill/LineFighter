using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private float _drawAlphaFocus;
    private float _drawAlphaUnfocus;
    private Image _eraserGauge;
    private Image _eraserGaugeContainer;
    private SpriteRenderer _eraserSpriteRenderer;
    private GameObject _fpsCounter;
    private GameObject _hud;
    private Image _pencilGauge;
    private Image _pencilGaugeContainer;
    private SpriteRenderer _pencilSpriteRenderer;

    public DrawType DrawMode { get; private set; }

    public enum DrawType
    {
        Draw,
        Erase
    }

    // Use this for initialization
    void Start()
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
            UpdateDrawUI();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            DrawMode = DrawType.Erase;
            UpdateDrawUI();
        }
    }

    private void InitializeHud()
    {
        _drawAlphaFocus = 1f;
        _drawAlphaUnfocus = 0.225f;
        _fpsCounter = GameObject.Find(Fields.GameObjects.FpsCounter);
        _hud = GameObject.Find(Fields.GameObjects.HUD);
        _eraserGauge = GameObject.Find(Fields.GameObjects.EraserGauge).GetComponent<Image>();
        _eraserGaugeContainer = GameObject.Find(Fields.GameObjects.EraserGaugeContainer).GetComponent<Image>();
        _eraserSpriteRenderer = GameObject.Find(Fields.GameObjects.EraserSprite).GetComponent<SpriteRenderer>();
        _pencilGauge = GameObject.Find(Fields.GameObjects.PencilGauge).GetComponent<Image>();
        _pencilGaugeContainer = GameObject.Find(Fields.GameObjects.PencilGaugeContainer).GetComponent<Image>();
        _pencilSpriteRenderer = GameObject.Find(Fields.GameObjects.PencilSprite).GetComponent<SpriteRenderer>();

        DrawMode = DrawType.Draw;

        if (!HudSettings.FpsCounterActive)
        {
            _fpsCounter.SetActive(false);
        }

        UpdateDrawUI();
    }

    private void UpdateDrawUI()
    {
        Color pencilColor = _pencilSpriteRenderer.color;
        Color pencilGaugeColor = _pencilGauge.color;
        Color eraserColor = _eraserSpriteRenderer.color;
        Color eraserGaugeColor = _eraserGauge.color;

        switch (DrawMode)
        {
            case DrawType.Draw:
                pencilColor.a = _drawAlphaFocus;
                pencilGaugeColor.a = _drawAlphaFocus;
                eraserColor.a = _drawAlphaUnfocus;
                eraserGaugeColor.a = _drawAlphaUnfocus;

                _pencilSpriteRenderer.color = pencilColor;
                _pencilGauge.color = pencilGaugeColor;
                _pencilGaugeContainer.color = pencilColor;

                _eraserSpriteRenderer.color = eraserColor;
                _eraserGauge.color = eraserGaugeColor;
                _eraserGaugeContainer.color = eraserColor;
                break;
            case DrawType.Erase:
                pencilColor.a = _drawAlphaUnfocus;
                pencilGaugeColor.a = _drawAlphaUnfocus;
                eraserColor.a = _drawAlphaFocus;
                eraserGaugeColor.a = _drawAlphaFocus;

                _pencilSpriteRenderer.color = pencilColor;
                _pencilGauge.color = pencilGaugeColor;
                _pencilGaugeContainer.color = pencilColor;

                _eraserSpriteRenderer.color = eraserColor;
                _eraserGauge.color = eraserGaugeColor;
                _eraserGaugeContainer.color = eraserColor;
                break;
        }
    }
}

