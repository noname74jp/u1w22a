using System.Diagnostics;
using UnityEngine;

namespace u1w22a
{
    /// <summary>
    /// 拍カウント用のタイマー。
    /// </summary>
    public class BeatTimer : MonoBehaviour
    {
        /// <summary>
        /// ループするミリ秒。
        /// </summary>
        public const long LoopMilliseconds = 60 * 4 * 1000;

        /// <summary>
        /// 拍カウント用の時間計測用。
        /// </summary>
        readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// 拍カウント用の時間。
        /// </summary>
        public int BeatTimeMilliseconds { get; private set; }

        /// <inheritdoc/>
        void Awake()
        {
            // 動かす必要はないが、動いていない状態を無くしたいので動かす
            ResetTimer();
        }

        /// <inheritdoc/>
        void Update()
        {
            BeatTimeMilliseconds = (int)(stopwatch.ElapsedMilliseconds % LoopMilliseconds);
        }

        /// <summary>
        /// タイマーをリセットする。
        /// </summary>
        public void ResetTimer()
        {
            stopwatch.Reset();
            stopwatch.Start();
            BeatTimeMilliseconds = 0;
        }
    }
}
