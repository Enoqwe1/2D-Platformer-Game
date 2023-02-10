using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlinlessAsk : MonoBehaviour
{
    public GameObject colorblindless, colorblindlessAsk;
    private Quiz quiz;
    // Start is called before the first frame update
    void Start()
    {
        quiz = FindObjectOfType<Quiz>();
    }

    public void Yes()
    {
        colorblindlessAsk.SetActive(false);
        colorblindless.SetActive(true);
        quiz.colorblindIsActivated = true;
    }
    public void No()
    {
        colorblindlessAsk.SetActive(false);
    }
}
