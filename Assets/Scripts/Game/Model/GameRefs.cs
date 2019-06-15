using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRefs : Singleton<GameRefs> {

	public ViewRefs m_viewRefs;
	public SoundRefs m_soundRefs;
    public AimRefs m_aimRefs;
    public BubbleViewRefs m_bubbleRefs;
    public GameplayViewRefs m_gameplayViewRefs;

    public ParticleSystem m_starConfetti
        ;
}
