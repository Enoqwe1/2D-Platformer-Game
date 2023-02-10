using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
public class GameManager : MonoBehaviour
{
    private Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;
    private CinemachineVirtualCamera CVC;
    [HideInInspector]
    public TextMeshProUGUI pointsForCoin, pointsForChest, timerText;

    private PlayerStats Ps;
    private HealthBar healthBar;
    [HideInInspector]
    public bool stopClock = false;

    private float respawnTimeStart, gameTime, time;
    private bool respawn;
    private string min, sec;
    private int totalpoint, totalChest;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        pointsForCoin = GameObject.Find("UICanvas/pointsForCoin").GetComponent<TextMeshProUGUI>();
        pointsForChest = GameObject.Find("UICanvas/pointsForChest").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.Find("UICanvas/Time").GetComponent<TextMeshProUGUI>();
        respawnPoint = GameObject.Find("GameManager/Respawn Point").transform;
        Ps = FindObjectOfType<PlayerStats>();
        healthBar = FindObjectOfType<HealthBar>();
        totalpoint = 0;
        totalChest = 0;
        gameTime = Time.time;
        pointsForCoin.text = totalpoint.ToString();
    }
    private void Update()
    {
        CheckRespawn();
        CheckClock();
    }
    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }
    private void CheckClock()
    {
        if (!stopClock)
        {
            time = Time.time - gameTime;

            min = ((int)time / 60).ToString();
            sec = (time % 60).ToString("f2");

            timerText.text = min + ":" + sec;
        }
    }
    private void CheckRespawn()
    {

        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            Ps = FindObjectOfType<PlayerStats>();
            healthBar.SetHealth(Ps.currentHealth);
            respawn = false;
        }
    }
    public void CoinAddToScore(int pointsForCoinPickup)
    {
        totalpoint += pointsForCoinPickup;
        pointsForCoin.text = totalpoint.ToString();
    }
    public void ChestAddToScore(int pointsForChestPickup)
    {
        totalChest += pointsForChestPickup;
        pointsForChest.text = totalChest.ToString() + "/8";
    }
    public void NewRespawnPoint(Transform newResPoint)
    {
        respawnPoint = newResPoint;
    }
    public void Health(float amount)
    {
        healthBar.SetHealth(amount);
    }
}
