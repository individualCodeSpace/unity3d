using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static Options _instance;
    //public Dropdown Dropd;
    private int i = -1;
    private bool m_bEnable = false;
    // Start is called before the first frame update
  
  
    private void Awake()
    {
      
        
    }
    void Start()
    {

        //初始化下拉选项
        string[] options = { "测试验证队伍1", "测试验证队伍2", "测试验证队伍3", "测试验证队伍4", "测试验证队伍5", "测试验证队伍6", "测试验证队伍7" };
        for (int i = 0; i < options.Length; i++)
        {
            gameObject.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(options[i], null));
        }
        gameObject.GetComponent<Dropdown>().value = i;
    }
    private void Update()
    {
        if (i != gameObject.GetComponent<Dropdown>().value)
        {
            m_bEnable = true;
            OnValueChanged();
        }
       
    }
    //选中队伍名称触发赛况匹配显示
    public void OnValueChanged()
    {
        i = gameObject.GetComponent<Dropdown>().value ;
        while (m_bEnable)
        {
          gameObject.GetComponent<ItemOptions>().GetItem();
            m_bEnable = !m_bEnable;
        }

    }
}
