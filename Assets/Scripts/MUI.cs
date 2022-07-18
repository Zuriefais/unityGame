using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MUI : MonoBehaviour
{
    SaveDate.PlayerData sv;

    public void Load(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void Start()
    {
        sv = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");
    }
}
