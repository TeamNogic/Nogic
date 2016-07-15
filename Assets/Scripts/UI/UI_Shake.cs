using UnityEngine;
using System.Collections;

public class UI_Shake : MonoBehaviour
{
    private float count = 0.0f;
    private float move = 0.0f;
    private float power = 20.0f;

    private Vector3 basePos;

    void Start()
    {
        basePos = this.GetComponent<RectTransform>().localPosition;
    }

    public void End()
    {
        this.GetComponent<RectTransform>().localPosition = basePos;
        Destroy(this.GetComponent<UI_Shake>());
    }

    void Update()
    {
        ++count;
        power -= Time.deltaTime * 75.0f;
        move = Mathf.Sin(count) * power;

        if (power <= 0.0f) this.End();
        else this.GetComponent<RectTransform>().localPosition = basePos + new Vector3(0.0f, move);
    }
}
