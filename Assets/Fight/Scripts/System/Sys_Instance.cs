using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class Sys_Instance : MonoBehaviour
{
    public GameObject[] tama1 = new GameObject[20];//形状
    public GameObject[] EfectTypes = new GameObject[12];//属性
    public GameObject[] AddEfectList = new GameObject[6];//追加属性
    public GameObject canvas;
    ///public GameObject Sys_CurrentObject;
    public Text text;
    public Image image;
    public Vector3 m_getpos;

    public Image Poison;//ポイゾン
    public Image Parasite;//パラサイト
    public Image Interference;//ノード妨害
    public Image Smoke;//スモーク
    public Image Festival;//フェスティバル

    public AudioClip shotSound;

    public int Seisei;

    public bool m_TextFlag;
    public int m_Type;
    public int m_Shape;
    public int m_kazu;//m_kazu を生成予定の数
    private int m_now_kazu;  //m_now_kazu を追加し、生成済みの数　に仕様変更
    public int m_MoveType;
    public string m_TamaType;
    public float m_time;
    private float m_nuw_time;
    public int m_get_kazu;
    public int turn;
    public int[] Old_NodeHindrance_turn;
    public int NodeHindrance_turn;
    public float TimeCount = 0.0f;//時間カウント
    public bool TimeOk;//時間計測
    public int m_count;

    public bool isEnd;
    public Vector3 targetPosition;

    public int Nodeselect;
    public int tagetP;

    Image images;
    //===================================================================//

    static public Image[] createTernState = new Image[2];
    static public Image[] createTernStateTern = new Image[2];

    static public Image[] createNodePenalty = new Image[2];

    static public Image[] createNodeHindrance = new Image[2];
    static public Image[] createNodeHindranceTern = new Image[2];

    static public void StateUpdate()
    {
        Sprite[] TernSprite = GameObject.Find("TernImage").GetComponent<Sys_TernImage>().TernSprite;

        for (int i = 0; i < 2; ++i)
        {
            Sys_PlayerData data = Sys_Status.Player[i];

            //ポイズン or パラサイトの状態異常が切れていたら消す
            if (data.State_Tern == 0)
            {
                Destroy(createTernState[i]);
                Destroy(createTernStateTern[i]);
            }
            //ターンを更新
            else createTernStateTern[i].GetComponent<Image>().sprite = TernSprite[data.State_Tern_Time];


            //ノード妨害の状態異常が切れていたら消す
            if (data.State_NodeKey == 0 && data.State_NodeEditor == 0)
            {
                Destroy(createNodePenalty[i]);
            }

            //スモーク or フェスティバルの状態異常が切れていたら消す
            if (data.State_NodeHindrance == 0)
            {
                Destroy(createNodeHindrance[i]);
                Destroy(createNodeHindranceTern[i]);
            }
            //ターンを更新
            else createNodeHindranceTern[i].GetComponent<Image>().sprite = TernSprite[data.State_NodeHindrance_Time];
        }
    }

    void Start()
    {
        TimeOk = false;
        isEnd = false;

        m_TextFlag = false;

        m_now_kazu = 0;

        m_nuw_time = 0.0f;
        canvas = GameObject.Find("StateCanvas");

        m_kazu = Sys_Status.Action_Object.Number;
        m_get_kazu = Sys_Status.Action_Object.Number;
        m_MoveType = Sys_Status.Action_Object.Move;

        turn = Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_Tern_Time;
        NodeHindrance_turn = Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_NodeHindrance_Time;
    }

    void Update()
    {
        if (TimeOk == true)//実行
        {
            TimeCount += Time.deltaTime;
            isEnd = false;
        }
        if (TimeCount >= 2.5f)//2.5f
        {
            Destroy(this.gameObject);
        }

        //攻撃が全ヒット　1回だけ
        if (!TimeOk && m_get_kazu == 0)
        {
            TimeOk = true;
            isEnd = true;

            int target = Sys_Status.targetPlayer;

            Sprite[] TernSprite = GameObject.Find("TernImage").GetComponent<Sys_TernImage>().TernSprite;
            Image TernImage = GameObject.Find("TernImage").GetComponent<Sys_TernImage>().TernImage;

            //定期ターン状態異常が有効であれば
            int stateTern = Sys_Status.Player_Wait[target].State_Tern; //一時的に保管
            if (stateTern != 0)
            {
                //相手のポイズンorパラサイトの情報で分岐して生成
                Destroy(createTernState[target]);
                createTernState[target] = Instantiate(stateTern == 1 ? Poison : Parasite, Vector2.zero, Poison.transform.rotation) as Image;
                createTernState[target].transform.SetParent(canvas.transform, false);

                Destroy(createTernStateTern[target]);
                createTernStateTern[target] = Instantiate(TernImage, Vector2.zero, TernImage.transform.rotation) as Image;
                createTernStateTern[target].transform.SetParent(canvas.transform, false);
                createTernStateTern[target].GetComponent<Image>().sprite = TernSprite[Sys_Status.Player_Wait[target].State_Tern_Time - 1];
            }

            //ノードキー妨害とノードエディタ妨害のどちらかが有効であれば「ノード妨害」を生成
            if (Sys_Status.Player_Wait[target].State_NodeKey != 0
                || Sys_Status.Player_Wait[target].State_NodeEditor != 0)
            {
                Destroy(createNodePenalty[target]);
                createNodePenalty[target] = Instantiate(Interference, Vector2.zero, Interference.transform.rotation) as Image;
                createNodePenalty[target].transform.SetParent(canvas.transform, false);
            }

            //相手のスモークorフェスティバルの情報で分岐して生成
            int state_NodeHindrance = Sys_Status.Player_Wait[target].State_NodeHindrance;
            if (state_NodeHindrance != 0)
            {
                Destroy(createNodeHindrance[target]);
                createNodeHindrance[target] = Instantiate(state_NodeHindrance == 1 ? Smoke : Festival
                    , Vector2.zero
                    , Smoke.transform.rotation) as Image;

                createNodeHindrance[target].transform.SetParent(canvas.transform, false);

                Destroy(createNodeHindranceTern[target]);
                createNodeHindranceTern[target] = Instantiate(TernImage, new Vector3(150, 180, 0)
                    , TernImage.transform.rotation) as Image;

                createNodeHindranceTern[target].transform.SetParent(canvas.transform, false);
                createNodeHindranceTern[target].GetComponent<Image>().sprite = TernSprite[Sys_Status.Player_Wait[target].State_NodeHindrance_Time - 1];
            }
        }

        Dictionary<string, int> typeList = new Dictionary<string, int>();
        typeList["ファイヤー"] = 0;
        typeList["ウォーター"] = 1;
        typeList["アイス"] = 2;
        typeList["サンダー"] = 3;
        typeList["グラス"] = 4;
        typeList["ウィンド"] = 5;
        typeList["ライト"] = 6;
        typeList["ダークネス"] = 7;
        typeList["ソイル"] = 8;
        typeList["ドラゴン"] = 9;
        typeList["ゴッド"] = 10;
        typeList["フィジックス"] = 11;
        m_Type = typeList[Sys_Status.Action_Object.Type];

        typeList["ナイフ"] = 0;
        typeList["カッター"] = 1;
        typeList["ソード"] = 2;
        typeList["ニードル"] = 3;
        typeList["メイス"] = 4;
        typeList["ハンマー"] = 5;
        typeList["シックル"] = 6;
        typeList["アロー"] = 7;
        typeList["ランス"] = 8;
        typeList["スピアー"] = 9;
        typeList["アックス"] = 10;
        typeList["バルディッシュ"] = 11;
        typeList["バレット"] = 12;
        typeList["スローイングスター"] = 13;
        typeList["ハルバード"] = 14;
        typeList["フラッシュ"] = 15;
        typeList["ソニック"] = 16;
        typeList["レーザー"] = 17;
        typeList["ボム"] = 18;
        typeList["メテオ"] = 19;
        m_Shape = typeList[Sys_Status.Action_Object.Shape];

        if (m_MoveType != 0)
        {
            m_nuw_time += Time.deltaTime;
        }
        if (m_nuw_time >= m_time && tama1 != null)
        {
            m_nuw_time = 0.0f;
            if (m_now_kazu < m_kazu)
            {
                GameObject createTama = null;
                GameObject createType = null;
                GameObject AddEfect = null;

                switch (m_MoveType)//バラバラに発射するようにする
                {
                    case 5://落下移動
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), 20, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;
                        createType = Instantiate(EfectTypes[m_Type], Vector3.zero, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 10://自身から発射
                        createTama = Instantiate(tama1[m_Shape], new Vector3(-20, 0, 0), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], Vector3.zero, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 20://地面から攻撃
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), -10, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], Vector3.zero, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 40://様々な方向から
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], Vector3.zero, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    default:
                        Debug.Log("Instance -> TypeError");
                        break;
                }

                Sys_Sound.Play(shotSound);

                createType.transform.SetParent(createTama.transform, false);

                createTama.GetComponent<Obj_Skill_Firing>().m_mokutekipos = targetPosition;

                createTama.GetComponent<Obj_Skill_Firing>().instance_data = this.gameObject;
                createTama.GetComponent<Obj_Skill_Firing>().attack = Sys_Status.Action_UI.Damage[m_now_kazu];

                for (int i = 0; i < Nodeselect / 4; ++i)
                {
                    AddEfect = Instantiate(AddEfectList[Random.Range(0, 8)],
                        targetPosition + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f)), Quaternion.identity) as GameObject;

                    AddEfect.transform.localScale = createTama.transform.localScale;
                }

                ++m_now_kazu;
            }
        }
        if (m_TextFlag == true)
        {
            images = Instantiate(image, text.transform.position, text.transform.rotation) as Image;//Canvasとテキスト生成
            //生成したオブジェクトの内部にある「UI_DamageNumber」にアクセスし、テキスト内容（ダメージ数）を更新する
            images.transform.FindChild("UI_DamageNumber").gameObject.GetComponent<Text>().text = text.text;
            images.transform.SetParent(canvas.transform, false); //こちらのほうが安全で、警告が出ない
            if (images != null)
            {
                //Debug.Log("ああ");
                images.transform.position = Camera.main.WorldToScreenPoint(m_getpos + new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0));
            }
            m_TextFlag = false;
        }
    }
}