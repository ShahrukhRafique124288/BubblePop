using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleRefs : MonoBehaviour
{
    public Transform m_bubbleTransform;
    public Image m_bubbleBg;
    public Text m_valueText;
    public Text m_mergeText;
    public Rigidbody2D m_rigidbody;
    public CircleCollider2D m_circleCollider;
    public ParticleSystem m_destroyParticles;

    public TrailRenderer m_trail;
}
