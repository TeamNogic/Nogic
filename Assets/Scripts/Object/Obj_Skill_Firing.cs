using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Obj_Skill_Firing : MonoBehaviour
{
    //public Par_Fireball ball;
    public UI_SkillName_Motion text;
    public GameObject instance_data;

    public Vector3 m_mokutekipos;//タゲット
    private float speed;
    public bool m_DistanceFlag;
    public int attack;
    public int m_Type;
    public bool m_flag;
    public bool m_ObjEndflag;
    public float lpos;
    float m_tagetLenght;
    void Start()
    {
        m_flag = false;
        m_DistanceFlag = true;
        m_ObjEndflag = false;

        speed = Sys_Status.Action_Object.Speed;
        transform.localScale = new Vector3(transform.localScale.x * Sys_Status.Action_Object.Scale
            , transform.localScale.y * Sys_Status.Action_Object.Scale
            , transform.localScale.z * Sys_Status.Action_Object.Scale);

        m_Type = Sys_Status.Action_Object.Move;
       m_tagetLenght = 1.0f * speed;
    }

    void Update()
    {

        if (m_flag == false)
        {
            float dist = Vector3.Distance(m_mokutekipos, transform.position);
            if (m_DistanceFlag == true && dist <= 10.0f)
            {
                if (m_Type == 40)
                {
                    transform.position += new Vector3(dist, dist, dist);
                }
                m_DistanceFlag = false;
            }
            else
            {
                m_DistanceFlag = false;
            }

            float tagetpos = Vector3.Distance(m_mokutekipos, transform.position);
            transform.LookAt(m_mokutekipos);
            transform.Translate(Vector3.forward * speed);

            // onTrigerEnter();
            //Debug.Log(m_Type);
            if (tagetpos <= m_tagetLenght)//目標地点までいけば//tagetpos
            {
                if (instance_data != null)
                {
                    instance_data.GetComponent<Sys_Instance>().m_TextFlag = true;//生成実行 
                    instance_data.GetComponent<Sys_Instance>().text.text = attack.ToString();//テキストにランダムな数字を代入
                    instance_data.GetComponent<Sys_Instance>().m_getpos = m_mokutekipos;
                    instance_data.GetComponent<Sys_Instance>().m_get_kazu -= 1;
                }

                if (transform.FindChild(Sys_Status.Action_Object.Type + "(Clone)") != null)
                {
                    this.transform.FindChild(Sys_Status.Action_Object.Type + "(Clone)").transform.position = m_mokutekipos;
                    this.transform.FindChild(Sys_Status.Action_Object.Type + "(Clone)").parent = null;
                }

                GameObject.Find("Character_" + (Sys_Status.targetPlayer + 1).ToString() + "(Clone)").GetComponent<Animator>().SetBool("Damage", true);

                m_flag = true;
            }
        }
        else
        {
            //Debug.Log(m_ObjEndflag);
            if (m_Type == 20)
            {
                Debug.Log(m_ObjEndflag);
                transform.LookAt(new Vector3(m_mokutekipos.x, 50, m_mokutekipos.z));
                transform.Translate(Vector3.forward * speed);
                float Up_pos = Vector3.Distance(new Vector3(m_mokutekipos.x, 50, m_mokutekipos.z), transform.position);
                if (Up_pos <= m_tagetLenght && m_ObjEndflag == false)
                {
                    m_ObjEndflag = true;
                }
            }
            else
            {
                //Debug.Log(m_ObjEndflag);
                m_ObjEndflag = true;
            }
            if (m_ObjEndflag == true)
            {
                Debug.Log(m_ObjEndflag);
                if (this.GetComponent<Obj_Scale>() == null) this.gameObject.AddComponent<Obj_Scale>();
                this.GetComponent<Obj_Scale>().Target = Vector2.zero;
                this.GetComponent<Obj_Scale>().Speed = 5.0f;
                this.GetComponent<Obj_Scale>().DestroyObjectFlag = true;
                this.GetComponent<Obj_Scale>().Restart();
                //Destroy(this.gameObject);
                Destroy(this.GetComponent<Obj_Skill_Firing>());
            }
        }
    }
    void onTrigerEnter()
    {
        //if(collider.gameObject.tag=="Chara_1")
        //if ()
        //{
        //onTrigerEnter();
        instance_data.GetComponent<Sys_Instance>().m_TextFlag = true;//生成実行 
        instance_data.GetComponent<Sys_Instance>().text.text = attack.ToString();//テキストにランダムな数字を代入
        instance_data.GetComponent<Sys_Instance>().m_getpos = m_mokutekipos;
        m_flag = true;
        //}
    }

}