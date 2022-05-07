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
        private SoundManager _soundManager;

        /// <summary>
        /// タイトルシーン。
        /// </summary>
        [SerializeField]
        private TitleScene _titleScene;

        /// <summary>
        /// インゲームシーン。
        /// </summary>
        [SerializeField]
        private InGameScene _inGameScene;

        /// <summary>
        /// サウンド管理クラス。
        /// </summary>
        public SoundManager SoundManager => _soundManager;

        /// <inheritdoc/>
        void Awake()
        {
            // 神はひとり
            Instance = this;
        }

        /// <inheritdoc/>
        void Start()
        {
            EnterTitle().Forget();
        }

        public async UniTask ChangeToTutorial()
        {
            await ExitTitle();
            await EnterInGame(true);
        }

        public async UniTask ChangeToInGame()
        {
            await ExitTitle();
            await EnterInGame(false);
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

        async UniTask EnterInGame(bool tutorial)
        {
            await _inGameScene.EnterAsync(tutorial);
        }

        async UniTask ExitInGame()
        {
            await _inGameScene.ExitAsync();
        }
    }
}
