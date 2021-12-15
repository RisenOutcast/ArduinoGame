using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RO
{
    public class MainMenu : MonoBehaviour
    {

        public void StartGame()
        {
            SceneManager.LoadScene("Game_Test");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}