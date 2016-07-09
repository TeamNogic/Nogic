using UnityEngine;
using System.Collections;

public class Sys_Sound : MonoBehaviour
{
    private static GameObject prefab;

    void Awake()
    {
        prefab = this.gameObject;
    }

    public static void Play(AudioClip clip)
    {
        prefab.GetComponent<Sys_Sound>().GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
