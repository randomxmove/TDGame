using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image icon;

    
    public BuildButton(string name, int price, Sprite image)
    {
        titleText.text = name;
        priceText.text = "$" + price.ToString();
        icon.sprite = image;
    }

    public void SetTitle(string title) => titleText.text = title;
    public void SetPrice(int price) => priceText.text = "$" + price.ToString();
    public void SetIcon(Sprite sprite) => icon.sprite = sprite;

   

}
