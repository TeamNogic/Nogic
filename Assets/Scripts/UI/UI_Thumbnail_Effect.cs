using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class UI_Thumbnail_Effect : MonoBehaviour
{
    public Image create;

    public string canvasName;

    void Update()
    {
        if (this.GetComponent<UI_Scale>() == null)
        {
            //生成
            create = Instantiate(create, Vector2.zero, Quaternion.identity) as Image;
            //画像継承
            create.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;

            //キャンバス
            create.transform.SetParent(GameObject.Find(canvasName).transform, false);
            //優先順位変更
            create.transform.SetSiblingIndex(create.GetComponent<Image>().transform.GetSiblingIndex() - 1);

            //座標変更
            create.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;

            //削除
            Destroy(this.GetComponent<UI_Thumbnail_Effect>());
        }
    }
}
