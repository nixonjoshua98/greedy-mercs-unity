using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI.Messages
{
    public class ProgressMessage : Message
    {
        [SerializeField] Text stagesCompletedText;

        public void Init(int stages)
        {
            stagesCompletedText.text = string.Format("{0} Stages", stages);
        }
    }
}
