using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    [SerializeField]
    private Char_Scene m_CharScene;
    [SerializeField]
    private GameObject m_FadeIn;

    void Update()
    {

    }

    public void Button_1()
    {
        m_CharScene.m_Select = 0;
        m_CharScene.m_State++;
    }

    public void Button_2()
    {
        m_CharScene.m_Select = 1;
        m_CharScene.m_State++;
    }

    public void Button_3()
    {
        m_CharScene.m_Select = 2;
        m_CharScene.m_State++;
    }

    public void Button_4()
    {
        m_CharScene.m_Select = 3;
        m_CharScene.m_State++;
    }

    public void Button_OK()
    {
        m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
    }

}