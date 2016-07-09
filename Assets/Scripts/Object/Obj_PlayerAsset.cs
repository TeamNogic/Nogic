using UnityEngine;
using System.Collections;

public class Obj_PlayerAsset : MonoBehaviour
{
    public Sprite thumbnail;        //サムネイル画像

    public AudioClip startSound;    //登場時ボイス
    public AudioClip nodeSound;     //ノードセレクト開始時ボイス
    public AudioClip attackSound;   //攻撃時ボイス
    public AudioClip damageSound;   //ダメージ受け時ボイス
}
