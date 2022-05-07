using System.Collections.Generic;

namespace u1w22a
{
    /// <summary>
    /// ウェーブのコマンド種類。
    /// </summary>
    public enum WaveCommandType
    {
        Begin,
        PlayBgm,
        SamuraiEnter,
        SamuraiExit,
        WaveMessage,
        End,
    }

    /// <summary>
    /// ウェーブのコマンド。
    /// </summary>
    public struct WaveCommand
    {
        /// <summary>
        /// ウェーブのコマンド種類。
        /// </summary>
        public readonly WaveCommandType CommandType { get; }

        /// <summary>
        /// 固有値1。
        /// </summary>
        public readonly int Value1 { get; }

        /// <summary>
        /// 固有値1。
        /// </summary>
        public readonly int Value2 { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="commandType">ウェーブのコマンド種類。</param>
        /// <param name="value1">固有値1。</param>
        /// <param name="value1">固有値2。</param>
        public WaveCommand(WaveCommandType commandType, int value1, int value2)
        {
            CommandType = commandType;
            Value1 = value1;
            Value2 = value2;
        }
    }

    /// <summary>
    /// ウェーブのコマンドリストの定義。
    /// </summary>
    /// <remarks>外部データ化するのが本来は正しい。</remarks>
    public static class WaveCommands
    {
        private static readonly IReadOnlyList<WaveCommand> TutorialWaveCommands = new WaveCommand[]
        {
            new(WaveCommandType.Begin, 0, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.Title85BPM, 85),
            new(WaveCommandType.WaveMessage, 100000, 0),
            new(WaveCommandType.WaveMessage, 100010, 0),
            new(WaveCommandType.SamuraiEnter, 0, 0),
            new(WaveCommandType.WaveMessage, 101000, 0),
            new(WaveCommandType.WaveMessage, 101010, 0),
            new(WaveCommandType.WaveMessage, 101020, 0),

            new(WaveCommandType.WaveMessage, 102000, 0),
            new(WaveCommandType.WaveMessage, 102010, 0),
            new(WaveCommandType.WaveMessage, 102020, 0),

            new(WaveCommandType.SamuraiExit, 0, 0),
            new(WaveCommandType.End, 0, 0),
        };
        private static readonly IReadOnlyList<WaveCommand> InGameWaveCommands = new WaveCommand[]
        {
            new(WaveCommandType.Begin, 0, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame128BPM, 128),
            new(WaveCommandType.SamuraiEnter, 0, 0),
            new(WaveCommandType.WaveMessage, 200000, 0),
            new(WaveCommandType.SamuraiExit, 0, 0),
            new(WaveCommandType.End, 0, 0),
        };

        public static IReadOnlyList<WaveCommand> GetWaveCommands(bool tutorial)
        {
            return tutorial ? TutorialWaveCommands : InGameWaveCommands;
        }
    }

    /// <summary>
    /// ウェーブのメッセージの定義。
    /// </summary>
    /// <remarks>外部データ化するのが本来は正しい。</remarks>
    public static class WaveMessages
    {
        private static readonly IReadOnlyDictionary<int, string> messageMap = new Dictionary<int, string>()
        {
            // 100000: チュートリアル
            { 100000, "プレイしていただきありがとうございます。" },
            { 100010, "これからこのゲームの遊び方をご説明します。" },
            { 101000, "いま画面左から駆けてきた若武者こそが本作の主人公、" },
            { 101010, "昇鯉　辰雄（のぼりごい　たつお）です。" },
            { 101020, "彼の体は一定の拍子で拡大縮小しています。" },
            { 102000, "画面右から曲者が襲いかかってきました。" },
            { 102010, "曲者も昇鯉と同じ拍子で拡大縮小しています。" },
            { 102020, "曲者をクリックしましょう。" },
            { 103000, "今度は曲者がふたり襲いかかってきました。" },
            { 103010, "昇鯉と同じ拍子の曲者をクリックしましょう。" },
            // 200000: インゲーム
            { 200000, "あの父上が、殿を弑し奉ったという。"},
            { 200010, "何かの間違いであろうとは思うが、しかし……"},
        };

        public static string GetMessage(int messageID)
        {
            if (messageMap.TryGetValue(messageID, out string message))
            {
                return message;
            }
            return string.Empty;
        }
    }
}
