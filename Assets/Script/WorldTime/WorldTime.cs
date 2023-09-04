using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChagne;

    public Sprite[] dayImgaes;
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
        StartCoroutine(AddMinute());

        //세이브 데이터 저장
        saveDay = new TimeSpan(1,8,20,0);
        currentTime = saveDay;
        season = 1;
        seasonChangeCheck = false;
    }

    private IEnumerator AddMinute()
    {
        currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChagne.Invoke(this,currentTime);
        
        if (currentTime.Hours== 6)
        {
            dayImge.sprite = dayImgaes[0];
        }
        else if(currentTime.Hours == 18)
        {
            dayImge.sprite = dayImgaes[1];
        }

        if(currentTime.Days %3 ==0 && seasonChangeCheck)
        {
            season++;
            seasonChangeCheck = false;

            if (season > 4)
            {
                season = 1;
            }
        }

        if(currentTime.Days % 3 != 0)
        {
            seasonChangeCheck = true;
        }

        yield return new WaitForSeconds(minuteLength);
        StartCoroutine(AddMinute());
    }
}
