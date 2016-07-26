using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sys_PlayerData
{
    public int TotalDamage = 0;                         //自身が与えた合計ダメージ

    public List<string> Weak = new List<string>();      //弱点
    public List<string> AddNode = new List<string>();   //相手から貰えるノード
    
    public int State_Tern = 0;                          //0:無し　1:ポイズン　2:パラサイト
    public int State_Tern_Time = 0;                     //状態異常ターン数
                                                        
    public int State_Status = 0;                        //0:無し　1:ウィーク　2:ダウン　3:アブソープション
    public int State_Status_Time = 0;                   //状態異常ターン数
    
    public int State_NodeKey = 0;                       //0:無し　1:リバース　2:カオス　3:フリーズ　4:ホールド
    public int State_NodeKey_Time = 0;                  //状態異常ターン数
    
    public int State_NodeHindrance = 0;                 //0:無し　1:スモーク　2:フェスティバル
    public int State_NodeHindrance_Time = 0;            //状態異常ターン数
    
    public int State_NodeEditor = 0;                    //0:無し　1:シールド　2:ヘル　3:ブレイク
    public int State_NodeEditor_Time = 0;               //状態異常ターン数
}

[System.Serializable]
public class Sys_PlayerData_TernWait
{
    public List<string> AddNode = new List<string>();   //相手から貰えるノード

    public int State_Tern = 0;                          //0:無し　1:ポイズン　2:パラサイト
    public int State_Tern_Time = 0;                     //状態異常ターン数

    public int State_Status = 0;                        //0:無し　1:ウィーク　2:ダウン　3:アブソープション
    public int State_Status_Time = 0;                   //状態異常ターン数

    public int State_NodeKey = 0;                       //0:無し　1:リバース　2:カオス　3:フリーズ　4:ホールド
    public int State_NodeKey_Time = 0;                  //状態異常ターン数

    public int State_NodeHindrance = 0;                 //0:無し　1:スモーク　2:フェスティバル
    public int State_NodeHindrance_Time = 0;            //状態異常ターン数

    public int State_NodeEditor = 0;                    //0:無し　1:シールド　2:ヘル　3:ブレイク
    public int State_NodeEditor_Time = 0;               //状態異常ターン数
}

[System.Serializable]
public class Sys_Action_Object
{
    public string Type = "フィジックス";    //属性
    public string Shape = "ナイフ";         //形状

    public int Number = 1;                  //生成するオブジェクトの数
    public float Scale = 1.0f;              //オブジェクト単体の大きさ
    public float Speed = 1.0f;              //オブジェクト単体の移動速度
    public int Move = 5;                    //移動方法

    public int State_Tern = 0;              //0:無し　1:ポイズン　2:パラサイト
    public int State_Status = 0;            //0:無し　1:ウィーク　2:ダウン　3:アブソープション
    public int State_NodeKey = 0;           //0:無し　1:リバース　2:カオス　3:フリーズ　4:バレッジ
    public int State_NodeHindrance = 0;     //0:無し　1:スモーク　2:フェスティバル
    public int State_NodeEditor = 0;        //0:無し　1:シールド　2:ヘル　3:ブレイク
}

[System.Serializable]
public class Sys_Action_UI
{
    public bool isCritical = false;                //クリティカルヒットしたかどうか
    public bool isTypeWeak = false;                //弱点であるかどうか

    public List<int> Damage = new List<int>();     //１つの物体が与えるダメージ（計算済み）

    //消去予定

    public int State_Tern = 0;                     //0:無し　1:ポイズン　2:パラサイト（UIで毒や寄生虫と表示）
    public int State_Tern_Time = 1;                //ポイズン or パラサイトの効果ターン数（UIでターン数表示）

    public int State_Status = 0;                   //0:無し　1:ウィーク　2:ダウン　3:アブソープション（UIで状態異常内容を表示）
    public int State_NodeHindrance_Time = 1;       //スモーク or フェスティバルの効果ターン数（UIでターン数だけ表示）

    public bool State_Etc = false;                 //妨害が発生するかどうか（UIで「ノード妨害追加！」と出る）
}

[System.Serializable]
public class Sys_Status : MonoBehaviour
{
    public static string stageName = "Map_1";                                   //ステージ名

    public static int activePlayer = 0;                                         //攻撃中のプレイヤー
    public static int targetPlayer = 1;                                         //攻撃を受けるプレイヤー
    
    public static List<Sys_PlayerData> Player = new List<Sys_PlayerData>();     //プレイヤーのステータス
    public static List<Sys_PlayerData_TernWait> Player_Wait
        = new List<Sys_PlayerData_TernWait>();                                  //プレイヤーの次のターン反映用ステータス

    public static Sys_Action_Object Action_Object = new Sys_Action_Object();    //アクションシーンで３Ｄオブジェクトに役立つ情報
    public static Sys_Action_UI Action_UI = new Sys_Action_UI();                //アクションシーンでＵＩに役立つ情報

    public static List<string> Attack_Name = new List<string>();                //選択されたノード名
    public static List<string> Attack_Comment = new List<string>();             //選択されたノードコメント

    public static void ForcedReset()
    {
        stageName = "Map_1";

        activePlayer = 0;
        targetPlayer = 1;

        Player.Clear();
        Player_Wait.Clear();

        Attack_Name.Clear();
        Attack_Comment.Clear();

        Sys_Node.NodeList.Clear();
        Sys_Node.Select.Clear();
    }
}
