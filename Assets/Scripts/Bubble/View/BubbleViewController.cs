using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BubbleViewController
{
    #region Varibales

    private readonly float m_bubbleMovementTime = 0.2f;
    private Sequence m_bubbleMovementSequence;
    private Sequence m_bubbleMergeSequence;

    #endregion Variables

    #region Initialization

    public BubbleViewController()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_bubbleMovementSequence = DOTween.Sequence();
        m_bubbleMergeSequence = DOTween.Sequence();
    }

    #endregion Initalization

    #region Spawning

    public void SpawnBubble(BubbleModel model, float scaleValue = 1,bool isInitialSpawning = false)
    {
        model.m_bubbleRefs.m_bubbleTransform.localPosition = model.m_position;
        model.m_bubbleRefs.m_valueText.text = model.m_value.ToString();
        model.m_bubbleRefs.m_bubbleBg.color = model.m_bubbleColor;

        if (isInitialSpawning)
        {
            DOTween.Sequence().Insert(Random.Range(0, 0.1f), model.m_bubbleRefs.m_bubbleTransform.DOScale(scaleValue, 0.3f).SetEase(Ease.Linear));
        }
        else
        {
            model.m_bubbleRefs.m_bubbleTransform.DOKill();
            model.m_bubbleRefs.m_bubbleTransform.DOScale(scaleValue, 0.3f).SetEase(Ease.Linear);
        }
    }

    #endregion Spawning

    #region Movement

    public void MoveBubbleToTarget(Transform bubbleTransform, List<Vector3> targetPositions, TweenCallback targetReached)
    {
        m_bubbleMovementSequence.Kill();
        m_bubbleMovementSequence = DOTween.Sequence();

        m_bubbleMovementSequence.Append(bubbleTransform.DOLocalPath(targetPositions.ToArray(), m_bubbleMovementTime * targetPositions.Count, PathType.Linear, PathMode.Full3D)
                               .SetEase(Ease.Linear))
                               .AppendCallback(targetReached)
                               .Play();
    }

    public void SetCurrentBubble(BubbleModel model)
    {
        model.m_bubbleRefs.m_bubbleTransform.DOKill();
        model.m_bubbleRefs.m_bubbleTransform.DOLocalMove(model.m_position, 0.2f).SetEase(Ease.Linear);
        model.m_bubbleRefs.m_bubbleTransform.DOScale(1, 0.2f).SetEase(Ease.Linear);
    }

    public void MoveBubbleToMerge(List<BubbleModel> bubblesToMerge,int mergePoint, TweenCallback mergeComplete)
    {
        m_bubbleMergeSequence = DOTween.Sequence();

        Vector3 mergePostion = bubblesToMerge[mergePoint].m_bubbleRefs.m_bubbleTransform.localPosition;

        foreach (BubbleModel bubbleModel in bubblesToMerge)
        {
            if (bubbleModel.m_gridIndex != bubblesToMerge[mergePoint].m_gridIndex)
            {
                m_bubbleMergeSequence.Insert(0, bubbleModel.m_bubbleRefs.m_bubbleTransform.DOLocalMove(mergePostion, 0.2f).SetEase(Ease.Linear))
                    .Insert(0, bubbleModel.m_bubbleRefs.m_valueText.DOFade(0, 0.2f).SetEase(Ease.Linear));
                    //.Insert(0.2f, bubbleModel.m_bubbleRefs.m_bubbleBg.DOFade(0, 0.3f).SetEase(Ease.Linear));

                bubbleModel.m_bubbleRefs.m_destroyParticles.startColor = bubbleModel.m_bubbleColor;
                bubbleModel.m_bubbleRefs.m_destroyParticles.Play();
            }
            else
            {
                bubbleModel.m_bubbleRefs.m_bubbleTransform.SetSiblingIndex(3);
            }
        }

        m_bubbleMergeSequence.OnComplete(mergeComplete);
    }

    public void MoveBubbleToNewPosition(Transform bubbleTransform, Vector3 newPosition)
    {
        bubbleTransform.DOKill();
        bubbleTransform.DOLocalMove(newPosition, 0.3f).SetEase(Ease.Linear);
    }

    public void ExplodeBubble(BubbleModel bubbleModel)
    {
        bubbleModel.m_bubbleRefs.m_bubbleTransform.DOKill();
        bubbleModel.m_bubbleRefs.m_bubbleBg.transform.DOScale(0, 1.2f).SetEase(Ease.Linear);

        bubbleModel.m_bubbleRefs.m_destroyParticles.startColor = bubbleModel.m_bubbleColor;
        bubbleModel.m_bubbleRefs.m_destroyParticles.Play();
    }

    public void ShowMergedValue(BubbleModel bubbleModel)
    {
        Color tempColor = bubbleModel.m_bubbleRefs.m_mergeText.color;
        tempColor.a = 1;
        bubbleModel.m_bubbleRefs.m_mergeText.color = tempColor;

        bubbleModel.m_bubbleRefs.m_mergeText.transform.localPosition = Vector3.zero;
        bubbleModel.m_bubbleRefs.m_bubbleTransform.SetAsLastSibling();

        DOTween.Sequence().Insert(0.05f, bubbleModel.m_bubbleRefs.m_mergeText.transform.DOLocalMoveY(80, 0.7f).SetEase(Ease.Linear))
            .Insert(0.6f, bubbleModel.m_bubbleRefs.m_mergeText.DOFade(0, 0.2f).SetEase(Ease.Linear));
    }

    public void BubbleDrop(BubbleModel bubbleModel)
    {
        bubbleModel.m_bubbleRefs.m_bubbleBg.transform.localScale = Vector3.zero;
        bubbleModel.m_bubbleRefs.m_destroyParticles.startColor = bubbleModel.m_bubbleColor;
        bubbleModel.m_bubbleRefs.m_destroyParticles.Play();
    }

    #endregion Movement
}
