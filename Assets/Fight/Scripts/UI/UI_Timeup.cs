using UnityEngine;
using System.Collections;

public class UI_Timeup : MonoBehaviour
{
    void Update()
    {
        if (this.GetComponent<UI_Scale>() == null)
        {
            this.gameObject.AddComponent<UI_Jump>();

            Destroy(this.GetComponent<UI_Timeup>());
        }
    }
}
