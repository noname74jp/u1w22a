using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// ウェーブのメッセージ表示。
    /// </summary>
    public class WaveMessage : MonoBehaviour
    {
        /// <summary>
        /// メッセージのルートオブジェクト。
        /// </summary>
        [SerializeField]
        GameObject _root;

        /// <summary>
        /// テキスト。
        /// </summary>
        [SerializeField]
        Text _text;

        /// <inheritdoc/>
        void OnEnable()
        {
            _root.SetActive(false);
        }

        public async UniTask DisplayMessage(string message, CancellationToken token)
        {
            _root.SetActive(true);
            _text.text = string.Empty;
            await _text.DOText(message, message.Length / 60.0f)
                .SetEase(Ease.Linear);
            await UniTask.WaitWhile(() => !Input.GetMouseButtonDown(0), cancellationToken: token);
            God.Instance.SoundManager.PlaySe(-1, SoundSeId.Click, false);
            await UniTask.Yield();
            _root.SetActive(false);
        }
    }
}
