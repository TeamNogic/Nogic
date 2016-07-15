using UnityEngine;
using System.Collections;

public class Sys_Debug : MonoBehaviour
{
    public GameObject debug;
    private GameObject create = null;

    Vector3 savePosition = new Vector3();
    Quaternion saveRotation = new Quaternion();

    private GameObject Map_1;
    private GameObject Map_2;

    void Start()
    {
        savePosition = this.transform.position;
        saveRotation = this.transform.rotation;

        Map_1 = GameObject.Find("Map_1");
        Map_2 = GameObject.Find("Map_2");
        Map_1.SetActive(true);
        Map_2.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (create == null) create = Instantiate(debug, new Vector3(0.0f, 10.0f, 0.0f), Quaternion.identity) as GameObject;
            else
            {
                Destroy(create);

                this.transform.position = savePosition;
                this.transform.rotation = saveRotation;
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Map_1.SetActive(true);
            Map_2.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Map_1.SetActive(false);
            Map_2.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {

        }
    }
}
