using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Sys_NodeEngine : MonoBehaviour
{
    private Image[] Create = new Image[4];              //選択ノード

    private List<Image> selectNode = new List<Image>(); //選択したノードを収納

    private float selectTime;                           //残り選択時間
    private float soundTime;                            //超えると鳴るサウンド間隔

    private GameObject nodeEditor;                      //キャッチしたノードエディタ

    private bool updateFlag;                            //更新を行うか

    public string nodeEditorName;                       //ノードエディタ名

    public Image baseNode;                              //ベースとなる画像管理オブジェクト

    public Image start;                                 //スタート表示
    public Image decisionBad;                           //妨害ノード選択時
    public Image decisionGood;                          //通常ノード選択時
    public Image decisionGreat;                         //レアノード選択時
    public Image decisionExcellent;                     //エクセレントノード選択時

    public Image paletteImage;                          //ノード画面
    public Image selectImage;                           //ノード選択画面

    public AudioClip timeSound;                         //カウントダウン音
    public AudioClip destroySound;                      //消去時のサウンド

    Sys_ParameterNode AddNode(string name, int rank, Sys_NodeGroup group, int option, string comment)
    {
        Sys_ParameterNode addNode = new Sys_ParameterNode();

        addNode.Name = name;
        addNode.Comment = comment;
        addNode.Rank = rank;
        addNode.Group = group;

        switch (group)
        {
            case Sys_NodeGroup.Core:
                addNode.GroupNext.Add(Sys_NodeGroup.Type);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Type:
                addNode.GroupPrev = Sys_NodeGroup.Core;

                addNode.GroupNext.Add(Sys_NodeGroup.Shape);
                addNode.GroupNext.Add(Sys_NodeGroup.TypePlus);
                addNode.GroupNext.Add(Sys_NodeGroup.TypeCritical);
                addNode.GroupNext.Add(Sys_NodeGroup.Critical);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Shape:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.GroupNext.Add(Sys_NodeGroup.Scale);
                addNode.GroupNext.Add(Sys_NodeGroup.Tern);
                addNode.GroupNext.Add(Sys_NodeGroup.Status);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeKey);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeHindrance);
                addNode.GroupNext.Add(Sys_NodeGroup.NodeEditor);
                addNode.GroupNext.Add(Sys_NodeGroup.Number);

                addNode.Angle = 30;
                break;

            case Sys_NodeGroup.TypePlus:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.Angle = 60;
                break;

            case Sys_NodeGroup.TypeCritical:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.Critical:
                addNode.GroupPrev = Sys_NodeGroup.Type;

                addNode.GroupNext.Add(Sys_NodeGroup.CriticalOption);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.CriticalOption:
                addNode.GroupPrev = Sys_NodeGroup.Critical;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Tern:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.TernTime);

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.TernTime:
                addNode.GroupPrev = Sys_NodeGroup.Tern;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.Scale:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 15;
                break;

            case Sys_NodeGroup.Status:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 30;
                break;

            case Sys_NodeGroup.NodeKey:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 45;
                break;

            case Sys_NodeGroup.NodeHindrance:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.NodeHindranceTime);

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.NodeHindranceTime:
                addNode.GroupPrev = Sys_NodeGroup.NodeHindrance;

                addNode.Angle = 90;
                break;

            case Sys_NodeGroup.NodeEditor:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.Angle = 75;
                break;

            case Sys_NodeGroup.Number:
                addNode.GroupPrev = Sys_NodeGroup.Shape;

                addNode.GroupNext.Add(Sys_NodeGroup.Move);
                addNode.GroupNext.Add(Sys_NodeGroup.NumberScale);
                addNode.GroupNext.Add(Sys_NodeGroup.Speed);

                addNode.Angle = 60;
                break;

            case Sys_NodeGroup.Move:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 0;
                break;

            case Sys_NodeGroup.NumberScale:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 45;
                break;

            case Sys_NodeGroup.Speed:
                addNode.GroupPrev = Sys_NodeGroup.Number;

                addNode.Angle = 90;
                break;
        }

        addNode.Option = option;

        addNode.This = null;

        if (Sys_Node.NodeList.Count != 0) addNode.SelectStart = Sys_Node.NodeList[Sys_Node.NodeList.Count - 1].SelectEnd + 1;
        else addNode.SelectStart = 0;

        addNode.SelectEnd = addNode.SelectStart + 200 / (int)Mathf.Pow(2.0f, (float)rank);

        addNode.Used = true;
        addNode.Penalty = false;

        Sys_Node.NodeList.Add(addNode);
        return addNode;
    }

    void CreateNode()
    {
        //各ノードを生成
        Create[0] = Instantiate(baseNode, new Vector3(-300.0f, 40.0f), Quaternion.identity) as Image;
        Create[0].transform.SetParent(nodeEditor.transform, false);

        Create[1] = Instantiate(baseNode, new Vector3(-200.0f, 120.0f), Quaternion.identity) as Image;
        Create[1].transform.SetParent(nodeEditor.transform, false);

        Create[2] = Instantiate(baseNode, new Vector3(-200.0f, -30.0f), Quaternion.identity) as Image;
        Create[2].transform.SetParent(nodeEditor.transform, false);

        Create[3] = Instantiate(baseNode, new Vector3(-100.0f, 40.0f), Quaternion.identity) as Image;
        Create[3].transform.SetParent(nodeEditor.transform, false);
    }

    void DeleteUI(Image image, float speed)
    {
        if (image.GetComponent<UI_Scale>() == null) image.gameObject.AddComponent<UI_Scale>();
        image.GetComponent<UI_Scale>().Setup(Vector2.zero, speed, true);
    }

    void Start()
    {
        nodeEditor = GameObject.Find("NodeEditor");

        Sys_Node.Select.Clear();
        Sys_Node.NodeList.Clear();

        Sys_Status.Attack_Name.Clear();
        Sys_Status.Attack_Comment.Clear();

        paletteImage = Instantiate(paletteImage, new Vector3(0.0f, 50.0f), Quaternion.identity) as Image;
        paletteImage.transform.SetParent(nodeEditor.transform, false);

        selectImage = Instantiate(selectImage, new Vector3(-200.0f, 50.0f), Quaternion.identity) as Image;
        selectImage.transform.SetParent(nodeEditor.transform, false);

        this.transform.SetParent(nodeEditor.transform, false);

        selectTime = 10.0f;
        soundTime = 9.0f;
        updateFlag = true;

        Sys_Node.Select[Sys_NodeGroup.Core] = AddNode("", 0, Sys_NodeGroup.Core, 0, ""); //コア
        Sys_Node.Select[Sys_NodeGroup.Core].This = this.gameObject;

        AddNode("ファイヤー", 1, Sys_NodeGroup.Type, 0, "火属性");
        AddNode("ウォーター", 1, Sys_NodeGroup.Type, 0, "水属性");
        AddNode("アイス", 1, Sys_NodeGroup.Type, 0, "氷属性");
        AddNode("サンダー", 1, Sys_NodeGroup.Type, 0, "雷属性");
        AddNode("グラス", 1, Sys_NodeGroup.Type, 0, "草属性");
        AddNode("ウィンド", 1, Sys_NodeGroup.Type, 0, "風属性");
        AddNode("ライト", 1, Sys_NodeGroup.Type, 0, "光属性");
        AddNode("ダークネス", 1, Sys_NodeGroup.Type, 0, "闇属性");
        AddNode("ソイル", 1, Sys_NodeGroup.Type, 0, "土属性");
        AddNode("ドラゴン", 1, Sys_NodeGroup.Type, 0, "竜属性");
        AddNode("ゴッド", 1, Sys_NodeGroup.Type, 0, "神属性");
        AddNode("フィジックス", 1, Sys_NodeGroup.Type, 0, "無属性");

        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("ブースト", 4, Sys_NodeGroup.TypePlus, 2, "ダメージ２倍");
        AddNode("バースト", 5, Sys_NodeGroup.TypePlus, 3, "ダメージ３倍");

        AddNode("スター", 3, Sys_NodeGroup.TypeCritical, 1, "弱点属性ダメージ２倍");
        AddNode("ギャラクシー", 5, Sys_NodeGroup.TypeCritical, 2, "弱点属性ダメージ４倍");
        AddNode("グレート", 3, Sys_NodeGroup.TypeCritical, 3, "弱点以外の属性ダメージ２倍");
        AddNode("マーベラス", 5, Sys_NodeGroup.TypeCritical, 4, "弱点以外の属性ダメージ４倍");

        AddNode("ワールド", 2, Sys_NodeGroup.Critical, 2, "クリティカルダメージ２倍");
        AddNode("ノヴァ", 3, Sys_NodeGroup.Critical, 4, "クリティカルダメージ４倍");
        AddNode("ヘヴン", 4, Sys_NodeGroup.Critical, 8, "クリティカルダメージ８倍");

        AddNode("ナイフ", 1, Sys_NodeGroup.Shape, 5, "ダメージボーナス＋５％");
        AddNode("カッター", 1, Sys_NodeGroup.Shape, 10, "ダメージボーナス＋１０％");
        AddNode("ソード", 1, Sys_NodeGroup.Shape, 15, "ダメージボーナス＋１５％");
        AddNode("ニードル", 1, Sys_NodeGroup.Shape, 20, "ダメージボーナス＋２０％");
        AddNode("メイス", 1, Sys_NodeGroup.Shape, 25, "ダメージボーナス＋２５％");
        AddNode("ハンマー", 2, Sys_NodeGroup.Shape, 30, "ダメージボーナス＋３０％");
        AddNode("シックル", 2, Sys_NodeGroup.Shape, 35, "ダメージボーナス＋３５％");
        AddNode("アロー", 2, Sys_NodeGroup.Shape, 40, "ダメージボーナス＋４０％");
        AddNode("ランス", 2, Sys_NodeGroup.Shape, 45, "ダメージボーナス＋４５％");
        AddNode("スピアー", 2, Sys_NodeGroup.Shape, 50, "ダメージボーナス＋５０％");
        AddNode("アックス", 3, Sys_NodeGroup.Shape, 55, "ダメージボーナス＋５５％");
        AddNode("バルディッシュ", 3, Sys_NodeGroup.Shape, 60, "ダメージボーナス＋６０％");
        AddNode("バレット", 3, Sys_NodeGroup.Shape, 65, "ダメージボーナス＋６５％");
        AddNode("スローイングスター", 3, Sys_NodeGroup.Shape, 70, "ダメージボーナス＋７０％");
        AddNode("ハルバード", 3, Sys_NodeGroup.Shape, 75, "ダメージボーナス＋７５％");
        AddNode("フラッシュ", 4, Sys_NodeGroup.Shape, 80, "ダメージボーナス＋８０％");
        AddNode("ソニック", 4, Sys_NodeGroup.Shape, 85, "ダメージボーナス＋８５％");
        AddNode("レーザー", 4, Sys_NodeGroup.Shape, 90, "ダメージボーナス＋９０％");
        AddNode("ボム", 4, Sys_NodeGroup.Shape, 95, "ダメージボーナス＋９５％");
        AddNode("メテオ", 4, Sys_NodeGroup.Shape, 100, "ダメージボーナス＋１００％");

        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("ファルコン", 2, Sys_NodeGroup.CriticalOption, 25, "クリティカル率プラス１５％");
        AddNode("フェニックス", 3, Sys_NodeGroup.CriticalOption, 50, "クリティカル率プラス４０％");
        AddNode("リーパー", 4, Sys_NodeGroup.CriticalOption, 75, "クリティカル率プラス６５％");
        AddNode("ゼウス", 5, Sys_NodeGroup.CriticalOption, 100, "必ずクリティカルヒット");

        AddNode("スーパー", 1, Sys_NodeGroup.Scale, 2, "更にダメージ２倍");
        AddNode("ハイパー", 3, Sys_NodeGroup.Scale, 4, "更にダメージ４倍");
        AddNode("アルティメット", 5, Sys_NodeGroup.Scale, 8, "更にダメージ８倍");

        AddNode("ポイズン", 1, Sys_NodeGroup.Tern, 1, "ターンごとに定期ダメージ");
        AddNode("パラサイト", 1, Sys_NodeGroup.Tern, 2, "ターンごとに定期ダメージ");

        AddNode("ウィーク", 5, Sys_NodeGroup.Status, 1, "全てが弱点");
        AddNode("ダウン", 4, Sys_NodeGroup.Status, 2, "弱点ダメージボーナス半減");
        AddNode("アブソープション", 5, Sys_NodeGroup.Status, 3, "弱点ダメージボーナス無効");

        AddNode("リバース", 5, Sys_NodeGroup.NodeKey, 1, "ノード選択のキーが反転する");
        AddNode("カオス", 4, Sys_NodeGroup.NodeKey, 2, "ノード選択のキーが時々移動する");
        AddNode("フリーズ", 3, Sys_NodeGroup.NodeKey, 3, "ノード選択の一部のキーが封印される");
        AddNode("スロウ", 3, Sys_NodeGroup.NodeKey, 4, "ノード選択でキーを連打する手間が増える");

        AddNode("スモーク", 3, Sys_NodeGroup.NodeHindrance, 1, "ノード選択画面を曇らせて妨害");
        AddNode("フェスティバル", 3, Sys_NodeGroup.NodeHindrance, 2, "ノード選択画面を音符マークで妨害");

        AddNode("シールド", 4, Sys_NodeGroup.NodeEditor, 1, "サポートノード禁止");
        AddNode("ヘル", 2, Sys_NodeGroup.NodeEditor, 2, "ペナルティノードの出現率が上昇");
        AddNode("ブレイク", 5, Sys_NodeGroup.NodeEditor, 3, "ノード選択時間が減る");

        AddNode("エクストラ", 1, Sys_NodeGroup.Number, 2, "攻撃個数ボーナス×２");
        AddNode("デラックス", 2, Sys_NodeGroup.Number, 3, "攻撃個数ボーナス×３");
        AddNode("グランド", 3, Sys_NodeGroup.Number, 4, "攻撃個数ボーナス×４");
        AddNode("リッチ", 5, Sys_NodeGroup.Number, 5, "攻撃個数ボーナス×５");

        //--------------------------------------------------------------------------------------------------------------------------------//

        AddNode("フォール", 1, Sys_NodeGroup.Move, 5, "落下移動　ダメージボーナス＋５％");
        AddNode("ショット", 2, Sys_NodeGroup.Move, 10, "発射　ダメージボーナス＋１０％");
        AddNode("グラウンド", 3, Sys_NodeGroup.Move, 20, "地面から　ダメージボーナス＋２０％");
        AddNode("ロックオン", 5, Sys_NodeGroup.Move, 40, "様々な方向から　ダメージボーナス＋４０％");

        AddNode("ショート", 1, Sys_NodeGroup.TernTime, 2, "効果継続２ターン");
        AddNode("ロング", 3, Sys_NodeGroup.TernTime, 3, "効果継続３ターン");
        AddNode("インフィニティ", 5, Sys_NodeGroup.TernTime, 5, "効果継続５ターン");

        AddNode("ストレンジ", 1, Sys_NodeGroup.NodeHindranceTime, 2, "ノード選択画面妨害の効果継続２ターン");
        AddNode("イリュージョン", 3, Sys_NodeGroup.NodeHindranceTime, 3, "ノード選択画面妨害の効果継続３ターン");
        AddNode("ファントム", 5, Sys_NodeGroup.NodeHindranceTime, 5, "ノード選択画面妨害の効果継続５ターン");

        AddNode("プリンス", 2, Sys_NodeGroup.NumberScale, 2, "更に攻撃個数ボーナス×２");
        AddNode("ヴァイスロイ", 3, Sys_NodeGroup.NumberScale, 3, "更に攻撃個数ボーナス×３");
        AddNode("キング", 4, Sys_NodeGroup.NumberScale, 4, "更に攻撃個数ボーナス×４");
        AddNode("エンペラー", 5, Sys_NodeGroup.NumberScale, 5, "更に攻撃個数ボーナス×５");

        AddNode("アルファ", 1, Sys_NodeGroup.Speed, 2, "加えてダメージ２倍");
        AddNode("ベータ", 2, Sys_NodeGroup.Speed, 3, "加えてダメージ３倍");
        AddNode("ガンマ", 3, Sys_NodeGroup.Speed, 4, "加えてダメージ４倍");
        AddNode("デルタ", 4, Sys_NodeGroup.Speed, 5, "加えてダメージ５倍");
        AddNode("イプシロン", 5, Sys_NodeGroup.Speed, 7, "加えてダメージ７倍");

        //--------------------------------------------------------------------------------------------------------------------------------//

        CreateNode();
    }

    void AttackExport()
    {
        //デバッグ画面をクリアする処理
        //var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        //var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        //clearMethod.Invoke(null, null);

        //全部初期化する
        Sys_Status.Action_Object = new Sys_Action_Object();
        Sys_Status.Action_UI = new Sys_Action_UI();

        //属性
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Type)) Sys_Status.Action_Object.Type = Sys_Node.Select[Sys_NodeGroup.Type].Name;
        //形状
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Shape)) Sys_Status.Action_Object.Shape = Sys_Node.Select[Sys_NodeGroup.Shape].Name;

        //出現個数
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Number)) Sys_Status.Action_Object.Number *= Sys_Node.Select[Sys_NodeGroup.Number].Option;
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.NumberScale)) Sys_Status.Action_Object.Number *= Sys_Node.Select[Sys_NodeGroup.NumberScale].Option;

        //オブジェクトの大きさ
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Scale)) Sys_Status.Action_Object.Scale = Sys_Node.Select[Sys_NodeGroup.Scale].Option;

        //移動速度
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Speed)) Sys_Status.Action_Object.Speed = Sys_Node.Select[Sys_NodeGroup.Speed].Option;

        //ダメージ倍率
        int damageScale = 1;

        //ダメージ倍増
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.TypePlus)) damageScale *= Sys_Node.Select[Sys_NodeGroup.TypePlus].Option;

        //弱点属性であるか判定
        for (int i = 0; i < Sys_Status.Player[Sys_Status.targetPlayer].Weak.Count; ++i)
        {
            //弱点であればダメージ倍増
            if (Sys_Status.Player[Sys_Status.targetPlayer].Weak[i] == Sys_Status.Action_Object.Type)
            {
                damageScale *= 2;
                Sys_Status.Action_UI.isTypeWeak = true;
            }
        }

        //弱点関係でダメージ倍増
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.TypeCritical))
        {
            switch (Sys_Node.Select[Sys_NodeGroup.TypeCritical].Option)
            {
                case 1: //弱点属性ダメージ２倍
                    if (Sys_Status.Action_UI.isTypeWeak) damageScale *= 2;
                    break;

                case 2: //弱点属性ダメージ４倍
                    if (Sys_Status.Action_UI.isTypeWeak) damageScale *= 4;
                    break;

                case 3: //弱点ではない属性ダメージ２倍
                    if (!Sys_Status.Action_UI.isTypeWeak) damageScale *= 2;
                    break;

                case 4: //弱点ではない属性ダメージ４倍
                    if (!Sys_Status.Action_UI.isTypeWeak) damageScale *= 4;
                    break;
            }
        }

        //クリティカル計算
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Critical))
        {
            //初期値は１０％の確率
            int criticalRate = 10;

            //クリティカル率改変ノードがあるなら変更
            if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.CriticalOption))
            {
                criticalRate = Sys_Node.Select[Sys_NodeGroup.CriticalOption].Option;
            }

            //クリティカルであるか判定
            Sys_Status.Action_UI.isCritical = Random.Range(0, 100) <= criticalRate;

            //クリティカルヒットしたらダメージ倍増
            if (Sys_Status.Action_UI.isCritical)
            {
                damageScale *= Sys_Node.Select[Sys_NodeGroup.Critical].Option;
            }
        }

        //オブジェクトの大きさでダメージ倍増
        damageScale *= (int)Sys_Status.Action_Object.Scale;
        //オブジェクトの移動速度でダメージ倍増
        damageScale *= (int)Sys_Status.Action_Object.Speed;

        //元となるダメージ数
        int baseDamage = 1000 * damageScale;
        //ダメージの振れ幅
        float damageRate = 0.0f;

        //形状があれば振れ幅変更
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Shape)) damageRate += Sys_Node.Select[Sys_NodeGroup.Shape].Option * 0.01f;
        //移動方法があれば振れ幅ボーナス追加
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Move))
        {
            Sys_Status.Action_Object.Move = Sys_Node.Select[Sys_NodeGroup.Move].Option;
            damageRate += Sys_Node.Select[Sys_NodeGroup.Move].Option * 0.01f;
        }
        //ダメージ計算
        for (int i = 0; i < Sys_Status.Action_Object.Number; i++)
        {
            float damage = baseDamage * Random.Range(1.0f, 1.0f + damageRate);

            //ダメージ追加
            Sys_Status.Action_UI.Damage.Add((int)damage);
            Sys_Status.Player[Sys_Status.activePlayer].TotalDamage += (int)damage;

            //回復ノードが選択された場合の回復量代入
            Sys_Status.Action_UI.Heal += (int)(damage * 0.1f);

            //Debug.Log(Sys_Status.Action_UI.Damage[i] + "ダメージ");
        }

        //ポイズン or パラサイト
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Tern))
        {
            //オプション追加
            Sys_Status.Action_UI.State_Tern = Sys_Node.Select[Sys_NodeGroup.Tern].Option;

            //効果適用
            Sys_Status.Action_Object.State_Tern = Sys_Node.Select[Sys_NodeGroup.Tern].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_Tern = Sys_Node.Select[Sys_NodeGroup.Tern].Option;

            //ターンノードがあるなら追加
            if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.TernTime))
            {
                Sys_Status.Action_UI.State_Tern_Time = Sys_Node.Select[Sys_NodeGroup.TernTime].Option;
                Sys_Status.Player[Sys_Status.targetPlayer].State_Tern_Time = Sys_Node.Select[Sys_NodeGroup.TernTime].Option + 1;
            }
            else Sys_Status.Player[Sys_Status.targetPlayer].State_Tern_Time = 2;
        }

        //ノード妨害
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.NodeHindrance))
        {
            //妨害フラグ
            Sys_Status.Action_UI.State_Etc = true;

            //効果適用
            Sys_Status.Action_Object.State_NodeHindrance = Sys_Node.Select[Sys_NodeGroup.NodeHindrance].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_NodeHindrance = Sys_Node.Select[Sys_NodeGroup.NodeHindrance].Option;

            //ターンノードがあるなら追加
            if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.NodeHindranceTime))
            {
                Sys_Status.Action_UI.State_NodeHindrance_Time = Sys_Node.Select[Sys_NodeGroup.NodeHindranceTime].Option;
                Sys_Status.Player[Sys_Status.targetPlayer].State_NodeHindrance_Time = Sys_Node.Select[Sys_NodeGroup.NodeHindranceTime].Option + 1;
            }
            else Sys_Status.Player[Sys_Status.targetPlayer].State_NodeHindrance_Time = 2;
        }

        //ノードキー妨害
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.NodeKey))
        {
            //妨害フラグ
            Sys_Status.Action_UI.State_Etc = true;

            //効果適用
            Sys_Status.Action_Object.State_NodeKey = Sys_Node.Select[Sys_NodeGroup.NodeKey].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_NodeKey = Sys_Node.Select[Sys_NodeGroup.NodeKey].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_NodeKey_Time = 2;
        }

        //ノードエディタ妨害
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.NodeEditor))
        {
            //妨害フラグ
            Sys_Status.Action_UI.State_Etc = true;

            //効果適用
            Sys_Status.Action_Object.State_NodeEditor = Sys_Node.Select[Sys_NodeGroup.NodeEditor].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_NodeEditor = Sys_Node.Select[Sys_NodeGroup.NodeEditor].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_NodeEditor_Time = 2;
        }

        //攻撃変化
        if (Sys_Node.Select.ContainsKey(Sys_NodeGroup.Status))
        {
            //追加
            Sys_Status.Action_UI.State_Status = Sys_Node.Select[Sys_NodeGroup.Status].Option;

            //効果適用
            Sys_Status.Action_Object.State_Status = Sys_Node.Select[Sys_NodeGroup.Status].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_Status = Sys_Node.Select[Sys_NodeGroup.Status].Option;
            Sys_Status.Player[Sys_Status.targetPlayer].State_Status_Time = 2;
        }

        updateFlag = false;
    }

    public bool isUpdate()
    {
        return updateFlag;
    }

    public bool isPerfect()
    {
        return 18 <= selectNode.Count;
    }

    public void End()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (Create[i] != null)
            {
                Create[i].GetComponent<Sys_Node>().Data.This = null;

                DeleteUI(Create[i], 2.5f);

                Create[i] = null;
            }
        }

        for (int i = 0; i < selectNode.Count; ++i)
        {
            DeleteUI(selectNode[i], 1.25f);
        }

        DeleteUI(paletteImage, 2.5f);
        DeleteUI(selectImage, 2.5f);
        DeleteUI(this.GetComponent<Image>(), 2.5f);

        Sys_Sound.Play(destroySound);
        Destroy(this.GetComponent<Sys_NodeEngine>());
    }

    void SelectNode(int target)
    {
        if (Create[target] == null) return;

        for (int i = 0; i < 4; ++i)
        {
            if (Create[i] == null) continue;

            if (i == target)
            {
                Image tmpSelect;

                //選択したノードが妨害ノードであれば
                if (Create[target].GetComponent<Sys_Node>().Data.Penalty) tmpSelect = Instantiate(decisionBad, Vector2.zero, Quaternion.identity) as Image;
                //選択したノードが通常ノードであれば
                else
                {
                    switch (Create[target].GetComponent<Sys_Node>().Data.Rank)
                    {
                        case 1:
                        case 2:
                            tmpSelect = Instantiate(decisionGood, Vector2.zero, Quaternion.identity) as Image;
                            break;

                        case 3:
                        case 4:
                            tmpSelect = Instantiate(decisionGreat, Vector2.zero, Quaternion.identity) as Image;
                            break;

                        case 5:
                            tmpSelect = Instantiate(decisionExcellent, Vector2.zero, Quaternion.identity) as Image;
                            break;

                        default:
                            tmpSelect = Instantiate(decisionBad, Vector2.zero, Quaternion.identity) as Image;
                            break;
                    }
                }

                tmpSelect.transform.SetParent(nodeEditor.transform, false);
                tmpSelect.transform.position = Create[i].transform.position;

                //選択信号を渡す
                Create[target].GetComponent<Sys_Node>().Enter();
                //選択済みリストに加える
                selectNode.Add(Create[target]);

                Create[target] = null;
            }
            //ノードを消去する
            else
            {
                Create[i].GetComponent<Sys_Node>().Data.This = null;

                DeleteUI(Create[i], 5.0f);

                Create[i] = null;
            }
        }

        if (1 == selectNode.Count)
        {
            Image tmp = Instantiate(start, new Vector2(0.0f, 200.0f), Quaternion.identity) as Image;
            tmp.transform.SetParent(nodeEditor.transform, false);
        }

        if (selectNode.Count < 18) CreateNode();
        else AttackExport();
    }

    void Update()
    {
        if (!updateFlag) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) SelectNode(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectNode(0);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) SelectNode(3);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectNode(2);

        //デバッグ
        if (Input.GetKey(KeyCode.Q))
        {
            int max = 0;

            for (int i = 0; i < 4; ++i)
            {
                if (i == max || Create[i] == null || Create[i].GetComponent<Sys_Node>().Data.Penalty) continue;
                
                if (Create[max].GetComponent<Sys_Node>().Data.Rank < Create[i].GetComponent<Sys_Node>().Data.Rank) max = i;
            }

            SelectNode(max);
        }

        //デバッグ
        if (Input.GetKey(KeyCode.W))
        {
            selectTime = 10.0f;
            soundTime = 9.0f;
        }

        //デバッグ
        if (Input.GetKey(KeyCode.E) && 1 <= selectNode.Count)
        {
            selectTime = 0.0f;
        }

        string attackName = "";

        //全てのノード選択済み枠を見る
        for (int i = 0; i < Sys_Status.Attack_Name.Count; ++i)
        {
            attackName += Sys_Status.Attack_Name[i] + " ";
        }

        if (0 < selectNode.Count) selectTime -= Time.deltaTime;

        //カウントダウンサウンドの再生処理
        if (selectTime <= soundTime)
        {
            Sys_Sound.Play(timeSound);

            if (soundTime <= 1.0f) soundTime -= 0.125f;
            else if (soundTime <= 2.0f) soundTime -= 0.25f;
            else if (soundTime <= 4.0f) soundTime -= 0.5f;
            else soundTime -= 1.0f;
        }

        //時間切れ
        if (selectTime <= 0.0f)
        {
            selectTime = 0.0f;
            AttackExport();
        }

        nodeEditor.transform.FindChild("ActionName").gameObject.GetComponent<Text>().text = attackName;
        nodeEditor.transform.FindChild("SelectTime").gameObject.GetComponent<Text>().text = "残り" + selectTime.ToString("00.00") + "秒";
    }
}
