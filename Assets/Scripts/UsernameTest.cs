using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UsernameTest : MonoBehaviour
{
    public GameObject userNameTest;
    public GameObject mainMenu;
    public TMP_InputField inputField;

    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
        if (SaveMenager.Load<SaveDate.PlayerData>("playerSave.json") == null)
        {
            Debug.Log("test");
            userNameTest.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void SaveName()
    {
        SaveDate.PlayerData playerData = new();
        playerData.playerName = inputField.text;
        SaveMenager.Save(playerData, "playerSave.json");
        userNameTest.SetActive(false);
        mainMenu.SetActive(true);
    }
}
