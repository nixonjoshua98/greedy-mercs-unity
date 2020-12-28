using System.Collections.Generic;

using UnityEngine;

public class MercsTab : MonoBehaviour
{
    static MercsTab Instance = null;

    [SerializeField] Transform heroRowsParent;

    [Space]

    [SerializeField] BuyAmountController buyAmount;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    List<CharacterRow> rows;

    void Awake()
    {
        Instance = this;

        rows = new List<CharacterRow>();

        for (int i = 0; i < heroRowsParent.childCount; ++i)
        {
            Transform child = heroRowsParent.GetChild(i);

            if (child.TryGetComponent(out CharacterRow row))
            {
                rows.Add(row);
            }
        }

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    void OnEnable()
    {
        InvokeRepeating("UpdateRows", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        if (IsInvoking("UpdateRows"))
            CancelInvoke("UpdateRows");
    }

    void OnHeroUnlocked(CharacterID _)
    {
        UpdateRows();
    }

    // === Internal Methods ===
    void UpdateRows()
    {
        foreach (CharacterRow row in rows)
        {
            row.gameObject.SetActive(row.TryUpdate());
        }
    }
}
