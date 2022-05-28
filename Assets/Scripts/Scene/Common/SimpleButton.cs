using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// 単純な自作ボタン。
    /// </summary>
    public class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// アニメーションの周期。
        /// </summary>
        /// <remarks>85BPMの半分</remarks>
        const float dulation = 60.0f / 85.0f * 0.5f;

        /// <summary>
        /// アニメーション対象の<see cref="Transform"/>。
        /// </summary>
        [SerializeField]
        Transform _targetTransform;

        /// <summary>
        /// 入力判定用の<see cref="Graphic"/>。
        /// </summary>
        [SerializeField]
        Graphic _graphic;

        /// <summary>
        /// クリックコールバック。
        /// </summary>
        Action _onClickCallback { get; set; }

        /// <summary>
        /// 実行中の<see cref="Sequence"/>。
        /// </summary>
        Sequence _sequence;

        /// <summary>
        /// ボタンの有効状態。
        /// </summary>
        public bool ButtonEnabled
        {
            get => _graphic.raycastTarget;
            set => _graphic.raycastTarget = value;
        }

        /// <summary>
        /// 拡大率をリセットする。
        /// </summary>
        public void ResetScale()
        {
            _targetTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// コールバックを設定する。
        /// </summary>
        /// <param name="onClickCallback">クリック時のコールバック。</param>
        public void SetClickCallback(Action onClickCallback)
        {
            _onClickCallback = onClickCallback;
        }

        /// <inheritdoc/>
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_targetTransform.DOScale(1.1f, dulation));
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Select, false);
        }

        /// <inheritdoc/>
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            // raycastTargetをfalseにすると呼ばれる場合があるためボタンが有効かどうかを確認
            if (!ButtonEnabled)
            {
                return;
            }

            //
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_targetTransform.DOScale(1.0f, dulation));
        }

        /// <inheritdoc/>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_targetTransform.DOScale(1.25f, dulation))
                .Append(_targetTransform.DOScale(1.1f, dulation))
                .Append(_targetTransform.DOScale(1.2f, dulation))
                .Append(_targetTransform.DOScale(1.1f, dulation))
                .SetLoops(-1);
            _graphic.raycastTarget = false;
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Submit, false);
            _onClickCallback?.Invoke();
        }

        /// <inheritdoc/>
        void OnDisable()
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
