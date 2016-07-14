using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sys_Line : MonoBehaviour
{
    [SerializeField, Tooltip("線の太さ")]
    private int m_LineWeight = 5;

    private Transform m_StartPos;
    private Transform m_TargetPos;
    private Color m_Color;
    private RectTransform m_RectTransform;
    private float m_ScreenRate = 0.0f;
    private float m_Alpha = 0.0f;

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Color = GetComponent<Image>().color;
    }

    void Update()
    {
        if (m_StartPos != null && m_TargetPos != null)
        {
            transform.localPosition = m_StartPos.localPosition;

            float length = Vector3.Distance(transform.localPosition, m_TargetPos.localPosition);
            Vector3 diff = (m_TargetPos.localPosition - transform.localPosition).normalized;


            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
            m_RectTransform.sizeDelta = new Vector2(m_LineWeight, length);
        }
    }

    public void SetStartPos(Transform startPos)
    {
        m_StartPos = startPos;
    }

    public void SetTargetPos(Transform targetPos)
    {
        m_TargetPos = targetPos;
    }

    public void DeleteLine()
    {
        m_Alpha -= Time.deltaTime;

        if (m_Alpha <= 0.0f)
        {
            m_Alpha = 0.0f;
            Destroy(gameObject);
        }

        m_Color.a = m_Alpha;
    }

}