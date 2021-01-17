
using UnityEngine;
using UnityEngine.UI;

namespace UI.Leaders
{
    public class LeaderRow : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text rankText;
        [SerializeField] Text valueText;
        [SerializeField] Text usernameText;

        public void Init(int rank, string username, string value)
        {
            rankText.text = string.Format("#{0}", rank);
            usernameText.text = username;
            valueText.text = value;
        }
    }
}