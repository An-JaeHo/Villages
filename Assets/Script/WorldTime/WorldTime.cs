using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChagne;

    public Sprite[] dayImgaes;
    public GameObject[] tiles;
    public Image dayImge;
    public float season;
    public bool seasonChangeCheck;
    public TimeSpan currentTime;

    [SerializeField]
    private float dayLength;
    private TimeSpan saveDay;
    private float minuteLength => dayLength/WorldTimeConstants.MinutesInDay;

    private void Start()
    {
        dayLength = 24;
        //세이브 데이터 저장
        saveDay = new TimeSpan(1,8,20,0);
        currentTime = saveDay;
        season = 1;
        seasonChangeCheck = false;
        StartCoroutine(AddMinute());
    }

    private IEnumerator AddMinute()
    {
        currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChagne.Invoke(this,currentTime);

        if (currentTime.Hours == 6)
        {
            if (currentTime.Days % 3 == 0 && seasonChangeCheck)
            {
                season++;
                seasonChangeCheck = false;

                if (season > 4)
                {
                    season = 1;
                }
            }

            switch (season)
            {
                case 1:
                    tiles[0].SetActive(true);
                    tiles[1].SetActive(false);
                    tiles[2].SetActive(false);
                    dayImge.sprite = dayImgaes[0];
                    break;
                case 2:
                    dayImge.sprite = dayImgaes[2];
                    break;
                case 3:
                    tiles[0].SetActive(false);
                    tiles[1].SetActive(true);
                    tiles[2].SetActive(false);
                    dayImge.sprite = dayImgaes[4];
                    break;
                case 4:
                    tiles[0].SetActive(false);
                    tiles[1].SetActive(false);
                    tiles[2].SetActive(true);
                    dayImge.sprite = dayImgaes[6];
                    break;

                default:
                    break;
            }
        }
        else if (currentTime.Hours == 18)
        {
            switch (season)
            {
                case 1:
                    dayImge.sprite = dayImgaes[1];
                    break;
                case 2:
                    dayImge.sprite = dayImgaes[3];
                    break;
                case 3:
                    dayImge.sprite = dayImgaes[5];
                    break;
                case 4:
                    dayImge.sprite = dayImgaes[7];
                    break;

                default:
                    break;
            }
        }


        if (currentTime.Days % 3 != 0)
        {
            seasonChangeCheck = true;
        }

        yield return new WaitForSeconds(minuteLength);
        StartCoroutine(AddMinute());
    }
}
