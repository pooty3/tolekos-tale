﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//This is to control a certain shop button, each is associated with its own tree
public class ShopButton : MonoBehaviour, ISelectHandler
{

    [SerializeField] Image itemIcon;
    [SerializeField] ItemDescription itemDescription;
    [SerializeField] TextMeshProUGUI costIcon, itemNameIcon;
    [SerializeField] Color available, unavailable, noGold;
    [SerializeField] SFX boughtSFX, failbuySFX;
    public ShopItemList list;
    public ShopItem item;
    public event Action OnBoughtItem;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyItem);
    }

    public void SetUp(ShopItemList list, ItemDescription itemDescription) {
        this.list = list;
        this.itemDescription = itemDescription;
        Refresh();

    }
    public void Refresh() {
        item = list.ChooseItem();
        itemIcon.sprite = item.icon;
        costIcon.text = "Price: " + item.cost.ToString();
        itemNameIcon.text = item.itemName;
        SetColor();
    }
    public void SetColor() {
        if (item.Buyable())
        {
            costIcon.color = available;

        }
        else {
            if (item.cost > PlayerStats.gold)
            {
                costIcon.color = noGold;
            }
            else {
                costIcon.color = unavailable;
            }

            
        
        }
    }



     public void OnSelect(BaseEventData eventData)
    {
        itemDescription.SetDescription(item);
    }
    public void BuyItem() {
        if (item.Buyable())
        {
            AudioManager.current.PlaySFX(boughtSFX);
            item.Buy();
            Refresh();
            itemDescription.SetDescription(item);

            OnBoughtItem?.Invoke();
        }
        else {
            AudioManager.current.PlaySFX(failbuySFX);
        }
    
    }
}
