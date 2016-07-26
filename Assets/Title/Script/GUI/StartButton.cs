using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public GameObject m_FadeIn;
    public AudioClip m_ClickAudio;

    private AudioSource m_AudioSource;
    private bool isStart = false;

    void Start()
    {
        m_AudioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isStart)
        {
            isStart = true;
            m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
            m_AudioSource.clip = m_ClickAudio;
            m_AudioSource.Play();
        }

    }

    public void OnClick()
    {
        if (!isStart)
        {
            isStart = true;
            m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
            m_AudioSource.clip = m_ClickAudio;
            m_AudioSource.Play();
        }

    }
}
