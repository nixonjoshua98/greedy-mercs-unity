using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace GreedyMercs
{
    using GM.Characters;

    public class SquadManager : MonoBehaviour
    {
        List<Transform> characterSpots;

        void Awake()
        {
            characterSpots = new List<Transform>();

            for (int i = 0; i < transform.childCount; ++i)
                characterSpots.Add(transform.GetChild(i));

            Events.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
        }

        void AddCharacter(CharacterID chara)
        {
            MercData data = StaticData.Mercs.GetMerc(chara);

            GameObject character = Instantiate(data.Prefab, transform);

            character.transform.position = characterSpots[0].position;

            Destroy(characterSpots[0].gameObject);

            characterSpots.RemoveAt(0);
        }

        void OnHeroUnlocked(CharacterID chara)
        {
            AddCharacter(chara);
        }
    }
}