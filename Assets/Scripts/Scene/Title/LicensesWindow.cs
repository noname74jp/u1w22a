using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace u1w22a
{
    /// <summary>
    /// ライセンス表示ウィンドウ。
    /// </summary>
    public class LicensesWindow : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// クリック時のイベントコールバック。
        /// </summary>
        Action _onClickCallback;

        /// <summary>
        /// ウィンドウを開く。
        /// </summary>
        /// <param name="onClickCallback">クリック時のコールバック。</param>
        /// <returns></returns>
        public async UniTask OpenAsync(Action onClickCallback)
        {
            _onClickCallback = onClickCallback;
            gameObject.SetActive(true);
            await UniTask.Yield();
        }

        /// <summary>
        /// ウィンドウを閉じる。
        /// </summary>
        public async UniTask CloseAsync()
        {
            await UniTask.Yield();
            gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _onClickCallback?.Invoke();
        }
    }
}
