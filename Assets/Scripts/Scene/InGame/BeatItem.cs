using DG.Tweening;
using UnityEngine;

public class BeatItem : MonoBehaviour
{
    /// <summary>
    /// アニメーション種類。
    /// </summary>
    enum AnimationType
    {
        /// <summary>
        /// なし。
        /// </summary>
        None = 0,
        /// <summary>
        /// 時計回り。
        /// </summary>
        Clockwise,
        /// <summary>
        /// 反時計回り。
        /// </summary>
        CounterClockwise,
        /// <summary>
        /// 拡大縮小。
        /// </summary>
        Scale,
        /// <summary>
        /// X方向のフリップ。
        /// </summary>
        FlipX,
        /// <summary>
        /// Y方向のフリップ。
        /// </summary>
        FlipY,
        /// <summary>
        /// アルファ。
        /// </summary>
        Alpha,
    }

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
    [SerializeField]
    AnimationType _animationType = AnimationType.Clockwise;

    /// <summary>
    /// BPM(Beats Per Minute)。
    /// </summary>
    [SerializeField]
    int _bpm = 120;

    /// <summary>
    /// クリック可能か。
    /// </summary>
    [SerializeField]
    bool _clickable = true;

    /// <summary>
    /// 実行中の<see cref="Sequence"/>。
    /// </summary>
    Sequence _sequence;

    /// <inheritdoc/>
    void Start()
    {
        // TODO: 再生は外部から制御する
        Play();
    }

    /// <summary>
    /// 再生する。
    /// </summary>
    public void Play()
    {
        Stop();
        _sequence = DOTween.Sequence();

        switch (_animationType)
        {
        case AnimationType.Clockwise:
            _sequence.Append(_targetTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, -360.0f), 60.0f / _bpm, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
            break;
        case AnimationType.CounterClockwise:
            _sequence.Append(_targetTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, 360.0f), 60.0f / _bpm, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
            break;
        case AnimationType.Scale:
            _sequence.Append(_targetTransform.DOScale(1.25f, 60.0f / _bpm * 0.5f))
                .Append(_targetTransform.DOScale(1.0f, 60.0f / _bpm * 0.5f))
                .SetLoops(-1);
            break;
        case AnimationType.FlipX:
            _sequence.Append(_targetTransform.DOLocalRotate(new Vector3(0.0f, 360.0f, 0.0f), 60.0f / _bpm, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
            break;
        case AnimationType.FlipY:
            _sequence.Append(_targetTransform.DOLocalRotate(new Vector3(360.0f, 0.0f, 0.0f), 60.0f / _bpm, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLoops(-1);
            break;
        case AnimationType.Alpha:
            _sequence.Append(_targetSpriteRnderer.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), 60.0f / _bpm * 0.5f))
                .Append(_targetSpriteRnderer.DOColor(Color.white, 60.0f / _bpm * 0.5f))
                .SetLoops(-1);
            break;
        }
    }

    /// <summary>
    /// 停止する。
    /// </summary>
    public void Stop()
    {
        _sequence?.Kill();
        _sequence = null;
    }

    /// <summary>
    /// クリックイベント。
    /// </summary>
    public void OnClick()
    {
        if (!_clickable)
        {
            return;
        }
        Debug.Log(gameObject.name);
    }
}
