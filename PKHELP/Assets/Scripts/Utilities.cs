using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class Utilities
{

    public static void RefreshPKLayoutGroup(Transform root, List<int> keys, Action<Button> onClickAction)
    {

        for (int i = 0; i < root.childCount; i++)
        {
            GameObject.Destroy(root.GetChild(i).gameObject);
        }

        foreach (int id in keys)
        {
            Sprite sprite = null;
            if (Main.Instance.pokeImages.TryGetValue(id, out sprite))
            {

            }
            else
            {
                Debug.LogWarning("NO Image: " + id);
            }

            GameObject pkGO = GameObject.Instantiate(Main.Instance.pkPrefab);
            pkGO.name = id.ToString();
            pkGO.transform.SetParent(root, false);

            // Load Image
            Image img = pkGO.transform.Find("Image").GetComponent<Image>();
            img.sprite = sprite;

            // Load Name
            Text txt = pkGO.GetComponentInChildren<Text>();
            txt.text = id.ToString();

            Button btn = pkGO.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                onClickAction(btn);
            });
        }
    }

    public static Dictionary<string, float> GetTypeCounters(string typeName)
    {
        Dictionary<string, float> result = new Dictionary<string, float>();

        foreach (string anyType in Main.Instance.types.Keys)
        {
            result.Add(anyType, 1f);
        }

        PKType type;
        if (Main.Instance.types.TryGetValue(typeName, out type) == false)
        {
            return result;
        }

        foreach (string counterTypeName in type.weaknesses)
        {
            result[counterTypeName] *= 2f;
        }
        foreach (string againstTypeName in type.strengths)
        {
            result[againstTypeName] *= 0.5f;
        }

        return result;
    }



}
