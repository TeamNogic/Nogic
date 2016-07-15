using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

[System.Serializable]
public enum Sys_NodeGroup
{
    //名前順に切り替えている
    Core,

    Type,               //Core
    TypePlus,           //Type
    TypeCritical,       //Type
    Critical,           //Type
    CriticalOption,     //Critical
    Shape,              //Type
    Tern,               //Shape
    TernTime,           //Tern
    Scale,              //Shape
    Status,             //Shape
    NodeKey,            //Shape
    NodeHindrance,      //Shape
    NodeHindranceTime,  //NodeHindrance
    NodeEditor,         //Shape
    Number,             //Shape
    Move,               //Number
    NumberScale,        //Number
    Speed,              //Number


    __Size__,
}

[System.Serializable]
public class Sys_ParameterNode
{
    //固定データ
    public string Name;                                                 //技名
    public string Comment;                                              //コメント
    public int Rank;                                                    //ランク

    public Sys_NodeGroup Group;                                         //グループ
    public Sys_NodeGroup GroupPrev;                                     //前に必要なグループ
    public List<Sys_NodeGroup> GroupNext = new List<Sys_NodeGroup>();   //次に連動可能なグループ

    public Vector3 Potision;                                            //移動先座標

    public int Option;                                                  //オプション番号

    //戦闘によって変化するデータ
    public GameObject This;                                             //ノードに紐づけしているオブジェクト

    public int SelectStart;                                             //選択範囲の開始
    public int SelectEnd;                                               //選択範囲の終了

    public bool Used;                                                   //戦闘で使用するか
    public bool Penalty;                                                //ペナルティノードになったか
    public bool AddNode;                                                //相手側のペナルティで含まれたノードであるか
}

[System.Serializable]
public class Sys_Node : MonoBehaviour
{
    public Image m_Line;                                                                //ライン

    public Sys_ParameterNode Data;                                                      //自身の管理データ

    public Sprite[] nodeImage = new Sprite[5];                                          //ノード画像

    private List<Sys_NodeGroup> NextNodeGroup = new List<Sys_NodeGroup>();              //次のグループの対象として出しても良いもの

    public static List<Sys_ParameterNode> NodeList = new List<Sys_ParameterNode>();     //ノードデッキ

    public static Dictionary<Sys_NodeGroup, Sys_ParameterNode>
        Select = new Dictionary<Sys_NodeGroup, Sys_ParameterNode>();                    //選択済みノードリスト

    public Image line;                                                                  //線

    private GameObject nodeEditor;                                                      //キャンバス

    private bool startGuard = false;                                                    //Start呼び出し防止
    private int penaltyCount;                                                           //ペナルティ番号

    void Awake()
    {
        nodeEditor = GameObject.Find("NodeEditor");
    }

