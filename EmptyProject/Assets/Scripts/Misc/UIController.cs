using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject starScreen;
    public GameObject creditsMenu;
    public GameObject GameOverMenu;

    public Text timePlayedTxt;
    public Text powerupsCollectedTxt;
    public Text enemiesKilledTxt;

    public Image loadBar;
    public Animator loadScreenAnim;

    bool isCreditsShowing = false;

    public void StartGame()
    {
        starScreen.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void ShowCredits()
    {
        isCreditsShowing = !isCreditsShowing;

        creditsMenu.SetActive(isCreditsShowing);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowGameOverScreen(GameSessionData sessionData)
    {
        timePlayedTxt.text = "TIME PLAYED: " + sessionData.timePlayed.ToString("00:00");
        powerupsCollectedTxt.text = "POWER-UPS COLLECTED: " + sessionData.powerupsCollected.ToString("N0");
        //enemiesKilledTxt.text = "ENEMIES DESTROYED: " + sessionData.enemiesKilled.ToString("N0");

        GameOverMenu.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    int spriteIncrement;
    float loadPercentage;
    public void UpdateProgressBar(int total, int completed)
    {
        loadPercentage = (float)completed / (float)total;
        if(loadBar!=null)
        {
            loadBar.fillAmount = loadPercentage;
        }

        if(loadPercentage==1)
        {
            loadScreenAnim.SetTrigger("FadeOut");
        }
    }
}