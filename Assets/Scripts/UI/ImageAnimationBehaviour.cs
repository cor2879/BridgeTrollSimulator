/**************************************************
 *  ImageAnimationBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class ImageAnimationBehaviour : UIHelperBehaviour
    {
        [SerializeField]
        private Sprite[] sprites;

        [SerializeField]
        private int framesPerSprite = 6;

        [SerializeField]
        private bool loop = false;

        [SerializeField]
        private bool destroyOnEnd = false;

        private Image image;

        public int CurrentIndex { get; private set; } = 0;

        public int CurrentFrame { get; private set; } = 0;

        public int FramesPerSprite { get => this.framesPerSprite; private set => this.framesPerSprite = value; }

        public bool Loop { get => this.loop; private set => this.loop = value; }

        public bool DestroyOnEnd { get => this.destroyOnEnd; private set => this.destroyOnEnd = value; }

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

        private void FixedUpdate()
        {
            if ((!this.Loop && this.CurrentIndex == this.sprites.Length) || !this.sprites.Any())
            {
                return;
            }

            if (++this.CurrentFrame < this.FramesPerSprite)
            {
                return;
            }

            this.Image.sprite = this.sprites[this.CurrentIndex];
            this.CurrentFrame = 0;

            if (++this.CurrentIndex >= this.sprites.Length)
            {
                if (this.Loop)
                {
                    this.CurrentIndex = 0;
                }
                else if (this.DestroyOnEnd)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public void ValidateUnityEditorParameter(MonoBehaviour parameter, string parameterName)
        {
            UIHelperBehaviour.ValidateUnityEditorParameter(parameter, parameterName, nameof(ImageAnimationBehaviour));
        }
    }
}
