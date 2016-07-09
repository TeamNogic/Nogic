using UnityEngine;
using System.Collections;

public class NodeParticle: MonoBehaviour
{
    public int m_Max = 10;
    public float m_Speed = 1.0f;
    public ParticleSystem m_Particle;

    void Start ()
    {
        m_Particle = GetComponent<ParticleSystem>();
        m_Particle.playbackSpeed = m_Speed;
    }
	
	void Update ()
    {
        m_Particle.maxParticles = Mathf.RoundToInt(m_Max);
    }
}
