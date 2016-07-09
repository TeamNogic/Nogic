using UnityEngine;
using System.Collections;

public class UI_Perfect : MonoBehaviour
{
    public AudioClip perfectSound;

    void Update()
    {
        if (this.GetComponent<UI_Scale>() == null)
        {
            this.gameObject.AddComponent<UI_Alpha>();
            this.gameObject.GetComponent<UI_Alpha>().Setup(1.0f, 0.0f, 0.5f, true);

            Sys_Sound.Play(perfectSound);

            Destroy(this.GetComponent<UI_Perfect>());
        }
    }
}
