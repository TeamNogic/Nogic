using UnityEngine;
using System.Collections;

public class UI_Move : MonoBehaviour
{
    private Vector3 target;   //移動目標

    public void Setup(Vector3 move)
    {
        target = this.GetComponent<RectTransform>().position + move;

        Start();
    }

    void Start()
    {

    }

    void Update()
    {
        //移動
        this.GetComponent<RectTransform>().position += (target - this.GetComponent<RectTransform>().position) * Time.deltaTime * 4.0f;

        //接近したら
        if (Vector2.Distance(target, this.GetComponent<RectTransform>().position) <= 0.01f)
        {
            //ズレ防止
            this.GetComponent<RectTransform>().position = target;
            //移動終了
            Destroy(this.GetComponent<UI_Move>());
        }
    }
}
