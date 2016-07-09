using UnityEngine;
using System.Collections;

public class UI_Rotate : MonoBehaviour
{
    private Vector3 start;                  //開始
    private Vector3 now;                    //現在

    private float oldDistance;              //前回フレームの残り移動距離

    public Vector3 target;                  //目標
    public float speed;                     //変化差分速度
    public bool destroyObjectFlag = false;  //オブジェクトごと削除するか
    public bool infinity = false;           //動作終了せずに無限ループするか

    public void Setup(Vector3 _target, float _speed, bool _destryoyObjectFlag)
    {
        target = _target;
        speed = _speed;
        destroyObjectFlag = _destryoyObjectFlag;

        Start();
    }

    void Start()
    {
        //初期値設定
        start = this.transform.localEulerAngles;
        now = this.transform.localEulerAngles;

        oldDistance = Vector3.Distance(target, now);
    }

    void Update()
    {
        //変化と適用
        now += (target - start) * Time.deltaTime * speed;
        this.transform.localEulerAngles = now;

        //前の距離－現在の距離で増加にしたら、目標を超えたことになる
        if (oldDistance <= Vector3.Distance(target, now) && !infinity)
        {
            //ズレ補正
            this.transform.localEulerAngles = target;

            //フラグで分岐消去
            if (destroyObjectFlag) Destroy(this.gameObject);

            Destroy(this.GetComponent<UI_Scale>());
        }
        //距離を保存
        else oldDistance = Vector3.Distance(target, now);
    }
}
