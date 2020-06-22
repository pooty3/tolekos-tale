﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Level Description")]
public class LevelDescription : ScriptableObject
{
    public string levelDescription;
    public string nextLevelSceneString;
    public Sprite backgroundImage;
}