using UnityEngine;

public class Stage_SelectButton : MonoBehaviour
{
    public AudioClip m_AudioClip;   //オーディオ
    public GameObject m_AudioSourceObj;   //オーディオオブジェクト

    [SerializeField]
    private Stage_Scene m_StageScene;

    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = m_AudioSourceObj.GetComponent<AudioSource>();
    }

    public void Button(int stageNum)
    {
        if (!m_StageScene.m_SelectInvalid)
        {
            if (m_StageScene.m_State == Stage_Scene.Stage_SceneState.Wait)
            {
                Stage_SelectData.stage = stageNum;
                m_StageScene.m_State = Stage_Scene.Stage_SceneState.SelectStage;
            }
        }
    }

    public void Button_OK()
    {
        m_StageScene.m_SelectEnd = true;
        m_AudioSource.PlayOneShot(m_AudioClip);
    }

}