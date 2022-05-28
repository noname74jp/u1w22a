using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        /// ストーリーボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _storyButton;

        /// <summary>
        /// スピードランボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _speedrunButton;

        /// <summary>
        /// ランキングボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _rankingButton;

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
        void Awake()
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
            _storyButton.ResetScale();
            _speedrunButton.ResetScale();
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
        /// ストーリーボタン押下時のコールバック。
        /// </summary>
        void OnClickStoryButton()
        {
            DisableButtons();
            God.Instance.ChangeToStopry().Forget();
        }

        /// <summary>
        /// スピードランボタン押下時のコールバック。
        /// </summary>
        void OnClickSpeedrunButton()
        {
            DisableButtons();
            God.Instance.ChangeToSpeedrun().Forget();
        }

        /// <summary>
        /// ランキングボタン押下時のコールバック。
        /// </summary>
        void OnClickRankingButton()
        {
            DisableButtons();
            int sceneCount = SceneManager.sceneCount;
            System.TimeSpan timeScore = new System.TimeSpan(0, 0, 59, 59, 999);
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(timeScore);
            UniTask.Create(async () =>
            {
                // シーンが開くまで待つ
                await UniTask.WaitWhile(() => SceneManager.sceneCount == sceneCount);
                // シーンが閉じるまで待つ
                await UniTask.WaitWhile(() => SceneManager.sceneCount != sceneCount);
                // 後処理
                _rankingButton.KillSequence();
                _rankingButton.ResetScale();
                EnableButtons();
            });
        }

        /// <summary>
        /// ライセンスボタン押下時のコールバック。
        /// </summary>
        void OnClickLicensesButton()
        {
            DisableButtons();
            _licensesWindow.OpenAsync(OnClickLicensesWindow).Forget();
            _licensesButton.KillSequence();
            _licensesButton.ResetScale();
        }

        /// <summary>
        /// ライセンスウィンドウ押下時のコールバック。
        /// </summary>
        void OnClickLicensesWindow()
        {
            _licensesWindow.CloseAsync().Forget();
            EnableButtons();
        }

        /// <summary>
        /// ボタンを有効にする。
        /// </summary>
        void EnableButtons()
        {
            _tutorialButton.SetClickCallback(OnClickTutorialButton);
            _tutorialButton.ButtonEnabled = true;
            _storyButton.SetClickCallback(OnClickStoryButton);
            _storyButton.ButtonEnabled = true;
            _speedrunButton.SetClickCallback(OnClickSpeedrunButton);
            _speedrunButton.ButtonEnabled = true;
            _rankingButton.SetClickCallback(OnClickRankingButton);
            _rankingButton.ButtonEnabled = true;
            _licensesButton.SetClickCallback(OnClickLicensesButton);
            _licensesButton.ButtonEnabled = true;
        }

        /// <summary>
        /// ボタンを無効にする。
        /// </summary>
        void DisableButtons()
        {
            _tutorialButton.SetClickCallback(null);
            _tutorialButton.ButtonEnabled = false;
            _storyButton.SetClickCallback(null);
            _storyButton.ButtonEnabled = false;
            _speedrunButton.SetClickCallback(null);
            _speedrunButton.ButtonEnabled = false;
            _rankingButton.SetClickCallback(null);
            _rankingButton.ButtonEnabled = false;
            _licensesButton.SetClickCallback(null);
            _licensesButton.ButtonEnabled = false;
        }
    }
}
