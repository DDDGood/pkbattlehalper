using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PKDex : MonoBehaviour
{
    [SerializeField]
    GameObject pkPrefab;

    [SerializeField]
    Transform pkRoot;


    void Awake()
    {
        Main.Instance.onDataLoaded += OnDataLoaded;

    }

    void OnDataLoaded()
    {
        foreach (int id in Main.Instance.pokedex.Keys)
        {
            PKData data = Main.Instance.pokedex[id];

            Sprite sprite = null;
            if (Main.Instance.pokeImages.TryGetValue(id, out sprite))
            {

            }
            else
            {
                Debug.LogWarning("NO Image: " + id);
            }

            GameObject pkGO = GameObject.Instantiate(pkPrefab);
            pkGO.transform.SetParent(pkRoot, false);

            // Load Image
            Image img = pkGO.transform.Find("Image").GetComponent<Image>();
            img.sprite = sprite;

            // Load Name
            Text txt = pkGO.GetComponentInChildren<Text>();
            LocalizationData localData;
            if (Main.Instance.localization.TryGetValue(id, out localData))
            {
                txt.text = localData.cht;
            }
            else
            {
                txt.text = data.name;
            }
        }


        RectTransform rectTransform = pkRoot.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, Main.Instance.pokedex.Count * 180 / 6);

        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
