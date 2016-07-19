using UnityEngine;
using System.Collections;

public class Sys_ImageSort : MonoBehaviour
{
    [SerializeField, Tooltip("描画順番。大きいほど手前に")]
    private int index = 0;

    void Update()
    {
        transform.SetSiblingIndex(index);
    }
}