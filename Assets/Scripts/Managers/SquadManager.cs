using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;


public class SquadManager : MonoBehaviour
{
    static SquadManager Instance = null;

    List<Transform> characterSpots;

    List<HeroAttack> attacks;


    void Awake()
    {
        Instance = this;

        attacks = new List<HeroAttack>();

        characterSpots = new List<Transform>();

        for (int i = 0; i < transform.childCount; ++i)
            characterSpots.Add(transform.GetChild(i));

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    IEnumerator Start()
    {
        for (int i = 0; i < CharacterResources.Instance.Characters.Count; ++i)
        {
            var chara = CharacterResources.Instance.Characters[i];

            if (GameState.Characters.TryGetState(chara.character, out UpgradeState _))
            {
                AddCharacter(chara);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void AddCharacter(ScriptableCharacter chara)
    {
        GameObject character = Instantiate(chara.prefab, transform);

        character.transform.position = characterSpots[0].position;

        attacks.Add(character.GetComponent<HeroAttack>());

        Destroy(characterSpots[0].gameObject);

        characterSpots.RemoveAt(0);
    }

    public static void ToggleAttacks(bool enabled)
    {

    }

    void OnHeroUnlocked(CharacterID chara)
    {
        AddCharacter(CharacterResources.Instance.GetCharacter(chara));
    }
}
