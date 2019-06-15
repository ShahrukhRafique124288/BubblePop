using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuViewController : MonoBehaviour
{
    #region Variables

    private MainMenuRefs m_refs;

    #endregion Variables

    #region Life Cycle Methods

    public void Open(GameObject obj, object viewModel = null)
    {
        if (m_refs == null)
        {
            m_refs = obj.GetComponent<MainMenuRefs>();

            m_refs.m_playButton.DOScale(1.9f, 0.45f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        SetState(true);
    }

    public void Close()
    {
        SetState(false);
    }

    #endregion #region Life Cycle Methods

    #region State Handling

    private void SetState(bool state)
    {
        m_refs.gameObject.SetActive(state);
    }

    #endregion State Handling
}
