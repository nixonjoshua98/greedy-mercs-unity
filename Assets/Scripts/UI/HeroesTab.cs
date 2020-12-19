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


    void UpdateRows()
    {
        foreach (HeroRow row in rows)
        {
            row.gameObject.SetActive(row.TryUpdate());
        }

        Invoke("UpdateRows", 0.5f);
    }

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
}
