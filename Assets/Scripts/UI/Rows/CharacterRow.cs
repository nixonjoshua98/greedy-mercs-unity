using UnityEngine;
using UnityEngine.UI;

using CharacterID = CharacterData.CharacterID;

public class CharacterRow : UpgradeRow
{
    [SerializeField] protected Text DamageText;

    [Space]

    [SerializeField] CharacterID charId;

    [Space]

    [Header("Prefabs")]
    [SerializeField] GameObject CharacterPanelObject;

    public override bool IsUnlocked { get { return GameState.Characters.TryGetHeroState(charId, out var _); } }

    protected override int GetBuyAmount()
    {
        if (MercsTab.BuyAmount == -1)
            return Formulas.AffordCharacterLevels(charId);

        return MercsTab.BuyAmount;
    }

    public override void UpdateRow()
    {
        var state = GameState.Characters.GetCharacter(charId);

        DamageText.text             = Utils.Format.FormatNumber(StatsCache.GetHeroDamage(charId)) + " DPS";
        CostText.text               = state.level >= StaticData.MAX_CHAR_LEVEL ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(charId, BuyAmount));

        UpdateText(state, StaticData.MAX_CHAR_LEVEL);
    }

    // === Button Callbacks ===

    public override void OnBuy()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcCharacterLevelUpCost(charId, levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            var state = GameState.Characters.GetCharacter(charId);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }

    public void OnInfo()
    {
        GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

        panel.GetComponent<CharacterPanel>().SetHero(charId);
    }
}