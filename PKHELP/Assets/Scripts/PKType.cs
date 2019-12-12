using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PKType
{
    public string name;
    public List<string> immunes = new List<string>();
    public List<string> weaknesses = new List<string>();
    public List<string> strengths = new List<string>();

}

public class PKTypeList
{
    public List<PKType> list = new List<PKType>();
}

public class TypeEffectiveness
{
    public float normal = 0;
    public float fire = 0;
    public float water = 0;

}
