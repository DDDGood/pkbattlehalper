using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PKMonInfo
{
    public int id;
    public string name;
    public string galar_dex;
    public int[] base_stats;
    public string[] abilities;


    public PKType type1;
    public PKType type2;

    public List<int> againstList;
    public List<int> counterList;
}
