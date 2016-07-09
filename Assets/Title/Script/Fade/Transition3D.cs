using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Transition3D : MonoBehaviour
{
    private Material m_Material = null;
    public Shader m_Shader = null;
    public float m_AlphaValue = 0.0f;
    public Texture2D m_MaskTexture = null;

    void Awake()
    {
        if (m_Shader == null) m_Shader = Shader.Find("Sprites/Transition2D");

        m_Material = new Material(m_Shader);
        m_Material.hideFlags = HideFlags.HideAndDontSave;
    }

    void Start ()
    {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_Material);
        m_Material.SetFloat("_AlphaValue", m_AlphaValue);
        m_Material.SetTexture("_MaskTex", m_MaskTexture);
    }
}
