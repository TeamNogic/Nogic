using UnityEngine;
using System.Collections;

using System.Collections.Generic;

[System.Serializable]
public class Sys_MoveData
{
    public Vector3 start;
    public Vector3 end;
    public float speed;
}

[System.Serializable]
public class Sys_Camera_Move : MonoBehaviour
{
    public Vector3 lookAt;
    public float waitTime;
    public List<Sys_MoveData> moveList;     //start から end までの移動（プレイヤーの座標から差分で移動する）

    private Sys_MoveData move;
    private List<int> moveLog = new List<int>();

    private Vector3 normal;
    private float oldDistance;
    
    private float nowWait;

    void Start()
    {
        moveLog.Add(Random.Range(0, moveList.Count));
        move = moveList[moveLog[0]];

        Camera.main.gameObject.transform.LookAt(lookAt);

        Camera.main.gameObject.transform.position = move.start + lookAt;
        normal = ((move.end + lookAt) - (move.start + lookAt)).normalized;
        oldDistance = Vector3.Distance(move.end + lookAt, Camera.main.gameObject.transform.position);

        nowWait = 0.0f;
    }

    void Update()
    {
        if (nowWait <= 0.0f)
        {
            Camera.main.gameObject.transform.position += normal * Time.deltaTime * move.speed;
            Camera.main.gameObject.transform.LookAt(lookAt);
        }

        if (oldDistance - Vector3.Distance(move.end + lookAt, Camera.main.gameObject.transform.position) <= 0.0f)
        {
            nowWait += Time.deltaTime;

            if (waitTime <= nowWait)
            {
                bool flag = true;
                int selectMove = 0;

                while (flag)
                {
                    flag = false;
                    selectMove = Random.Range(0, moveList.Count);

                    for (int i = 0; i < moveLog.Count; ++i)
                    {
                        if (moveLog[i] == selectMove) flag = true;
                    }
                }

                moveLog.Add(selectMove);
                move = moveList[selectMove];

                if (moveList.Count <= moveLog.Count)
                {
                    moveLog.Clear();
                    moveLog.Add(selectMove);
                }

                Camera.main.gameObject.transform.position = move.start + lookAt;
                Camera.main.gameObject.transform.LookAt(lookAt);
                normal = ((move.end + lookAt) - (move.start + lookAt)).normalized;

                nowWait = 0.0f;
            }
        }

        oldDistance = Vector3.Distance(move.end + lookAt, Camera.main.gameObject.transform.position);
    }
}
