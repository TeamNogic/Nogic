using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Blur : MonoBehaviour
{
    public float m_BulrWidth = 0.00052083333f;
    public float m_BulrHeight = 0.00092592592f;
    public Material material = null;
    public Shader m_Shader = null;

    void Awake()
    {
        if (m_Shader == null)
        {
            m_Shader = Shader.Find("Custom/Blur");
        }

        material = new Material(m_Shader);
        material.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
        material.SetFloat("_Width", m_BulrWidth);
        material.SetFloat("_Height", m_BulrHeight);
    }
}
