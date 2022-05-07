using UnityEngine;
using UnityEngine.UI;

namespace u1w22a
{
    /// <summary>
    /// 音量制御
    /// </summary>
    public class VolumeController : MonoBehaviour
    {
        /// <summary>
        /// ボリュームスライダー。
        /// </summary>
        [SerializeField]
        Slider _volumeSlider;

        /// <inheritdoc/>
        void Start()
        {
            _volumeSlider.onValueChanged.AddListener((value) =>
            {
                God.Instance.SoundManager.SetMasterVolume(value / _volumeSlider.maxValue);
            });
        }
    }
}
