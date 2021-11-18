using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;


[System.Serializable]
public class BattleManagerData
{
    public List<int> playerList = new List<int>();
    public List<BattlerData> battlers = new List<BattlerData>();
}


public class BattleManager : MonoBehaviour
{
    static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BattleManager>();
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        LoadBattlersData();
    }


    public Transform playerBattlersRoot;
    public Transform targetBattlersRoot;
    public Transform battlersRoot;

    public Transform playerScoresRoot;
    public GameObject pkScorePrefab;


    public List<int> playerList = new List<int>();
    public List<int> targetList = new List<int>();
    public Dictionary<int, BattlerData> battlers = new Dictionary<int, BattlerData>();


    Button highlightPKButton;
    Button highlightPlayerPKButton;
    List<Button> selectedTargetBattlersButton;

    // Start is called before the first frame update
    void Start()
    {
    }
    void OnEnable()
    {
        RefreshPlayerBattlerList();
        RefreshAllBattlerList();
    }

    // Update is called once per frame
    void Update()
    {

    }



    // public void OpenModifyBattlerPanel()
    // {
    //     if (highlightPKButton == null)
    //         return;
    //     int index;
    //     if (int.TryParse(highlightPKButton.name, out index) == false)
    //         return;
    //     BattlerData data;
    //     if (battlers.TryGetValue(index, out data) == false)
    //         return;

    //     panel.Init(data, AddBattler);
    // }

    void RefreshPlayerBattlerList()
    {
        Utilities.RefreshPKLayoutGroup(playerBattlersRoot, playerList, OnClickPlayerBattler);
    }
    void RefreshTargetBattlerList()
    {
        Utilities.RefreshPKLayoutGroup(targetBattlersRoot, targetList, OnClickTargetBattler);
        Score();
    }
    void RefreshAllBattlerList()
    {
        List<int> keys = BattleManager.Instance.battlers.Keys.ToList();
        Utilities.RefreshPKLayoutGroup(battlersRoot, keys, OnClickBattler);
    }

    public bool TryGetBattlerByString(string id, out BattlerData battlerData)
    {
        battlerData = null;
        int index;
        if (int.TryParse(id, out index) == false)
            return false;
        if (battlers.TryGetValue(index, out battlerData) == false)
            return false;

        return true;
    }


    class ScoreResult
    {
        public float against = 0;
        public float counter = 0;

    }

    void Score()
    {
        List<ScoreResult> playerScores = new List<ScoreResult>();
        foreach (int id in playerList)
        {
            BattlerData battlerData = battlers[id];
            ScoreResult score = new ScoreResult();

            foreach (int targetID in targetList)
            {
                if (battlerData.againstList.Contains(targetID))
                {
                    score.against += 1;
                }
                if (battlerData.counterList.Contains(targetID))
                {
                    score.counter += 1;
                }
            }
            playerScores.Add(score);
        }

        RefreshScoreLayout(playerScores);
    }

    void RefreshScoreLayout(List<ScoreResult> scores)
    {
        for (int i = 0; i < playerScoresRoot.childCount; i++)
        {
            GameObject.Destroy(playerScoresRoot.GetChild(i).gameObject);
        }
        foreach (ScoreResult score in scores)
        {
            GameObject scoreGO = Instantiate(pkScorePrefab);
            scoreGO.transform.SetParent(playerScoresRoot);

            Text[] texts = scoreGO.GetComponentsInChildren<Text>();

            texts[0].text = score.against.ToString();
            texts[1].text = score.counter.ToString();
        }
    }


    public void ClearTargetBattlers()
    {
        foreach (Transform btnTrans in battlersRoot.transform)
        {
            Button btn = btnTrans.GetComponent<Button>();
            btn.image.color = Color.white;
        }

        targetList.Clear();

        RefreshTargetBattlerList();
    }




    // public void OpenAddBattlerPanel()
    // {
    //     panel.Init(new BattlerData(), AddBattler);
    // }
    // public void AddBattler(BattlerData data)
    // {
    //     battlers[data.pkID] = data;
    //     RefreshBattlerList();
    // }


    // public void RemoveBattler()
    // {
    //     if (highlightPKButton == null)
    //         return;

    //     try
    //     {
    //         BattlerData data;
    //         int id = int.Parse(highlightPKButton.name);
    //         if (battlers.TryGetValue(id, out data))
    //         {
    //             battlers.Remove(id);
    //             RefreshBattlerList();
    //         }
    //     }
    //     catch { }

    // }


    // public void AddToPlayer()
    // {
    //     if (highlightPKButton == null)
    //         return;
    //     BattlerData data;
    //     if (TryGetBattlerByString(highlightPKButton.name, out data))
    //     {
    //         playerList.Add(data.pkID);
    //         Utilities.RefreshPKLayoutGroup(playerBattlersRoot, playerList, OnClickPlayerBattler);
    //         OnClickBattler(highlightPKButton);
    //         SaveBattlersData();
    //     }
    // }

    // public void RemovePlayerBattler()
    // {

    //     if (highlightPlayerPKButton == null)
    //         return;

    //     int id = int.Parse(highlightPlayerPKButton.name);
    //     if (playerList.Contains(id))
    //     {
    //         playerList.Remove(id);
    //         Utilities.RefreshPKLayoutGroup(playerBattlersRoot, playerList, OnClickPlayerBattler);

    //         SaveBattlersData();
    //     }
    // }



    // void RefreshBattlerList()
    // {
    //     for (int i = 0; i < battlersRoot.childCount; i++)
    //     {
    //         Destroy(battlersRoot.GetChild(i).gameObject);
    //     }

    //     foreach (int id in battlers.Keys)
    //     {
    //         BattlerData data = battlers[id];

    //         Sprite sprite = null;
    //         if (Main.Instance.pokeImages.TryGetValue(id, out sprite))
    //         {

    //         }
    //         else
    //         {
    //             Debug.LogWarning("NO Image: " + id);
    //         }

    //         GameObject pkGO = GameObject.Instantiate(pkPrefab);
    //         pkGO.name = id.ToString();
    //         pkGO.transform.SetParent(battlersRoot, false);

    //         // Load Image
    //         Image img = pkGO.transform.Find("Image").GetComponent<Image>();
    //         img.sprite = sprite;

    //         // Load Name
    //         Text txt = pkGO.GetComponentInChildren<Text>();
    //         txt.text = id.ToString();

    //         Button btn = pkGO.GetComponent<Button>();
    //         btn.onClick.AddListener(() =>
    //         {
    //             OnClickBattler(btn);
    //         });
    //     }

    //     SaveBattlersData();
    // }




    public void SaveBattlersData()
    {
        BattleManagerData saveData = new BattleManagerData();
        saveData.playerList = playerList;
        foreach (BattlerData data in battlers.Values)
        {
            saveData.battlers.Add(data);
        }

        string saveString = JsonUtility.ToJson(saveData);
        Debug.Log("Save: " + saveString);

        string folderPath = Application.persistentDataPath;
        string path = Path.Combine(folderPath, "battlers.json");

        File.WriteAllText(path, saveString);

        Debug.Log("Save to Path: " + path);
    }


    void LoadBattlersData()
    {
        string folderPath = Application.persistentDataPath;
        string path = Path.Combine(folderPath, "battlers.json");

        if (File.Exists(path) == false)
        {
            return;
        }

        string saveString = System.IO.File.ReadAllText(path);

        BattleManagerData saveData = JsonUtility.FromJson<BattleManagerData>(saveString);

        playerList = saveData.playerList;

        foreach (BattlerData data in saveData.battlers)
        {
            battlers.Add(data.pkID, data);
        }

        RefreshAllBattlerList();
    }






    void OnClickPlayerBattler(Button btn)
    {
        if (highlightPlayerPKButton == null)
        {
            btn.image.color = Color.cyan;
            highlightPlayerPKButton = btn;
        }
        else if (highlightPlayerPKButton == btn)
        {
            btn.image.color = Color.white;
            highlightPlayerPKButton = null;
        }
        else
        {
            highlightPlayerPKButton.image.color = Color.white;
            btn.image.color = Color.cyan;
            highlightPlayerPKButton = btn;
        }
    }
    void OnClickTargetBattler(Button btn)
    {

    }
    void OnClickBattler(Button btn)
    {
        int index;
        if (int.TryParse(btn.name, out index) == false)
            return;

        if (targetList.Contains(index) == false)
        {
            targetList.Add(index);
            btn.image.color = Color.cyan;
            RefreshTargetBattlerList();
        }
        else
        {
            targetList.Remove(index);
            btn.image.color = Color.white;
            RefreshTargetBattlerList();
        }

        return;
    }






}
