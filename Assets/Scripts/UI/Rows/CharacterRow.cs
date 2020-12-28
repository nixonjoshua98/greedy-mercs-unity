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

    [SerializeField] GameObject CharaPanel;

    int BuyAmount {
        get
        {
            if (MercsTab.BuyAmount == -1)
                return Mathf.Max(1, Formulas.CalcAffordableCharacterLevels(characterId));

            return MercsTab.BuyAmount;
        }
    }

    void UpdateRow(UpgradeState state)
    {
        LevelText.text          = "Level " + state.level.ToString();
        BuyText.text            = "x" + BuyAmount.ToString();
        DamageText.text         = Utils.Format.FormatNumber(StatsCache.GetHeroDamage(characterId));
        CostText.text           = Utils.Format.FormatNumber(Formulas.CalcCharacterLevelUpCost(characterId, BuyAmount));
    }

    public bool TryUpdate()
    {
        if (GameState.Characters.TryGetHeroState(characterId, out var state))
        {
            UpdateRow(state);

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

            UpdateRow(state);
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