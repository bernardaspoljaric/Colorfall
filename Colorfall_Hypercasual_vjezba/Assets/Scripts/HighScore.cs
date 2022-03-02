using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    //WEB:https://dreamlo.com/lb/jUVGiz1VnUCZvDINbWN8iAN2xm0eIOoEGIm3JMzGn_IA
    //Podatke za stringove upisujemo sa stranice dreamlo.com
    //const - varijabla se više ne može mjenjati
    const string privateCode = "jUVGiz1VnUCZvDINbWN8iAN2xm0eIOoEGIm3JMzGn_IA";
    const string publicCode = "619b51b28f40bb12787d0de8";
    //Za android i iOS potrebno https
    const string webURL = "http://dreamlo.com/lb/";

    [Header("Player input")]
    [SerializeField] InputField playerName;
    public string userNick;

    public DisplayHighScore displayHighScore;
    highscore[] highscoreList;

    private void Awake()
    {
        //Provjera u PlayerPrefsu ima li igraè ime
        userNick = PlayerPrefs.GetString("playerUsername");
        displayHighScore = GetComponent<DisplayHighScore>();
    }

    private void Start()
    {
        if(userNick == string.Empty)
        {
            userNick = "Player" + Random.Range(1000, 100000).ToString();
        }
    }

    //Igraè je upisao novo ime i želi s tim novim imenom uploadati svoj score
    public void ChangeDataByMe()
    {
        if(playerName.text != string.Empty)
        {
            //uèiati ime iz inputa
            userNick = playerName.text;
            //spremiti novo ime u PlayerPrefs
            PlayerPrefs.SetString("playerUsername", userNick);
            //Dodjeli highscore
            int maxScore = PlayerPrefs.GetInt("Highscore");
            //Dodaj novi highscore s imenom i bodovima
            AddNewHighscore(userNick, maxScore);
            displayHighScore.myHighscore.text = userNick + " - " + PlayerPrefs.GetInt("Highscore");
        } 
    }

    public void AddNewHighscore(string username, int score)
    {
        //Pozovi korutinu
        StartCoroutine(UploadNewHighscore(username, score));
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        //Na koji link šaljemo zahtjev
        UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
        yield return www.SendWebRequest();

        //Ako je zahtjev uspješan - nemamo error
        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Upload successfull");
            //Vratiti listu highscore
            DownloadHighscores();
        }
        //Ako je zahtjev fail - imamo error
        else
        {
            Debug.Log("Error uploading: " + www.error);
        }
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        UnityWebRequest www = new UnityWebRequest(webURL + publicCode + "/pipe/");

        //DownloadHandleBuffer - preuzimanje podataka u bajtovima i Unity potom te podatke u cijelinu u našoj memoriji
        //Napomena - dok se skida je u RAM memoriji, tek kada se skine sve složi se u cijelinu i stavlja na disk

        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        www.downloadHandler = dh;

        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Download successfull");
            Debug.Log(www.downloadHandler.text);

            //Formatiranje teksta
            FormatHighscore(www.downloadHandler.text);

            //Prikaz na UI u Unityu
            displayHighScore.ShowOnTextWhenHighscoreDownloaded(highscoreList);

        }
        else
        {
            Debug.Log("Error downloading: " + www.error);
        }
    }

    // Formatiranje skinutog sadržaja
    void FormatHighscore(string textStream)
    {
        //Složi podatke u array
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        //Stvori array odreðene dužine
        highscoreList = new highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            //Razdvojiti sve podatke svakog reda gdje se nalazi |
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            //Uèitaj prvi razdvojeni podatak - username
            string username = entryInfo[0];
            //Uèitaj drugi razdvojeni podatak - score
            int score = int.Parse(entryInfo[1]);
            //Popuni array za prikaz sa podacima
            highscoreList[i] = new highscore(username, score);
        }
    }

    //Jedan blok memorije a može mu se pristupiti iz više izvora ili više naèina
    public struct highscore
    {
        public string username;
        public int score;

        public highscore(string usernameInput, int scoreInput)
        {
            this.username = usernameInput;
            this.score = scoreInput;
        }
    }
}
