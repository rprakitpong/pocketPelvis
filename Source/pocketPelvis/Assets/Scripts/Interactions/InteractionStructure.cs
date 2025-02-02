﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionStructure : InteractionPanel {

    //Interaction structure panel
    //Functionalities for panel with info button commented out

    private Image structureStatusImage;
    private Color defaultColor;
    private Color highlightColor;
    private Color fadeColor;
    private Color hideColor;

    public InteractionStructure(string name) : base(name)
    {
        SetUpButton(name);
        //base.SetUpButtonRecolor();

        defaultColor = new Color(.18f, .525f, .871f, 1f);
        highlightColor = new Color(.329f, .627f, 1f, 1f);
        fadeColor = new Color(.18f, .525f, .871f, .5f);
        hideColor = new Color(.18f, .525f, .871f, 0f);

        EventManager.Instance.StateChangedEvent += OnStateChanged;
    }

    private void SetUpButton(string name)
    {
        thisInteraction = InteractionLoader.Instance.GetStructureInteraction();

        GameObject thisButton = thisInteraction.transform.Find("nameBtn").gameObject;
        Button stateBtn = thisButton.GetComponent<Button>();
        stateBtn.onClick.AddListener(delegate { EventManager.Instance.publishStateEvent(name, ""); });

        thisButton.GetComponentInChildren<Text>().text = name;

        structureStatusImage = thisInteraction.transform.Find("structureStatusImage").GetComponent<Image>();
    }

    private void OnStateChanged(string name, string state)
    {
        if (name == this.name)
        {
            STATE currentState = (STATE)Enum.Parse(typeof(STATE), state);
            switch (currentState)
            {
                case STATE.DEFAULT:
                    defaultState();
                    break;
                case STATE.HIGHLIGHT:
                    highlightState();
                    break;
                case STATE.FADE:
                    fadeState();
                    break;
                case STATE.HIDE:
                    hideState();
                    break;
            }
        }
    }

    private void hideState()
    {
        structureStatusImage.color = hideColor;
    }

    private void fadeState()
    {
        structureStatusImage.color = fadeColor;
    }

    private void highlightState()
    {
        structureStatusImage.color = highlightColor;
    }

    private void defaultState()
    {
        structureStatusImage.color = defaultColor;
    }
}