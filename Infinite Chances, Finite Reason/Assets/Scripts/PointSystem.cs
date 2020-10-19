using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    public int currentPoints;
    public int HighScore;

    public Text score;

    private bool isRewinding = false;
    public List<int> PastScore;

    // Start is called before the first frame update
    void Start()
    {
        currentPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.gameOver){
            currentPoints += 1;
            score.text = "Score:" + currentPoints;
            if(currentPoints > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore",currentPoints);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopRewind();
    }


    private void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Rewind()
    {
        currentPoints = PastScore[0];
        PastScore.RemoveAt(0);
    }
    void Record()
    {
        PastScore.Insert(0, currentPoints);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
