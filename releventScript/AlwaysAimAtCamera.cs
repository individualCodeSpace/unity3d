/******************************************************************************************************************************************************
 
 功能描述：让地图内组件跟随摄像机旋转，以保持物体渲染面正对相机；

 原作者：赖宝林
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysAimAtCamera : MonoBehaviour
{
    //public Transform TrdObj;
    //public RectTransform UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //var cam = Camera.main;
        //var campos = cam.WorldToScreenPoint(TrdObj.position);
        //UI.anchoredPosition = campos;
        if (!Camera.main)
        {
            return;
        }
        else
        {
            //获取当前摄像机三维坐标；
            var cam = Camera.main;
            //将挂了该脚本的对象坐标转换成屏幕对应坐标
            var campos = cam.WorldToScreenPoint(transform.position);
            //将该对象z轴对应相机视锥体深度
            campos.z = cam.nearClipPlane;
            //再将屏幕坐标映射回世界坐标中，这样就能实现对象一直正对相机
            var worldpos = cam.ScreenToWorldPoint(campos);
            //对当前对象进行位置设置；
            transform.LookAt(worldpos);
            transform.Rotate(0, 180, 0);
        }

    }
}


