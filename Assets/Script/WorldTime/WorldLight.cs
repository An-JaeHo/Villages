using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    private Light2D light;
    
    [SerializeField]
    private WorldTime worldTime;

    [SerializeField]
    private Gradient gradient;

    private void Awake()
    {
        light  = GetComponent<Light2D>();
        worldTime.WorldTimeChagne += OnworldTimeChaged;
    }

    private void OnDestroy()
    {
        worldTime.WorldTimeChagne -= OnworldTimeChaged;
    }

    private void OnworldTimeChaged(object sender, TimeSpan newTime)
    {
        light.color = gradient.Evaluate(PercentOfDay(newTime));
    }

    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float)timeSpan.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
    }
}
