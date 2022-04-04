using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GM
{
    public class TypeWriter : MonoBehaviour
    {
        [SerializeField] private string TextToDisplay;
        [SerializeField] private float MsPerChar = 250;
        [SerializeField] private bool Loop;
        [Space]
        [SerializeField] private TMP_Text Text;
        private Stopwatch stopWatch;
        private int numCharactersShown = 1;

        private void Awake()
        {
            Text.text = "";
            stopWatch = Stopwatch.StartNew();
        }

        private void FixedUpdate()
        {
            if (stopWatch.ElapsedMilliseconds >= MsPerChar)
            {
                stopWatch.Restart();

                if (numCharactersShown >= TextToDisplay.Length && Loop)
                {
                    numCharactersShown = 1;
                }
                else if (numCharactersShown < TextToDisplay.Length)
                {
                    int pos = TextToDisplay.Substring(numCharactersShown).ToList().FindIndex(x => !char.IsWhiteSpace(x));

                    numCharactersShown += (pos + 1);
                }
            }

            Text.text = TextToDisplay.Substring(0, numCharactersShown);
        }
    }
}
