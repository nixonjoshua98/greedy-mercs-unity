using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LeaderRow : MonoBehaviour
{
    [SerializeField] Text RankText;
    [SerializeField] Text UsernameText;
    [SerializeField] Text ValueText;

    public void Init(int rank, string username, string value)
    {
        RankText.text       = string.Format("#{0}", rank);
        UsernameText.text   = username;
        ValueText.text      = value;
    }
}
