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

    // ===
    int maxCharacterLevel;

    protected int BuyAmount { get { return MercsTab.BuyAmount == -1 ? Formulas.AffordCharacterLevels(character.character) : Mathf.Min(Formulas.AffordCharacterLevels(character.character), MercsTab.BuyAmount); } }

    public void SetCharacter(ScriptableCharacter chara)
    {
        character = chara;

        Name.text = chara.name;
        Icon.sprite = chara.icon;

        maxCharacterLevel = StaticData.MAX_CHAR_LEVEL;

        UpdateRow();

        InvokeRepeating("UpdateRow", 0.5f, 0.5f);
    }

    void UpdateRow()
    {
        var state = GameState.Characters.Get(character.character);

        Damage.text         = Utils.Format.FormatNumber(StatsCache.GetCharacterDamage(character.character)) + " DPS";
        UpgradeCost.text    = state.level >= maxCharacterLevel ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(character.character, BuyAmount));
        LevelText.text      = "Level " + state.level.ToString();
        UpgradeAmount.text  = state.level >= maxCharacterLevel ? "" : "x" + BuyAmount.ToString();

        UpgradeButton.interactable = state.level < maxCharacterLevel;
    }

    // === Button Callbacks ===

    public void OnUpgrade()
    {
        int levelsBuying = BuyAmount;

        var state = GameState.Characters.Get(character.character);

        BigDouble cost = Formulas.CalcCharacterLevelUpCost(character.character, levelsBuying);

        if (state.level < maxCharacterLevel && GameState.Player.gold >= cost)
        {
            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }

    public void OnShowInfo()
    {
        GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

        panel.GetComponent<CharacterPanel>().SetHero(character.character);
    }
}