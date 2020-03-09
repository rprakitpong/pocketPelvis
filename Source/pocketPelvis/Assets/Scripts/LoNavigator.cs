﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LoNavigator : MonoBehaviour
{
    public Text buttonText;
    private GameObject stepButtons;
    private Button[] buttons;
    
    private void Start()
    {
        if (LearningObjectives.instance.learningObject.isNewUser)
        {
            buttonText.text = "START LEARNING";
        }
        else
        {
            buttonText.text = "RESUME LEARNING";
        }
        
        
    }
    public void StarButtonOnClick(string array)
    {
        string[] splitArray = array.Split(char.Parse("-"));
        int LO = int.Parse(splitArray[0]);
        int step = int.Parse(splitArray[1]);
        //update data
        setCurrentLO(LO, step);
        if (LearningObjectives.instance.learningObject.isNewUser)
            LearningObjectives.instance.learningObject.isNewUser = false;
        LearningObjectives.instance.SaveLOs();
    }
    public void LearningButton()
    {
        if (LearningObjectives.instance.learningObject.isNewUser)
        {
            setCurrentLO(1, 1);
            LearningObjectives.instance.learningObject.isNewUser = false;
            LearningObjectives.instance.SaveLOs();
        }
    }
    public void ResetUser()
    {
        LearningObjectives.instance.ResetLOs();
        buttonText.text = "START LEARNING";
    }
    public void DisplayLoUI()
    {
        GameObject uiGroup = GameObject.Find("LearningObjectives");
        foreach(Transform loUI in uiGroup.transform)
        {
            loUI.gameObject.SetActive(false);
        }
        uiGroup.transform.GetChild(LearningObjectives.instance.learningObject.lastLO - 1)
            .gameObject.SetActive(true);
    }
    public void DisplayStepButtons()
    {
        stepButtons = GameObject.Find("StepButtons");
        int LO = LearningObjectives.instance.learningObject.lastLO;
        int step = LearningObjectives.instance.learningObject.lastStep;
        int count = LearningObjectives.instance.learningObject.learningObjects[LO-1]
            .learningObjectAchievement.Count;
        // looking throuhg all the step buttons and enable gameobjects based on number of steps it has for each LO
        foreach (Transform loUI in stepButtons.transform)
        {
            if (count > 0)
            {
                loUI.gameObject.SetActive(true);
                count--;
            }
            else
            {
                loUI.gameObject.SetActive(false);
            }

        }
        buttons = stepButtons.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
        buttons[step-1].interactable = false;
    }
    public void StepButtonPressDown(Button clickedButton)
    {
        //buttons = stepButtons.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
        int buttonIndex = System.Array.IndexOf(buttons, clickedButton);
        LearningObjectives.instance.learningObject.lastStep = buttonIndex + 1;
        LearningObjectives.instance.SaveLOs();
        clickedButton.interactable = false;
    }
    /// <summary>
    /// + for going to previous lo step
    /// - for going to next lo step
    /// </summary>
    /// <param name="control"></param>
    public void StepsControl(string control)
    {
        int LO = LearningObjectives.instance.learningObject.lastLO;
        int step = LearningObjectives.instance.learningObject.lastStep;
        if (control == "+")
        {
            if (buttons.Length > step)
            {
                StepButtonPressDown(buttons[step]);
            }
            else
            {
                if(LO < LearningObjectives.instance.learningObject.learningObjects.Count)
                {
                    //if 
                    setCurrentLO(LO+1, 1);
                    DisplayStepButtons();
                }
            }
            
        }
        else if (control == "-")
        {
            if (step - 2 >= 0)
            {
                StepButtonPressDown(buttons[step - 2]);
            }
            else
            {
                if (LO > 1)
                {
                    setCurrentLO(LO - 1, LearningObjectives.instance.learningObject.learningObjects[LO-2].learningObjectAchievement.Count);
                    DisplayStepButtons();
                }
            }
            
        }
    }
    public void setCurrentLO(int LO,int step)
    {
        LearningObjectives.instance.learningObject.lastLO = LO;
        LearningObjectives.instance.learningObject.lastStep = step;
        LearningObjectives.instance.SaveLOs();
    }
}
