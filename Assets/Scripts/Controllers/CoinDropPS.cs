using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropPS : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;

    void Awake()
    {
        EventManager.OnKillEnemy.AddListener(Activate);
        EventManager.OnKilledBoss.AddListener(Activate);
    }

    void Activate()
    {
        var emission = ps.emission;

        emission.rateOverTime = Mathf.Min(50,  5 + (5 * (GameState.Stage.stage / 10.0f)));

        ps.Play();
    }
}
