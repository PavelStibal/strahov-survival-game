﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviourSingleton<DayNightController> {

    public Text dayNightText;

    public CameraDayController cameraMovementDay;
    public CameraNightController cameraMovementNight;

    public GameObject[] onlyDayObjects;
    public GameObject[] onlyNightObjects;

    private DayNightPhase phase;
    private int dayCounter = 0;

    // Get the current phase
    public DayNightPhase Phase 
    {
        get {
            return phase;
        }
    }

    // Which phase should be starting? => Day
    public void Start()
    {
        StartDayPhase();
    }

    public void SwitchPhase()
    {       
        if (phase == DayNightPhase.DAY)
        {
            ScreenFader.Instance.onFadeOut.RemoveListener(StartDayPhase);
            ScreenFader.Instance.onFadeOut.AddListener(StartNightPhase);
        }
        else if (phase == DayNightPhase.NIGHT)
        {
            LightsController.Instance.SetLampsOn(false);
            ScreenFader.Instance.onFadeOut.RemoveListener(StartNightPhase);
            ScreenFader.Instance.onFadeOut.AddListener(StartDayPhase);
        }

        ScreenFader.Instance.FadeOut();

        Invoke("InvokeFadeIn", 2f); //to hide camera change
    }

    private void InvokeFadeIn() {
        ScreenFader.Instance.FadeIn();
    }

    private void StartNightPhase()
    {
        LightsController.Instance.SetLampsOn(true);
        LightsController.Instance.SetDirectionalLightOn(false);

        phase = DayNightPhase.NIGHT;

        MainUISoundManager.Instance.StopSound("day");
        MainUISoundManager.Instance.PlaySound("main");
        MainUISoundManager.Instance.PlaySound("night");

        dayNightText.text = "Night " + dayCounter;

        MainCanvasManager.Instance.HideBuildMenu();
        ActivateProperObjectsAndScripts();
    }

    private void StartDayPhase()
    {
        LightsController.Instance.SetDirectionalLightOn(true);

        dayCounter++;

        phase = DayNightPhase.DAY;

        MainUISoundManager.Instance.StopSound("main");
        MainUISoundManager.Instance.StopSound("night");
        MainUISoundManager.Instance.PlaySound("day");

        dayNightText.text = "Day " + dayCounter;

        MainCanvasManager.Instance.ShowBuildMenu();
        ActivateProperObjectsAndScripts();
    }

    private void ActivateProperObjectsAndScripts()
    {
        cameraMovementDay.enabled = IsDay;
        cameraMovementNight.enabled = IsNight;

        MainObjectManager.Instance.UpdateSwitchableObjects(phase);
    }

    public bool IsDay {
        get {
            return phase == DayNightPhase.DAY;
        }
    }

    public bool IsNight {
        get {
            return phase == DayNightPhase.NIGHT;
        }
    }
}
