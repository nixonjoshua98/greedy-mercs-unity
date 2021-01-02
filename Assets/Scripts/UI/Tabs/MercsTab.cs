using System;
using System.Collections;

using UnityEngine;

public class MercsTab : MonoBehaviour
{
    static MercsTab Instance = null;

    [Header("Transforms")]
    [SerializeField] Transform scrollContent;

    [Header("Controllers")]
    [SerializeField] BuyAmountController buyAmount;

    [Header("Prefabs")]
    [SerializeField] GameObject characterRowObject;

    public static int BuyAmount { get { return Instance.buyAmount.BuyAmount; } }

    void Awake()
    {
        Instance = this;

        EventManager.OnHeroUnlocked.AddListener(OnHeroUnlocked);
    }

    IEnumerator Start()
    {
        foreach (var chara in ResourceManager.Instance.Characters)
        {
            if (GameState.Characters.TryGetHeroState(chara.character, out UpgradeState _))
                AddRow(chara);

            yield return new WaitForFixedUpdate();
        }
    }

    void AddRow(ScriptableCharacter chara)
    {
        GameObject spawnedRow = Instantiate(characterRowObject, scrollContent);

        spawnedRow.transform.SetSiblingIndex(0);

        CharacterRow row = spawnedRow.GetComponent<CharacterRow>();

        row.SetCharacter(chara);
    }

    void OnHeroUnlocked(CharacterData.CharacterID chara)
    {
        AddRow(ResourceManager.Instance.GetCharacter(chara));
    }
}
