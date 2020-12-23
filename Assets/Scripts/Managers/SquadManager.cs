using System;

using UnityEngine;

public class SquadManager : MonoBehaviour
{
    [SerializeField] Transform[] HeroLocations;

    void Awake()
    {
        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    void Start()
    {
        UpdateSquad();
    }

    void UpdateSquad()
    {
        var values = Enum.GetValues(typeof(HeroID));

        for (int i = 0; i < values.Length; ++i)
        {
            HeroID current = (HeroID)values.GetValue(i);

            Transform spawnPoint = HeroLocations[i];

            if (spawnPoint.childCount == 0 && GameState.TryGetHeroState(current, out HeroState _))
            {
                GameObject hero = Instantiate(HeroResources.GetHeroGameObject(current), spawnPoint);

                hero.transform.localPosition = Vector3.zero;
            }
        }
    }

    void OnHeroUnlocked(HeroID _)
    {
        UpdateSquad();
    }
}
