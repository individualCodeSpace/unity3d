using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxtCurrentTime : MonoBehaviour
{
    //private static string text;

    // Start is called before the first frame update
    public Text CurrentTime;

   // public static object text { get; private set; }

    private void Update()
    {
        DateTime NowTime = DateTime.Now.ToLocalTime();
        CurrentTime.text = NowTime.ToString("yyy/MM/dd HH:mm:ss");
    }
}
