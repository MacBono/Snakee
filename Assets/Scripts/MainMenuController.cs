using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Text highScore;
    // Start is called before the first frame update
    void Start()
    {
        HighScoreFunction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    void HighScoreFunction()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
    
}
