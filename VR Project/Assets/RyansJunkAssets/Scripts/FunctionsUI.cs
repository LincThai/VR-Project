using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionsUI : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Asset Scene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
