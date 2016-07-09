using UnityEngine;
using System.Collections;

public class Par_EndDestroy : MonoBehaviour
{
    void LateUpdate()
    {
        //パーティクル再生終了で消去
        if (!this.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
