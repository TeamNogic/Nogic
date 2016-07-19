using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    private Canvas m_Canvas;

    void Start()
    {
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_Canvas.transform as RectTransform, Input.mousePosition, m_Canvas.worldCamera, out pos);

        transform.position = m_Canvas.transform.TransformPoint(pos);
    }
}