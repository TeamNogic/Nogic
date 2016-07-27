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

    [SerializeField]
    private Image[] m_CursorImage;   //選択時に表示するカーソル
    [SerializeField]
    private Sprite[] m_CursorSprite;   //MainCursorのスプライト
    [SerializeField]
    private GameObject[] m_CharPrefab;   //キャラクターモデル
    [SerializeField]
    private GameObject[] m_Empty;   //選択したキャラクターを生成する座標
    [SerializeField]
    private AudioClip[] m_AudioClip;   //オーディオ
    [SerializeField]
    private Canvas m_Canvas;   //キャンバス
    [SerializeField]
    private Image m_MainCursor;   //マウスに追従するカーソル
    [SerializeField]
    private Button m_ButtonOk;   //決定ボタン
    [SerializeField]
    private GameObject m_FadeIn;   //フェードインパネル
    [SerializeField]
    private GameObject m_Smoke;   //スモークパーティクル

    public GameObject m_AudioSourceObj;

    private GameObject[] m_InstantiateModel = new GameObject[2];    //選択したキャラクターモデル
    private Image[] cursor = new Image[2]; //選択時に生成されるカーソル
    private AudioSource m_AudioSource;
    private float m_Count = 0.0f;

    void Start()
    {
        m_ButtonOk.gameObject.SetActive(false);
        m_AudioSource = m_AudioSourceObj.GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (m_State)
        {
            case Char_SceneState.WaitP1:
                //プレイヤー１を選択中
                break;

            case Char_SceneState.SelectP1:
                //決定音
                m_AudioSource.PlayOneShot(m_AudioClip[0]);

                //クリックした場所にカーソルを表示
                cursor[0] = Instantiate(m_CursorImage[0], m_MainCursor.transform.localPosition, Quaternion.identity) as Image;
                cursor[0].transform.SetParent(m_Canvas.transform, false);

                //カーソルの画像を変更
                m_MainCursor.sprite = m_CursorSprite[1];

                //Player1モデル
                m_InstantiateModel[0] = Instantiate(
                    m_CharPrefab[Char_SelectData.player_1],
                    m_Empty[0].transform.position,
                   new Quaternion(0.0f, 100.0f, 0.0f, 1.0f)) as GameObject;

                m_State = Char_SceneState.WaitP2;

                break;

            case Char_SceneState.WaitP2:
                m_Count += Time.deltaTime;
                //プレイヤー1の選択キャンセル
                if (Input.GetMouseButtonDown(1) && m_Count > 1.0f)
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

                //カーソルの画像を変更
                m_MainCursor.sprite = m_CursorSprite[2];

                //Player2モデルを生成
                m_InstantiateModel[1] = Instantiate(
                    m_CharPrefab[Char_SelectData.player_2],
                    m_Empty[1].transform.position,
                    new Quaternion(0.0f, 100.0f, 0.0f, 1.0f)) as GameObject;

                m_State = Char_SceneState.SelectFinish;

                break;

            case Char_SceneState.SelectFinish:
                m_Count += Time.deltaTime;
                //決定ボタンを表示
                m_ButtonOk.gameObject.SetActive(true);

                //プレイヤー２の選択キャンセル
                if (Input.GetMouseButtonDown(1) && m_Count > 1.0f)
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

                //選択終了
                if (m_SelectEnd)
                {
                    m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
                }
                break;
        }
    }
}

