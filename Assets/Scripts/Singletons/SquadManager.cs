using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace GreedyMercs
{
    using GM.Characters;

    using GreedyMercs.Characters;

    public class SquadManager : MonoBehaviour
    {
        static SquadManager Instance = null;

        List<Transform> characterSpots;

        List<CharacterAttack> attacks;

        void Awake()
        {
            Instance = this;

            attacks = new List<CharacterAttack>();

            characterSpots = new List<Transform>();

            for (int i = 0; i < transform.childCount; ++i)
                characterSpots.Add(transform.GetChild(i));

            Events.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
        }

        IEnumerator Start()
        {
            foreach (MercContainer chara in StaticData.CharacterList.mercsArray)
            {
                if (GameState.Characters.Contains(chara.ID))
                {
                    AddCharacter(chara);

                    yield return new WaitForSecondsRealtime(0.1f);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        public static void ToggleAttacking(bool val)
        {
            foreach (var atk in Instance.attacks)
                atk.ToggleAttacking(val);
        }

        void AddCharacter(MercContainer chara)
        {
            GameObject character = Instantiate(chara.Prefab, transform);

            character.transform.position = characterSpots[0].position;

            attacks.Add(character.GetComponent<CharacterAttack>());

            Destroy(characterSpots[0].gameObject);

            characterSpots.RemoveAt(0);
        }

        void OnHeroUnlocked(CharacterID chara)
        {
            AddCharacter(StaticData.CharacterList.Get(chara));
        }
    }
}
