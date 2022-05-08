using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace u1w22a
{
    public class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
        /// 入力判定用の<see cref="Graphic"/>。
        /// </summary>
        [SerializeField]
        Graphic _graphic;

        /// <summary>
        /// クリックコールバック。
        /// </summary>
        private Action _onClickCallback { get; set; }

        /// <summary>
        /// 実行中の<see cref="Sequence"/>。
        /// </summary>
        Sequence _sequence = null;

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
            _onClickCallback?.Invoke();
            _graphic.raycastTarget = false;
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Submit, false);
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
