using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject loseScreen;

    private bool openMenu;

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    private void FixedUpdate()
    {
        if(gameController.GetState() == GameController.GameState.PlayerLose)
        {
            loseScreen.SetActive(true);
        }
    }
    
    public void ResetGame()
    {
        Scene curScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(curScene.buildIndex);
    }

    public void ToggleMenu()
    {
        if (openMenu)
        {
            menuPanel.SetActive(false);
            openMenu = false;
        }
        else if (!openMenu)
        {
            menuPanel.SetActive(true);
            openMenu = true;
        }
    }
}
