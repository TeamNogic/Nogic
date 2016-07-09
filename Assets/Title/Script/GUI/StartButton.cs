using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    public GameObject m_FadeIn;
    public AudioClip m_ClickAudio;

    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = this.GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        m_FadeIn.GetComponent<FadeIn>().m_IsFade = true;
        m_AudioSource.clip = m_ClickAudio;
        m_AudioSource.Play();
    }
}
