using UnityEngine;

public class Stage_SelectData : MonoBehaviour
{
    private static int m_Stage = 0;
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

    public static void InitData()
    {
        m_Stage = 0;
    }

    public static int stage
    {
        get
        {
            return m_Stage;
        }

        set
        {
            m_Stage = value;
        }
    }
}