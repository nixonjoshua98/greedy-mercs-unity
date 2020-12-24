using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroesTab : MonoBehaviour
{
    [SerializeField] Transform heroRowsParent;
    [Space]
    [SerializeField] Image[] BuyAmountImages;

    // = Static Fields ==
    public static int BuyAmount = 1;
    // == Static Fields ==

    List<HeroRow> rows;

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

    void OnHeroUnlocked(HeroID _)
    {
        UpdateRows();
    }

    // === Internal Methods ===
    void UpdateRows()
    {
        foreach (HeroRow row in rows)
        {
            row.gameObject.SetActive(row.TryUpdate());
        }
    }

    // === Button Callbacks === 

    public void OnBuyAmountChange(int newBuyAmount)
    {
        BuyAmountImages[(int)Mathf.Log10(BuyAmount)].color = new Color(1, 1, 1, 190.0f / 255);
        BuyAmountImages[(int)Mathf.Log10(newBuyAmount)].color = new Color(0, 1, 0, 190.0f / 255);

        BuyAmount = newBuyAmount;
    }
}
