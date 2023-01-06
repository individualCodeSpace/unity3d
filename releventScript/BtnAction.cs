/******************************************************************************************************************************************************
 author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnAction : MonoBehaviour
{
    public Button[] Buttons;
    public GameObject[] ChildPanel;
    public void BtnClick(int index)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
        Buttons[index].interactable = false;
    }
    public void BtnSwitchPanel(int index)
    {
        for (int i = 0; i < ChildPanel.Length; i++)
        {
            ChildPanel[i].SetActive(false);
        }
        ChildPanel[index].SetActive(true);
    }
}
