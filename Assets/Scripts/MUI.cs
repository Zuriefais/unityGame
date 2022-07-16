using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MUI : MonoBehaviour
{
    public void Load(int level)
    {
        SceneManager.LoadScene(level);
    }
}
