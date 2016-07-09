﻿using UnityEngine;
using System.Collections;

public class UI_NodeMove : MonoBehaviour
{
    public int angle;           //移動角度
    public GameObject target;   //移動開始オブジェクト

    private Vector3 position;   //現在の座標
    
    public void Setup(int _angle, GameObject _target)
    {
        angle = _angle;
        target = _target;

        Start();
    }

    void Start()
    {
        //ラジアン変換
        float Radian = angle * Mathf.PI / 180.0f;
        
        //移動開始オブジェクトも、このスクリプトで移動中だった場合は、参照場所を変化させる
        if (target.GetComponent<UI_NodeMove>() != null) position = target.GetComponent<UI_NodeMove>().position + new Vector3(Mathf.Cos(Radian), Mathf.Sin(Radian), 0.0f) * Screen.height * 0.2f;
        else position = target.GetComponent<RectTransform>().position + new Vector3(Mathf.Cos(Radian), Mathf.Sin(Radian), 0.0f) * Screen.height * 0.2f;
    }

    void Update()
    {
        //移動
        this.GetComponent<RectTransform>().position += (position - this.GetComponent<RectTransform>().position) * Time.deltaTime * 4.0f;

        //接近したら
        if (Vector2.Distance(position, this.GetComponent<RectTransform>().position) <= 0.01f)
        {
            //ズレ防止
            this.GetComponent<RectTransform>().position = position;
            //移動終了
            Destroy(this.GetComponent<UI_NodeMove>());
        }
    }
}
