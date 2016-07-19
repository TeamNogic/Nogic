using UnityEngine;
using System.Collections;

public class UI_Tern : MonoBehaviour
{
    private float time = 0.0f;

    void Update()
    {
        time += Time.deltaTime;

        if (3.0f <= time)
        {
            this.gameObject.AddComponent<UI_Scale>();
            this.GetComponent<UI_Scale>().Setup(new Vector2(0.0f, 1.0f), 2.0f, true);

            Destroy(this.GetComponent<UI_Tern>());
        }
    }
}
