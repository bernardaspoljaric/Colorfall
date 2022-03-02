using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScore : MonoBehaviour
{
    public Text[] highscoreTexts; // Prikazuje u svakom tekstu po jedan rezulat
    public Text myHighscore; // Prikaz igraèevog highscora
    [SerializeField]
    int refreshRate = 30; //Vrijeme ponovnog zahtjeva ua highscore
    HighScore highscoreManager;

    private void Start()
    {
        highscoreManager = GetComponent<HighScore>();
    }

    public void OnButtonClickToUpdateScore()
    {
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            highscoreTexts[i].text = i + 1 + "Loading...";
        }

        //Zapoèni refreshRate
        StartCoroutine(RefreshScore());
    }

    public void ShowOnTextWhenHighscoreDownloaded(HighScore.highscore[] highList)
    {
        for (int i = 0; i < highscoreTexts.Length; i++)
        {
            
            if (highList.Length > 1)
            {
                if(i >= highList.Length)
                {
                    highscoreTexts[i].text = "Data does not exist";
                }
                else if(i < highList.Length)
                {
                    highscoreTexts[i].text = highList[i].username + "\t" + highList[i].score;
                }
            }
        }
        // Prikaz score zasebno u igri
        myHighscore.text = highscoreManager.userNick + "\t" + PlayerPrefs.GetInt("Highscore");
    }

    IEnumerator RefreshScore()
    {
        while (true)
        {
            highscoreManager.DownloadHighscores();
            yield return new WaitForSeconds(refreshRate);
        }
    }

}
