using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using SkillData;

public class GoldRush : MonoBehaviour
{
    [SerializeField] SkillID skill;

    [Header("Components")]
    [SerializeField] Button ActivateButton;

    [Header("Prefabs")]
    [SerializeField] GameObject CoinShowerPS;

    public void OnClick()
    {
        SkillState state = GameState.Skills.Get(skill);

        if (!state.IsActive)
        {
            GameState.Skills.ActivateSkill(skill);

            Activate();
        }
    }

    void Activate()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        ActivateButton.interactable = false;

        ParticleSystem ps = Instantiate(CoinShowerPS).GetComponent<ParticleSystem>();

        yield return new WaitWhile(() => { return GameState.Skills.Get(skill).IsActive; });

        ps.Stop();

        yield return new WaitUntil(() => { return ps.particleCount == 0; });

        Destroy(ps.gameObject);

        ActivateButton.interactable = true;
    }
}