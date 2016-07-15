using UnityEngine;
using System.Collections;

public class UI_Confetti : MonoBehaviour
{
    private float move;

    void Awake()
    {
        //紙吹雪の画像変更
        this.GetComponent<UI_ImageChange>().select = Random.Range(0, 4);

        //回転ランダム化
        this.GetComponent<UI_Rotate>().Setup(new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f))
            , Random.Range(1.0f, 2.0f), false);

        //移動速度
        move = Random.Range(-40.0f, 40.0f);
    }
    
    void Update()
    {
        this.transform.position += new Vector3(move, -150.0f) * Time.deltaTime;

        if (this.transform.position.y <= -100.0f) Destroy(this.gameObject);
    }
}
