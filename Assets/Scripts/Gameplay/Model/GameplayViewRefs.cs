using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayViewRefs : MonoBehaviour
{
    public Text m_ScoreText;
    public Text m_streakText;
    public Text m_currentLevelText;
    public Text m_nextLevelText;

    public Image m_levelFillBar;
    public Image m_currentLevelBG;
    public Image m_nextLevelBG;

    public GameObject m_levelUpBadge;
    public Button m_levelBadgeCross;

    public Text m_levelBadgeText;

    public Transform m_bubbleContainer;

    public Image m_perfectImage
        ;
}
