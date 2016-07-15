using UnityEngine;
using System.Collections;

public class RadiationBlurSeed : MonoBehaviour
{
    public float m_BulrWidth = 0.00052083333f;
    public float m_BulrHeight = 0.00092592592f;
    public float m_BulrPower = 5.0f;
    public float m_SurvivalTime = 1.0f;
    public Transform m_Target;

    RadiationBlurInfo m_Info;

    void Start()
    {
        if (m_Target == null) m_Target = this.transform;

        m_Info.bulrWidth = m_BulrWidth;
        m_Info.bulrHeight = m_BulrHeight;
        m_Info.bulrPower = m_BulrPower;
        m_Info.survivalTime = m_SurvivalTime;
        m_Info.radiationCenter = Camera.main.WorldToViewportPoint(m_Target.position);

        Camera.main.gameObject.AddComponent<RadiationBlur>().m_Info = m_Info;
    }
}
