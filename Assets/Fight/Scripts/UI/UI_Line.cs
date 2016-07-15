using UnityEngine;
using System.Collections;

public class UI_Line : MonoBehaviour
{
    public GameObject Target;   //対象のオブジェクト

    private Texture2D texture = new Texture2D(Screen.width, Screen.height);
    UI_DrawTexture2D drawTex2D = new UI_DrawTexture2D();

    void Start()
    {
        GUITexture tex = GetComponent<GUITexture>();
        tex.texture = texture;

        // テクスチャを真ん中に持ってきます
        Rect rect = new Rect();
        rect.Set(0.0f, 0.0f, Screen.width, Screen.height);
        tex.pixelInset = rect;
        
        // テクスチャへ描画開始.
        drawTex2D.Begin(texture);
    }

    void Update()
    {
        drawTex2D.Clear(new Color(0.0f, 0.0f, 0.0f));

        // 線の描画
        drawTex2D.DrawLine((int)this.transform.position.x, (int)this.transform.position.y, (int)Target.transform.position.x, (int)Target.transform.position.y, new Color(1.0f, 0.0f, 0.0f));
        // テクスチャへ描画終了.

        drawTex2D.End();
    }
}