    void Start()
    {
        if (startGuard) return;

        //全てのノード選択済み枠を見る
        for (Sys_NodeGroup i = 0; i != Sys_NodeGroup.__Size__; ++i)
        {
            //選択済みノードが存在したら
            if (Select.ContainsKey(i))
            {
                //選択済みノードの次のグループを検索する
                for (int j = 0; j < Select[i].GroupNext.Count; ++j)
                {
                    //次のグループが存在しなかったら
                    if (!Select.ContainsKey(Select[i].GroupNext[j]))
                    {
                        //次の出現ノード候補に追加
                        NextNodeGroup.Add(Select[i].GroupNext[j]);
                    }
                }
            }
        }

        bool selectFlag = false; //候補に選ばれる可能性のあるノード

        for (int i = 0; i < NextNodeGroup.Count && !selectFlag; ++i)
        {
            for (int j = 0; j < NodeList.Count && !selectFlag; ++j)
            {
                //１つでも候補があれば true
                if (NextNodeGroup[i] == NodeList[j].Group && NodeList[j].This == null) selectFlag = true;
            }
        }

        //１つも候補が無かった場合はノードを表示しない（消去）
        if (!selectFlag)
        {
            Destroy(this.gameObject);
            return;
        }

        Data = null;

        //ノードが確定するまで無限ループ
        while (Data == null)
        {
            //セレクト番号をランダム取得
            int selectNumber = Random.Range(0, NodeList[NodeList.Count - 1].SelectEnd);

            //セレクト番号に選ばれたノードを探す
            for (int i = 0; i < NodeList.Count; ++i)
            {
                //範囲内であれば
                if (NodeList[i].This == null && NodeList[i].SelectStart <= selectNumber && selectNumber <= NodeList[i].SelectEnd)
                {
                    //候補グループに含まれているかすべて確認する
                    for (int j = 0; j < NextNodeGroup.Count; ++j)
                    {
                        //含まれていたら
                        if (NodeList[i].Group == NextNodeGroup[j])
                        {
                            //ノード確定
                            NodeList[i].This = this.gameObject;
                            Data = NodeList[i];

                            break;
                        }
                    }

                    break;
                }
            }
        }

        //ペナルティノード変化(状態異常：ヘル　の場合は出現率が２倍)
        Data.Penalty = Random.Range(0, Sys_Status.Player[Sys_Status.activePlayer].State_NodeEditor == 2 ? 5 : 10) == 0;
        Data.AddNode = false;
        this.transform.FindChild("Danger").GetComponent<Image>().enabled = Data.Penalty;

        //ランク変更と技名の設定
        this.GetComponent<Image>().sprite = nodeImage[Data.Rank - 1];
        this.transform.FindChild("Name").gameObject.GetComponent<Text>().text = Data.Name;
    }

    public void SelectEnter(int pos, int penaltyCount)
    {
        //Start関数を呼び出さない
        startGuard = true;

        //ノード確定
        NodeList[pos].This = this.gameObject;
        Data = NodeList[pos];

        //ペナルティを無効化する
        Data.Penalty = false;
        Data.AddNode = true;
        this.transform.FindChild("Danger").GetComponent<Image>().enabled = Data.Penalty;
        //ペナルティカウント（何個目のペナルティか）を代入
        this.penaltyCount = penaltyCount;

        //ランク変更と技名の設定
        this.GetComponent<Image>().sprite = nodeImage[Data.Rank - 1];
        this.transform.FindChild("Name").gameObject.GetComponent<Text>().text = Data.Name;

        //選択済みにする
        this.Enter();
    }

    public void Enter()
    {
        //ノードを移動開始する
        this.gameObject.AddComponent<UI_NodeMove>();
        this.GetComponent<UI_NodeMove>().Setup_Target(Data.Potision);

        //サイズを調整
        if (this.GetComponent<UI_Scale>() == null) this.gameObject.AddComponent<UI_Scale>();
        this.GetComponent<UI_Scale>().Setup(new Vector2(0.375f, 0.15f), 2.5f, false);

        if (!startGuard)
        {
            //前のノードに向けて線を引く
            line = Instantiate(m_Line, transform.position, Quaternion.identity) as Image;
            line.transform.SetParent(nodeEditor.transform, false);
            line.GetComponent<Sys_Line>().SetTargetPos(transform);
            line.GetComponent<Sys_Line>().SetStartPos((Select[Data.GroupPrev].This).transform);
        }

        //ノード情報追加　形状名は最後に出るようにする
        if (!Data.Penalty)
        {
            if (Data.Group == Sys_NodeGroup.Shape)
            {
                Sys_Status.Attack_Name.Add(Data.Name);
                Sys_Status.Attack_Comment.Add(Data.Comment);
            }
            else
            {
                Sys_Status.Attack_Name.Insert(0, Data.Name);
                Sys_Status.Attack_Comment.Insert(0, Data.Comment);
            }
        }

        //選択されたのでリストに追加
        Select[Data.Group] = Data;
    }
}