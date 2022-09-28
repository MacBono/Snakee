using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int maxSize;
    public int currentSize;
    public int xBound;
    public int yBound;
    public int score;
    public GameObject foodPrefab;
    public GameObject currentFood;
    public GameObject snakePrefab;
    public Snake head;
    public Snake tail;
    public int URDL;
    public Vector2 nextPos;
    public Text scoreText;

    // Start is called before the first frame update
    void OnEnable()
    {
        Snake.hit += hit;
    }
    void Start()
    {
        InvokeRepeating("TimerInvoke", 0, 0.1f);
        FoodFunction();
    }
    void OnDisable()
    {
        Snake.hit -= hit;
    }
    // Update is called once per frame
    void Update()
    {
        ComChangeDirection();
    }

    void TimerInvoke()
    {
        Movement();
        if (currentSize >= maxSize)
        {
            TailFunction();
        }
        else
        {
            currentSize++;
        }
    }

    void Movement()
    {
        GameObject temp;
        nextPos = head.transform.position;
        switch (URDL)
        {
            case 0:
                nextPos = new Vector2(nextPos.x, nextPos.y + 1);
                break;
            case 1:
                nextPos = new Vector2(nextPos.x + 1, nextPos.y);
                break;
            case 2:
                nextPos = new Vector2(nextPos.x, nextPos.y - 1);
                break;
            case 3:
                nextPos = new Vector2(nextPos.x - 1, nextPos.y);
                break;
        }
        if(nextPos.x < -xBound)
        {
            nextPos.x = xBound;
        }
        if(nextPos.x > xBound)
        {
            nextPos.x = -xBound;
        }
        if(nextPos.y < -yBound+4)
        {
            nextPos.y = yBound-4;
        }
        if(nextPos.y > yBound-4)
        {
            nextPos.y = -yBound+4;
        }

        temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
        head.SetNext(temp.GetComponent<Snake>());
        head = temp.GetComponent<Snake>();

        return;

    }

    void ComChangeDirection()
    {
        if (URDL != 2 && Input.GetKeyDown(KeyCode.W))
        {
            URDL = 0;
            return;
        }
        else if (URDL != 3 && Input.GetKeyDown(KeyCode.D))
        {
            URDL = 1;
            return;
        }
        else if (URDL != 0 && Input.GetKeyDown(KeyCode.S))
        {
            URDL = 2;
            return;
        }
        else if (URDL != 1 && Input.GetKeyDown(KeyCode.A))
        {
            URDL = 3;
            return;
        }
    }

    void TailFunction()
    {
        Snake tempSnake = tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }

    void FoodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound + 4, yBound -4);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        StartCoroutine(CheckRender(currentFood));
    }

    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if (IN.GetComponent<Renderer>().isVisible == false)
        {
            if (IN.tag == "Food")
            {
                Destroy(IN);
                FoodFunction();
            }
        }
    }

    void hit(string WhatWasSent)
    {
        if (WhatWasSent == "Food")
        {
            FoodFunction();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            int temp = PlayerPrefs.GetInt("HighScore");
            if(score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        if(WhatWasSent == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
