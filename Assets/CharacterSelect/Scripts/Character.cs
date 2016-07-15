using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;

    private Animator m_Anim;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Anim.SetTrigger("Attack");
    }

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, m_Speed * Time.deltaTime, 0.0f));
    }
}