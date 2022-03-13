using System.Collections.Generic;
using UnityEngine;

namespace GM.Navigation
{
    public class TabGroup : MonoBehaviour
    {
        List<TabButton> Buttons = new List<TabButton>();

        TabButton SelectedButton = null;

        public void Subscribe(TabButton button)
        {
            Buttons.Add(button);
        }

        void Start()
        {
            Buttons.ForEach(button =>
            {
                if (button.InitiallySelected)
                {
                    if (SelectedButton is not null)
                        GMLogger.Editor($"TabGroup '{name}' has multiple initally selected tabs");

                    OnTabSelected(button);
                }
            });
        }

        public void OnTabSelected(TabButton button)
        {
            if (SelectedButton == button)
                return;

            InvokeEvents(in button);

            SelectedButton = button;
        }

        void InvokeEvents(in TabButton newlySelected)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                TabButton button = Buttons[i];

                if (button == newlySelected)
                    button.Select();

                else if (button == SelectedButton)
                    button.DeSelect();
            }
        }
    }
}
