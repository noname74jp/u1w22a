using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace u1w22a
{
    /// <summary>
    /// BGMのID。
    /// </summary>
    public enum SoundBgmId
    {
        /// <summary>
        /// タイトル。
        /// </summary>
        Title85BPM,
        /// <summary>
        /// インゲーム85BPM。
        /// </summary>
        InGame85BPM,
        /// <summary>
        /// インゲーム128BPM。
        /// </summary>
        InGame128BPM,
        /// <summary>
        /// インゲーム150BPM。
        /// </summary>
        InGame150BPM,
    }

    /// <summary>
    /// SEのID。
    /// </summary>
    public enum SoundSeId
    {
    }

    /// <summary>
    /// ボイスのID。
    /// </summary>
    public enum SoundVoiceId
    {
    }

    /// <summary>
    /// サウンド管理。
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// オーディオミキサー。
        /// </summary>
        [SerializeField]
        AudioMixer _mixer;

        /// <summary>
        /// BGMの<see cref="AudioSource"/>のルート。
        /// </summary>
        [SerializeField]
        Transform _bgmSourcesRoot;

        /// <summary>
        /// SEの<see cref="AudioSource"/>のルート。
        /// </summary>
        [SerializeField]
        Transform _seSourcesRoot;

        /// <summary>
        /// ボイスの<see cref="AudioSource"/>のルート。
        /// </summary>
        [SerializeField]
        Transform _voiceSourcesRoot;

        /// <summary>
        /// BGMの<see cref="AudioClip"/>のリスト。
        /// </summary>
        [SerializeField]
        List<AudioClip> _bgmAudioClips;

        /// <summary>
        /// SEの<see cref="AudioClip"/>のリスト。
        /// </summary>
        [SerializeField]
        List<AudioClip> _seAudioClips;

        /// <summary>
        /// ボイスの<see cref="AudioClip"/>のリスト。
        /// </summary>
        [SerializeField]
        List<AudioClip> _voiceAudioClips;

        /// <summary>
        /// BGMの<see cref="AudioSource"/>。
        /// </summary>
        List<AudioSource> _bgmSources = new List<AudioSource>();

        /// <summary>
        /// SEの<see cref="AudioSource"/>。
        /// </summary>
        List<AudioSource> _seSources = new List<AudioSource>();

        /// <summary>
        /// ボイスの<see cref="AudioSource"/>。
        /// </summary>
        List<AudioSource> _voiceSources = new List<AudioSource>();

        /// <inheritdoc/>
        protected void Awake()
        {
            // BGMのAudioSource取得
            for (int i = 0; i < _bgmSourcesRoot.childCount; ++i)
            {
                _bgmSources.Add(_bgmSourcesRoot.GetChild(i).GetComponent<AudioSource>());
            }

            // SEのAudioSource取得
            for (int i = 0; i < _seSourcesRoot.childCount; ++i)
            {
                _seSources.Add(_seSourcesRoot.GetChild(i).GetComponent<AudioSource>());
            }

            // ボイスのAudioSource取得
            for (int i = 0; i < _voiceSourcesRoot.childCount; ++i)
            {
                _voiceSources.Add(_voiceSourcesRoot.GetChild(i).GetComponent<AudioSource>());
            }
        }

        /// <summary>
        /// マスターボリュームを取得する。
        /// </summary>
        /// <returns>ボリューム(0.0〜1.0)。</returns>
        public float GetMasterVolume()
        {
            return GetVolume("MasterVolume");
        }

        /// <summary>
        /// BGMボリュームを取得する。
        /// </summary>
        /// <returns>ボリューム(0.0〜1.0)。</returns>
        public float GetBgmVolume()
        {
            return GetVolume("BgmVolume");
        }

        /// <summary>
        /// SEボリュームを取得する。
        /// </summary>
        /// <returns>ボリューム(0.0〜1.0)。</returns>
        public float GetSeVolume()
        {
            return GetVolume("SeVolume");
        }

        /// <summary>
        /// ボリュームを取得する。
        /// </summary>
        /// <returns>ボリューム(0.0〜1.0)。</returns>
        float GetVolume(string label)
        {
            _mixer.GetFloat(label, out float dB);
            float volume = ConvertDBToVolume(dB);
            return volume;
        }

        /// <summary>
        /// dbをボリュームに変換する。
        /// </summary>
        /// <param name="dB">dB(-80〜0)。</param>
        /// <returns>ボリューム(0.0〜1.0)。</returns>
        float ConvertDBToVolume(float dB)
        {
            return Mathf.Clamp01(Mathf.Pow(10.0f, dB / 20.0f));
        }

        /// <summary>
        /// マスターボリュームを設定する。
        /// </summary>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        public void SetMasterVolume(float volume)
        {
            SetVolume("MasterVolume", volume);
        }

        /// <summary>
        /// BGMのボリュームを設定する。
        /// </summary>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        public void SetBgmVolume(float volume)
        {
            SetVolume("BgmVolume", volume);
        }

        /// <summary>
        /// SEのボリュームを設定する。
        /// </summary>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        public void SetSeVolume(float volume)
        {
            SetVolume("SeVolume", volume);
        }

        /// <summary>
        /// ボイスのボリュームを設定する。
        /// </summary>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        public void SetVoiceVolume(float volume)
        {
            SetVolume("VoiceVolume", volume);
        }

        /// <summary>
        /// ボリュームを設定する。
        /// </summary>
        /// <param name="label">設定するラベル</param>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        void SetVolume(string label, float volume)
        {
            float dB = ConvertVolumeToDB(volume);
            _mixer.SetFloat(label, dB);
        }

        /// <summary>
        /// ボリュームをdbに変換する。
        /// </summary>
        /// <param name="volume">ボリューム(0.0〜1.0)。</param>
        /// <returns>dB(-80〜0)。</returns>
        float ConvertVolumeToDB(float volume)
        {
            return Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp01(volume)), -80f, 0f);
        }

        /// <summary>
        /// BGMを再生する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。-1で空きトラックを自動検索。</param>
        /// <param name="id">再生するBGMのID。</param>
        /// <param name="loop">ループするか否か。</param>
        /// <param name="pan">パン。</param>
        /// <param name="pan">ピッチ。</param>
        /// <returns>再生トラックのインデックス</returns>
        public int PlayBgm(int trackIndex, SoundBgmId id, bool loop, float pan = 0.0f, float pitch = 1.0f)
        {
            AudioSource audioSource = trackIndex >= 0 ? _bgmSources[trackIndex] : _bgmSources.FirstOrDefault(x => !x.isPlaying);
            AudioClip audioClip = _bgmAudioClips[(int)id];
            Play(audioSource, audioClip, loop, pan, pitch);
            return trackIndex;
        }

        /// <summary>
        /// SEを再生する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。-1で空きトラックを自動検索。</param>
        /// <param name="id">再生するSEのID。</param>
        /// <param name="loop">ループするか否か。</param>
        /// <param name="pan">パン。</param>
        /// <param name="pan">ピッチ。</param>
        /// <returns>再生トラックのインデックス</returns>
        public int PlaySe(int trackIndex, SoundSeId id, bool loop, float pan = 0.0f, float pitch = 1.0f)
        {
            AudioSource audioSource = trackIndex >= 0 ? _seSources[trackIndex] : _seSources.FirstOrDefault(x => !x.isPlaying);
            AudioClip audioClip = _seAudioClips[(int)id];
            Play(audioSource, audioClip, loop, pan, pitch);
            return trackIndex;
        }

        /// <summary>
        /// ボイスを再生する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。-1で空きトラックを自動検索。</param>
        /// <param name="id">再生するSEのID。</param>
        /// <param name="loop">ループするか否か。</param>
        /// <param name="pan">パン。</param>
        /// <param name="pan">ピッチ。</param>
        /// <returns>再生トラックのインデックス</returns>
        public int PlayVoice(int trackIndex, SoundVoiceId id, bool loop, float pan = 0.0f, float pitch = 1.0f)
        {
            AudioSource audioSource = trackIndex >= 0 ? _voiceSources[trackIndex] : _voiceSources.FirstOrDefault(x => !x.isPlaying);
            AudioClip audioClip = _voiceAudioClips[(int)id];
            Play(audioSource, audioClip, loop, pan, pitch);
            return trackIndex;
        }

        /// <summary>
        /// <see cref="AudioSource"/>で<see cref="AudioClip"/>を再生する。
        /// </summary>
        /// <param name="audioSource">再生に使用する<see cref="AudioSource"/>。</param>
        /// <param name="audioClip">再生する<see cref="AudioClip"/>。</param>
        /// <param name="loop">ループするか否か。</param>
        /// <param name="pan">パン。</param>
        /// <param name="pan">ピッチ。</param>
        void Play(AudioSource audioSource, AudioClip audioClip, bool loop, float pan, float pitch)
        {
            // 引数がnullなら何もしない
            if (audioSource == null || audioClip == null)
            {
                return;
            }

            // 再生中なら停止
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // 再生
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.panStereo = pan;
            audioSource.pitch = pitch;
            audioSource.volume = 1.0f;
            audioSource.Play();
        }

        /// <summary>
        /// BGMをフェードアウトする。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        /// <param name="duration">フェード時間。</param>
        public void FadeOutBgm(int trackIndex, float duration = 0.5f)
        {
            AudioSource audioSource = _bgmSources[trackIndex];
            if (audioSource.isPlaying)
            {
                audioSource.DOFade(0.0f, duration);
            }
        }

        /// <summary>
        /// BGMを停止する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        public void StopBgm(int trackIndex)
        {
            AudioSource audioSource = _bgmSources[trackIndex];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 全てのBGMを停止する。
        /// </summary>
        public void StopAllBgm()
        {
            foreach (AudioSource audioSource in _bgmSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        /// <summary>
        /// SEを停止する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        public void StopSe(int trackIndex)
        {
            AudioSource audioSource = _seSources[trackIndex];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 全てのSEを停止する。
        /// </summary>
        public void StopAllSe()
        {
            foreach (AudioSource audioSource in _seSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        /// <summary>
        /// ボイスを停止する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        public void StopVoice(int trackIndex)
        {
            AudioSource audioSource = _voiceSources[trackIndex];
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 全てのボイスを停止する。
        /// </summary>
        public void StopAllVoice()
        {
            foreach (AudioSource audioSource in _voiceSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        /// <summary>
        /// 全てのサウンドを停止する。
        /// </summary>
        public void StopAllSound()
        {
            StopAllBgm();
            StopAllSe();
            StopAllVoice();
        }

        /// <summary>
        /// BGMが再生中か否かを取得する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        /// <returns>再生中ならtrue、停止中ならfalse。</returns>
        public bool IsPlayingBgm(int trackIndex)
        {
            AudioSource audioSource = _bgmSources[trackIndex];
            return audioSource.isPlaying;
        }

        /// <summary>
        /// SEが再生中か否かを取得する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        /// <returns>再生中ならtrue、停止中ならfalse。</returns>
        public bool IsPlayingSe(int trackIndex)
        {
            AudioSource audioSource = _seSources[trackIndex];
            return audioSource.isPlaying;
        }

        /// <summary>
        /// ボイスが再生中か否かを取得する。
        /// </summary>
        /// <param name="trackIndex">再生トラックのインデックス。</param>
        /// <returns>再生中ならtrue、停止中ならfalse。</returns>
        public bool IsPlayingVoice(int trackIndex)
        {
            AudioSource audioSource = _voiceSources[trackIndex];
            return audioSource.isPlaying;
        }
    }
}
