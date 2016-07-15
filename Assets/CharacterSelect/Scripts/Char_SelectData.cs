using UnityEngine;

public class Char_SelectData : MonoBehaviour
{
    private static int m_Player_1 = 0;
    private static int m_Player_2 = 0;
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            // シーンを切り替えてもオブジェクトを破棄せずに残す
            DontDestroyOnLoad(gameObject);
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Init()
    {
        m_Player_1 = 0;
        m_Player_2 = 0;
    }

    public static int player_1
    {
        get
        {
            return m_Player_1;
        }

        set
        {
            m_Player_1 = value;
        }
    }

    public static int player_2
    {
        get
        {
            return m_Player_2;
        }

        set
        {
            m_Player_2 = value;
        }
    }
}