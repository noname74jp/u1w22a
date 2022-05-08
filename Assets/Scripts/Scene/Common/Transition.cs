using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// 画像を使用した単純なトランジション。
    /// </summary>
    public class Transition : MonoBehaviour
    {
        /// <summary>
        /// 画像。
        /// </summary>
        [SerializeField]
        private Image _image;

        /// <summary>
        /// 標準のフェード時間。
        /// </summary>
        [SerializeField]
        private float _defaultFadeTime = 0.5f;

        /// <summary>
        /// タイムスケールが有効か否か。
        /// </summary>
        [SerializeField]
        private bool _enableTimeScale = false;

        /// <summary>
        /// キャンセル処理用。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <inheritdoc/>
        protected void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// 表示するスプライトを設定する。
        /// </summary>
        /// <param name="sprite">表示するスプライト。</param>
        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        /// <summary>
        /// フェードイン。
        /// </summary>
        /// <param name="fadeTime">フェード時間。NaNなら標準のフェード時間が指定される。</param>
        /// <returns></returns>
        public async UniTask FadeIn(float fadeTime = float.NaN)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await Fade(fadeTime, 1.0f, 0.0f, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// フェードアウト。
        /// </summary>
        /// <param name="fadeTime">フェード時間。NaNなら標準のフェード時間が指定される。</param>
        public async UniTask FadeOut(float fadeTime = float.NaN)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await Fade(fadeTime, 0.0f, 1.0f, _cancellationTokenSource.Token);
        }

        /// <summary>
        /// フェード処理を行う。
        /// </summary>
        /// <param name="fadeTime">フェード時間。NaNなら標準のフェード時間が指定される。</param>
        /// <param name="beginAlpha">開始時のアルファ値。</param>
        /// <param name="endAlpha">終了時のアルファ値。</param>
        /// <param name="cancellationToken">キャンセルトークン。</param>
        /// <returns></returns>
        private async UniTask Fade(float fadeTime, float beginAlpha, float endAlpha, CancellationToken cancellationToken)
        {
            // 初期状態を設定
            if (float.IsNaN(fadeTime))
            {
                fadeTime = _defaultFadeTime;
            }
            float pastTime = 0;
            Color color = new Color(_image.color.r, _image.color.g, _image.color.b, beginAlpha);
            _image.color = color;
            _image.enabled = true;

            // アルファ値を毎フレーム更新
            while (pastTime < fadeTime)
            {
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, cancellationToken);
                pastTime += _enableTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
                color.a = Mathf.Lerp(beginAlpha, endAlpha, pastTime / fadeTime);
                _image.color = color;
            }

            // アルファ値が0なら描画処理が無駄なので表示しない
            _image.enabled = endAlpha != 0.0f;
        }
    }
}
