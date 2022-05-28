using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// ビートを刻むオブジェクト。
    /// </summary>
    public class BeatItem : MonoBehaviour
    {
        /// <summary>
        /// アニメーション種類。
        /// </summary>
        public enum AnimationType
        {
            /// <summary>
            /// 無効。
            /// </summary>
            Invalid = -1,
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

        /// <summary>
        /// 初期化する。
        /// </summary>
        /// <param name="bpm">BPM。</param>
        /// <param name="type">アニメーション種類。</param>
        public void Initialize(int bpm, AnimationType type)
        {
            _bpm = bpm;
            if (type != AnimationType.Invalid)
            {
                _animationType = type;
            }
            _graphic.raycastTarget = false;
            ResetAnimation();
        }

        /// <summary>
        /// 選択可能か否かを設定する。
        /// </summary>
        /// <param name="selectable">選択可能ならtrue、不可能ならfalse。</param>
        public void SetSelectable(bool selectable)
        {
            // raycastTargetで選択の可否を制御する
            _graphic.raycastTarget = selectable;
        }

        /// <summary>
        /// 表示するスプライトを設定する。
        /// </summary>
        /// <param name="sprite">表示するスプライト。</param>
        public void SetSprite(Sprite sprite)
        {
            _targetSpriteRnderer.sprite = sprite;
        }

        /// <inheritdoc/>
        void Update()
        {
            // 選択可能な敵はフラッシュ中は動かない(書き換えを見せないため)
            if (God.Instance.InGameScene.Attacking && _graphic.raycastTarget)
            {
                return;
            }

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

        /// <summary>
        /// レートからローカルスケールを設定する。
        /// </summary>
        /// <param name="rate">レート[0-1]。</param>
        void SetLocalScale(float rate)
        {
            float scale;
            if (rate < 0.25f)
            {
                // 基準のスケールから大きく拡大
                scale = 1.0f + 0.40f * (rate / 0.25f);
            }
            else if (rate < 0.5f)
            {
                // 基準のスケールへ縮小
                scale = 1.0f + 0.40f * ((0.5f - rate) / 0.25f);
            }
            else if (rate < 0.75f)
            {
                // 基準のスケールから小さく拡大
                scale = 1.0f + 0.20f * ((rate - 0.5f) / 0.25f);
            }
            else
            {
                // 基準のスケールへ縮小
                scale = 1.0f + 0.20f * ((1.0f - rate) / 0.25f);
            }
            _targetTransform.localScale = new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// レートからアルファ値を設定する。
        /// </summary>
        /// <param name="rate">レート[0-1]。</param>
        void SetAlpha(float rate)
        {
            float alpha;
            if (rate < 0.25f)
            {
                // 透明から不透明へ
                alpha = 0.0f + 1.0f * (rate / 0.25f);
            }
            else if (rate < 0.5f)
            {
                // 不透明から半透明へ
                alpha = 0.5f + 0.5f * ((0.5f - rate) / 0.25f);
            }
            else if (rate < 0.75f)
            {
                // 半透明から不透明へ
                alpha = 0.5f + 0.5f * ((rate - 0.5f) / 0.25f);
            }
            else
            {
                // 不透明から透明へ
                alpha = 0.0f + 1.0f * ((1.0f - rate) / 0.25f);
            }
            _targetSpriteRnderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }

        /// <summary>
        /// クリックイベント。
        /// </summary>
        public void OnClick()
        {
            // raycastTargetがfalseならクリック不可
            if (!_graphic.raycastTarget)
            {
                return;
            }

            // BPMが一致なら倒す
            if (_bpm == God.Instance.InGameScene.CurrentBPM)
            {
                ResetAnimation();
                _animationType = AnimationType.None;
                _graphic.raycastTarget = false;
                gameObject.SetActive(false);
                God.Instance.SoundManager.PlaySe(-1, SoundSeId.AttackOK, false);
                God.Instance.InGameScene.Attack(this, true).Forget();
            }
            // BPMが不一致なら演出のみで倒さない
            else
            {
                God.Instance.SoundManager.PlaySe(-1, SoundSeId.AttackNG, false);
                God.Instance.InGameScene.Attack(this, false).Forget();
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
