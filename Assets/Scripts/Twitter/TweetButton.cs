#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace u1w22a
{
    /// <summary>
    /// ポップアップブロックを回避するツイートボタン。
    /// </summary>
    /// <remarks>
    /// 1. ボタンにこのコンポーネントを設定する。
    /// 2. ボタンを有効にするタイミングで<see cref="SetTweetData(TweetData)"/>を呼び出し。
    /// 3. ボタン押下時に<see cref="TweetFromUnity"/>を呼び出し。
    /// </remarks>
    public class TweetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /// <summary>
        /// アニメーションの間隔。
        /// </summary>
        const float dulation = 60.0f / 85.0f * 0.5f; // 85BPMの半分

        /// <summary>
        /// アニメーション対象の<see cref="Transform"/>。
        /// </summary>
        [SerializeField]
        Transform _targetTransform;

        /// <summary>
        /// 実行中の<see cref="Sequence"/>。
        /// </summary>
        Sequence _sequence = null;

        /// <summary>
        /// ツイートする生メッセージ。
        /// </summary>
        private string _rawMessage = string.Empty;

        /// <summary>
        /// ツイートするクエリー文字列。
        /// </summary>
        private string queryString = string.Empty;

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        extern private static void EnableTweetEvent(string rawMessage);
        [DllImport("__Internal")]
        extern private static void DisableTweetEvent();
        [DllImport("__Internal")]
        extern public static void TweetFromUnity(string rawMessage);
#else
        private static void EnableTweetEvent(string rawMessage) { }
        private static void DisableTweetEvent() { }
        public static void TweetFromUnity(string rawMessage) { }
#endif

        /// <inheritdoc/>
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            EnableTweetEvent(queryString);
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_targetTransform.DOScale(1.1f, dulation));
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Select, false);
        }

        /// <inheritdoc/>
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            DisableTweetEvent();
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_targetTransform.DOScale(1.0f, dulation));
        }

        /// <inheritdoc/>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            TweetFromUnity(_rawMessage);
        }

        /// <summary>
        /// ツイートするデータを設定する。
        /// </summary>
        /// <param name="data">ツイートするデータ。</param>
        public void SetTweetData(TweetData data)
        {
            _rawMessage = data.GenerateRawMessage();
            queryString = data.GenerateQueryString();
        }

        /// <inheritdoc/>
        private void OnDisable()
        {
            KillSequence();
        }

        /// <summary>
        /// シーケンスを止める。
        /// </summary>
        public void KillSequence()
        {
            _sequence?.Kill();
            _sequence = null;
        }
    }
}
