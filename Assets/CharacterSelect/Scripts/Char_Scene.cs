using UnityEngine;
using UnityEngine.UI;

public class Char_Scene : MonoBehaviour
{
    public enum Char_SceneState
    {
        WaitP1,
        SelectP1,
        WaitP2,
        SelectP2,
        SelectFinish,
        End
    }

    public Char_SceneState m_State;

    [HideInInspector]
    public int m_Select;

    [SerializeField]
    private Canvas m_Canvas;
    [SerializeField]
    private Image m_MainCursor;
    [SerializeField]
    private Sprite[] m_CursorSprite;
    [SerializeField]
    private Image[] m_CursorImage;
    [SerializeField]
    private Button m_ButtonOk;
    [SerializeField]
    private GameObject[] m_CharPrefab;
    [SerializeField]
    private GameObject[] m_Empty;
    [SerializeField]
    private float m_WaitTime;

    private float m_TimeCount = 0.0f;

    void Start()
    {
        m_ButtonOk.gameObject.SetActive(false);
    }

    void Update()
    {
        Scene();
    }

    void Scene()
    {
        switch (m_State)
        {
            case Char_SceneState.WaitP1:
                //none
                break;

            case Char_SceneState.SelectP1:
                Char_SelectData.player_1 = m_Select;
                //カーソル
                Image cursor1 = Instantiate(m_CursorImage[0],
                    m_MainCursor.transform.localPosition,
                    Quaternion.identity) as Image;

                cursor1.transform.SetParent(m_Canvas.transform, false);

                //Player1モデル
                Instantiate(m_CharPrefab[Char_SelectData.player_1],
                    m_Empty[0].transform.position,
                    new Quaternion(0.0f, 100.0f, 0.0f, 1.0f));

                m_MainCursor.sprite = m_CursorSprite[0];
                m_State++;

                break;

            case Char_SceneState.WaitP2:
                //none

                break;

            case Char_SceneState.SelectP2:
                Char_SelectData.player_2 = m_Select;
                //カーソル
                Image cursor2 = Instantiate(m_CursorImage[1],
                    m_MainCursor.transform.localPosition,
                    Quaternion.identity) as Image;
                    
                cursor2.transform.SetParent(m_Canvas.transform, false);

                //Player2モデル
                Instantiate(m_CharPrefab[Char_SelectData.player_2],
                    m_Empty[1].transform.position,
                    new Quaternion(0.0f, 100.0f, 0.0f, 1.0f));
                m_State++;

                break;

            case Char_SceneState.SelectFinish:
                m_ButtonOk.gameObject.SetActive(true);
                m_State++;
                break;

            case Char_SceneState.End:
                break;

        }
    }

    void ShowCharacter()
    {
        switch (Char_SelectData.player_1)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;
        }
    }
}

