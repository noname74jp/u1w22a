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
        SimpleButton _licensesButton;

        /// <summary>
        /// ライセンスウィンドウ。
        /// </summary>
        [SerializeField]
        LicensesWindow _licensesWindow;

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
            _licensesButton.ResetScale();
            God.Instance.SoundManager.PlayBgm(0, SoundBgmId.Title85BPM, true);
            await God.Instance.Transition.FadeIn();
            EnableButtons();
        }

        /// <summary>
        /// 退場。
        /// </summary>
        public async UniTask ExitAsync()
        {
            God.Instance.SoundManager.FadeOutBgm(0);
            await God.Instance.Transition.FadeOut();
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
        private void OnClickLicensesButton()
        {
            DisableButtons();
            _licensesWindow.OpenAsync(OnClickLicensesWindow).Forget();
            _licensesButton.KillSequence();
            _licensesButton.ResetScale();
        }

        /// <summary>
        /// ライセンスウィンドウ押下時のコールバック。
        /// </summary>
        private void OnClickLicensesWindow()
        {
            _licensesWindow.CloseAsync().Forget();
            EnableButtons();
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
            _licensesButton.SetClickCallback(OnClickLicensesButton);
            _licensesButton.ButtonEnabled = true;

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
            _licensesButton.SetClickCallback(null);
            _licensesButton.ButtonEnabled = false;
        }
    }
}
