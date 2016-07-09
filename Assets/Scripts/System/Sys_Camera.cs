using UnityEngine;
using System.Collections;

public class Sys_Camera : MonoBehaviour
{
    private Vector3 normal;
    private Vector3 target;
    private float oldDistance;

    private float length;
    private float positionY;

    private Vector3 lookAt;
    private float speed;

    private float time = 0;
    private int move = 0;

    public static Vector3 getPosition()
    {
        return Camera.main.transform.position;
    }

    public static void Setup(Vector3 _start, Vector3 _target, Vector3 _lookAt, float _speed)
    {
        Camera.main.GetComponent<Sys_Camera>().target = _target;
        Camera.main.GetComponent<Sys_Camera>().speed = _speed;
        Camera.main.GetComponent<Sys_Camera>().lookAt = _lookAt;
        
        Camera.main.GetComponent<Sys_Camera>().time = 0.0f;
        Camera.main.GetComponent<Sys_Camera>().move = 1;

        Camera.main.GetComponent<Sys_Camera>().normal = (_target - _start).normalized;
        Camera.main.GetComponent<Sys_Camera>().oldDistance = Vector3.Distance(_target, _start);

        Camera.main.GetComponent<Sys_Camera>().transform.position = _start;

        Camera.main.GetComponent<Sys_Camera>().Update();
    }

    public static void Setup_Rotate(Vector3 _center, float _length, float _positionY, float _speed)
    {
        Camera.main.GetComponent<Sys_Camera>().lookAt = _center;
        Camera.main.GetComponent<Sys_Camera>().length = _length;
        Camera.main.GetComponent<Sys_Camera>().positionY = _positionY;
        Camera.main.GetComponent<Sys_Camera>().speed = _speed;

        Camera.main.GetComponent<Sys_Camera>().time = 0.0f;
        Camera.main.GetComponent<Sys_Camera>().move = 2;

        Camera.main.GetComponent<Sys_Camera>().Update();
    }

    public static bool isMove()
    {
        return Camera.main.GetComponent<Sys_Camera>().move != 0;
    }

    public static void StopMove()
    {
        Camera.main.GetComponent<Sys_Camera>().move = 0;
    }

    public static float moveTime()
    {
        return Camera.main.GetComponent<Sys_Camera>().time;
    }

    void Update()
    {
        time += Time.deltaTime;

        switch (move)
        {
            case 1:
                this.transform.position += normal * Time.deltaTime * speed;
                
                if (oldDistance - Vector3.Distance(target, this.transform.position) <= 0.0f) move = 0;

                oldDistance = Vector3.Distance(target, this.transform.position);

                this.transform.LookAt(lookAt);
                break;

            case 2:
                this.transform.position =
                    new Vector3
                    ( lookAt.x + Mathf.Cos(time * speed) * length
                    , positionY
                    , lookAt.z + Mathf.Sin(time * speed) * length);

                this.transform.LookAt(lookAt);
                break;
        }
    }
}
