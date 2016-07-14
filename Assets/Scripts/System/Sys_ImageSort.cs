using UnityEngine;
using System.Collections;

public class Sys_ImageSort : MonoBehaviour
{
    [SerializeField, Tooltip("描画順番。大きいほど手前に")]
    private int index = 0;

    void Start()
    {
        transform.SetSiblingIndex(index);
    }
}

/*
効果文字		0
Node_palette	1
Node_Select		2
Node_Core		3
Node			4
Line			4
*/
