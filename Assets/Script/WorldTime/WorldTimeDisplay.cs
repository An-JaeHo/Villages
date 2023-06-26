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

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        worldTime.WorldTimeChagne += OnWorldTimeChanged;
    }

    private void OnDestroy()
    {
        worldTime.WorldTimeChagne -= OnWorldTimeChanged;
    }

    private void OnWorldTimeChanged(object sender, TimeSpan newTime)
    {
        text.SetText(newTime.ToString(@"hh\:mm"));
        dayCount.SetText(newTime.Days + " D");
    }
}
