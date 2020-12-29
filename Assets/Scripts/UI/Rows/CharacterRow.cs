using UnityEngine;
using UnityEngine.UI;

public class CharacterRow : MonoBehaviour
{
    [SerializeField] CharacterID characterId;

    public CharacterID heroId {  get { return characterId; } }

    [Space]

    [SerializeField] Text DamageText;
    [SerializeField] Text BuyText;
    [SerializeField] Text CostText;
    [SerializeField] Text LevelText;

    [Space]

    [SerializeField] Button UpgradeButton;

    [Space]

    [SerializeField] GameObject CharaPanel;

    int BuyAmount {
        get
        {
            if (MercsTab.BuyAmount == -1)
                return Formulas.AffordCharacterLevels(characterId);

            return MercsTab.BuyAmount;
        }
    }

    void UpdateRow()
    {
        var state = GameState.Characters.GetCharacter(heroId);

        LevelText.text              = "Level " + state.level.ToString();
        BuyText.text                = state.level >= StaticData.MAX_CHAR_LEVEL ? "" : "x" + BuyAmount.ToString();
        DamageText.text             = Utils.Format.FormatNumber(StatsCache.GetHeroDamage(characterId));
        CostText.text               = state.level >= StaticData.MAX_CHAR_LEVEL ? "MAX" : Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(characterId, BuyAmount));
        UpgradeButton.interactable  = state.level < StaticData.MAX_CHAR_LEVEL;
    }

    public bool TryUpdate()
    {
        if (GameState.Characters.TryGetHeroState(characterId, out var state))
        {
            UpdateRow();

            return true;
        }

        return false;
    }

    // === Button Callbacks ===

    public void OnBuyButton()
    {
        int levelsBuying = BuyAmount;

        BigDouble cost = Formulas.CalcCharacterLevelUpCost(characterId, levelsBuying);

        if (GameState.Player.gold >= cost)
        {
            var state = GameState.Characters.GetCharacter(characterId);

            state.level += levelsBuying;

            GameState.Player.gold -= cost;

            UpdateRow();
        }
    }

    public void OnInfoButton()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject panel = Instantiate(CharaPanel, Vector3.zero, Quaternion.identity);

        panel.GetComponent<CharacterPanel>().SetHero(characterId);

        panel.transform.SetParent(canvas.transform, false);
    }
}