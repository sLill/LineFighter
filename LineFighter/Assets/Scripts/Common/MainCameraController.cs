using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Draw/Erase
        if (Input.GetMouseButtonDown(1))
        {
            HudController hudController = GameObject.FindObjectOfType<HudController>();
            Sprite cursorSprite;
            Vector2 hotspot;

            if (hudController.DrawMode == HudController.DrawType.Draw)
            {
                cursorSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(Fields.AssetPaths.DrawCursor);
                hotspot = new Vector2(0, 48);
            }
            else
            {
                string path = string.Empty;
                switch (Eraser.Size)
                {
                    case Eraser.EraserSize.Small:
                        path = Fields.AssetPaths.EraseCursorSmall;
                        break;
                    default:
                        path = Fields.AssetPaths.EraseCursorSmall;
                        break;
                }

                cursorSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                hotspot = new Vector2(24, 24);
            }

            Cursor.SetCursor(cursorSprite.texture, hotspot, CursorMode.Auto);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
