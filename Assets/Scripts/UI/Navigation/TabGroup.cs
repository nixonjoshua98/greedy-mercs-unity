using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SRC.UI.Navigation
{
    public class TabGroup : MonoBehaviour
    {
        private readonly List<TabButton> Buttons = new();
        private TabButton SelectedButton = null;

        public void Subscribe(TabButton button)
        {
            Buttons.Add(button);
        }

        private void Start()
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

        public void Select(string identifer)
        {
            var btn = Buttons.FirstOrDefault(b => b.Identifier == identifer);

            if (btn is not null)
            {
                OnTabSelected(btn);
            }
        }

        public void OnTabSelected(TabButton button)
        {
            if (SelectedButton == button)
                return;

            InvokeEvents(button);

            SelectedButton = button;
        }

        private void InvokeEvents(TabButton newlySelected)
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
