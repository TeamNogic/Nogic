using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButton : MonoBehaviour
{
    [SerializeField]
    private Char_Scene m_CharScene;

    void Update()
    {

    }

    public void Button_1()
    {
        m_CharScene.m_Select = 0;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP1)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP1;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP2)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP2;
    }

    public void Button_2()
    {
        m_CharScene.m_Select = 1;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP1)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP1;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP2)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP2;
    }

    public void Button_3()
    {
        m_CharScene.m_Select = 2;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP1)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP1;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP2)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP2;
    }

    public void Button_4()
    {
        m_CharScene.m_Select = 3;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP1)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP1;

        if (m_CharScene.m_State == Char_Scene.Char_SceneState.WaitP2)
            m_CharScene.m_State = Char_Scene.Char_SceneState.SelectP2;
    }

    public void Button_OK()
    {
        m_CharScene.m_SelectEnd = true;
    }

}