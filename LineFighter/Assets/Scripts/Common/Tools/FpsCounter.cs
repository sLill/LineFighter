using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    #region Member Variables
    private Text _fpsText;
    private Stopwatch _stopwatch;
    private int _frameCounter = 0;
    #endregion Member Variables

    #region MonoBehaviour
    void Start()
    {
        _fpsText = this.GetComponentInParent<Text>();

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopwatch.ElapsedMilliseconds < 1000)
        {
            _frameCounter++;
        }
        else
        {
            _fpsText.text = _frameCounter.ToString();
            _frameCounter = 0;
            _stopwatch.Stop();
            _stopwatch = Stopwatch.StartNew();
        }
    }
    #endregion MonoBehaviour
}
