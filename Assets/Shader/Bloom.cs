using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Bloom : MonoBehaviour 
{
    [Range(0.0f, 2.0f)] public float m_Threshhold;   
    private Material m_ExtractionMaterial;
    private Material m_AdditiveMaterial;
    private Material m_BluerMaterial;
    public RenderTexture m_HightLuminance;
    public RenderTexture m_BlurHightLuminance;

    void Start()
    {
        m_Threshhold = 0.5f;
        m_ExtractionMaterial = new Material(Shader.Find("Custom/HightLuminanceExtraction"));
        m_AdditiveMaterial = new Material(Shader.Find("Custom/BloomAdditive"));
        m_BluerMaterial = new Material(Shader.Find("Custom/Blur"));

        if (!m_HightLuminance)
        {
            m_HightLuminance = new RenderTexture(Screen.width / 4, Screen.height / 4, 16, RenderTextureFormat.DefaultHDR);
            m_HightLuminance.Create();

            m_BlurHightLuminance = new RenderTexture(Screen.width / 8, Screen.height / 8, 16, RenderTextureFormat.DefaultHDR);
            m_BlurHightLuminance.Create();
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //輝度抽出
        m_ExtractionMaterial.SetFloat("_Threshhold", m_Threshhold);
        Graphics.Blit(source, m_HightLuminance, m_ExtractionMaterial);

        Graphics.Blit(m_HightLuminance, m_BlurHightLuminance, m_BluerMaterial);

        //シーンの画像をコピー
        Graphics.Blit(source, destination);

        //ブルーム画像を加算ブレンド
        Graphics.Blit(m_BlurHightLuminance, destination, m_AdditiveMaterial);
    }
}
