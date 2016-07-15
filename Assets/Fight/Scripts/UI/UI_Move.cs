using UnityEngine;
using System.Collections;

public class UI_Move : MonoBehaviour
{
    private Vector3 target;   //移動目標
    private float prev;

    public void Setup(Vector3 move)
    {
        target = this.GetComponent<RectTransform>().localPosition + move;
        prev = Vector2.Distance(target, this.GetComponent<RectTransform>().localPosition);

        Start();
    }

    public void Setup_Target(Vector3 move)
    {
        target = move;
        prev = Vector2.Distance(target, this.GetComponent<RectTransform>().localPosition);

        Start();
    }

    public Vector3 getTarget()
    {
        return target;
    }

    void Start()
    {

    }

    void Update()
    {
        //移動
        this.GetComponent<RectTransform>().localPosition += (target - this.GetComponent<RectTransform>().localPosition) * Time.deltaTime * 4.0f;

        //接近したら
        if (prev < Vector2.Distance(target, this.GetComponent<RectTransform>().localPosition))
        {
            //ズレ防止
            this.GetComponent<RectTransform>().localPosition = target;
            //移動終了
            Destroy(this.GetComponent<UI_Move>());
        }

        prev = Vector2.Distance(target, this.GetComponent<RectTransform>().localPosition);
    }
}
