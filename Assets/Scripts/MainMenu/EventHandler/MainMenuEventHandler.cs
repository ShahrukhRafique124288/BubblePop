using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    public void StartGame()
    {
        GameController.Instance.StartGame();
    }

    public void SettingsClicked()
    {
        EventManager.DoFireOpenViewEvent(Views.Settings);
    }
}
