using UnityEngine;
using System.Collections;

public class UI_MusicalScore : MonoBehaviour
{
    private float move;

    void Awake()
    {
        //楽譜の画像変更
        this.GetComponent<UI_ImageChange>().select = Random.Range(0, 3);
    }

    void Update()
    {
        this.transform.position += new Vector3(300.0f * Time.deltaTime, 0.0f);

        if (1000.0f < this.transform.position.x) Destroy(this.gameObject);
    }
}
