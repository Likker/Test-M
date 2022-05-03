using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverEndStrike : MonoBehaviour
{
    public Player m_Player;

    private void Awake()
    {
        m_Player = GetComponentInParent<Player>();
    }

    public void OnEndStrike()
    {
        m_Player.OnEndStrike();
    }
}
