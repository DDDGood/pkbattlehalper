using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class PKData
{
    public int id;
    public string name;
    public string galar_dex;
    public int[] base_stats;
    public string[] abilities;
    public string[] types;
}

[Serializable]
public class PKDataList
{
    public List<PKData> list = new List<PKData>();
}
