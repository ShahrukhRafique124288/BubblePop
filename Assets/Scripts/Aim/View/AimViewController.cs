using UnityEngine;
using DG.Tweening;

public class AimViewController
{
    #region Variables & Properties

    private AimRefs m_refs;

    private Sequence m_bubblePositionSeq;

    private float m_lastxValue;
    
    public Vector3 m_RaycastStartPosition
    {
        get
        {
            return m_refs.m_rayCastStartPos.position;
        } 
    }
    
    public Vector3 m_BubblePosition
    {
        get
        {
            return m_refs.m_dummyBubbleTransform.position;
        }
    }
    
    #endregion Variables & Properties

    #region Initialization

    public AimViewController()
    {
        Initialize(); 
    }

    private void Initialize()
    {
        m_refs = GameRefs.Instance.m_aimRefs;
        m_bubblePositionSeq = DOTween.Sequence();

        HideBubble();
    }

    #endregion Initalization

    #region Line Rendere Handling

    public void SetLineRendererPositionCount(int count)
    {
        m_refs.m_lineRenderer.positionCount = count;
    }
    
    public void SetLineRendererPosition(int index,Vector3 position)
    {
        m_refs.m_lineRenderer.SetPosition(index, position);
    }
    
    public void SetLineRendererPosition(int count, int index,Vector3 position)
    {
        SetLineRendererPositionCount(count);
        m_refs.m_lineRenderer.SetPosition(index, position);
    }

    #endregion Line Rendere Handling

    #region Bubble Positioning

    public Vector3 GetBubbleLocalPosition(Vector3 bubbleWorldPosition)
    {
        m_refs.m_dummyBubbleTransform.position = bubbleWorldPosition;

        Vector3 bubbleLocalPosition = m_refs.m_dummyBubbleTransform.localPosition;
        bubbleLocalPosition.z = 0;
        m_refs.m_dummyBubbleTransform.localPosition = bubbleLocalPosition;

        return bubbleLocalPosition;
    }

    public void ShowBubbleAtThrowPosition(Vector2 throwPosition)
    {
        Color bubbleColor = GameplayData.g_currentBubbleColor;
        bubbleColor.a = 0.5f;
        m_refs.m_bubbleBg.color = bubbleColor;

        if (Mathf.Approximately(m_lastxValue, throwPosition.x))
            return;
        
        m_lastxValue = throwPosition.x;

        m_bubblePositionSeq.Kill();
        m_bubblePositionSeq = DOTween.Sequence();

        m_bubblePositionSeq.Append(m_refs.m_bubbleTransform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear).OnComplete(delegate ()
         {
             m_refs.m_bubbleTransform.localPosition = throwPosition;
         }))
        .Append(m_refs.m_bubbleTransform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear))
        .Play();
    }

    public void HideBubble()
    {
        m_bubblePositionSeq.Kill();
        m_refs.m_bubbleTransform.localScale = Vector3.zero;
        SetLineRendererPositionCount(1);
        m_lastxValue = 0;
    }

    #endregion Bubble Positioning
}
