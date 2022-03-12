using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可序列化与反序列化标识
[System.Serializable]
public class Save_item
{
    public List<long> ID = new List<long>();
    public List<string> TeamName = new List<string>();
    public List<string> Subject = new List<string>();
    public List<string> Round = new List<string>();
    public List<string> Task = new List<string>();
    public List<int> Grade = new List<int>();
}
public class Save_BeforeData
{
    public List<long> ID = new List<long>();
    public List<string> SubjectMess = new List<string>();
    public List<string> TeamName = new List<string>();
    public List<string> EquipmentMess = new List<string>();
    public List<string> GradeDepend = new List<string>();
}
public class Save_GradeData
{
    public List<long> ID = new List<long>();
    public List<string> TeamName = new List<string>();
    public List<string> SubjectGrade = new List<string>();
    public List<int> Composite = new List<int>();
}