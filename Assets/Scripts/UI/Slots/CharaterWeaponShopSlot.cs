using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CharaterWeaponShopSlot : MonoBehaviour
{
    public Text ownedText;

    public Image icon;
    public Image iconBorder;

    public Button button;

    public void SetWeaponsOwned(int owned)
    {
        ownedText.text = owned.ToString();
    }
}
