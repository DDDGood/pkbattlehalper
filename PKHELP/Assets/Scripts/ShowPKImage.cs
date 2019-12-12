using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPKImage : MonoBehaviour
{
    Image image;
    InputField inputField;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        inputField = GetComponentInChildren<InputField>();


        inputField.onEndEdit.AddListener((string text) => { ShowImage(text); });
    }



    public void ShowImage(string text)
    {
        int index = int.Parse(text);

        Debug.Log("ShowImage: " + index);
        Sprite s;
        if (Main.Instance.pokeImages.TryGetValue(index, out s))
        {
            image.sprite = s;
        }
        else
        {
            Debug.LogWarning("No Image");
        }

    }
}
