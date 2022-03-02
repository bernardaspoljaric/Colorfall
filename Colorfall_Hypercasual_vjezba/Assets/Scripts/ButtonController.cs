using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Ball ballScript;
    public Animator shopAnimation;

    public Image shopPanel;
    public Image optionsPanel;
    public Image leaderboardPanel;
    public Text ball1Price;
    public Text ball2Price;
    public Text ball3Price;

    public GameObject ball;
    public Material[] materials;

    private void Start()
    {
        ball.GetComponent<MeshRenderer>().material = materials[PlayerPrefs.GetInt("Material")];
        ball1Price.text = PlayerPrefs.GetString("Ball1", "200");
        ball2Price.text = PlayerPrefs.GetString("Ball2", "350");
        ball3Price.text = PlayerPrefs.GetString("Ball3", "550");
    }
    public void LoadShop()
    {
        ball.SetActive(false);
        shopPanel.gameObject.SetActive(true);
    }
    public void LoadOptions()
    {
        ball.SetActive(false);
        optionsPanel.gameObject.SetActive(true);
    }
    public void LoadLeaderboard()
    {
        ball.SetActive(false);
        leaderboardPanel.gameObject.SetActive(true);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BuyMaterialBall1()
    {
        if(ball1Price.text != "CHOOSE")
        {
            if (ballScript.coins > 200)
            {
                ballScript.coins -= 200;
                ballScript.coinsTextShop.text = ballScript.coins.ToString();
                PlayerPrefs.SetInt("Coins", ballScript.coins);
                ball1Price.text = "CHOOSE";
                PlayerPrefs.SetString("Ball1", "CHOOSE");
            }
        }
        else
        {
            ball.GetComponent<MeshRenderer>().material = materials[1];
            PlayerPrefs.SetInt("Material", 1);
        }
    }

    public void BuyMaterialBall2()
    {
        if (ball3Price.text != "CHOOSE")
        {
            if (ballScript.coins > 350)
            {
                ballScript.coins -= 350;
                ballScript.coinsTextShop.text = ballScript.coins.ToString();
                PlayerPrefs.SetInt("Coins", ballScript.coins);
                ball3Price.text = "CHOOSE";
                PlayerPrefs.SetString("Ball2", "CHOOSE");
            }
        }
        else
        {
            ball.GetComponent<MeshRenderer>().material = materials[2];
            PlayerPrefs.SetInt("Material", 2);
        }
    }

    public void BuyMaterialBall3()
    {
        if (ball2Price.text != "CHOOSE")
        {
            if (ballScript.coins > 550)
            {
                ballScript.coins -= 550;
                ballScript.coinsTextShop.text = ballScript.coins.ToString();
                PlayerPrefs.SetInt("Coins", ballScript.coins);
                ball2Price.text = "CHOOSE";
                PlayerPrefs.SetString("Ball3", "CHOOSE");
            }
        }
        else
        {
            ball.GetComponent<MeshRenderer>().material = materials[3];
            PlayerPrefs.SetInt("Material", 3);
        }
    }

    public void ChangeBallMaterialToNone()
    {
        ball.GetComponent<MeshRenderer>().material = materials[0];
        PlayerPrefs.SetInt("Material", 0);
    }

}
