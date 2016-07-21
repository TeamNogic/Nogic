using UnityEngine;
using UnityEngine.UI;

public class Stage_Scene : MonoBehaviour
{
    public enum Stage_SceneState
    {
        Wait,         //ステージを選択中
        SelectStage,       //ステージを選択時に一度だけ
        SelectFinish,   //ステージ選択完了
    }

    [HideInInspector]
    public Stage_SceneState m_State;
    [HideInInspector]
    public bool m_SelectEnd = false;
    [HideInInspector]
    public bool m_SelectInvalid = false;

    [SerializeField]
    private Image m_CursorImage;   //選択時に表示するカーソル
    [SerializeField]
    private Sprite m_CursorSprite;   //MainCursorのスプライト
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

    public GameObject m_AudioSourceObj;

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
            case Stage_SceneState.Wait:
                //プレイヤー１を選択中
                break;

            case Stage_SceneState.SelectStage:
                //決定音
                m_AudioSource.PlayOneShot(m_AudioClip[0]);
                m_ButtonOk.gameObject.SetActive(true);

                //クリックした場所にカーソルを表示
                cursor[0] = Instantiate(m_CursorImage, m_MainCursor.transform.localPosition, Quaternion.identity) as Image;
                cursor[0].transform.SetParent(m_Canvas.transform, false);
                m_State = Stage_SceneState.SelectFinish;
                break;

            case Stage_SceneState.SelectFinish:
                m_Count += Time.deltaTime;
                //プレイヤー1の選択キャンセル
                if (Input.GetMouseButtonDown(1) && m_Count > 1.0f)
                {
                    m_Count = 0.0f;
                    m_AudioSource.PlayOneShot(m_AudioClip[1]);
                    Destroy(cursor[0].gameObject);
                    m_MainCursor.sprite = m_CursorSprite;
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

