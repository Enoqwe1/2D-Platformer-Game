using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChestOpen : MonoBehaviour
{
    public Animator animator;
    private bool isOpen;
    [SerializeField]
    private GameObject chestLocation;
    private GameManager GM;
    private Quiz quiz;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        quiz = GameObject.Find("Quiz").GetComponent<Quiz>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButton("Use") && !isOpen && other.tag == "Player")
        {
            isOpen = true;
            quiz.PanelOpener();
            GM.ChestAddToScore(1);
            GM.NewRespawnPoint(chestLocation.transform);
            animator.SetBool("In", true);
            animator.SetBool("Out", false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("In", false);
        animator.SetBool("Out", true);
    }
}
