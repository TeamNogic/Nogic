using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public Material m_FadeIn;
    public float m_FadeSpeed = 1.0f;
    public string m_SceneName = "main";
    public bool m_IsFade = false;

    private float m_FadeCnt = 0.0f;

	void Update ()
    {
        m_FadeIn.SetFloat("_AlphaValue", m_FadeCnt);

        if (m_IsFade)
        {
            if (m_FadeCnt > 1.0f)
            {
                Application.LoadLevel(m_SceneName);
            }

            m_FadeCnt += m_FadeSpeed * Time.deltaTime;
        }
    }
}
