using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    public Material m_FadeOut;
    public float m_FadeSpeed = 1.0f;
    public float m_StartTime = 0.0f;

    private bool m_IsFade = false;
    private float m_FadeCnt = 1.0f;
    private float m_StartCnt = 1.0f;

    void Start()
    {
        m_FadeOut.SetFloat("_AlphaValue", 1.0f);
        transform.SetSiblingIndex(50);
    }

    void Update()
    {
        if (m_StartCnt > m_StartTime)
        {
            m_IsFade = true;
        }
        else
        {
            m_StartCnt += Time.deltaTime;
        }
        
        if (m_IsFade)
        {
            if (m_FadeCnt < 0.0f)
            {
                m_IsFade = false;
            }

            m_FadeCnt -= m_FadeSpeed * Time.deltaTime;
            m_FadeOut.SetFloat("_AlphaValue", m_FadeCnt);
        }
        m_FadeCnt = Mathf.Clamp(1.2f, 0f, m_FadeCnt);
    }
}
