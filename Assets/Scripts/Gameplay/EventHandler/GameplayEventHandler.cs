using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEventHandler : MonoBehaviour
{
    public void HideLevelUpBadge()
    {
        EventManager.DoFireHideLevelUpBadgeEvent();
    }

    public void PauseGame()
    {
        EventManager.DoFirePauseGameEvent();
    }
}
