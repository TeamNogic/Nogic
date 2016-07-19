using UnityEngine;
using System.Collections;

public class Sys_SoundFadeOut : MonoBehaviour
{
    private float volume;

    void Start()
    {
        volume = this.GetComponent<AudioSource>().volume;
    }

    void Update()
    {
        volume -= Time.deltaTime * 0.25f;

        if (volume <= 0.0f) Destroy(this.gameObject);
        else this.GetComponent<AudioSource>().volume = volume;
    }
}
