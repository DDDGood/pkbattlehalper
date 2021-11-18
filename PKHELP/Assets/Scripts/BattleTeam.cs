using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleTeam : MonoBehaviour
{
    public Transform playerBattlersRoot;
    public Button AddPlayerBattlerButton;
    public Button RemovePlayerBattlerButton;


    public Transform battlersRoot;
    public Button battlerModifyButton;



    public BattlerPanel battlerPanel;


    Button highlightPKButton;
    Button highlightPlayerPKButton;




    // Start is called before the first frame update
    void Start()
    {
        RefreshPlayerBattlerList();
        RefreshAllBattlerList();
    }

    void RefreshPlayerBattlerList()
    {
        Utilities.RefreshPKLayoutGroup(playerBattlersRoot, BattleManager.Instance.playerList, OnClickPlayerBattler);
    }
    void RefreshAllBattlerList()
    {
        List<int> keys = BattleManager.Instance.battlers.Keys.ToList();
        Utilities.RefreshPKLayoutGroup(battlersRoot, keys, OnClickBattler);
    }



    void OnClickPlayerBattler(Button btn)
    {
        if (highlightPlayerPKButton == null)
        {
            btn.image.color = Color.cyan;
            highlightPlayerPKButton = btn;
            RemovePlayerBattlerButton.gameObject.SetActive(true);
        }
        else if (highlightPlayerPKButton == btn)
        {
            btn.image.color = Color.white;
            highlightPlayerPKButton = null;
            RemovePlayerBattlerButton.gameObject.SetActive(false);
        }
        else
        {
            highlightPlayerPKButton.image.color = Color.white;
            btn.image.color = Color.cyan;
            highlightPlayerPKButton = btn;
            RemovePlayerBattlerButton.gameObject.SetActive(true);
        }
    }

    public void AddToPlayer()
    {
        if (highlightPKButton == null)
            return;
        BattlerData data;
        if (BattleManager.Instance.TryGetBattlerByString(highlightPKButton.name, out data))
        {
            BattleManager.Instance.playerList.Add(data.pkID);
            Utilities.RefreshPKLayoutGroup(playerBattlersRoot, BattleManager.Instance.playerList, OnClickPlayerBattler);
            OnClickBattler(highlightPKButton);
            BattleManager.Instance.SaveBattlersData();
        }
    }

    void OnClickBattler(Button btn)
    {
        if (highlightPKButton == null)
        {
            btn.image.color = Color.cyan;
            highlightPKButton = btn;
            battlerModifyButton.gameObject.SetActive(true);
            AddPlayerBattlerButton.gameObject.SetActive(true);
        }
        else if (highlightPKButton == btn)
        {
            btn.image.color = Color.white;
            highlightPKButton = null;
            battlerModifyButton.gameObject.SetActive(false);
            AddPlayerBattlerButton.gameObject.SetActive(false);
        }
        else
        {
            highlightPKButton.image.color = Color.white;
            btn.image.color = Color.cyan;
            highlightPKButton = btn;
            battlerModifyButton.gameObject.SetActive(true);
            AddPlayerBattlerButton.gameObject.SetActive(true);
        }
    }


    public void RemovePlayerBattler()
    {

        if (highlightPlayerPKButton == null)
            return;

        int id = int.Parse(highlightPlayerPKButton.name);
        if (BattleManager.Instance.playerList.Contains(id))
        {
            BattleManager.Instance.playerList.Remove(id);
            RefreshPlayerBattlerList();

            BattleManager.Instance.SaveBattlersData();
        }

        RefreshPlayerBattlerList();
    }



    public void OpenModifyBattlerPanel()
    {
        if (highlightPKButton == null)
            return;
        int index;
        if (int.TryParse(highlightPKButton.name, out index) == false)
            return;
        BattlerData data;
        if (BattleManager.Instance.battlers.TryGetValue(index, out data) == false)
            return;

        battlerPanel.Init(data, AddBattler);
    }
    public void OpenAddBattlerPanel()
    {
        battlerPanel.Init(new BattlerData(), AddBattler);
    }
    public void AddBattler(BattlerData data)
    {
        BattleManager.Instance.battlers[data.pkID] = data;
        RefreshAllBattlerList();
        BattleManager.Instance.SaveBattlersData();
    }
    public void RemoveBattler()
    {
        if (highlightPKButton == null)
            return;


        BattlerData data;
        int id = int.Parse(highlightPKButton.name);
        if (BattleManager.Instance.battlers.TryGetValue(id, out data))
        {
            BattleManager.Instance.battlers.Remove(id);
            RefreshAllBattlerList();

            //TODO: update counter/againster list pk
        }

        BattleManager.Instance.SaveBattlersData();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
