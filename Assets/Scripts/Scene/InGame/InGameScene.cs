using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        /// キャンセルトークンソース。
        /// </summary>
        CancellationTokenSource _cts;

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
                break;
            case WaveCommandType.PlayBgm:
                God.Instance.SoundManager.PlayBgm(0, (SoundBgmId)command.Value1, true);
                God.Instance.BeatTimer.ResetTimer();
                _samuraiBeatItem.Initialize(command.Value2, BeatItem.AnimationType.Scale, false);
                break;
            case WaveCommandType.SamuraiEnter:
                await _samuraiBeatItem.transform.DOLocalMoveX(-256.0f, 1.0f);
                break;
            case WaveCommandType.SamuraiExit:
                God.Instance.SoundManager.FadeOutBgm(0, 3.0f);
                await _samuraiBeatItem.transform.DOLocalMoveX(768.0f, 3.0f)
                    .SetEase(Ease.InSine);
                break;
            case WaveCommandType.WaveMessage:
                string message = WaveMessages.GetMessage(command.Value1);
                await _waveMessage.DisplayMessage(message, token);
                break;
            case WaveCommandType.End:
                God.Instance.SoundManager.StopAllBgm();
                onClick();
                break;
            }
        }
        #endregion
    }
}
