using Cysharp.Threading.Tasks;
using UnityEngine;

namespace u1w22a
{
    /// <summary>
    /// タイトルシーン。
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        /// <summary>
        /// <see cref="Canvas"/>。
        /// </summary>
        [SerializeField]
        Canvas _canvas;

        /// <summary>
        /// チュートリアルボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _tutorialButton;

        /// <summary>
        /// インゲームボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _inGameButton;

        /// <summary>
        /// ライセンスボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _LicenseButton;

        /// <inheritdoc/>
        private void Awake()
        {
            _canvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// 入場。
        /// </summary>
        public async UniTask EnterAsync()
        {
            _canvas.gameObject.SetActive(true);
            _tutorialButton.ResetScale();
            _inGameButton.ResetScale();
            _LicenseButton.ResetScale();
            God.Instance.SoundManager.PlayBgm(0, SoundBgmId.Title85BPM, true);
            await UniTask.Yield();
            EnableButtons();
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

        /// <summary>
        /// チュートリアルボタン押下時のコールバック。
        /// </summary>
        private void OnClickTutorialButton()
        {
            DisableButtons();
            God.Instance.ChangeToTutorial().Forget();
        }

        /// <summary>
        /// インゲームボタン押下時のコールバック。
        /// </summary>
        private void OnClickInGameButton()
        {
            DisableButtons();
            God.Instance.ChangeToInGame().Forget();
        }

        /// <summary>
        /// ライセンスボタン押下時のコールバック。
        /// </summary>
        private void OnClickLicenseButton()
        {
            DisableButtons();
        }

        /// <summary>
        /// ボタンを有効にする。
        /// </summary>
        private void EnableButtons()
        {
            _tutorialButton.SetClickCallback(OnClickTutorialButton);
            _tutorialButton.ButtonEnabled = true;
            _inGameButton.SetClickCallback(OnClickInGameButton);
            _inGameButton.ButtonEnabled = true;
            _LicenseButton.SetClickCallback(OnClickLicenseButton);
            _LicenseButton.ButtonEnabled = true;

        }

        /// <summary>
        /// ボタンを無効にする。
        /// </summary>
        private void DisableButtons()
        {
            _tutorialButton.SetClickCallback(null);
            _tutorialButton.ButtonEnabled = false;
            _inGameButton.SetClickCallback(null);
            _inGameButton.ButtonEnabled = false;
            _LicenseButton.SetClickCallback(null);
            _LicenseButton.ButtonEnabled = false;
        }
    }
}
