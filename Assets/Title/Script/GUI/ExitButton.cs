using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Exit click!");
        Application.Quit();
    }
}
