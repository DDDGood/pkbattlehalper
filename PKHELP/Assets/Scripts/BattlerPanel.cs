using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BattlerPanel : MonoBehaviour
{

    Action<BattlerData> onSave;
    BattlerData data;

    public InputField nameField;
    public InputField indexField;
    public Image image;


    public InputField addAgainstField;
    public Transform againstRoot;
    public Button removeAgainstButton;
    Button highlightAgainstButton;

    public InputField addCounterField;
    public Transform counterRoot;
    public Button removeCounterButton;
    Button highlightCounterButton;

    public void Init(BattlerData data = null, Action<BattlerData> onSave = null)
    {
        if (data == null)
            data = new BattlerData();
        this.data = data;

        if (onSave != null)
            this.onSave += onSave;

        nameField.text = data.name;
        nameField.onEndEdit.AddListener((string name) => { data.name = name; });

        indexField.text = data.pkID.ToString();
        indexField.onEndEdit.AddListener((string index) => { OnIndexUpdate(index); });

        Sprite sprite = null;
        Main.Instance.pokeImages.TryGetValue(data.pkID, out sprite);
        image.sprite = sprite;


        Utilities.RefreshPKLayoutGroup(againstRoot, data.againstList, OnClickAgainstButton);
        Utilities.RefreshPKLayoutGroup(counterRoot, data.counterList, OnClickCounterButton);

        gameObject.SetActive(true);
    }

    void OnIndexUpdate(string index)
    {
        int id;
        try
        {
            id = int.Parse(index);
            if (Main.Instance.pokedex.ContainsKey(id))
            {
                data.pkID = id;
            }
            Sprite sprite;
            if (Main.Instance.pokeImages.TryGetValue(id, out sprite))
            {
                image.sprite = sprite;
            }
        }
        catch
        {
            Debug.LogError("Failed");
        }
    }






    public void AddAgainst()
    {
        if (String.IsNullOrEmpty(addAgainstField.text))
            return;
        int id;
        if (int.TryParse(addAgainstField.text, out id))
        {
            BattlerData targetBattler;
            if (BattleManager.Instance.battlers.TryGetValue(id, out targetBattler) == false)
                return;

            data.againstList.Add(id);

            if (targetBattler.counterList.Contains(data.pkID) == false)
                targetBattler.counterList.Add(data.pkID);
        }

        Utilities.RefreshPKLayoutGroup(againstRoot, data.againstList, OnClickAgainstButton);
    }
    public void RemoveAgainst()
    {
        if (highlightAgainstButton == null)
            return;

        int id;
        if (int.TryParse(highlightAgainstButton.name, out id))
        {
            if (data.againstList.Contains(id))
            {
                BattlerData targetBattler;
                if (BattleManager.Instance.battlers.TryGetValue(id, out targetBattler) == false)
                    return;

                data.againstList.Remove(id);

                if (targetBattler.counterList.Contains(data.pkID) == true)
                    targetBattler.counterList.Remove(data.pkID);
            }
        }
        Utilities.RefreshPKLayoutGroup(againstRoot, data.againstList, OnClickAgainstButton);
    }

    void OnClickAgainstButton(Button btn)
    {
        if (highlightAgainstButton == null)
        {
            btn.image.color = Color.cyan;
            highlightAgainstButton = btn;
            removeAgainstButton.gameObject.SetActive(true);
        }
        else if (highlightAgainstButton == btn)
        {
            btn.image.color = Color.white;
            highlightAgainstButton = null;
            removeAgainstButton.gameObject.SetActive(false);
        }
        else
        {
            highlightAgainstButton.image.color = Color.white;
            btn.image.color = Color.cyan;
            highlightAgainstButton = btn;
            removeAgainstButton.gameObject.SetActive(true);
        }
    }



    public void AddCounter()
    {
        if (String.IsNullOrEmpty(addCounterField.text))
            return;
        int id;
        if (int.TryParse(addCounterField.text, out id))
        {
            BattlerData targetBattler;
            if (BattleManager.Instance.battlers.TryGetValue(id, out targetBattler) == false)
                return;
            data.counterList.Add(id);

            if (targetBattler.againstList.Contains(data.pkID) == false)
                targetBattler.againstList.Add(data.pkID);
        }

        Utilities.RefreshPKLayoutGroup(counterRoot, data.counterList, OnClickCounterButton);
    }
    public void RemoveCounter()
    {
        if (highlightCounterButton == null)
            return;

        int id;
        if (int.TryParse(highlightCounterButton.name, out id))
        {
            if (data.counterList.Contains(id))
            {
                BattlerData targetBattler;
                if (BattleManager.Instance.battlers.TryGetValue(id, out targetBattler) == false)
                    return;
                data.counterList.Remove(id);


                if (targetBattler.againstList.Contains(data.pkID) == true)
                    targetBattler.againstList.Remove(data.pkID);

            }
        }

        Utilities.RefreshPKLayoutGroup(counterRoot, data.counterList, OnClickCounterButton);
    }

    void OnClickCounterButton(Button btn)
    {
        if (highlightCounterButton == null)
        {
            btn.image.color = Color.cyan;
            highlightCounterButton = btn;
            removeCounterButton.gameObject.SetActive(true);
        }
        else if (highlightCounterButton == btn)
        {
            btn.image.color = Color.white;
            highlightCounterButton = null;
            removeCounterButton.gameObject.SetActive(false);
        }
        else
        {
            highlightCounterButton.image.color = Color.white;
            btn.image.color = Color.cyan;
            highlightCounterButton = btn;
            removeCounterButton.gameObject.SetActive(true);
        }
    }


    public void Save()
    {
        Debug.Log(data);
        Debug.Log(data.name);
        if (String.IsNullOrEmpty(data.name))
            return;

        if (data.pkID == -1)
            return;

        if (onSave != null)
            onSave(data);

        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        data = null;
        gameObject.SetActive(false);
    }





}
