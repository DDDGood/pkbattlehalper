using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LocalizationData
{
    public int id;
    public string eng;
    public string cht;
}


[Serializable]
public class LocalizationDataList
{
    public List<LocalizationData> list = new List<LocalizationData>();
}


