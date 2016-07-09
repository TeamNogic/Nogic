using UnityEngine;
using System.Collections;

public struct RadiationBlurInfo
{
   public float bulrWidth;
   public float bulrHeight;
   public float bulrPower;
   public float survivalTime;
   public Vector4 radiationCenter;
}

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RadiationBlur : MonoBehaviour
{
    public Material material = null;
    public Shader m_Shader = null;
    public RadiationBlurInfo m_Info;

    void Awake()
    {
        if (m_Shader == null)
        {
            m_Shader = Shader.Find("Custom/RadiationBlur");
        }

        material = new Material(m_Shader);
        material.hideFlags = HideFlags.HideAndDontSave;
    }

    void Start()
    {
        Destroy(this, m_Info.survivalTime);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
        material.SetFloat("_Width", m_Info.bulrWidth);
        material.SetFloat("_Height", m_Info.bulrHeight);
        material.SetFloat("_BlurPower", m_Info.bulrPower);
        material.SetVector("_RadiationCenter", m_Info.radiationCenter);

        Debug.Log(m_Info.radiationCenter.z);
    }
}
