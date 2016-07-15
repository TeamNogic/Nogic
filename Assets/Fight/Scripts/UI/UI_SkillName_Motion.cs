using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UI_SkillName_Motion : MonoBehaviour
{

    public Image image;
    private float m_Speed;
    private float m_Scale = 0.0f;
    private float m_Counter = 0.0f;
    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Image>();
        image.transform.localScale = Vector3.zero;
        m_Speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Counter <= 1.0f && m_Scale < 0.5f)
        {
            m_Scale += 0.01f * m_Speed;
        }
        else if (m_Counter <= 1.0f)
        {
            m_Counter += Time.deltaTime;
        }
        else if (m_Counter >= 1.0f)
        {
            m_Scale -= 0.01f * m_Speed;
            if (m_Scale <= 0.0f)
            {
                m_Scale = 0.0f;
                Destroy(this.gameObject);
            }
        }
        image.transform.localScale = new Vector3(m_Scale, m_Scale, m_Scale);
    }
}