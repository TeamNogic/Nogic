using UnityEngine;
using System.Collections;

public class Sys_Current : MonoBehaviour
{
    public int[,] m_kazu = new int[2, 7];
    public int[,] count=new int[2,7];
    public int m_Ok;
    public bool[] m_StateAppear; 
    public Vector3[] m_getpos = new Vector3[2];
    public GameObject[] Poisons = new GameObject[2];
    public GameObject[] Parasites = new GameObject[2];
    public GameObject[] Interferences = new GameObject[2];
    public GameObject[] turns = new GameObject[2];
    public GameObject[] Smokes = new GameObject[2];
    public GameObject[] Festivals = new GameObject[2];
    public GameObject[] NodeHindranceBangous = new GameObject[2];
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < 2; i++)
        {
            //Debug.Log(Sys_Status.Player.Count);
            if (Sys_Status.Player.Count != 0)
            {
                if (Sys_Status.Player_Wait[i].State_NodeKey != 0 && Sys_Status.Player_Wait[i].State_NodeEditor != 0)//ノード妨害
                {
                    if (GameObject.Find("Interference(Clone)") != null && m_kazu[i, 0] == 0)
                    {
                        Interferences[i] = GameObject.Find("Interference(Clone)");
                        Interferences[i].gameObject.name = "Interference(Clone)" + i;
                        m_kazu[i, 0] = 1;
                    }
                    else if (count[i,0] == 2 && Interferences[i] != null)
                    {
                       Destroy(GameObject.Find("Interference(Clone)" + i));
                       Interferences[i] = null;
                       m_kazu[i, 0] = 0;
                       count[i,0] = 1;
                    }
                }else
                {
                    Destroy(GameObject.Find("Interference(Clone)" + i));
                    Interferences[i] = null;
                    //count[i] = 1;
                }

                switch (Sys_Status.Player_Wait[i].State_Tern)
                {
                    case 0:
                        Debug.Log("o");
                        if (Poisons[i] != null) Destroy(GameObject.Find("Poison(Clone)" + i));
                        if (Parasites[i] != null) Destroy(GameObject.Find("Poison(Clone)" + i));
                        break;
                    case 1:
                        if (GameObject.Find("Poison(Clone)") != null && m_kazu[i, 1] == 0)
                        {
                            Poisons[i] = GameObject.Find("Poison(Clone)");
                            Poisons[i].gameObject.name = "Poison(Clone)" + i;
                            m_kazu[i, 1] = 1;
                            Debug.Log("ua");
                        }
                        else if (count[i,1] == 2 && Poisons[i] != null)
                        {
                            Destroy(GameObject.Find("Poison(Clone)" + i));
                            Poisons[i] = null;
                            m_kazu[i, 1] = 0;
                            Debug.Log("ea");
                            count[i,1] = 1;
                        }
                        break;
                    case 2:
                        if (GameObject.Find("Parasite(Clone)") != null && m_kazu[i, 2] == 0)
                        {
                            Parasites[i] = GameObject.Find("Parasite(Clone)");
                            Parasites[i].gameObject.name = "Parasite(Clone)" + i;
                            m_kazu[i, 2] = 1;
                            Debug.Log("ub");
                        }
                        else if (count[i,2] == 2 && Parasites[i] != null)
                        {
                            Destroy(GameObject.Find("Parasite(Clone)" + i));
                            Parasites[i] = null;
                            m_kazu[i, 2] = 0;
                            Debug.Log("eb");
                            count[i,2] = 1;
                        }
                        break;
                }

                if (Sys_Status.Player_Wait[i].State_Tern_Time != 0 && GameObject.Find("turn(Clone)") != null && m_kazu[i, 3] == 0)
                {
                    turns[i] = GameObject.Find("turn(Clone)");
                    turns[i].gameObject.name = "turn(Clone)" + i;
                    m_kazu[i, 3] = 1;
                }
                else if (count[i,3] == 2 && turns[i] != null)
                {
                    Destroy(GameObject.Find("turn(Clone)" + i));
                    turns[i] = null;
                    m_kazu[i, 3] = 0;
                    count[i,3] = 1;
                }

                switch (Sys_Status.Player_Wait[i].State_NodeHindrance)
                {
                    case 0:
                        if (Smokes[i] != null) Destroy(GameObject.Find("Smoke(Clone)" + i));
                        if (Festivals[i] != null) Destroy(GameObject.Find("Festival(Clone)" + i));
                        break;
                    case 1:
                        if (GameObject.Find("Smoke(Clone)") != null && m_kazu[i, 4] == 0)
                        {
                            Smokes[i] = GameObject.Find("Smoke(Clone)");
                            Smokes[i].gameObject.name = "Smoke(Clone)" + i;
                            m_kazu[i, 4] = 1;
                        }
                        else if (count[i,4] == 2 && Smokes[i] != null)
                        {
                            Destroy(GameObject.Find("Smoke(Clone)" + i));
                            Smokes[i] = null;
                            m_kazu[i, 4] = 0;
                            count[i,4] = 1;
                        }
                        break;
                    case 2:
                        if (GameObject.Find("Festival(Clone)") != null && m_kazu[i, 5] == 0)
                        {
                            Festivals[i] = GameObject.Find("Festival(Clone)");
                            Festivals[i].gameObject.name = "Festival(Clone)" + i;
                            m_kazu[i, 5] = 1;
                        }
                        else if (count[i,5] == 2 && Festivals[i] != null)
                        {
                            Destroy(GameObject.Find("Festival(Clone)" + i));
                            Festivals[i] = null;
                            m_kazu[i, 5] = 0;
                            count[i,5] = 1;
                        }
                        break;
                }
                if (Sys_Status.Player_Wait[i].State_NodeHindrance_Time != 0 && GameObject.Find("NodeHindranceBangou(Clone)") != null && m_kazu[i, 6] == 0)
                {
                    NodeHindranceBangous[i] = GameObject.Find("NodeHindranceBangou(Clone)");
                    NodeHindranceBangous[i].gameObject.name = "NodeHindranceBangou(Clone)" + i;
                    m_kazu[i, 6] = 1;
                }
                else if (count[i,6] == 2 && NodeHindranceBangous[i] != null)
                {
                    Destroy(GameObject.Find("NodeHindranceBangou(Clone)" + i));
                    NodeHindranceBangous[i] = null;
                    m_kazu[i, 6] = 0;
                    count[i,6] = 1;
                }

                if (Poisons[i] != null)
                {
                    Poisons[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(-60, 0, 0);
                }
                if (Parasites[i] != null)
                {
                    Parasites[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(-60, 0, 0);
                }
                if (Interferences[i] != null)
                {
                    Interferences[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(0, 80, 0);
                }
                if (turns[i] != null)
                {
                    turns[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(60, 0, 0);
                }
                if (Smokes[i] != null)
                {
                    Smokes[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(-60, 30, 0);
                }
                if (Festivals[i] != null)
                {
                    Festivals[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(-60, 30, 0);
                }
                if (NodeHindranceBangous[i] != null)
                {
                    NodeHindranceBangous[i].transform.position = Camera.main.WorldToScreenPoint(m_getpos[i]) + new Vector3(60, 30, 0);
                }
            }
        }
    }
}