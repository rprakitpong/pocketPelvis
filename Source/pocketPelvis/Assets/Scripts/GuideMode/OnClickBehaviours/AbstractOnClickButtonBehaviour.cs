﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class AbstractOnClickButtonBehaviour : MonoBehaviour
{
    private Button button;
    protected SaveDataManager saveDataManager;

    protected virtual void Awake()
    {
        // initialize the class' fields and set the onclick listener for the button
        button = GetComponent<Button>();
        saveDataManager = SaveDataManager.Instance;

        button.onClick.AddListener(OnClickButton);
    }

    protected abstract void OnClickButton();
}
