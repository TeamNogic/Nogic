using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour
{
    private bool isStart = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isStart)
        {
            Application.Quit();
        }

    }

    public void OnClick()
    {
        if (!isStart)
            Application.Quit();
    }
}
