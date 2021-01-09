using UnityEngine;
using UnityEngine.UI;

public class CharacterRow : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image Icon;

    [Header("Text")]
    [SerializeField] Text Name;
    [SerializeField] Text Damage;
    [SerializeField] Text UpgradeAmount;
    [SerializeField] Text UpgradeCost;
    [SerializeField] Text LevelText;

    [Header("Buttons")]
    [SerializeField] Button UpgradeButton;

    [Header("Prefabs")]
    [SerializeField] GameObject CharacterPanelObject;

    ScriptableCharacter character;

    protected int BuyAmount
    {
        get
        {
            if (MercsTab.BuyAmount == -1)
                return Formulas.AffordCharacterLevels(character.character);

            var state = GameState.Characters.Get(character.character);

            return Mathf.Min(MercsTab.BuyAmount, StaticData.MAX_CHAR_LEVEL - state.level);
        }
    }

    public void SetCharacter(ScriptableCharacter chara)
    {
        character = chara;

        Name.text   = chara.name;
        Icon.sprite = chara.icon;

        UpdateRow();

        InvokeRepeating("UpdateRow", 0.25f, 0.25f);
    }

    void UpdateRow()
    {
        var state = GameState.Characters.Get(character.character);

        Damage.text         = Utils.Format.FormatNumber(StatsCache.GetCharacterDamage(character.character)) + " DPS";
        UpgradeCost.text    = state.level >= StaticData.MAX_CHAR_LEVEL ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(character.character, BuyAmount));
        LevelText.text      = "Level " + state.level.ToString();
        UpgradeAmount.text  = state.level >= StaticData.MAX_CHAR_LEVEL ? "" : "x" + BuyAmount.ToString();

        UpgradeButton.interactable = state.level < StaticData.MAX_CHAR_LEVEL;
    }

    // === Button Callbacks ===

    public void OnUpgrade()
    {
        int levelsBuying = BuyAmount;

        var state = GameState.Characters.Get(character.character);

        BigDouble cost = Formulas.CalcCharacterLevelUpCost(character.character, levelsBuying);

        if (state.level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
        {
            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();

            Events.OnCharacterLevelUp.Invoke(character.character);
        }
    }

    public void OnShowInfo()
    {
        GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

        panel.GetComponent<CharacterPanel>().SetHero(character.character);
    }
}