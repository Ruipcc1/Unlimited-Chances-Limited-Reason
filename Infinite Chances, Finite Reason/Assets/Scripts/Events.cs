using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    public PlayerController playerScript;
    public PointSystem scoreScript;
    public TileManager tileScript;

    public GameObject gameOverPanel;

    private bool Rewinded = false;

    public float TimeLeft;

    private void Start()
    {
        TimeLeft = 0;
    }
    private void Update()
    {
        if (Rewinded)
        {
            if (TimeLeft >= 0)
            {
                TimeLeft -= Time.deltaTime;
                Rewind();
            }
            else
            {
                Stop();
                Rewinded = false;
                TimeLeft = 0;
            }
        }
    }

    public void RewindTime()
    {
        Time.timeScale = 1;
        Rewinded = true;
        PlayerManager.gameOver = false;
        gameOverPanel.SetActive(false);
        TimeLeft = 1f;
    }

    public void Rewind()
    {
        playerScript.StartRewind();
        scoreScript.StartRewind();
        tileScript.StartRewind();
    }

    public void Stop()
    {
        playerScript.StopRewind();
        scoreScript.StopRewind();
        tileScript.StopRewind();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
