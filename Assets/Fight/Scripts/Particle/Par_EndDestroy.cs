using UnityEngine;
using System.Collections;

public class Par_EndDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    void LateUpdate()
    {
        //パーティクル再生終了で消去
        if (!this.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
