using Cysharp.Threading.Tasks;
using UnityEngine;

namespace u1w22a
{
    /// <summary>
    /// インゲームシーン。
    /// </summary>
    public class InGameScene : MonoBehaviour
    {
        /// <summary>
        /// <see cref="Canvas"/>。
        /// </summary>
        [SerializeField]
        Canvas _canvas;

        /// <summary>
        /// タイトルボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _titleButton;

        /// <inheritdoc/>
        private void Awake()
        {
            _canvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// 入場。
        /// </summary>
        public async UniTask EnterAsync(bool tutorial)
        {
            _canvas.gameObject.SetActive(true);
            _titleButton.ResetScale();
            if (tutorial)
            {
                God.Instance.SoundManager.PlayBgm(0, SoundBgmId.Title85BPM, true);
            }
            else
            {
                God.Instance.SoundManager.PlayBgm(0, SoundBgmId.InGame128BPM, true);
            }
            await UniTask.Yield();
            _titleButton.SetClickCallback(() =>
                {
                    _titleButton.SetClickCallback(null);
                    _titleButton.ButtonEnabled = false;
                    God.Instance.ChangeToTitle().Forget();
                });
            _titleButton.ButtonEnabled = true;
        }

        /// <summary>
        /// 退場。
        /// </summary>
        public async UniTask ExitAsync()
        {
            God.Instance.SoundManager.FadeOutBgm(0);
            await UniTask.Delay(1000);
            _canvas.gameObject.SetActive(false);
        }
    }
}
