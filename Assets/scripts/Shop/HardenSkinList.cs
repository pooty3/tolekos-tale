﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This encompass the harden skin skill tree
public class HardenSkinList : ShopItemList
{
    public override ShopItem ChooseItem()
    {
        if (PlayerStats.hardenSkinLevel < shopitemlist.Count) {
            return shopitemlist[PlayerStats.hardenSkinLevel];
            
        } else {
            return shopitemlist[shopitemlist.Count - 1];
             }
    }
}
