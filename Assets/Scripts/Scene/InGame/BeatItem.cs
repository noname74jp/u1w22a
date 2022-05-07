using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    public class BeatItem : MonoBehaviour
    {
        /// <summary>
        /// アニメーション種類。
        /// </summary>
        public enum AnimationType
        {
            /// <summary>
            /// なし。
            /// </summary>
            None = 0,
            /// <summary>
            /// 拡大縮小。
            /// </summary>
            Scale,
            /// <summary>
            /// アルファ。
            /// </summary>
            Alpha,
        }

        /// <summary>
        /// 入力判定用の<see cref="Graphic"/>。
        /// </summary>
        [SerializeField]
        Graphic _graphic;

        /// <summary>
        /// アニメーション対象の<see cref="Transform"/>。
        /// </summary>
        [SerializeField]
        Transform _targetTransform;

        /// <summary>
        /// アニメーション対象の<see cref="Transform"/>。
        /// </summary>
        [SerializeField]
        SpriteRenderer _targetSpriteRnderer;

        /// <summary>
        /// アニメーション種類。
        /// </summary>
        AnimationType _animationType = AnimationType.Scale;

        /// <summary>
        /// BPM(Beats Per Minute)。
        /// </summary>
        int _bpm = 120;

        public void Initialize(int bpm, AnimationType type, bool selectable = true)
        {
            _bpm = bpm;
            _animationType = type;
            _graphic.raycastTarget = selectable;
        }

        /// <inheritdoc/>
        void Update()
        {
            float barDurationMilliseconds = 4.0f * 60000.0f / _bpm; // 4拍1小節の時間
            float rate = (God.Instance.BeatTimer.BeatTimeMilliseconds % barDurationMilliseconds) / barDurationMilliseconds;
            switch (_animationType)
            {
            case AnimationType.None:
                break;
            case AnimationType.Scale:
                SetLocalScale(rate);
                break;
            case AnimationType.Alpha:
                SetAlpha(rate);
                break;
            }
        }

        void SetLocalScale(float rate)
        {
            float scale;
            if (rate < 0.25f)
            {
                scale = 1.0f + 0.25f * (rate / 0.25f);
            }
            else if (rate < 0.5f)
            {
                scale = 1.0f + 0.25f * ((0.5f - rate) / 0.25f);
            }
            else if (rate < 0.75f)
            {
                scale = 1.0f + 0.15f * ((rate - 0.5f) / 0.25f);
            }
            else
            {
                scale = 1.0f + 0.15f * ((1.0f - rate) / 0.25f);
            }
            _targetTransform.localScale = new Vector3(scale, scale, scale);
        }

        void SetAlpha(float rate)
        {
            float alpha;
            if (rate < 0.25f)
            {
                alpha = 0.0f + 1.0f * (rate / 0.25f);
            }
            else if (rate < 0.5f)
            {
                alpha = 0.5f + 0.5f * ((0.5f - rate) / 0.25f);
            }
            else if (rate < 0.75f)
            {
                alpha = 0.5f + 0.5f * ((rate - 0.5f) / 0.25f);
            }
            else
            {
                alpha = 0.0f + 1.0f * ((1.0f - rate) / 0.25f);
            }
            _targetSpriteRnderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }

        /// <summary>
        /// クリックイベント。
        /// </summary>
        public void OnClick()
        {
            if (!_graphic.raycastTarget)
            {
                return;
            }
            Debug.Log(gameObject.name + " : " + _bpm);
            if (_bpm == 128)
            {
                ResetAnimation();
                _animationType = AnimationType.None;
                _graphic.raycastTarget = false;
            }
        }

        /// <summary>
        /// アニメーションをリセットする。
        /// </summary>
        void ResetAnimation()
        {
            _targetTransform.localScale = Vector3.one;
            _targetSpriteRnderer.color = Color.white;
        }
    }
}
