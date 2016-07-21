using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Obj_Alpha : MonoBehaviour
{
    private float now;                      //現在

    private bool move;                      //進行方向

    public float target;                    //目標
    public float speed;                     //変化速度
    public bool destroyObjectFlag = false;  //オブジェクトごと削除するか

    public Material mat;

    MeshRenderer meshRenderer;

    public void Setup(float _target, float _speed, bool _destryoyObjectFlag)
    {
        target = _target;
        speed = _speed;
        destroyObjectFlag = _destryoyObjectFlag;

        Start();
    }

    void Awake()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        //初期値設定
        now = mat.GetFloat("_AlphaValue");

        //進行方向確認
        move = 0.0f < target - now;
    }

    void Update()
    {
        //変化と適用
        now += (move ? speed : -speed) * Time.deltaTime;

        //meshRenderer.materials[0].color = 
        //    new Color(meshRenderer.materials[0].color.r
        //    , meshRenderer.materials[0].color.g
        //    , meshRenderer.materials[0].color.b
        //    , now);

        mat.SetFloat("_AlphaValue", now);

        //目標に到達したら
        if ((move && target <= now) || (!move && now <= target))
        {
            //ズレ補正
            //meshRenderer.materials[0].color =
            //new Color(meshRenderer.materials[0].color.r
            //, meshRenderer.materials[0].color.g
            //, meshRenderer.materials[0].color.b
            //, target);

            mat.SetFloat("_AlphaValue", target);

            //フラグで分岐消去
            if (destroyObjectFlag) Destroy(this.gameObject);

            Destroy(this.GetComponent<Obj_Alpha>());
        }
    }
}
