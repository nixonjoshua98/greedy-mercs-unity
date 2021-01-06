﻿using System;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;


public class SquadManager : MonoBehaviour
{
    static SquadManager Instance = null;

    [SerializeField] Transform SquadParent;

    [SerializeField] List<Transform> locations;

    void Awake()
    {
        Instance = this;

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    IEnumerator Start()
    {
        for (int i = 0; i < CharacterResources.Instance.Characters.Count; ++i)
        {
            var chara = CharacterResources.Instance.Characters[i];

            if (GameState.Characters.TryGetState(chara.character, out UpgradeState _))
                AddCharacter(chara);

            yield return new WaitForFixedUpdate();
        }
    }

    void AddCharacter(ScriptableCharacter chara)
    {
        GameObject character = Instantiate(chara.prefab, locations[0]);

        character.transform.localPosition = Vector3.zero;

        locations.RemoveAt(0);
    }

    public static void ToggleAttacks(bool enabled)
    {
        HeroAttack[] attacks = Instance.SquadParent.GetComponentsInChildren<HeroAttack>();

        foreach (HeroAttack atk in attacks)
            atk.enabled = enabled;
    }

    void OnHeroUnlocked(CharacterID chara)
    {
        AddCharacter(CharacterResources.Instance.GetCharacter(chara));
    }
}
