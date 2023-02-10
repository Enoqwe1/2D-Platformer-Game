using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    private GameManager GM;
    private TextMeshProUGUI timerText, totalPoint;
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        timerText = GameObject.Find("UICanvas/Time").GetComponent<TextMeshProUGUI>();
        totalPoint = GameObject.Find("UICanvas/pointsForCoin").GetComponent<TextMeshProUGUI>();
        timerText.text = GM.timerText.text;
        totalPoint.text = GM.pointsForCoin.text;
        Destroy(GM);
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Quit()
    {
        Application.Quit();
    }

}
