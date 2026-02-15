#pragma warning disable CS0649
/**************************************************
 *  UISliderPanelBehaviour.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    using BeautifulInterface = Interface.Elements.Scripts;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;

    public class UISliderPanelBehaviour : UIHelperBehaviour
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private InputField input;

        [SerializeField]
        private int maxValue;

        [SerializeField]
        private int defaultMaxMalue = -1;

        [SerializeField]
        private int minValue;

        [SerializeField]
        private string labelText;

        [SerializeField]
        private new string tag;

        [SerializeField]
        private int scrollIndex = -1;

        public Slider Slider
        {
            get
            {
                if (this.slider == null)
                {
                    throw new UIException($"The {nameof(this.Slider)} needs to be set in the Unity Editor.");
                }

                return this.slider;
            }
        }

        public InputField Input
        {
            get
            {
                if (this.input == null)
                {
                    throw new UIException($"The {nameof(this.Input)} needs to be set in the Unity Editor.");
                }

                return this.input;
            }
        }

        public Text Label
        {
            get
            {
                if (this.text == null)
                {
                    throw new UIException($"The {nameof(this.text)} needs to be set in the Unity Editor.");
                }

                return this.text;
            }
        }

        public string LabelText
        {
            get => this.Label.text;
            set => this.Label.text = value;
        }

        public int Value
        {
            get
            {
                int.TryParse(this.Input.text, out int value);

                return value;
            }

            set
            {
                var number = MathfExtension.MaxOrMin(value, this.MaxValue, this.MinValue);

                this.Input.text = number.ToString();
                this.Slider.value = number;
            }
        }

        public int MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                this.maxValue = value;
                this.Slider.maxValue = this.MaxValue;
            }
        }

        public int DefaultMaxValue
        {
            get
            {
                return this.defaultMaxMalue;
            }

            set
            {
                this.defaultMaxMalue = value;
            }
        }

        public int MinValue
        {
            get => this.minValue;

            set
            {
                this.minValue = value;
                this.Slider.minValue = value;
            }
        }

        public string Tag
        {
            get => this.tag;
        }

        public UnityEvent<float> OnValueChanged { get; set; } = new Slider.SliderEvent();

        public int ScrollIndex
        {
            get
            {
                if (this.scrollIndex == -1)
                {
                    throw new UIException($"{nameof(this.scrollIndex)} needs to be set in the Unity Editor.");
                }

                return this.scrollIndex;
            }
        }

        public OnSelectScrollBehaviour Scrollbar { get; set; }

        public void Start()
        {
            this.SetUpSlider();
            this.SetUpInput();
        }

        public void Update()
        {
            this.SetUpLabel();
        }

        public void StartLabelAnimation()
        {
            var textUI = this.Label as BeautifulInterface.TextUI;

            if (textUI != null && !textUI.IsAnimating)
            {
                textUI.StartAnimation();
            }
        }

        private void ValueChanged(float value)
        {
            this.OnValueChanged?.Invoke(value);
        }

        private void SetUpLabel()
        {
            this.LabelText = this.labelText;
        }

        private void SetUpSlider()
        {
            this.Slider.onValueChanged.AddListener(this.PrintValue);
            this.Slider.onValueChanged.AddListener(this.ValueChanged);
            this.Slider.minValue = this.MinValue;
            this.Slider.maxValue = this.MaxValue;
            this.Slider.GetComponent<ScrollToViewBehaviour>().ScrollMapPosition = this.ScrollIndex;
            this.Slider.GetComponent<ScrollToViewBehaviour>().Scrollbar = this.Scrollbar;
        }

        private void SetUpInput()
        {
            this.Input.onValueChanged.AddListener(this.ValidateMinAndMaxValue);
            this.Input.onValueChanged.AddListener(this.SetSlider);
            this.Input.text = this.Slider.value.ToString();
        }

        private void PrintValue(float value)
        {
            var textValue = value.ToString();

            if (!string.Equals(this.Input.text, textValue))
            {
                this.Input.text = value.ToString();
            }
        }

        private void SetSlider(string value)
        {
            int.TryParse(value, out int sliderValue);

            if (!(this.Slider.value == sliderValue))
            {
                this.Slider.value = sliderValue;
            }
        }

        public void ValidateMinAndMaxValue(string value)
        {
            int.TryParse(value, out int numericValue);

            this.Input.text = MathfExtension.MaxOrMin(numericValue, this.MaxValue, this.MinValue).ToString();
        }
    }
}
