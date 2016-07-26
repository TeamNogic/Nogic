using UnityEngine;
using UnityEngine.UI;

public class Char_Scene : MonoBehaviour
{
    public enum Char_SceneState
    {
        WaitP1,         //プレイヤー１を選択中
        SelectP1,       //プレイヤー１を選択時に一度だけ
        WaitP2,         //プレイヤー2を選択中
        SelectP2,       //プレイヤー2を選択時に一度だけ
        SelectFinish,   //プレイヤー選択完了
    }

    [HideInInspector]
    public Char_SceneState m_State;
    [HideInInspector]
    public bool m_SelectEnd = false;
    [HideInInspector]
    public bool m_SelectInvalid = false;

    public Image[] m_CursorImage;   //選択時に表示するカーソル
    public Sprite[] m_CursorSprite;   //MainCursorのスプライト
    public GameObject[] m_CharPrefab;   //キャラクターモデル
    public GameObject[] m_Empty;   //選択したキャラクターを生成する座標
    public AudioClip[] m_AudioClip;   //オーディオ
    public Transform[] m_Button;   //キーボードでの選択時カーソルを移動させる座標
    public Canvas m_Canvas;   //キャンバス
    public Image m_MainCursor;   //マウスに追従するカーソル
    public Button m_ButtonOk;   //決定ボタン
    public GameObject m_FadeIn;   //フェードインパネル
    public GameObject m_Smoke;   //スモークパーティクル

    public GameObject m_AudioSourceObj;

    private GameObject[] m_InstantiateModel = new GameObject[2];    //選択したキャラクターモデル
    private Image[] cursor = new Image[2]; //選択時に生成されるカーソル
    private AudioSource m_AudioSource;
    private float m_Count = 0.0f;
    private bool m_KeyboardSelect = false;

    void Start()
    {
        Cursor.visible = false;
        m_ButtonOk.gameObject.SetActive(false);
        m_AudioSource = m_AudioSourceObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (m_State)
        {
            case Char_SceneState.WaitP1:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    m_KeyboardSelect = true;
                    Char_SelectData.player_1 = 0;
                    m_State = Char_SceneState.SelectP1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    m_KeyboardSelect = true;
                    Char_SelectData.player_1 = 1;
                    m_State = Char_SceneState.SelectP1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    m_KeyboardSelect = true;
                    Char_SelectData.player_1 = 2;
                    m_State = Char_SceneState.SelectP1;

                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    m_KeyboardSelect = true;
                    Char_SelectData.player_1 = 3;
                    m_State = Char_SceneState.SelectP1;
                }
                //プレイヤー１を選択中
                break;

            case Char_SceneState.SelectP1:
                //決定音
                m_AudioSource.PlayOneShot(m_AudioClip[0]);

                //クリックした場所にカーソルを表示
                cursor[0] = Instantiate(m_CursorImage[0], m_MainCursor.transform.localPosition, Quaternion.identity) as Image;
                cursor[0].transform.SetParent(m_Canvas.transform, false);

                //キーボード選択時カーソルを移動
                if (m_KeyboardSelect)
                    cursor[0].transform.localPosition = m_Button[Char_SelectData.player_1].localPosition;

                //カーソルの画像を変更
                m_MainCursor.sprite = m_CursorSprite[1];

                //Player1モデル
                m_InstantiateModel[0] = Instantiate(
                    m_CharPrefab[Char_SelectData.player_1],
                    m_Empty[0].transform.position,
                   new Quaternion(0.0f, 100.0f, 0.0f, 1.0f)) as GameObject;

                m_KeyboardSelect = false;

                m_State = Char_SceneState.WaitP2;

                break;

            case Char_SceneState.WaitP2:
                m_Count += Time.deltaTime;

                //キーボードで選択時
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (Char_SelectData.player_1 == 0)
                    {
                        m_AudioSource.PlayOneShot(m_AudioClip[2]);
                    }
                    else
                    {
                        m_KeyboardSelect = true;
                        Char_SelectData.player_2 = 0;
                        m_State = Char_SceneState.SelectP2;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (Char_SelectData.player_1 == 1)
                    {
                        m_AudioSource.PlayOneShot(m_AudioClip[2]);
                    }
                    else
                    {
                        m_KeyboardSelect = true;
                        Char_SelectData.player_2 = 1;
                        m_State = Char_SceneState.SelectP2;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (Char_SelectData.player_1 == 5)
                    {
                        m_AudioSource.PlayOneShot(m_AudioClip[2]);
                    }
                    else
                    {
                        m_KeyboardSelect = true;
                        Char_SelectData.player_2 = 2;
                        m_State = Char_SceneState.SelectP2;
                    }

                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    if (Char_SelectData.player_1 == 3)
                    {
                        m_AudioSource.PlayOneShot(m_AudioClip[2]);
                    }
                    else
                    {
                        m_KeyboardSelect = true;
                        Char_SelectData.player_2 = 3;
                        m_State = Char_SceneState.SelectP2;
                    }
                }
                //プレイヤー1の選択キャンセル
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace)) && m_Count > 1.0f)
                {
                    m_Count = 0.0f;
                    m_AudioSource.PlayOneShot(m_AudioClip[1]);
                    Instantiate(m_Smoke, m_Empty[0].transform.position, m_Smoke.transform.rotation);
                    Destroy(m_InstantiateModel[0].gameObject);
                    Destroy(cursor[0].gameObject);
                    m_MainCursor.sprite = m_CursorSprite[0];
                    m_State = Char_SceneState.WaitP1;
                }

                break;

            case Char_SceneState.SelectP2:
                //決定音
                m_AudioSource.PlayOneShot(m_AudioClip[0]);

                //クリックした場所にカーソルを表示
                cursor[1] = Instantiate(m_CursorImage[1], m_MainCursor.transform.localPosition, Quaternion.identity) as Image;
                cursor[1].transform.SetParent(m_Canvas.transform, false);

                //キーボード選択時カーソルを移動
                if (m_KeyboardSelect)
                    cursor[1].transform.localPosition = m_Button[Char_SelectData.player_2].localPosition;

                //カーソルの画像を変更
                m_MainCursor.sprite = m_CursorSprite[2];

                //Player2モデルを生成
                m_InstantiateModel[1] = Instantiate(
                    m_CharPrefab[Char_SelectData.player_2],
                    m_Empty[1].transform.position,
                    new Quaternion(0.0f, 100.0f, 0.0f, 1.0f)) as GameObject;

                m_KeyboardSelect = false;

                m_State = Char_SceneState.SelectFinish;

                break;

            case Char_SceneState.SelectFinish:
                m_Count += Time.deltaTime;

                //決定ボタンを表示
                m_ButtonOk.gameObject.SetActive(true);

                //プレイヤー２の選択キャンセル
                if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace)) && m_Count > 1.0f)
                {
                    m_Count = 0.0f;
                    m_MainCursor.sprite = m_CursorSprite[1];
                    m_AudioSource.PlayOneShot(m_AudioClip[1]);
                    Instantiate(m_Smoke, m_Empty[1].transform.position, m_Smoke.transform.rotation);
                    Destroy(m_InstantiateModel[1].gameObject);
                    Destroy(cursor[1].gameObject);
                    m_ButtonOk.gameObject.SetActive(false);
                    m_State = Char_SceneState.WaitP2;
                }

                //キーボードの場合Enterで決定
                if (Input.GetKeyDown(KeyCode.Return))
                    m_SelectEnd = true;

                //選択終了
                if (m_SelectEnd)
                {
                    m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
                }
                break;
        }
    }
}

