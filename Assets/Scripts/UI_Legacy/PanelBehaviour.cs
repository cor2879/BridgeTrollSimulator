/**************************************************
 *  PanelBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    public class PanelBehaviour : BeautifulInterface.BasePanel
    {
        private ButtonsPanelBehaviour buttonsPanel;

        public ButtonsPanelBehaviour ButtonsPanel
        {
            get
            {
                if (this.buttonsPanel == null)
                {
                    this.buttonsPanel = this.GetComponent<ButtonsPanelBehaviour>();
                }

                return this.buttonsPanel;
            }
        }

        public virtual void Show()
        {
            base.Show(BeautifulInterface.CanvasSide.Centre);

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Activate();
                this.ButtonsPanel.DefaultButton.Select();
            }
        }

        public virtual void Hide()
        {
            base.Hide(BeautifulInterface.CanvasSide.Centre);

            if (this.ButtonsPanel != null)
            {
                this.ButtonsPanel.Deactivate();
            }
        }
    }
}
