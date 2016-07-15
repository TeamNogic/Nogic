using UnityEngine;
using System.Collections;

public class Obj_Scale : MonoBehaviour
{
    private Vector3 start;                  //開始
    private Vector3 Now;                    //現在

    private float OldDistance;              //前回フレームの残り移動距離

    public Vector3 Target;                  //目標
    public float Speed;                     //変化差分速度
    public bool DestroyObjectFlag = false;  //オブジェクトごと削除するか

    public void Restart()
    {
        Start();
    }

    void Start()
    {
        //初期値設定
        start = this.transform.localScale;
        Now = this.transform.localScale;

        OldDistance = Vector3.Distance(Target, Now);
    }

    void Update()
    {
        //変化と適用
        Now += (Target - start) * Time.deltaTime * Speed;
        this.transform.localScale = Now;

        //前の距離－現在の距離でマイナスになったら、目標を超えたことになる
        if (OldDistance - Vector3.Distance(Target, Now) <= 0.0f)
        {
            //ズレ補正
            this.transform.localScale = Target;

            //フラグで分岐消去
            if (DestroyObjectFlag) Destroy(this.gameObject);

            Destroy(GetComponent<Obj_Scale>());
        }

        //距離を保存
        else OldDistance = Vector3.Distance(Target, Now);
    }
}
