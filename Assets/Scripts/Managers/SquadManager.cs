using System;

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
        UpdateSquad();
    }

    void UpdateSquad()
    {
        var values = Enum.GetValues(typeof(CharacterID));

        for (int i = 0; i < values.Length; ++i)
        {
            CharacterID current = (CharacterID)values.GetValue(i);

            Transform spawnPoint = HeroLocations[i];

            if (spawnPoint.childCount == 0 && GameState.TryGetHeroState(current, out HeroState _))
            {
                GameObject character = Instantiate(HeroResources.GetHeroGameObject(current), spawnPoint);

                character.transform.localPosition = Vector3.zero;
            }
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
        UpdateSquad();
    }
}
