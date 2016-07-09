using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthofField : MonoBehaviour
{
    private Material m_Material = null;
    public Shader m_Shader = null;

    void Start ()
    {
        Camera camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;

        if (m_Shader == null)
        {
            m_Shader = Shader.Find("Custom/Blur");
        }

        if (m_Material == null)
        {
            m_Material = new Material(m_Shader);
        } 
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_Material);
    }
}
