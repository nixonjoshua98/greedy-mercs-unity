using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace GreedyMercs
{
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
            foreach (CharacterSO chara in StaticData.CharacterList.CharacterList)
            {
                if (GameState.Characters.Contains(chara.CharacterID))
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

        void AddCharacter(CharacterSO chara)
        {
            GameObject character = Instantiate(chara.prefab, transform);

            character.transform.position = characterSpots[0].position;

            attacks.Add(character.GetComponent<CharacterAttack>());

            Destroy(characterSpots[0].gameObject);

            characterSpots.RemoveAt(0);
        }

        void OnHeroUnlocked(CharacterID chara)
        {
            AddCharacter(StaticData.CharacterList.Get(chara));
        }

        public static IEnumerator MoveOut(float duration)
        {
            foreach (var atk in Instance.attacks)
            {
                CharacterController character = atk.gameObject.GetComponent<CharacterController>();

                character.Flip();

                atk.ToggleAttacking(false);

                atk.Anim.Play("Walk");

                Vector3 start = atk.transform.localPosition;
                Vector3 end  = start - new Vector3(10.0f, 0, 0);

                Instance.StartCoroutine(Utils.Lerp.Local(atk.gameObject, start, end, duration));
            }

            yield return new WaitForSeconds(duration);
        }
    }
}
