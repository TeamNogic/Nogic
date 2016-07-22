using UnityEngine;
using System.Collections;

public class Sys_Current : MonoBehaviour
{
    public int[,] m_kazu = new int[2, 7];
    public int[] count=new int[2];
    public int[,] m_get = new int[2, 7];
    public int m_Ok;
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
                if (Sys_Status.Player[i].State_NodeKey != 0 && Sys_Status.Player[i].State_NodeEditor != 0)
                {
                    if (GameObject.Find("Interference(Clone)") != null &&m_kazu[i, 0] == 0)
                    {
                        Interferences[i] = GameObject.Find("Interference(Clone)");
                        Interferences[i].gameObject.name = "Interference(Clone)" + i;
                        m_kazu[i, 0] = 1;
                    }
                    else if (count[i] == 2 && Interferences[i] != null && m_Ok == 1)
                    {
                            Interferences[i] = null;
                            Destroy(GameObject.Find("Interference(Clone)" + i));
                            m_kazu[i, 0] = 0;
                            m_Ok = 0;
                    }
                }
                switch (Sys_Status.Player[i].State_Tern)
                {
                    case 0:
                        if (Poisons[i] != null) Destroy(GameObject.Find("Poison(Clone)" + i));
                        if (Parasites[i] != null) Destroy(GameObject.Find("Poison(Clone)" + i));
                        break;
                    case 1:
                        if (GameObject.Find("Poison(Clone)") != null && m_kazu[i, 1] == 0)
                        {
                            Poisons[i] = GameObject.Find("Poison(Clone)");
                            Poisons[i].gameObject.name = "Poison(Clone)" + i;
                            m_kazu[i, 1] = 1;
                        }
                        else if (count[i] == 2 && Poisons[i] != null && m_Ok==1)
                        {
                                Poisons[i] = null;
                                Destroy(GameObject.Find("Poison(Clone)" + i));
                                m_kazu[i, 1] = 0;
                                m_Ok = 0;
                        }
                        break;
                    case 2:
                        if (GameObject.Find("Parasite(Clone)") != null && m_kazu[i, 2] == 0)//生成
                        {
                            Parasites[i] = GameObject.Find("Parasite(Clone)");
                            Parasites[i].gameObject.name = "Parasite(Clone)" + i;
                            m_kazu[i, 2] = 1;
                        }
                        else if (count[i] == 2 && Parasites[i] != null && m_Ok == 1)//削除
                        {
                                Parasites[i] = null;
                                Destroy(GameObject.Find("Parasite(Clone)" + i));
                                m_kazu[i, 2] = 0;
                                m_Ok = 0;
                        }
                        break;
                }
                if (Sys_Status.Player[i].State_Tern_Time != 0 && GameObject.Find("turn(Clone)") != null && m_kazu[i, 3] == 0)
                {
                    turns[i] = GameObject.Find("turn(Clone)");
                    turns[i].gameObject.name = "turn(Clone)" + i;
                    m_kazu[i, 3] = 1;
                }
                else if (count[i] == 2 && turns[i] != null && m_Ok == 1)
                {
                        turns[i] = null;
                        Destroy(GameObject.Find("turn(Clone)" + i));
                        m_kazu[i, 3] = 0;
                        m_Ok = 0;
                }
                switch (Sys_Status.Player[i].State_NodeHindrance)
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
                        else if (count[i] == 2 && Smokes[i] != null && m_Ok == 1)
                        {
                                Smokes[i] = null;
                                Destroy(GameObject.Find("Smoke(Clone)" + i));
                                m_kazu[i, 4] = 0;
                                m_Ok = 0;
                        }
                        break;
                    case 2:
                        if (GameObject.Find("Festival(Clone)") != null && m_kazu[i, 5] == 0)
                        {
                            Festivals[i] = GameObject.Find("Festival(Clone)");
                            Festivals[i].gameObject.name = "Festival(Clone)" + i;
                            m_kazu[i, 5] = 1;
                        }
                        else if (count[i] == 2 && Festivals[i] != null && m_Ok == 1)
                        {
                                Festivals[i] = null;
                                Destroy(GameObject.Find("Festival(Clone)" + i));
                                m_kazu[i, 5] = 0;
                                m_Ok = 0;
                        }
                        break;
                }
                if (Sys_Status.Player[i].State_NodeHindrance_Time != 0 && GameObject.Find("NodeHindranceBangou(Clone)") != null && m_kazu[i, 6] == 0)
                {
                    NodeHindranceBangous[i] = GameObject.Find("NodeHindranceBangou(Clone)");
                    NodeHindranceBangous[i].gameObject.name = "NodeHindranceBangou(Clone)" + i;
                    m_kazu[i, 6] = 1;
                }
                else if (count[i] == 2 && NodeHindranceBangous[i] != null && m_Ok == 1)
                {
                        NodeHindranceBangous[i] = null;
                        Destroy(GameObject.Find("NodeHindranceBangou(Clone)" + i));
                        m_kazu[i, 6] = 0;
                        m_Ok = 0;
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