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

    public Image smokeImage;                            //煙妨害
    public Image confettiImage;                         //紙吹雪妨害
    public Image musicalScoreImage;                     //楽譜マーク妨害
    public Image nodeKeyStateImage;                     //ノード選択妨害説明

    public AudioClip timeSound;                         //カウントダウン音
    public AudioClip destroySound;                      //消去時のサウンド
    public AudioClip chaosMove;                         //カオス妨害ノード移動時

    void CreateNode()
    {
        int notCreate = -1;

        if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey == 3) notCreate = Random.Range(0, 4);

        //各ノードを生成
        if (notCreate != 0)
        {
            Create[0] = Instantiate(baseNode, new Vector3(-300.0f, 40.0f), Quaternion.identity) as Image;
            Create[0].transform.SetParent(nodeEditor.transform, false);
        }

        if (notCreate != 1)
        {
            Create[1] = Instantiate(baseNode, new Vector3(-200.0f, 120.0f), Quaternion.identity) as Image;
            Create[1].transform.SetParent(nodeEditor.transform, false);
        }

        if (notCreate != 2)
        {
            Create[2] = Instantiate(baseNode, new Vector3(-200.0f, -30.0f), Quaternion.identity) as Image;
            Create[2].transform.SetParent(nodeEditor.transform, false);
        }

        if (notCreate != 3)
        {
            Create[3] = Instantiate(baseNode, new Vector3(-100.0f, 40.0f), Quaternion.identity) as Image;
            Create[3].transform.SetParent(nodeEditor.transform, false);
        }
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

        Sys_Status.Attack_Name.Clear();
        Sys_Status.Attack_Comment.Clear();

        paletteImage = Instantiate(paletteImage, new Vector3(0.0f, 50.0f), Quaternion.identity) as Image;
        paletteImage.transform.SetParent(nodeEditor.transform, false);

        selectImage = Instantiate(selectImage, new Vector3(-200.0f, 50.0f), Quaternion.identity) as Image;
        selectImage.transform.SetParent(nodeEditor.transform, false);

        if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey != 0)
        {
            nodeKeyStateImage = Instantiate(nodeKeyStateImage, new Vector3(-200.0f, 225.0f), Quaternion.identity) as Image;
            nodeKeyStateImage.transform.SetParent(nodeEditor.transform, false);
            nodeKeyStateImage.GetComponent<UI_ImageChange>().select = Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey - 1;
        }

        this.transform.SetParent(nodeEditor.transform, false);

        selectTime = 10.0f;
        soundTime = 9.0f;
        updateFlag = true;

        Sys_Node.Select[Sys_NodeGroup.Core] = Sys_NodeCreate.AddNode("", 0, Sys_NodeGroup.Core, 0, ""); //コア
        Sys_Node.Select[Sys_NodeGroup.Core].This = this.gameObject;

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
            DeleteUI(selectNode[i].GetComponent<Sys_Node>().line, 1.25f);

            DeleteUI(selectNode[i], 1.25f);
        }

        if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey != 0) DeleteUI(nodeKeyStateImage, 2.5f);
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
                //妨害で移動中の場合は阻止する
                Destroy(Create[target].GetComponent<UI_Move>());
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

        //選択キー反転妨害
        if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) SelectNode(2);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectNode(3);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) SelectNode(0);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectNode(1);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) SelectNode(1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectNode(0);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) SelectNode(3);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectNode(2);
        }

        //ノード入れ替え妨害
        if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeKey == 2 && Random.Range(0, 60) == 0)
        {
            //移動対象を選択
            int moveA = 0;
            int moveB = 0;

            //別のノードになるまでランダムループ
            while ((moveA == moveB) || Create[moveA] == null || Create[moveB] == null)
            {
                moveA = Random.Range(0, 4);
                moveB = Random.Range(0, 4);
            }

            //データ位置の移動
            Image tmp = Create[moveA];
            Create[moveA] = Create[moveB];
            Create[moveB] = tmp;

            //座標の移動
            Vector3 tmpTarget = Create[moveA].GetComponent<UI_Move>() == null
                ? Create[moveA].transform.position
                : Create[moveA].GetComponent<UI_Move>().getTarget();

            if (Create[moveA].GetComponent<UI_Move>() == null) Create[moveA].gameObject.AddComponent<UI_Move>();

            Create[moveA].GetComponent<UI_Move>().Setup_Target(
                Create[moveB].GetComponent<UI_Move>() == null
                ? Create[moveB].transform.position
                : Create[moveB].GetComponent<UI_Move>().getTarget());

            if (Create[moveB].GetComponent<UI_Move>() == null) Create[moveB].gameObject.AddComponent<UI_Move>();

            Create[moveB].GetComponent<UI_Move>().Setup_Target(tmpTarget);

            //サウンドの再生
            Sys_Sound.Play(chaosMove);
        }

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

        //ノードが１つ以上選択されていたら
        if (0 < selectNode.Count)
        {
            //時間を減らす
            selectTime -= Time.deltaTime;

            //一定確率で煙妨害
            if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeHindrance == 1 && Random.Range(0, 15) == 0)
            {
                Image tmp = Instantiate(smokeImage, new Vector2(Random.Range(-274.0f, -126.0f), Random.Range(-10.0f, 114.0f)), Quaternion.identity) as Image;
                tmp.transform.SetParent(nodeEditor.transform, false);
            }

            //紙吹雪妨害
            if (Sys_Status.Player[Sys_Status.activePlayer].State_NodeHindrance == 2)
            {
                //一定確率で楽譜妨害
                if (Random.Range(0, 30) == 0)
                {
                    Image tmpM = Instantiate(musicalScoreImage, new Vector2(-500.0f, 50.0f), Quaternion.identity) as Image;
                    tmpM.transform.SetParent(nodeEditor.transform, false);
                }

                Image tmp = Instantiate(confettiImage, new Vector2(Random.Range(-274.0f, -126.0f), 350.0f), Quaternion.identity) as Image;
                tmp.transform.SetParent(nodeEditor.transform, false);

                tmp = Instantiate(confettiImage, new Vector2(Random.Range(-274.0f, -126.0f), 350.0f), Quaternion.identity) as Image;
                tmp.transform.SetParent(nodeEditor.transform, false);
            }
        }

        //描画順番を妨害より手前に設定
        for (int i = 0; i < 4; ++i)
        {
            if (Create[i] == null) continue;

            Create[i].transform.SetSiblingIndex(selectImage.transform.GetSiblingIndex() + 1);
        }

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
