using UnityEngine;
using System.Collections;

public class Par_Fireball : MonoBehaviour
{
    public ParticleSystem particle;
    
    public float m_Size;
    public bool m_extinguishFlag = false;
    public bool m_extFlag = false;
    public float m_destroytime;
    GameObject get;
    //public Color colour;
    void Start()
    {
        //particle.startColor = colour
        //GetComponent<CRETR>();
        //particle = efect.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.startSize = m_Size;
        }
    }

    void Update()
    {
        /*
        if (GameObject.Find("CRETR").transform.IsChildOf(transform))
        {
            particle =  ("CRETR");
        }*/

        if (particle != null)
        {
            particle.startSize = m_Size;
            if (m_extinguishFlag == true)
            {
                this.transform.DetachChildren();
                Destroy(this.gameObject, m_destroytime);
            }
        }
    }
}
