using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    public Material m_FadeIn;
    public float m_FadeSpeed = 1.0f;
    public string m_SceneName = "main";
    public bool m_IsFade = false;

    private float m_FadeCnt = 0.0f;
    
    void Start()
    {
        transform.SetSiblingIndex(50);
    }

	void Update ()
    {
        m_FadeIn.SetFloat("_AlphaValue", m_FadeCnt);

        if (m_IsFade)
        {
            if (m_FadeCnt > 1.0f)
            {
                SceneManager.LoadScene(m_SceneName);
            }

            m_FadeCnt += m_FadeSpeed * Time.deltaTime;
        }
    }
}
