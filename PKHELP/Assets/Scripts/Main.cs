using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{
    static Main _instance;
    public static Main Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Main>();
            return _instance;
        }
    }




    public TextAsset pokedexText;
    public TextAsset localizationText;
    public TextAsset typeText;

    public GameObject pkPrefab;


    public Dictionary<int, LocalizationData> localization = new Dictionary<int, LocalizationData>();
    public Dictionary<int, PKData> pokedex = new Dictionary<int, PKData>();
    public Dictionary<int, Sprite> pokeImages = new Dictionary<int, Sprite>();
    public Dictionary<string, PKType> types = new Dictionary<string, PKType>();



    public Action onDataLoaded;




    // Start is called before the first frame update
    void Start()
    {
        LoadLocalization();
        LoadImages();
        LoadPKData();
        LoadTypeData();

        if (onDataLoaded != null)
            onDataLoaded();
    }

    // Update is called once per frame
    void Update()
    {

    }








    void LoadLocalization()
    {
        if (localizationText == null)
            return;

        LocalizationDataList localizationDataList = JsonUtility.FromJson<LocalizationDataList>(localizationText.text);
        foreach (LocalizationData data in localizationDataList.list)
        {
            localization.Add(data.id, data);
        }
    }


    void LoadPKData()
    {
        if (pokedexText == null)
            return;

        PKDataList pkDataList = JsonUtility.FromJson<PKDataList>(pokedexText.text);
        foreach (PKData data in pkDataList.list)
        {
            pokedex.Add(data.id, data);
            Debug.Log("PKDexAdd: " + data.id + " " + data.name);
        }
    }

    void LoadImages()
    {
        string path = Path.Combine("Images", "pokedex");

        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        foreach (Sprite sprite in sprites)
        {
            pokeImages.Add(int.Parse(sprite.name), sprite);
        }

    }

    void LoadTypeData()
    {
        if (typeText == null)
            return;

        PKTypeList typeList = JsonUtility.FromJson<PKTypeList>(typeText.text);
        foreach (PKType type in typeList.list)
        {
            types.Add(type.name, type);

        }


    }




}
