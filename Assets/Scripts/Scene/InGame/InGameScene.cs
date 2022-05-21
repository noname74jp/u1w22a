using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
        /// ゲームモード。
        /// </summary>
        public enum GameMode
        {
            /// <summary>
            /// チュートリアル。
            /// </summary>
            Tutorial,
            /// <summary>
            /// ストーリー。
            /// </summary>
            Story,
            /// <summary>
            /// スピードラン。
            /// </summary>
            Speedrun,
        }

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
        /// 時間。
        /// </summary>
        [SerializeField]
        Text _timeText;

        /// <summary>
        /// 失敗回数。
        /// </summary>
        [SerializeField]
        Text _failText;

        /// <summary>
        /// ゲームモード。
        /// </summary>
        GameMode _gameMode;

        /// <summary>
        /// ゲーム終了ボタンのルート。
        /// </summary>
        [SerializeField]
        GameObject _gameEndButtonsRoot;

        /// <summary>
        /// ランキングボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _rankingButton;

        /// <summary>
        /// ツイートボタン。
        /// </summary>
        [SerializeField]
        TweetButton _tweetButton;

        /// <summary>
        /// 終了ボタン。
        /// </summary>
        [SerializeField]
        SimpleButton _endButton;

        /// <summary>
        /// 時間。
        /// </summary>
        [SerializeField]
        Text _playTimeText;

        /// <summary>
        /// 失敗。
        /// </summary>
        [SerializeField]
        Text _failCountText;

        /// <summary>
        /// ランキング時間。
        /// </summary>
        [SerializeField]
        Text _rankingTimeText;

        /// <summary>
        /// キャンセルトークンソース。
        /// </summary>
        CancellationTokenSource _cts;

        /// <summary>
        /// 残りの敵の数。
        /// </summary>
        List<BeatItem> _remainingEnemies = new List<BeatItem>();

        /// <summary>
        /// プレイ時間。
        /// </summary>
        Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// 失敗回数。
        /// </summary>
        int _failCount;

        /// <summary>
        /// 現在のBPM。
        /// </summary>
        public int CurrentBPM { get; private set; }

        /// <summary>
        /// 攻撃中か。
        /// </summary>
        public bool Attacking => _flashImage.gameObject.activeSelf;

        /// <inheritdoc/>
        private void Awake()
        {
            _canvas.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        private void Update()
        {
            _timeText.text = (_stopwatch.ElapsedMilliseconds / 1000).ToString();
        }

        /// <summary>
        /// 入場。
        /// </summary>
        /// <param name="gameMode">ゲームモード。</param>
        /// <returns></returns>
        public async UniTask EnterAsync(GameMode gameMode)
        {
            _gameMode = gameMode;
            _canvas.gameObject.SetActive(true);
            _titleButton.ResetScale();
            _gameEndButtonsRoot.SetActive(false);
            StartWaveCommands(_gameMode == GameMode.Tutorial);
            God.Instance.BeatTimer.ResetTimer();
            await God.Instance.Transition.FadeIn();
            _titleButton.SetClickCallback(onClickTitleButton);
            _titleButton.ButtonEnabled = true;
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
        /// タイトルボタンをクリックしたときのコールバック。
        /// </summary>
        void onClickTitleButton()
        {
            _titleButton.SetClickCallback(null);
            _titleButton.ButtonEnabled = false;
            StopWaveCommands();
            God.Instance.ChangeToTitle().Forget();
        }

        #region wave commands

        /// <summary>
        /// コマンド実行を開始する。
        /// </summary>
        public void StartWaveCommands(bool tutorial)
        {
            IReadOnlyList<WaveCommand> commands = WaveCommands.GetWaveCommands(tutorial);
            _cts = new CancellationTokenSource();
            DoCommands(commands, _cts.Token).Forget();
        }

        /// <summary>
        /// コマンド実行を停止する。
        /// </summary>
        public void StopWaveCommands()
        {
            _cts.Cancel();
        }

        /// <summary>
        /// コマンドを実行する。
        /// </summary>
        /// <param name="commands">コマンドリスト。</param>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns></returns>
        async UniTask DoCommands(IReadOnlyList<WaveCommand> commands, CancellationToken token)
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

        /// <summary>
        /// コマンドを実行する。
        /// </summary>
        /// <param name="command">コマンド。</param>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns></returns>
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
                _remainingEnemies.Clear();
                _stopwatch.Reset();
                _stopwatch.Start();
                _timeText.text = "0";
                _failCount = 0;
                _failText.text = "0";
                break;
            case WaveCommandType.PlayBgm:
                God.Instance.SoundManager.PlayBgm(0, (SoundBgmId)command.Value1, true);
                God.Instance.BeatTimer.ResetTimer();
                _samuraiBeatItem.Initialize(command.Value2, BeatItem.AnimationType.Scale);
                CurrentBPM = command.Value2;
                break;
            case WaveCommandType.SamuraiEnter:
                God.Instance.SoundManager.PlaySe(-1, SoundSeId.Horagai, false);
                await _samuraiBeatItem.transform.DOLocalMoveX(-256.0f, 1.0f);
                break;
            case WaveCommandType.SamuraiExit:
                God.Instance.SoundManager.FadeOutBgm(0, 3.0f);
                await _samuraiBeatItem.transform.DOLocalMoveX(768.0f, 3.0f)
                    .SetEase(Ease.InSine);
                break;
            case WaveCommandType.EnemyEnter:
                await EnterEnemies(command, token);
                break;
            case WaveCommandType.EnemyBeat:
                await UniTask.WaitWhile(() => _remainingEnemies.Count > command.Value1, cancellationToken: token);
                break;
            case WaveCommandType.WaveMessage:
                if (_gameMode != GameMode.Speedrun)
                {
                    var message = WaveMessages.GetMessage(command.Value1);
                    await _waveMessage.DisplayMessage(message, token);
                }
                break;
            case WaveCommandType.Ranking:
                _stopwatch.Stop();
                if (_gameMode == GameMode.Story || _gameMode == GameMode.Speedrun)
                {
                    await ShowRanking(token);
                }
                break;
            case WaveCommandType.End:
                God.Instance.SoundManager.StopAllBgm();
                onClickTitleButton();
                break;
            }
        }

        /// <summary>
        /// コマンドに従って敵が入場する。
        /// </summary>
        /// <param name="command">コマンド。</param>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns></returns>
        private async UniTask EnterEnemies(WaveCommand command, CancellationToken token)
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
                var bpm = CurrentBPM + addValue;
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
            UnityEngine.Debug.Log($"_remainingEnemies.Count = {_remainingEnemies.Count}");
        }

        /// <summary>
        /// 敵を攻撃する。
        /// </summary>
        /// <param name="target">攻撃対象。</param>
        /// <param name="success">成功したか否か。</param>
        /// <returns></returns>
        public async UniTask Attack(BeatItem target, bool success)
        {
            _flashImage.gameObject.SetActive(true);
            if (success)
            {
                _flashImage.color = new Color(0.80f, 0.78f, 0.015f, 0.0f);
                await _flashImage.DOColor(new Color(0.90f, 0.88f, 0.15f, 0.7f), 0.05f);
                await UniTask.Delay(200);
                _remainingEnemies.Remove(target);
                if (_remainingEnemies.Count != 0)
                {
                    int index = UnityEngine.Random.Range(0, _remainingEnemies.Count);
                    var reinitializeEnemy = _remainingEnemies[index];
                    reinitializeEnemy.Initialize(CurrentBPM, BeatItem.AnimationType.Invalid);
                    reinitializeEnemy.SetSelectable(true);
                }
                await _flashImage.DOColor(new Color(0.90f, 0.88f, 0.15f, 0.0f), 0.05f);
            }
            else
            {
                _flashImage.color = new Color(0.8f, 0.0f, 0.0f, 0.0f);
                await _flashImage.DOColor(new Color(0.80f, 0.0f, 0.0f, 0.7f), 0.05f);
                await UniTask.Delay(200);
                ++_failCount;
                _failText.text = _failCount.ToString();
                await _flashImage.DOColor(new Color(0.80f, 0.0f, 0.0f, 0.0f), 0.05f);
            }
            _flashImage.gameObject.SetActive(false);
            UnityEngine.Debug.Log($"_remainingEnemies.Count = {_remainingEnemies.Count}");
        }

        /// <summary>
        /// ランキングなどを表示する
        /// </summary>
        /// <param name="token">キャンセルトークン。</param>
        /// <returns></returns>
        private async UniTask ShowRanking(CancellationToken token)
        {
            // 初期化
            _rankingButton.gameObject.SetActive(false);
            _tweetButton.gameObject.SetActive(false);
            _endButton.gameObject.SetActive(false);
            _gameEndButtonsRoot.SetActive(true);
            _playTimeText.text = "";
            _failCountText.text = "";
            _rankingTimeText.text = "";

            // 時間演出
            long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            long totalMilliseconds = elapsedMilliseconds + 5000 * _failCount;
            await _playTimeText.DOText($"遊戯時間：{elapsedMilliseconds / 60000:00}:{(elapsedMilliseconds / 1000) % 60:00}.{elapsedMilliseconds % 1000:000}", 0.5f);
            await _failCountText.DOText($"失敗補正：{_failCount}回 × 5秒", 0.5f);
            await _rankingTimeText.DOText($"合計時間：{totalMilliseconds / 60000:00}:{(totalMilliseconds / 1000) % 60:00}.{totalMilliseconds % 1000:000}", 0.5f);
            await UniTask.Delay(500, cancellationToken: token);

            // ランキング表示
            System.TimeSpan timeScore = new System.TimeSpan(0, 0, 0, 0, (int)totalMilliseconds);
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(timeScore);
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Submit, false);
            await UniTask.Delay(500, cancellationToken: token);

            // ランキングボタン設定
            _rankingButton.ResetScale();
            _rankingButton.ButtonEnabled = true;
            _rankingButton.gameObject.SetActive(true);
            _rankingButton.SetClickCallback(() =>
            {
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(timeScore);
                _rankingButton.ResetScale();
                _rankingButton.ButtonEnabled = true;
            });

            // ツイートボタン設定
            TweetData tweetData = new TweetData()
            {
                _url = "https://unityroom.com/games/sengokubeats",
                _message = $"湯煮亭を{totalMilliseconds / 60000:00}:{(totalMilliseconds / 1000) % 60:00}.{totalMilliseconds % 1000:000}で倒した",
                _hashTags = new string[] { "unity1week", "sengokubeats" },
            };
            _tweetButton.SetTweetData(tweetData);
            _tweetButton.gameObject.SetActive(true);

            // 終了ボタン設定
            bool end = false;
            _endButton.ResetScale();
            _endButton.ButtonEnabled = true;
            _endButton.gameObject.SetActive(true);
            _endButton.SetClickCallback(() => { end = true; });

            // 終了ボタンが押されたら終了
            await UniTask.WaitWhile(() => !end, cancellationToken: token);
            _rankingButton.SetClickCallback(null);
            _endButton.SetClickCallback(null);
            _gameEndButtonsRoot.SetActive(false);
        }

        #endregion
    }
}
