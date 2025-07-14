using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void MainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(0);
    }
}
