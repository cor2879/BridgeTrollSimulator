#pragma warning disable CS0649
/**************************************************
 *  BadgesPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;

    [RequireComponent(typeof(ButtonsPanelBehaviour))]
    public class BadgesPanelBehaviour : UIHelperBehaviour
    {
        public const float RowStartingXPosition = 6.0f;
        public const float RowStartingYPosition = 120.0f;
        public const float ViewPortHeight = 390.0f;

        [SerializeField]
        private GameObject badgeRowPanelPrefab;

        [SerializeField]
        private Image viewPort;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Scrollbar scrollbar;

        private ButtonsPanelBehaviour buttonsPanel;

        public GameObject BadgeRowPanelPrefab { get => this.badgeRowPanelPrefab; }

        private List<BadgeRowPanelBehaviour> Rows { get; } = new List<BadgeRowPanelBehaviour>();

        private ButtonsPanelBehaviour ButtonsPanel
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

        private Image ViewPort
        {
            get
            {
                if (this.viewPort == null)
                {
                    throw new UIException($"{nameof(this.viewPort)} not set!");
                }

                return this.viewPort;
            }
        }

        private Button CloseButton
        {
            get
            {
                if (this.closeButton == null)
                {
                    throw new UIException($"{nameof(this.closeButton)} not set!");
                }

                return this.closeButton;
            }
        }

        private Scrollbar Scrollbar
        {
            get
            {
                if (this.scrollbar == null)
                {
                    throw new UIException($"{nameof(this.scrollbar)} not set!");
                }

                return this.scrollbar;
            }
        }

        public void AddBadge(Badge badge)
        {
            if (!this.Rows.Any())
            {
                var rowPanel = GetBadgeRowPanel();
                // rowPanel.transform.localPosition = new Vector3(RowStartingXPosition, RowStartingYPosition, 0.0f);
                // rowPanel.transform.position = new Vector3(6.0f, -175f, 0.0f);
                Debug.Log($"Setting row panel position: X:{rowPanel.transform.localPosition.x}, Y:{rowPanel.transform.localPosition.y}");
                rowPanel.Scrollbar = this.Scrollbar.GetComponent<OnSelectScrollBehaviour>();
                rowPanel.RowNumber = 0;
                this.Rows.Add(rowPanel);
            }

            this.Rows.Last().AddBadge(badge);
        }

        public void AddRow(BadgeRowPanelBehaviour row)
        {
            this.Rows.Add(row);
        }

        public BadgeRowPanelBehaviour GetBadgeRowPanel()
        {
            if (this.badgeRowPanelPrefab == null)
            {
                throw new PrefabNotSetException($"The {nameof(this.badgeRowPanelPrefab)} prefab was not set!");
            }

            var badgeRowPanel = Instantiate(this.badgeRowPanelPrefab, this.ViewPort.transform).GetComponent<BadgeRowPanelBehaviour>();
            badgeRowPanel.BadgesPanel = this;

            return badgeRowPanel;
        }

        public void Start()
        {
            this.OnEnabled.AddListener(this.Enabled);
            this.OnDisabled.AddListener(this.Disabled);
            this.CloseButton.onClick.AddListener(() => this.Close());
            this.CloseButton.GetComponent<SelectableStateBehaviour>().ButtonsPanel = this.ButtonsPanel;

        }

        public void FixedUpdate()
        {
            if (InputExtension.IsCancelPressed())
            {
                StartCoroutine(
                    nameof(base.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        InputExtension.IsCancelPressed,
                        () =>
                        {
                            this.Close();
                        }));
            }

            this.ViewPort.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BadgeRowPanelBehaviour.RowHeight * this.Rows.Count + 2.0f);
        }

        private void ClearBadges()
        {
            foreach (var row in this.Rows)
            {
                Destroy(row.gameObject);
            }

            this.Rows.Clear();
        }

        private void LoadBadges()
        {
            foreach (var badge in Badge.GetStaticBadges().Where(badge => badge.Earned))
            {
                this.AddBadge(badge);
            }
            this.SetUpBadgeNavigation();
            this.SetUpOnSelectScroll();
        }

        public override void Enable()
        {
            this.ClearBadges();
            this.LoadBadges();
            base.Enable();
        }

        private void SetUpBadgeNavigation()
        {
            if (!this.Rows.Any())
            {
                this.ButtonsPanel.DefaultButton = this.CloseButton.GetComponent<SelectableStateBehaviour>();
                return;
            }

            for (var rowIndex = 0; rowIndex < this.Rows.Count; rowIndex++)
            {
                var badgeRow = this.Rows[rowIndex];

                for (var columnIndex = 0; columnIndex < badgeRow.Badges.Length; columnIndex++)
                {
                    var badge = badgeRow.Badges[columnIndex];

                    badge.Button.navigation = new Navigation()
                    {
                        mode = Navigation.Mode.Explicit,
                        selectOnLeft = columnIndex == 0 ?
                            (rowIndex > 0 ? this.Rows[rowIndex - 1].Badges.Last().Button : null) :
                            badgeRow.Badges[columnIndex - 1].Button,
                        selectOnRight = columnIndex == badgeRow.Badges.Length - 1 ?
                            (rowIndex < this.Rows.Count - 1 ? this.Rows[rowIndex + 1].Badges[0].Button : this.CloseButton) :
                            badgeRow.Badges[columnIndex + 1].Button,
                        selectOnDown = rowIndex < this.Rows.Count - 1 ?
                            (this.Rows[rowIndex + 1].Badges.ElementAtOrDefault(columnIndex) != null ?
                                this.Rows[rowIndex + 1].Badges[columnIndex].Button : this.Rows[rowIndex + 1].Badges.Last().Button) :
                            this.CloseButton,
                        selectOnUp = rowIndex > 0 ? this.Rows[rowIndex - 1].Badges[columnIndex].Button : null
                    };

                    badge.SelectableState.ButtonsPanel = this.ButtonsPanel;
                    this.ButtonsPanel.Buttons.Add(badge.SelectableState);
                }
            }

            this.CloseButton.navigation = new Navigation()
            {
                mode = Navigation.Mode.Explicit,
                selectOnLeft = this.Rows.Last().Badges.Last().Button,
                selectOnUp = this.Rows.Last().Badges.Last().Button
            };

            this.ButtonsPanel.Buttons.Add(this.CloseButton.GetComponent<SelectableStateBehaviour>());

            this.ButtonsPanel.DefaultButton = this.Rows.First().Badges.First().SelectableState;
        }

        private void SetUpOnSelectScroll()
        {
            var onSelectScrollBehaviour = this.Scrollbar.GetComponent<OnSelectScrollBehaviour>();

            onSelectScrollBehaviour.StepHeight = BadgeRowPanelBehaviour.RowHeight;
            onSelectScrollBehaviour.ViewPortHeight = BadgesPanelBehaviour.ViewPortHeight;
            onSelectScrollBehaviour.RowCount = this.Rows.Count;

            this.Scrollbar.value = 1.0f;
        }

        public void Close()
        {
            // TitleScreenBehaviour.Instance.MoreButtonsPanel.Show();
            this.Disable();
        }

        private void Enabled()
        {
            this.ButtonsPanel.Activate();
            this.ButtonsPanel.DefaultButton.Select();
            this.Scrollbar.value = 1.0f;
        }

        private void Disabled()
        {
            this.ButtonsPanel.SelectedButton = null;
        }
    }
}
