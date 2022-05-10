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

        public async UniTask ChangeToTutorial()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Tutorial);
        }

        public async UniTask ChangeToStopry()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Story);
        }

        public async UniTask ChangeToSpeedrun()
        {
            await ExitTitle();
            await UniTask.Delay(500);
            await EnterInGame(InGameScene.GameMode.Speedrun);
        }

        public async UniTask ChangeToTitle()
        {
            await ExitInGame();
            await EnterTitle();
        }

        async UniTask EnterTitle()
        {
            await _titleScene.EnterAsync();
        }

        async UniTask ExitTitle()
        {
            await _titleScene.ExitAsync();
        }

        async UniTask EnterInGame(InGameScene.GameMode gameMode)
        {
            await _inGameScene.EnterAsync(gameMode);
        }

        async UniTask ExitInGame()
        {
            await _inGameScene.ExitAsync();
        }
    }
}
