#pragma warning disable CS0649
/**************************************************
 *  BadgeRowPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;

    public class BadgeRowPanelBehaviour : UIHelperBehaviour
    {
        public const int Capacity = 4;
        public const float DisplayPanelStartingXPosition = -390.0f;
        public const float DisplayPanelStartingYPosition = 150.0f;
        public const float DisplayPanelWidth = 260.0f;
        public const float RowHeight = 350.0f;

        [SerializeField]
        private GameObject badgeDisplayPanelPrefab;

        [SerializeField]
        private GameObject badgeRowPanelPrefab;

        [SerializeField]
        private BadgesPanelBehaviour badgesPanel;

        [SerializeField, ReadOnly]
        private int rowNumber;

        private List<BadgeDisplayPanelBehaviour> BadgePanels { get; } = new List<BadgeDisplayPanelBehaviour>(Capacity);

        public BadgeDisplayPanelBehaviour[] Badges { get => this.BadgePanels.ToArray(); }

        public BadgesPanelBehaviour BadgesPanel
        {
            get => this.badgesPanel; set => this.badgesPanel = value;
        }

        public int RowNumber { get => this.rowNumber; set => this.rowNumber = value; }

        public OnSelectScrollBehaviour Scrollbar { get; set; }

        public void AddBadge(Badge badge)
        {
            if (this.BadgePanels.Count < BadgeRowPanelBehaviour.Capacity)
            {
                var badgeDisplayPanel = GetBadgeDisplayPanelBehaviour();

                if (this.BadgePanels.Count > 0)
                {
                    badgeDisplayPanel.transform.localPosition = new Vector3(
                            this.BadgePanels[0].transform.localPosition.x + (BadgeRowPanelBehaviour.DisplayPanelWidth * this.BadgePanels.Count),
                            this.BadgePanels[0].transform.localPosition.y,
                            0.0f);
                }
                this.BadgePanels.Add(badgeDisplayPanel);
                badgeDisplayPanel.Row = this;
                badgeDisplayPanel.Bind(badge);
            }
            else
            {
                var badgeRowPanel = GetBadgeRowPanelBehaviour();
                badgeRowPanel.RowNumber = this.RowNumber + 1;
                badgeRowPanel.Scrollbar = this.Scrollbar;
                badgeRowPanel.transform.localPosition = new Vector3(
                    this.transform.localPosition.x,
                    this.transform.localPosition.y - BadgeRowPanelBehaviour.RowHeight,
                    0.0f);
                // TitleScreenBehaviour.Instance.BadgesPanel.AddRow(badgeRowPanel);
                badgeRowPanel.AddBadge(badge);
            }
        }

        public void Clear()
        {
            foreach (var badgeDisplayPanel in this.BadgePanels)
            {
                Destroy(badgeDisplayPanel.gameObject);
            }

            this.BadgePanels.Clear();
        }

        public BadgeDisplayPanelBehaviour GetBadgeDisplayPanelBehaviour()
        {
            if (this.badgeDisplayPanelPrefab == null)
            {
                throw new PrefabNotSetException($"The {nameof(this.badgeDisplayPanelPrefab)} was not set!");
            }

            var badgeDisplayPanel = Instantiate(this.badgeDisplayPanelPrefab, this.transform).GetComponent<BadgeDisplayPanelBehaviour>();

            return badgeDisplayPanel;
        }

        public BadgeRowPanelBehaviour GetBadgeRowPanelBehaviour()
        {
            if (this.badgeRowPanelPrefab == null)
            {
                throw new PrefabNotSetException($"The {nameof(this.badgeRowPanelPrefab)} was not set!");
            }

            return this.BadgesPanel.GetBadgeRowPanel();
        }
    }
}
