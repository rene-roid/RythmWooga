using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaueMenu : MonoBehaviour
{
    public bool IsPaused = false;
    public GameObject PauseMenuUI;
    public TMP_Text countDown;
    public GameObject buttons;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                StartCoroutine(CountDown());
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0;
        IsPaused = true;
        PauseMenuUI.SetActive(true);
        buttons.SetActive(true);
        countDown.text = "";
    }

    public void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1;
        IsPaused = false;
        PauseMenuUI.SetActive(false);
    }

    // Count to 3 and show it in countDown.text
    IEnumerator CountDown()
    {
        buttons.SetActive(false);
        for (int i = 3; i > 0; i--)
        {
            countDown.text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        countDown.text = "GO!";
        yield return new WaitForSecondsRealtime(.5f);
        countDown.text = "";
        ResumeGame();
    }

}
