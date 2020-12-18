using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesTab : MonoBehaviour
{
    [SerializeField] Transform heroRowsParent;

    List<HeroRow> rows;

    void Awake()
    {
        rows = new List<HeroRow>();

        for (int i = 0; i < heroRowsParent.childCount; ++i)
        {
            Transform child = heroRowsParent.GetChild(i);

            rows.Add(child.GetComponent<HeroRow>());
        }
    }

    void OnEnable()
    {
        foreach (HeroRow row in rows)
        {
            row.gameObject.SetActive(false);

            if (GameState.TryGetHeroState(row.heroId, out HeroState state))
            {
                row.UpdateRow(state);

                row.gameObject.SetActive(true);
            }
        }
    }
}
