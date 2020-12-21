using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesTab : MonoBehaviour
{
    [SerializeField] Transform heroRowsParent;

    List<HeroRow> rows;

    public void OnEnable()
    {
        UpdateRows();
    }

    public void OnDisable()
    {
        if (IsInvoking("UpdateRows"))
        {
            CancelInvoke("UpdateRows");
        }
    }

    void Awake()
    {
        rows = new List<HeroRow>();

        for (int i = 0; i < heroRowsParent.childCount; ++i)
        {
            Transform child = heroRowsParent.GetChild(i);

            if (child.TryGetComponent(out HeroRow row))
            {
                rows.Add(row);
            }
        }
    }


    void UpdateRows()
    {
        foreach (HeroRow row in rows)
        {
            row.gameObject.SetActive(row.TryUpdate());
        }

        Invoke("UpdateRows", 0.5f);
    }
}
