using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    /*private Rigidbody2D rb;
    public GameObject player;
    private bool isDashing = true;
    private float dashTime = 0.1f, dashTimeLeft, lastImageXpos;
    [HideInInspector]
    public float lastDash = -100f;
    public float dashCoolDown = 2.5f, distanceBetweenImages = 0.1f;
    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Time.time >= (lastDash + dashCoolDown))
        {
            AttemToDash();
        }
        CheckDash();
    }
    private void AttemToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(50, 0);
                dashTimeLeft -= Time.deltaTime;
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
        }
    }*/
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
