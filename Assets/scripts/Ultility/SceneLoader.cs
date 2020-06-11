﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void Awake()
    {
        if (GameManager.sceneLoader == null)
        {
            GameManager.sceneLoader = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else {
            Destroy(this.gameObject);
        }
        
    }
    private void Start()
    {
        GameManager.OnGameover += OnGameOverEvent;
    }

    private void OnDestroy()
    {
        GameManager.OnGameover -= OnGameOverEvent;
    }
    public void OnGameOverEvent(bool victory) {

        GameManager.victory = victory;
        EndScene();
    }
    public void QuitGame()
    {
        Application.Quit();

    }

    public void LoadScene(string str) {
        SceneManager.LoadScene(str);
    }

    public void LoadShopScene(LevelDescription levelDescription) {
        GameManager.levelDescription = levelDescription;
        SceneManager.LoadScene("Shop Scene");
    
    }

    public void ReturnToStartPage() {
        LoadScene("StartPage");
    }
    public void EndScene()
    {
        StartCoroutine(WaitAndLoad("EndScene"));

    }

  
    IEnumerator WaitAndLoad(string str) {
        yield return new WaitForSeconds(2f);
        LoadScene(str);
    }

   
  

}