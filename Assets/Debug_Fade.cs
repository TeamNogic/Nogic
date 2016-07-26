using UnityEngine;
using UnityEngine.UI;

public class Debug_Fade : MonoBehaviour
{
    public Material UNK;
    private GUIStyle style = new GUIStyle();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.green;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 50), "_AlphaValue: " + UNK.GetFloat("_AlphaValue"), style);
    }
}