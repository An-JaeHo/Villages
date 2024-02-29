using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WorldTimeDisplay : MonoBehaviour
{
    [SerializeField]
    private WorldTime worldTime;

    private TMP_Text text;

    public TMP_Text dayCount;

    private void Update()
    {
        text.SetText(worldTime.currentTime.ToString(@"hh\:mm"));
        dayCount.SetText(worldTime.currentTime.Days + " D");
    }

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        //worldTime.WorldTimeChagne += OnWorldTimeChanged;
    }

    private void OnDestroy()
    {
        //worldTime.WorldTimeChagne -= OnWorldTimeChanged;
    }
}
