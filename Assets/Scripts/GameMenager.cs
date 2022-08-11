using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenager : MonoBehaviour
{
    private SaveDate.Settings settings;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public void Start()
    {
        settings = SaveMenager.Load<SaveDate.Settings>("Settings.json");
        if (settings == null)
        {
            settings = new();
            settings.isFulscreen = Screen.fullScreen;
        }
        SetFulScreen(settings.isFulscreen);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void SetFulScreen(bool state)
    {
        Screen.fullScreen = state;
        settings.isFulscreen = state;
        SaveMenager.Save(settings, "Settings.json");
    }
    
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
