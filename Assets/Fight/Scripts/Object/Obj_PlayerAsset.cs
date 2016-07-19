using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Obj_PlayerAsset : MonoBehaviour
{
    public Sprite thumbnail;        //サムネイル画像

    public AudioClip startSound;    //登場時ボイス
    public AudioClip nodeSound;     //ノードセレクト開始時ボイス
    public AudioClip attackSound;   //攻撃時ボイス
    public AudioClip damageSound;   //ダメージ受け時ボイス
    public AudioClip winSound;      //勝利ボイス
    public AudioClip loseSound;     //敗退ボイス

    public List<string> weak;       //弱点属性
}
