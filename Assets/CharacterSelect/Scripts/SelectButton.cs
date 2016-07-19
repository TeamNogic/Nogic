using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public AudioClip m_AudioClip;   //オーディオ
    public GameObject m_AudioSourceObj;   //オーディオオブジェクト

    [SerializeField]
    private Char_Scene m_CharScene;

    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = m_AudioSourceObj.GetComponent<AudioSource>();
    }

    public void Button(int charNum)
    {
        if (!m_CharScene.m_SelectInvalid)
        {
            if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP1)
            {
                Char_SelectData.player_1 = charNum;
                m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP1;
            }


            if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP2)
            {
                if (Char_SelectData.player_1 == charNum)
                {
                    m_AudioSource.PlayOneShot(m_AudioClip);
                }
                else
                {
                    Char_SelectData.player_2 = charNum;
                    m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP2;
                }
            }

        }
    }

    public void Button_OK()
    {
        m_CharScene.m_SelectEnd = true;
    }

}