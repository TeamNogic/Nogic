using UnityEngine;
using System.Collections;

public class Sys_LookAt : MonoBehaviour
{
    void Update ()
    {
        this.transform.LookAt(Camera.main.transform.position);
	}
}