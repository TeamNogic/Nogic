using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ImageChange : MonoBehaviour
{
    public List<Sprite> image = new List<Sprite>();
    public int select = 0;
    
    void Start()
    {
        this.GetComponent<Image>().sprite = image[select];

        Destroy(this.GetComponent<UI_ImageChange>());
    }
}
