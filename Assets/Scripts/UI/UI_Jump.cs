using UnityEngine;
using System.Collections;

public class UI_Jump : MonoBehaviour
{
    private float jump = 0.0f;
    private bool move;
    private float ScreenHeight;

    void Start()
    {
        move = Random.Range(0, 2) == 0;
        ScreenHeight = Screen.height;
    }

    void Update()
    {
        jump -= Time.deltaTime * (ScreenHeight * 0.05f);
        this.transform.position += new Vector3(move ? 2.0f : -2.0f, jump);

        if (this.transform.position.y <= -100.0f) Destroy(this.gameObject);
    }
}
