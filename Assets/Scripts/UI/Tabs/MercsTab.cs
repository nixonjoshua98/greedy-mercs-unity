using UnityEngine;

using CharacterID = CharacterData.CharacterID;

public class MercsTab : MonoBehaviour
{
    static MercsTab Instance = null;

    [SerializeField] Transform heroRowsParent;

    [Space]

    [SerializeField] BuyAmountController buyAmount;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    CharacterRow[] rows;

    void Awake()
    {
        Instance = this;

        rows = heroRowsParent.GetComponentsInChildren<CharacterRow>();

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    void OnEnable()
    {
        ToggleRows();

        InvokeRepeating("ToggleRows", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        CancelInvoke("ToggleRows");
    }

    void ToggleRows()
    {
        foreach (CharacterRow row in rows)
        {
            row.gameObject.SetActive(row.IsUnlocked);
        }
    }

    void OnHeroUnlocked(CharacterID _)
    {
        ToggleRows();
    }
}
