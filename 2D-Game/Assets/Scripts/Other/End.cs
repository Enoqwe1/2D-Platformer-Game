using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private Quiz quiz;
    private GameManager GM;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        quiz = FindObjectOfType<Quiz>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && quiz.totalChest == 8)
        {
            GM.stopClock = true;
            LoadScene();
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
