using UnityEngine;
using System.Collections;

public class Sys_Line : MonoBehaviour
{
    LineRenderer line;          //線描画

    public GameObject Target;   //対象のオブジェクト

    void Start()
    {
        //線の初期設定
        line = gameObject.AddComponent<LineRenderer>();
        line.material.color = Color.yellow;
        line.SetVertexCount(2);
    }

    void Update()
    {
        //ターゲットが存在する場合は線を引く
        if (Target != null)
        {
            line.SetWidth(0.3f, 0.3f);

            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, Target.transform.position);
        }
        //無い場合は見えないようにする
        else line.SetWidth(0.0f, 0.0f);
    }
}
