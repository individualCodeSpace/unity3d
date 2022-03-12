using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDate : MonoBehaviour
{

    public GameObject[] Panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CloseAllPanel()
    {
        for (int i = 0; i <= 2; i++)
        {
            Panel[i].SetActive(false);
        }
    }
    public void BtnBefore()
    {
        CloseAllPanel();
        Panel[0].SetActive(true);
    }
    public void BtnPlaying()
    {
        CloseAllPanel();
        Panel[1].SetActive(true);
    }
    public void BtnEnd()
    {
        CloseAllPanel();
        Panel[2].SetActive(true);
    }
}
