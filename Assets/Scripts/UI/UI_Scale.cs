using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class UI_Scale : MonoBehaviour
{
    private Vector2 start;                  //開始
    private Vector2 Now;                    //現在

    private float OldDistance;              //前回フレームの残り移動距離
    
    public Vector2 Target;                  //目標
    public float Speed;                     //変化差分速度
    public bool DestroyObjectFlag = false;  //オブジェクトごと削除するか
    
    public void Setup(Vector2 _target, float _speed, bool _destryoyObjectFlag)
    {
        Target = _target;
        Speed = _speed;
        DestroyObjectFlag = _destryoyObjectFlag;

        Start();
    }

    void Start()
    {
        //初期値設定
        start = this.transform.localScale;
        Now = this.transform.localScale;
        
        OldDistance = Vector2.Distance(Target, Now);
    }

    void Update()
    {
        //変化と適用
        Now += (Target - start) * Time.deltaTime * Speed;
        this.transform.localScale = Now;

        //前の距離－現在の距離で増加にしたら、目標を超えたことになる
        if (OldDistance <= Vector2.Distance(Target, Now))
        {
            //ズレ補正
            this.transform.localScale = Target;

            //フラグで分岐消去
            if (DestroyObjectFlag) Destroy(this.gameObject);

            Destroy(this.GetComponent<UI_Scale>());
        }
        //距離を保存
        else OldDistance = Vector2.Distance(Target, Now);
    }
}
