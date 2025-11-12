#pragma warning disable CS0649
/**************************************************
 *  BadgeDisplayPanelBehaviour.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    [RequireComponent(typeof(Image))]
    public class BadgeDisplayPanelBehaviour : UIHelperBehaviour
    {
        private static readonly Color highlightColor = new Color(0.92549f, 0.87058817f, 0.0666667f, 0.588235f);

        private static readonly Color defaultColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        [SerializeField]
        private Button badgeButton;

        [SerializeField]
        private Image mask;

        [SerializeField]
        private RawImage rawImage;

        [SerializeField]
        private TextLabelBehaviour displayNameText;

        [SerializeField]
        private TextLabelBehaviour descriptionText;

        [SerializeField]
        private TextLabelBehaviour bonusDescriptionText;

        private Image image;

        private SelectableStateBehaviour selectableState;

        public Badge Badge { get; private set; }

        public Button Button
        {
            get => this.badgeButton;
        }

        public Image Image
        {
            get
            {
                if (this.image == null)
                {
                    this.image = this.GetComponent<Image>();
                }

                return this.image;
            }
        }

        public SelectableStateBehaviour SelectableState
        {
            get
            {
                if (this.selectableState == null)
                {
                    this.selectableState = this.Button.GetComponent<SelectableStateBehaviour>();
                }

                return this.selectableState;
            }
        }

        public Color BackgroundColor
        {
            get => this.Image.color;
            set => this.Image.color = value;
        }

        public string BonusDescription
        {
            get => this.bonusDescriptionText.Text;
            set => this.bonusDescriptionText.Text = value;
        }

        public string Description
        {
            get => this.descriptionText.Text;
            set => this.descriptionText.Text = value;
        }

        public string DisplayName
        {
            get => this.displayNameText.Text;
            set => this.displayNameText.Text = value;
        }

        public BadgeRowPanelBehaviour Row { get; set; }

        public Texture Texture
        {
            get => this.rawImage.texture;
            set => this.rawImage.texture = value;
        }

        public void Bind(Badge badge)
        {
            Validator.ArgumentIsNotNull(badge, nameof(badge));

            var scrollToViewBehaviour = this.Button.GetComponent<ScrollToViewBehaviour>();
            scrollToViewBehaviour.Scrollbar = this.Row.Scrollbar;
            scrollToViewBehaviour.ScrollMapPosition = this.Row.RowNumber;

            this.Badge = badge;
            this.Texture = Resources.Load<Texture2D>(badge.TextureName);
            this.DisplayName = badge.DisplayName;
            this.Description = badge.Description;
            this.BonusDescription = badge.BonusDescription;

            this.rawImage.gameObject.SetActive(badge.Enabled);

            this.badgeButton.onClick.AddListener(() =>
            {
                this.Badge.Enabled = !this.Badge.Enabled;
                this.rawImage.gameObject.SetActive(badge.Enabled);
            });
        }

        private void Update()
        {
            if (this.SelectableState.IsSelected)
            {
                this.BackgroundColor = highlightColor;
            }
            else
            {
                this.BackgroundColor = defaultColor;
            }
        }
    }
}
