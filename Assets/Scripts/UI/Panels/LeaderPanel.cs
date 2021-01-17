using SimpleJSON;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Leaders
{
    public class LeaderPanel : MonoBehaviour
    {
        [SerializeField] Text CenterText;

        [Header("References")]
        [SerializeField] Transform RowParent;

        [Header("Prefabs")]
        [SerializeField] GameObject LeaderRow;

        void Awake()
        {
            Server.GetPlayerLeaderboard(this, ServerCallback);
        }

        void ServerCallback(long code, string compressed)
        {
            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                StartCoroutine(CreateLeaderboard(node));
            }
            else
            {
                CenterText.text = "Failed to fetch leaderboard :(";
            }
        }

        IEnumerator CreateLeaderboard(JSONNode node)
        {
            int rank = 1;

            foreach (JSONNode player in node["players"].AsArray)
            {
                string username = player.HasKey("username") ? player["username"].Value : "Rogue Mercenary";
                int value       = player["maxPrestigeStage"].AsInt;

                LeaderRow row = Utils.UI.Instantiate(LeaderRow, RowParent, Vector3.zero).GetComponent<LeaderRow>();

                row.Init(rank, username, value.ToString("N0"));

                rank++;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}