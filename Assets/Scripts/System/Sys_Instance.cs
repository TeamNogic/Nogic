using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class Sys_Instance : MonoBehaviour
{
    public GameObject[] tama1 = new GameObject[20];//形状
    public GameObject[] EfectTypes = new GameObject[12];//属性
    public GameObject canvas;
    public Text text;
    public Image image;
    public Vector3 m_getpos;

    public Image Poison;//ポイゾン
    public Image Parasite;//パラサイト
    public Image Interference;//ノード妨害
    public Image Smoke;//スモーク
    public Image Festival;//フェスティバル

    public Sprite[] sprite = new Sprite[6];
    public Image State_turns;//パラサイトとポイズンターン数
    public Image State_NodeHindrance;//スモークとフェスティバルターン数

    public AudioClip shotSound;

    public int Seisei;
    public int[] m_Seisei = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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
    public int Oldturn;
    public int turn;
    public int Old_NodeHindrance_turn;
    public int NodeHindrance_turn;
    public float TimeCount = 0.0f;//時間カウント
    public bool TimeOk;//時間計測
    public int m_count;

    public bool isEnd;
    public Vector3 targetPosition;

    Image images;
    void Start()
    {
        TimeOk = false;
        isEnd = false;

        m_TextFlag = false;

        m_now_kazu = 0;

        m_nuw_time = 0.0f;
        Oldturn = 0;

        canvas = GameObject.Find("NodeEditor");

        m_kazu = Sys_Status.Action_Object.Number;
        m_get_kazu = Sys_Status.Action_Object.Number;
        m_MoveType = Sys_Status.Action_Object.Move;
        turn = Sys_Status.Action_UI.State_Tern_Time;
        NodeHindrance_turn = Sys_Status.Action_UI.State_NodeHindrance_Time;
        State_turns.sprite = null;
        State_NodeHindrance.sprite = null;
    }

    void Update()
    {
        Image image_dousa = null;
        Image image_bougay = null;
        Image image_NodeHindrance = null;
        // Image image_Festival = null;
        Image turns = null;
        Image NodeHindrance_turns = null;

        if (image_dousa != null)
        {
            image_dousa.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        }
        if (image_bougay != null)
        {
            image_bougay.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        }
        if (image_NodeHindrance != null)
        {
            image_NodeHindrance.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        }
        if (turns != null)
        {
            turns.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        }
        if (NodeHindrance_turns != null)
        {
            NodeHindrance_turns.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        }

        if (TimeOk == true)//実行
        {
            TimeCount += Time.deltaTime;
            isEnd = false;
        }
        if (TimeCount >= 2.5f)//2.5f
        {
            Destroy(this.gameObject);
        }
        //Debug.Log(m_get_kazu);
        if (!TimeOk && m_get_kazu == 0 && (m_Seisei[0] == 0 || m_Seisei[1] == 0 || m_Seisei[2] == 0 || m_Seisei[3] == 0 || m_Seisei[4] == 0))
        {
            Debug.Log(m_get_kazu);
            if (GameObject.Find("Poison(Clone)") != null) Destroy(GameObject.Find("Poison(Clone)"));
            if (GameObject.Find("Parasite(Clone)") != null) Destroy(GameObject.Find("Parasite(Clone)"));
            if (GameObject.Find("Interference(Clone)") != null) Destroy(GameObject.Find("Interference(Clone)"));
            if (GameObject.Find("Smoke(Clone)") != null) Destroy(GameObject.Find("Smoke(Clone)"));
            if (GameObject.Find("Festival(Clone)") != null) Destroy(GameObject.Find("Festival(Clone)"));
            if (GameObject.Find("TurnBangou(Clone)") != null) Destroy(GameObject.Find("TurnBangou(Clone)"));
            if (GameObject.Find("NodeHindranceBangou(Clone)") != null) Destroy(GameObject.Find("NodeHindranceBangou(Clone)"));
            TimeOk = true;
            isEnd = true;

            if (image_dousa == null)
            {
                switch (Sys_Status.Action_UI.State_Tern)
                {
                    case 0:
                        m_Seisei[0] = 1;
                        break;
                    case 1:
                        image_dousa = Instantiate(Poison, Poison.transform.position, Poison.transform.rotation) as Image;
                        image_dousa.transform.SetParent(canvas.transform, false);
                        m_Seisei[0] = 1;
                        break;
                    case 2:
                        image_dousa = Instantiate(Parasite, Parasite.transform.position, Parasite.transform.rotation) as Image;
                        image_dousa.transform.SetParent(canvas.transform, false);
                        m_Seisei[0] = 1;
                        break;
                }
            }
            if (Sys_Status.Action_UI.State_Etc == true && image_bougay == null) //妨害が発生するかどうか（UIで「ノード妨害追加！」と出る）
            {
                image_bougay = Instantiate(Interference, Interference.transform.position, Interference.transform.rotation) as Image;
                image_bougay.transform.SetParent(canvas.transform, false);
                m_Seisei[1] = 1;
            }
            else
            {
                m_Seisei[1] = 1;
            }

            if (image_NodeHindrance == null)
            {
                switch (Sys_Status.Action_Object.State_NodeHindrance)//1:スモーク　2:フェスティバル
                {
                    case 0:
                        m_Seisei[2] = 1;
                        break;
                    case 1:
                        image_NodeHindrance = Instantiate(Smoke, Smoke.transform.position, Smoke.transform.rotation) as Image;
                        image_NodeHindrance.transform.SetParent(canvas.transform, false);
                        m_Seisei[2] = 1;
                        break;
                    case 2:
                        image_NodeHindrance = Instantiate(Festival, Festival.transform.position, Festival.transform.rotation) as Image;
                        image_NodeHindrance.transform.SetParent(canvas.transform, false);
                        m_Seisei[2] = 1;
                        break;
                }
            }

            if (Oldturn != turn)
            {
                Oldturn = turn;
                if (Oldturn != 0)
                {
                    State_turns.sprite = sprite[Oldturn];
                }
                else
                {
                    m_Seisei[3] = 1;
                }
            }
            if (State_turns.sprite != null && turns == null)//ターン数
            {
                turns = Instantiate(State_turns, State_turns.transform.position, State_turns.transform.rotation) as Image;
                turns.transform.SetParent(canvas.transform, false);
                m_Seisei[3] = 1;
            }
            if (Old_NodeHindrance_turn != NodeHindrance_turn)
            {
                Old_NodeHindrance_turn = NodeHindrance_turn;
                if (Old_NodeHindrance_turn != 0)
                {
                    State_NodeHindrance.sprite = sprite[Old_NodeHindrance_turn];//スモーク or フェスティバルの効果ターン数
                }
                else
                {
                    m_Seisei[4] = 1;
                }
            }
            if (State_NodeHindrance.sprite != null && NodeHindrance_turns == null)
            {
                NodeHindrance_turns = Instantiate(State_NodeHindrance, State_NodeHindrance.transform.position, State_NodeHindrance.transform.rotation) as Image;
                NodeHindrance_turns.transform.SetParent(canvas.transform, false);
                m_Seisei[4] = 1;
            }
        }

        switch (Sys_Status.Action_Object.Type)
        {
            case "ファイヤー":
                m_Type = 0;
                break;
            case "ウォーター":
                m_Type = 1;
                break;
            case "アイス":
                m_Type = 2;
                break;
            case "サンダー":
                m_Type = 3;
                break;
            case "グラス":
                m_Type = 4;
                break;
            case "ウィンド":
                m_Type = 5;
                break;
            case "ライト":
                m_Type = 6;
                break;
            case "ダークネス":
                m_Type = 7;
                break;
            case "ソイル":
                m_Type = 8;
                break;
            case "ドラゴン":
                m_Type = 9;
                break;
            case "ゴッド":
                m_Type = 10;
                break;
            case "フィジックス":
                m_Type = 11;
                break;
        }
        switch (Sys_Status.Action_Object.Shape)
        {
            case "ナイフ":
                m_Shape = 0;
                break;
            case "カッター":
                m_Shape = 1;
                break;
            case "ソード":
                m_Shape = 2;
                break;
            case "ニードル":
                m_Shape = 3;
                break;
            case "メイス":
                m_Shape = 4;
                break;
            case "ハンマー":
                m_Shape = 5;
                break;
            case "シックル":
                m_Shape = 6;
                break;
            case "アロー":
                m_Shape = 7;
                break;
            case "ランス":
                m_Shape = 8;
                break;
            case "スピアー":
                m_Shape = 9;
                break;
            case "アックス":
                m_Shape = 10;
                break;
            case "バルディッシュ":
                m_Shape = 11;
                break;
            case "バレット":
                m_Shape = 12;
                break;
            case "スローイングスター":
                m_Shape = 13;
                break;
            case "ハルバード":
                m_Shape = 14;
                break;
            case "フラッシュ":
                m_Shape = 15;
                break;
            case "ソニック":
                m_Shape = 16;
                break;
            case "レーザー":
                m_Shape = 17;
                break;
            case "ボム":
                m_Shape = 18;
                break;
            case "メテオ":
                m_Shape = 19;
                break;
        }
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


                switch (m_MoveType)//バラバラに発射するようにする
                {
                    case 5://落下移動
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), 20, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), createTama.transform.rotation) as GameObject;
                        break;

                    case 10://自身から発射
                        createTama = Instantiate(tama1[m_Shape], new Vector3(-20, 0, 0), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), createTama.transform.rotation) as GameObject;
                        break;

                    case 20://地面から攻撃
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), -10, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), createTama.transform.rotation) as GameObject;
                        break;

                    case 40://様々な方向から
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), createTama.transform.rotation) as GameObject;
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
                images.transform.position = Camera.main.WorldToScreenPoint(m_getpos + new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0));
            }
            m_TextFlag = false;
        }
    }
}