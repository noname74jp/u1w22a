using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

        /// <summary>
        /// ウェーブのメッセージ。
        /// </summary>
        [SerializeField]
        WaveMessage _waveMessage;

        /// <summary>
        /// 侍(プレイヤー)。
        /// </summary>
        [SerializeField]
        BeatItem _samuraiBeatItem;

        /// <summary>
        /// 敵。
        /// </summary>
        [SerializeField]
        BeatItem[] _enemyBeatItems;

        /// <summary>
        /// 敵スプライト。
        /// </summary>
        [SerializeField]
        Sprite[] _enemySprites;

        /// <summary>
        /// フラッシュ画像。
        /// </summary>
        [SerializeField]
        Image _flashImage;

        /// <summary>
        /// キャンセルトークンソース。
        /// </summary>
        CancellationTokenSource _cts;

        /// <summary>
        /// 残りの敵の数。
        /// </summary>
        List<BeatItem> _remainingEnemies = new List<BeatItem>();

        public int CurrentBeat { get; private set; }

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
            StartWaveCommands(tutorial);
            God.Instance.BeatTimer.ResetTimer();
            await UniTask.Yield();
            _titleButton.SetClickCallback(onClick);
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

        void onClick()
        {
            _titleButton.SetClickCallback(null);
            _titleButton.ButtonEnabled = false;
            StopWaveCommands();
            God.Instance.ChangeToTitle().Forget();
        }

        #region wave commands

        public void StartWaveCommands(bool tutorial)
        {
            IReadOnlyList<WaveCommand> commands = WaveCommands.GetWaveCommands(tutorial);
            _cts = new CancellationTokenSource();
            CommandLoop(commands, _cts.Token).Forget();
        }
        public void StopWaveCommands()
        {
            _cts.Cancel();
        }

        async UniTask CommandLoop(IReadOnlyList<WaveCommand> commands, CancellationToken token)
        {
            try
            {
                foreach (var command in commands)
                {
                    await DoCommand(command, token);
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
                UnityEngine.Debug.Log(e.StackTrace);
            }
        }

        async UniTask DoCommand(WaveCommand command, CancellationToken token)
        {
            switch (command.CommandType)
            {
            case WaveCommandType.Begin:
                _samuraiBeatItem.transform.localPosition = new Vector3(-768.0f, 0, 0);
                foreach (var enemy in _enemyBeatItems)
                {
                    enemy.transform.localPosition = new Vector3(768.0f, 0, 0);
                    enemy.SetSelectable(false);
                }
                _flashImage.gameObject.SetActive(false);
                break;
            case WaveCommandType.PlayBgm:
                God.Instance.SoundManager.PlayBgm(0, (SoundBgmId)command.Value1, true);
                God.Instance.BeatTimer.ResetTimer();
                _samuraiBeatItem.Initialize(command.Value2, BeatItem.AnimationType.Scale);
                CurrentBeat = command.Value2;
                break;
            case WaveCommandType.SamuraiEnter:
                await _samuraiBeatItem.transform.DOLocalMoveX(-256.0f, 1.0f);
                break;
            case WaveCommandType.SamuraiExit:
                God.Instance.SoundManager.FadeOutBgm(0, 3.0f);
                await _samuraiBeatItem.transform.DOLocalMoveX(768.0f, 3.0f)
                    .SetEase(Ease.InSine);
                break;
            case WaveCommandType.EnemyEnter:
                await EnemyEnter(command, token);
                break;
            case WaveCommandType.EnemyBeat:
                await UniTask.WaitWhile(() => _remainingEnemies.Count > command.Value1, cancellationToken: token);
                break;
            case WaveCommandType.WaveMessage:
                var message = WaveMessages.GetMessage(command.Value1);
                await _waveMessage.DisplayMessage(message, token);
                break;
            case WaveCommandType.End:
                God.Instance.SoundManager.StopAllBgm();
                onClick();
                break;
            }
        }

        private async UniTask EnemyEnter(WaveCommand command, CancellationToken token)
        {
            var enemiesParam = WaveEnemies.GetEnemies(command.Value1);
            var bpms = new List<int>();
            for (var i = 0; i < enemiesParam.Count; ++i)
            {
                var addValue = i * command.Value2;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    addValue = -addValue;
                }
                var bpm = CurrentBeat + addValue;
                bpms.Add(bpm);
            }

            var tasks = new List<UniTask>();
            for (var i = 0; i < enemiesParam.Count; ++i)
            {
                var enemyParam = enemiesParam[i];
                var enemyItem = _enemyBeatItems[i];
                var bpmIndex = UnityEngine.Random.Range(0, bpms.Count);
                var bpm = bpms[bpmIndex];
                bpms.RemoveAt(bpmIndex);
                enemyItem.gameObject.SetActive(true);
                enemyItem.Initialize(bpm, enemyParam.AnimationType);
                enemyItem.SetSprite(_enemySprites[(int)enemyParam.EnemyType]);
                switch (enemyParam.EnemyAppearType)
                {
                case EnemyAppearType.Move:
                    enemyItem.transform.localPosition = new Vector3(768.0f, enemyParam.Y, 0);
                    Tweener tweener = enemyItem.transform.DOLocalMoveX(enemyParam.X, 1.0f);
                    tasks.Add(tweener.ToUniTask());
                    break;
                case EnemyAppearType.Fade:
                    enemyItem.transform.localPosition = new Vector3(enemyParam.X, enemyParam.Y, 0);
                    break;
                }
            }

            //
            await UniTask.WhenAll(tasks);

            //
            for (var i = 0; i < enemiesParam.Count; ++i)
            {
                var enemyItem = _enemyBeatItems[i];
                enemyItem.SetSelectable(true);
                _remainingEnemies.Add(enemyItem);
            }
        }

        public async UniTask Flash(BeatItem target, bool success)
        {
            _flashImage.gameObject.SetActive(true);
            if (success)
            {
                _flashImage.color = new Color(0.75f, 0.73f, 0.012f);
                _remainingEnemies.Remove(target);
                if (_remainingEnemies.Count != 0)
                {
                    int index = UnityEngine.Random.Range(0, _remainingEnemies.Count);
                    var reinitializeEnemy = _remainingEnemies[index];
                    reinitializeEnemy.Initialize(CurrentBeat, BeatItem.AnimationType.Invalid);
                    reinitializeEnemy.SetSelectable(true);
                }
            }
            else
            {
                _flashImage.color = new Color(0.75f, 0.0f, 0.0f);
            }
            await UniTask.Delay(200);
            _flashImage.gameObject.SetActive(false);
        }

        #endregion
    }
}
