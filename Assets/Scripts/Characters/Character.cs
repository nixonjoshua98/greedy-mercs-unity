using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace GreedyMercs
{
    public class Character : MonoBehaviour
    {
        [SerializeField] CharacterSO character;

        // === Components ===
        [Header("Components")]
        [SerializeField] ParticleSystem levelupParticles;

        // === Public Accessors ===
        public CharacterID CharacterID { get { return character.CharacterID; } }

        // === Private Attributes ===
        SpriteRenderer[] renderers;

        void Awake()
        {
            Events.OnCharacterLevelUp.AddListener((CharacterID chara) => { if (chara == character.CharacterID) OnLevelup(); });
        }

        IEnumerator Start()
        {
            renderers = transform.GetComponentsInChildren<SpriteRenderer>();

            int maxSortingLayer = 0;

            foreach (SpriteRenderer sr in renderers)
            {
                sr.sortingOrder = 100 - (int)transform.position.y;

                maxSortingLayer = Mathf.Max(sr.sortingOrder, maxSortingLayer);

                yield return new WaitForEndOfFrame();
            }

            levelupParticles.GetComponent<ParticleSystemRenderer>().sortingOrder = maxSortingLayer + 1;
        }

        // === Event Callbacks ===

        void OnLevelup()
        {
            levelupParticles.Play();
        }

        // === Helper Methods ===

        public void Flip()
        {
            Vector3 scale = transform.localScale;

            scale.x *= -1.0f;

            transform.localScale = scale;
        }
    }
}