using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BattlerData
{

    public string name = "";
    public int pkID = -1;


    public List<int> againstList = new List<int>();
    public List<int> counterList = new List<int>();

}
