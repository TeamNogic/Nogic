using UnityEngine;
using System.Collections;

public class UI_Alpha : MonoBehaviour {

    private float now;                      //現在

    private bool move;                      //進行方向

    public float start;                     //開始
    public float target;                    //目標
    public float speed;                     //変化速度
    public bool destroyObjectFlag = false;  //オブジェクトごと削除するか

    CanvasRenderer canvasRenderer;

    public void Setup(float _start, float _target, float _speed, bool _destryoyObjectFlag)
    {
        start = _start;
        target = _target;
        speed = _speed;
        destroyObjectFlag = _destryoyObjectFlag;

        Start();
    }

    void Start()
    {
        canvasRenderer = this.GetComponent<CanvasRenderer>();

        //初期値設定
        now = start;

        //進行方向確認
        move = 0.0f < target - now;

        canvasRenderer.SetColor(
            new Color(canvasRenderer.GetColor().r
            , canvasRenderer.GetColor().g
            , canvasRenderer.GetColor().b
            , now));
    }

    void Update()
    {
        //変化と適用
        now += (move ? speed : -speed) * Time.deltaTime;

        canvasRenderer.SetColor(
            new Color(canvasRenderer.GetColor().r
            , canvasRenderer.GetColor().g
            , canvasRenderer.GetColor().b
            , now));

        //目標に到達したら
        if ((move && target <= now) || (!move && now <= target))
        {
            //ズレ補正
            canvasRenderer.SetColor(
            new Color(canvasRenderer.GetColor().r
            , canvasRenderer.GetColor().g
            , canvasRenderer.GetColor().b
            , target));

            //フラグで分岐消去
            if (destroyObjectFlag) Destroy(this.gameObject);

            Destroy(this.GetComponent<UI_Alpha>());
        }
    }
}
