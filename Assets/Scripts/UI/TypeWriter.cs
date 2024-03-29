using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GM.UI
{
    public class TypeWriter : MonoBehaviour
    {
        public string Template;
        [SerializeField] string TextToDisplay;
        [SerializeField] float MsPerChar = 250;
        [SerializeField] bool Loop;
        [Space]
        [SerializeField] TMP_Text Text;

        private Stopwatch stopWatch;
        private int numCharactersShown = 1;

        void Awake()
        {
            if (Template == null || Template == string.Empty)
                Template = "%TEXT%";
        }

        private void OnEnable()
        {
            Text.text = "";
            numCharactersShown = 1;
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

            Text.text = Template.Replace("%TEXT%", TextToDisplay.Substring(0, numCharactersShown));
        }
    }
}
