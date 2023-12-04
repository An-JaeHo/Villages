using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepUiController : MonoBehaviour
{
    private WorldTime worldTime;
    private bool timeCheck;
    private PlayerController player;
    private int nowHours;

    private void Start()
    {
        worldTime = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldTime>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeCheck = false;
        nowHours = worldTime.currentTime.Hours;
    }

    private void Update()
    {
        if(timeCheck)
        {
            if (nowHours + 1 == worldTime.currentTime.Hours)
            {
                if(player.stamina <= player.maxStamina)
                {
                    player.stamina += 10f;
                }
                    
                nowHours = worldTime.currentTime.Hours;
            }
            

            if(worldTime.currentTime.Hours == 7)
            {
                Time.timeScale = 1f;
                timeCheck = false;
                player.moveCheck = true;
                gameObject.SetActive(false);
            }
        }
    }

    public void SleepButton()
    {
        Time.timeScale = 5f;

        timeCheck = true;
        player.moveCheck = false;
        player.rigid.velocity = Vector2.zero;
        player.moveHorizontal = 0;
        player.moveVertical = 0;
    }
}
