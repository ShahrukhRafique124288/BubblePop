using UnityEngine;
using DG.Tweening;

public static class AnimationHandler {

	private static Sequence m_slideDownSeq;
	private static Sequence m_shakeSeq;

	public static void Initialize()
	{
		m_slideDownSeq = DOTween.Sequence ();
		m_shakeSeq = DOTween.Sequence ();
	}

	public static void PlayPunchAnimation(Transform obj,float animationTime, float punchSize, TweenCallback animationCompleteCallBack)
	{
		obj.DOKill(true);
		obj.DOPunchScale (new Vector3 (punchSize, punchSize, 0f), animationTime, 2, 1f).OnComplete (animationCompleteCallBack);
	}

	public static void SlideDown(Transform obj, float duration = 0.15f)
	{
		if (m_slideDownSeq.IsPlaying ()) {
			m_slideDownSeq.Complete ();
			m_slideDownSeq.Kill ();
		}

		obj.DOLocalMoveY (1000, 0);

		m_slideDownSeq.Append (obj.DOLocalMoveY (0, duration).SetEase (Ease.OutBack))
			.Play ();
	}

	public static void SlideBackUp(Transform obj, TweenCallback callback, float duration = 0.15f)
	{
		if (m_slideDownSeq.IsPlaying ()) {
			m_slideDownSeq.Complete ();
			m_slideDownSeq.Kill ();
		}

		obj.DOLocalMoveY (0, 0);

		m_slideDownSeq.Append (obj.DOLocalMoveY (1000, duration).SetEase (Ease.Linear).OnComplete (callback))
			.Play ();
	}

    public static void PlayShakeAnim(Transform obj, float duration = 0.15f)
    {
        m_shakeSeq.Kill();
        obj.localScale = Vector3.one * 0.5f;
        obj.DORotate(Vector3.zero, 0);
        m_shakeSeq = DOTween.Sequence();
        m_shakeSeq
            .Append(obj.DOScale(Vector3.one * 0.55f, duration).SetEase(Ease.Linear))
            .Append(obj.DOPunchRotation(Vector3.one * -5, duration, 6)).SetEase(Ease.Linear)
            .Append(obj.DOPunchRotation(Vector3.one * 5, duration, 6)).SetEase(Ease.Linear)
            .Append(obj.DOScale(Vector3.one * 0.5f, duration).SetEase(Ease.Linear))
            .AppendInterval(0.2f)
            .SetLoops(6).Play();
    }
}
