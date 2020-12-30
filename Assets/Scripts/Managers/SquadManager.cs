using System;
using System.Collections;

using UnityEngine;


public class SquadManager : MonoBehaviour
{
    static SquadManager Instance = null;

    [SerializeField] Transform[] HeroLocations;

    void Awake()
    {
        Instance = this;

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    void Start()
    {
        StartCoroutine(UpdateSquad());
    }

    IEnumerator UpdateSquad()
    {
        var values = Enum.GetValues(typeof(CharacterID));

        for (int i = 0; i < values.Length; ++i)
        {
            CharacterID current = (CharacterID)values.GetValue(i);

            Transform spawnPoint = HeroLocations[i];

            if (spawnPoint.childCount == 0 && GameState.Characters.TryGetHeroState(current, out UpgradeState _))
            {
                GameObject character = Instantiate(CharacterResources.GetHeroGameObject(current), spawnPoint);

                character.transform.localPosition = Vector3.zero;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public static void ToggleAttacks(bool enabled)
    {
        HeroAttack[] attacks = Instance.HeroLocations[0].parent.GetComponentsInChildren<HeroAttack>();

        foreach (HeroAttack atk in attacks)
            atk.enabled = enabled;
    }

    void OnHeroUnlocked(CharacterID _)
    {
        StartCoroutine(UpdateSquad());
    }
}
