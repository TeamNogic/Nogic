using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        TimeOk = false;
        isEnd = false;

        m_TextFlag = false;

        m_now_kazu = 0;

        m_nuw_time = 0.0f;
        canvas = GameObject.Find("NodeEditor");

        m_kazu = Sys_Status.Action_Object.Number;
        m_get_kazu = Sys_Status.Action_Object.Number;
        m_MoveType = Sys_Status.Action_Object.Move;
        turn = Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_Tern_Time;
        NodeHindrance_turn = Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_NodeHindrance_Time;
        //tagetP = Sys_Status.targetPlayer;
        State_turns.sprite = null;
        State_NodeHindrance.sprite = null;
    }

    void Update()
    {
        //GameObject.Find("Current").GetComponent<Sys_Current>().m_getpos = targetPosition;
        Image image_dousa = null;
        Image image_bougay = null;
        Image image_NodeHindrance = null;
        //Image image_Festival = null;
        Image turns = null;
        Image NodeHindrance_turns = null;

        if (TimeOk == true)//実行
        {
            TimeCount += Time.deltaTime;
            isEnd = false;
        }
        if (TimeCount >= 2.5f)//2.5f
        {
            Destroy(this.gameObject);
        }
        if (!TimeOk && m_get_kazu == 0 && (m_Seisei[0] == 0 || m_Seisei[1] == 0 || m_Seisei[2] == 0 || m_Seisei[3] == 0 || m_Seisei[4] == 0))
        {
            //Debug.Log(m_get_kazu);
            /*if (GameObject.Find("Poison(Clone)") != null) Destroy(GameObject.Find("Poison(Clone)"));
            if (GameObject.Find("Parasite(Clone)") != null) Destroy(GameObject.Find("Parasite(Clone)"));
            if (GameObject.Find("Interference(Clone)") != null) Destroy(GameObject.Find("Interference(Clone)"));
            if (GameObject.Find("Smoke(Clone)") != null) Destroy(GameObject.Find("Smoke(Clone)"));
            if (GameObject.Find("Festival(Clone)") != null) Destroy(GameObject.Find("Festival(Clone)"));
            if (GameObject.Find("turn(Clone)") != null) Destroy(GameObject.Find("turn(Clone)"));
            if (GameObject.Find("NodeHindranceBangou(Clone)") != null) Destroy(GameObject.Find("NodeHindranceBangou(Clone)"));*/
            TimeOk = true;
            isEnd = true;
            //for (int i = 0; i < 2; i++)
            //{
            if (image_dousa == null)
            {
                switch (Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_Tern)
                //switch (Sys_Status.Action_UI.State_Tern)
                {
                    case 0:
                        //Debug.Log("ooo");
                        m_Seisei[0] = 1;
                        break;
                    case 1:
                        //Debug.Log("aaa");
                        image_dousa = Instantiate(Poison, new Vector3(-100, 130, 0), Poison.transform.rotation) as Image;
                        image_dousa.transform.SetParent(canvas.transform, false);
                        m_Seisei[0] = 1;
                        break;
                    case 2:
                        //Debug.Log("bbb");
                        image_dousa = Instantiate(Parasite, new Vector3(-100, 130, 0), Parasite.transform.rotation) as Image;
                        image_dousa.transform.SetParent(canvas.transform, false);
                        m_Seisei[0] = 1;
                        break;
                }
                if (Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_NodeKey != 0 && Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_NodeEditor != 0 && image_bougay == null)
                //if (Sys_Status.Action_UI.State_Etc == true && image_bougay == null) //妨害が発生するかどうか（UIで「ノード妨害追加！」と出る）
                {
                    image_bougay = Instantiate(Interference, new Vector3(0, 270, 0), Interference.transform.rotation) as Image;//Interference.transform.position
                    image_bougay.transform.SetParent(canvas.transform, false);
                    m_Seisei[1] = 1;
                }
                else
                {
                    m_Seisei[1] = 1;
                }

            }
            if (image_NodeHindrance == null)
            {
                switch (Sys_Status.Player_Wait[Sys_Status.targetPlayer].State_NodeHindrance)//1:スモーク　2:フェスティバル
                {
                    case 0:
                        m_Seisei[2] = 1;
                        break;
                    case 1:
                        image_NodeHindrance = Instantiate(Smoke, new Vector3(-100, 180, 0), Smoke.transform.rotation) as Image;
                        image_NodeHindrance.transform.SetParent(canvas.transform, false);
                        m_Seisei[2] = 1;
                        break;
                    case 2:
                        image_NodeHindrance = Instantiate(Festival, new Vector3(-100, 180, 0), Festival.transform.rotation) as Image;
                        image_NodeHindrance.transform.SetParent(canvas.transform, false);
                        m_Seisei[2] = 1;
                        break;
                }
            }
            //}
            Debug.Log(turn);
            //if (Oldturn[Sys_Status.targetPlayer] != turn)
            //{
            //    Oldturn[Sys_Status.targetPlayer] = turn;
            //    if (Oldturn[Sys_Status.targetPlayer] != 0)
            //    {
            //        State_turns.sprite = sprite[Oldturn[Sys_Status.targetPlayer]];
            //    }
            //    else
            //    {
            //        Debug.Log("www");
            //        m_Seisei[3] = 1;
            //    }
            //}
            if (State_turns.sprite != null && turns == null)//ターン数
            {
                turns = Instantiate(State_turns, new Vector3(100, 130, 0), State_turns.transform.rotation) as Image;
                turns.transform.SetParent(canvas.transform, false);
                m_Seisei[3] = 1;
            }
            if (Old_NodeHindrance_turn[Sys_Status.targetPlayer] != NodeHindrance_turn)
            {
                Old_NodeHindrance_turn[Sys_Status.targetPlayer] = NodeHindrance_turn;
                if (Old_NodeHindrance_turn[Sys_Status.targetPlayer] != 0)
                {
                    State_NodeHindrance.sprite = sprite[Old_NodeHindrance_turn[Sys_Status.targetPlayer]];//スモーク or フェスティバルの効果ターン数
                }
                else
                {
                    Debug.Log("zzz");
                    m_Seisei[4] = 1;
                }
            }
            if (State_NodeHindrance.sprite != null && NodeHindrance_turns == null)
            {
                NodeHindrance_turns = Instantiate(State_NodeHindrance, new Vector3(100, 180, 0), State_NodeHindrance.transform.rotation) as Image;
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
                GameObject AddEfect = null;

                switch (m_MoveType)//バラバラに発射するようにする
                {
                    case 5://落下移動
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), 20, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 10://自身から発射
                        createTama = Instantiate(tama1[m_Shape], new Vector3(-20, 0, 0), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 20://地面から攻撃
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), -10, Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
                        break;

                    case 40://様々な方向から
                        createTama = Instantiate(tama1[m_Shape], new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f)), tama1[m_Shape].transform.rotation) as GameObject;//球生成
                        createType = Instantiate(EfectTypes[m_Type], new Vector3(0, 0, 0), Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)) as GameObject;
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