using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;

    void Start()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Controles");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
