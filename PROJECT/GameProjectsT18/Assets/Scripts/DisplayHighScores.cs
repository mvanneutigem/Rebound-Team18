using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour
{
    public int _levelIndex = 1;
    public Text[] HighscoresTexts;
    private HighScores highScoreManager;

	void Start ()
	{
	    

        for (int i = 0; i < HighscoresTexts.Length; ++i)
	    {
	        HighscoresTexts[i].text = i + 1 + ". Loading...";//one based board
	    }

	    highScoreManager = GetComponent<HighScores>();
        _levelIndex = PlayerPrefs.GetInt("Scene");
        highScoreManager.SetLeaderboard(_levelIndex - 3);

        StartCoroutine(RefreshHighScores());
	}

    public void OnHighscoresDownloaded(Highscore[] highscoreList)
    {
        //Highscore[] localHighscores = new Highscore[5]; ;
        //for (int i = 0; i < highscoreList.Length; ++i)
        //{
        //    if (highscoreList[i].levelIndex == levelIndex)
        //        localHighscores[i] = highscoreList[i];
        //}
        for (int i = 0; i < HighscoresTexts.Length; ++i)
        {
            HighscoresTexts[i].text = i + 1 + ". ";//one based board
            if (highscoreList.Length > i)//ignore if not enough entries
            {
                HighscoresTexts[i].text += highscoreList[i].username + " - " + highscoreList[i].score;
            }
        }

    }

    IEnumerator RefreshHighScores()
    {
        while (true)
        {
            highScoreManager.DownloadHighscores();
            yield return new WaitForSeconds(30);
        }
    }
}
