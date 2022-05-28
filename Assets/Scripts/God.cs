using Cysharp.Threading.Tasks;
using UnityEngine;

namespace u1w22a
{
    /// <summary>
    /// 神クラス。
    /// </summary>
    public class God : MonoBehaviour
    {
        /// <summary>
        /// インスタンス。
        /// </summary>
        public static God Instance;

        /// <summary>
        /// サウンド管理。
        /// </summary>
        [SerializeField]
        SoundManager _soundManager;

        /// <summary>
        /// タイトルシーン。
        /// </summary>
        [SerializeField]
        TitleScene _titleScene;

        /// <summary>
        /// インゲームシーン。
        /// </summary>
        [SerializeField]
        InGameScene _inGameScene;

        /// <summary>
        /// 拍カウント用のタイマー。
        /// </summary>
        [SerializeField]
        BeatTimer _beatTimer;

        /// <summary>
        /// トランジション。
        /// </summary>
        [SerializeField]
        Transition _transition;

        /// <summary>
        /// サウンド管理クラス。
        /// </summary>
        public SoundManager SoundManager => _soundManager;

        /// <summary>
        /// インゲームシーン。
        /// </summary>
        public InGameScene InGameScene => _inGameScene;

        /// <summary>
        /// 拍カウント用のタイマー。
        /// </summary>
        public BeatTimer BeatTimer => _beatTimer;

        /// <summary>
        /// トランジション。
        /// </summary>
        public Transition Transition => _transition;

        /// <inheritdoc/>
        void Awake()
        {
            // 神はひとり
            Instance = this;
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        }

        /// <inheritdoc/>
        void Start()
        {
            EnterTitle().Forget();
        }

        /// <summary>
        /// チュートリアルに遷移する。
        /// </summary>
        /// <returns></returns>
        public async UniTask ChangeToTutorial()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Tutorial);
        }

        /// <summary>
        /// ストーリーに遷移する。
        /// </summary>
        /// <returns></returns>
        public async UniTask ChangeToStopry()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Story);
        }

        /// <summary>
        /// スピードランに遷移する。
        /// </summary>
        /// <returns></returns>
        public async UniTask ChangeToSpeedrun()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Speedrun);
        }

        /// <summary>
        /// タイトルに遷移する。
        /// </summary>
        /// <returns></returns>
        public async UniTask ChangeToTitle()
        {
            await ExitInGame();
            await EnterTitle();
        }

        /// <summary>
        /// タイトルに入場する。
        /// </summary>
        /// <returns></returns>
        async UniTask EnterTitle()
        {
            await _titleScene.EnterAsync();
        }

        /// <summary>
        /// タイトルから退場する。
        /// </summary>
        /// <returns></returns>
        async UniTask ExitTitle()
        {
            await _titleScene.ExitAsync();
        }

        /// <summary>
        /// インゲームに入場する。
        /// </summary>
        /// <returns></returns>
        async UniTask EnterInGame(InGameScene.GameMode gameMode)
        {
            await _inGameScene.EnterAsync(gameMode);
        }

        /// <summary>
        /// インゲームから退場する。
        /// </summary>
        /// <returns></returns>
        async UniTask ExitInGame()
        {
            await _inGameScene.ExitAsync();
        }
    }
}
