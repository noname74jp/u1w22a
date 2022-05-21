using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// <see cref="CanvasScaler"/>を更新する。
    /// </summary>
    public class CanvasScalerController : MonoBehaviour
    {
        /// <summary>
        /// 制御する<see cref="CanvasScaler"/>。
        /// </summary>
        [SerializeField]
        CanvasScaler[] _canvasScalers;

        /// <summary>
        /// 基準となる解像度。
        /// </summary>
        [SerializeField]
        Vector2 _referenceResolution;

        /// <inheritdoc/>
        void Update()
        {
            // キャンバスのマッチ設定（動的に画面が変わる可能性があるため毎フレーム設定する）
            float scale = (Screen.width * _referenceResolution.y >= Screen.height * _referenceResolution.x) ? 1.0f : 0.0f;
            foreach (CanvasScaler canvasScaler in _canvasScalers)
            {
                canvasScaler.matchWidthOrHeight = scale;
            }
        }
    }
}
