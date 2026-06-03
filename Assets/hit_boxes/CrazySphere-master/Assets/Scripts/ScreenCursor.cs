using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    public Texture CursorTexture;

    void Start()
    {
        Cursor.visible = true;
    }

    void OnGUI()
    {
        if (CursorTexture == null) return;

        float left = Input.mousePosition.x - CursorTexture.width / 2;
        float top = Screen.height - Input.mousePosition.y - CursorTexture.height / 2;

        GUI.DrawTexture(new Rect(left, top, CursorTexture.width, CursorTexture.height), CursorTexture);
    }
}