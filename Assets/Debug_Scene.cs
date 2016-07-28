using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Scene : MonoBehaviour
{
    private GUIStyle style = new GUIStyle();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.red;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 50), "ActiveScene Name: " + SceneManager.GetActiveScene().name, style);
    }
}