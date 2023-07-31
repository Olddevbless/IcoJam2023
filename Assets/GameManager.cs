using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    
    [SerializeField]GameObject pauseMenu;
    [SerializeField]GameObject endMenu;
    private static GameManager _instance = null;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();  
        Time.timeScale = 1.0f;
        endMenu.SetActive(true);
        pauseMenu.SetActive(true);
        pauseMenu.GetComponent<Canvas>().enabled = false;
        endMenu.GetComponent<Canvas>().enabled = false;
    }
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }
    public void PauseGame()
    {
        pauseMenu.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
       
    }
    public void Continue()
    {
        pauseMenu.GetComponent<Canvas>().enabled =false;
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void EndGame()
    {
        endMenu.GetComponent<Canvas>().enabled = true ;
        Time.timeScale = 0.0f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            EndGame();
        }
    }
}
