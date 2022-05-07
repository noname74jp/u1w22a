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
        EnemyEnter,
        EnemyBeat,
        WaveMessage,
        End,
    }

    /// <summary>
    /// 敵種類。
    /// </summary>
    public enum EnemyType
    {
        Father,
        Brother,
        XinobiBlack,
        XinobiWhite,
        SamuraiLow,
        Rohnin,
        SamuraiHigh,
    }

    /// <summary>
    /// 敵出現種類。
    /// </summary>
    public enum EnemyAppearType
    {
        Move,
        Fade,
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
        /// 固有値2。
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
    /// 敵パラメーター。
    /// </summary>
    public struct EnemyParam
    {
        /// <summary>
        /// 敵種類。
        /// </summary>
        public readonly EnemyType EnemyType { get; }

        /// <summary>
        /// 敵出現種類。
        /// </summary>
        public readonly EnemyAppearType EnemyAppearType { get; }

        /// <summary>
        /// x座標。
        /// </summary>
        public readonly int X { get; }

        /// <summary>
        /// y座標。
        /// </summary>
        public readonly int Y { get; }

        /// <summary>
        /// アニメーション種類。
        /// </summary>
        public readonly BeatItem.AnimationType AnimationType { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="enemyType">ウェーブのコマンド種類。</param>
        /// <param name="x">x座標(1/64にした値)。</param>
        /// <param name="y">y座標(1/64にした値)。</param>
        public EnemyParam(EnemyType enemyType, EnemyAppearType enemyAppearType, int x, int y)
        {
            EnemyType = enemyType;
            EnemyAppearType = enemyAppearType;
            X = x * 64;
            Y = y * 64;
            switch (enemyType)
            {
            case EnemyType.XinobiBlack:
                AnimationType = BeatItem.AnimationType.Alpha;
                break;
            default:
                AnimationType = BeatItem.AnimationType.Scale;
                break;
            }
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
            // プレイしていただきありがとうございます。
            new(WaveCommandType.Begin, 0, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.Title85BPM, 85),
            new(WaveCommandType.WaveMessage, 100000, 0),
            new(WaveCommandType.WaveMessage, 100010, 0),

            // いま画面左から駆けてきた若武者こそが本作の主人公、
            new(WaveCommandType.SamuraiEnter, 0, 0),
            new(WaveCommandType.WaveMessage, 101000, 0),
            new(WaveCommandType.WaveMessage, 101010, 0),

            // 画面右から曲者が襲いかかってきました。
            new(WaveCommandType.EnemyEnter, 102000, 0),
            new(WaveCommandType.WaveMessage, 102000, 0),
            new(WaveCommandType.WaveMessage, 102010, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.WaveMessage, 102020, 0),

            // 今度は曲者がふたり襲いかかってきました。
            new(WaveCommandType.EnemyEnter, 103000, 20),
            new(WaveCommandType.WaveMessage, 103000, 0),
            new(WaveCommandType.WaveMessage, 103010, 0),
            new(WaveCommandType.WaveMessage, 103020, 0),
            new(WaveCommandType.WaveMessage, 103030, 0),
            new(WaveCommandType.EnemyBeat, 1, 0),
            new(WaveCommandType.WaveMessage, 103040, 0),
            new(WaveCommandType.WaveMessage, 103050, 0),
            new(WaveCommandType.WaveMessage, 103060, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.WaveMessage, 103070, 0),

            // 曲者の人数が増えても同じ要領で倒していきます。
            new(WaveCommandType.EnemyEnter, 104000, 15),
            new(WaveCommandType.WaveMessage, 104000, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // 忍者のテンポの取り方は他の敵と少し違います。
            new(WaveCommandType.EnemyEnter, 105000, 15),
            new(WaveCommandType.WaveMessage, 105000, 0),
            new(WaveCommandType.WaveMessage, 105010, 0),
            new(WaveCommandType.WaveMessage, 105020, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // これで遊戯指南は終了です。
            new(WaveCommandType.WaveMessage, 106000, 0),
            new(WaveCommandType.SamuraiExit, 0, 0),
            new(WaveCommandType.End, 0, 0),
        };
        private static readonly IReadOnlyList<WaveCommand> InGameWaveCommands = new WaveCommand[]
        {
            new(WaveCommandType.Begin, 0, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame128BPM, 128),
            new(WaveCommandType.SamuraiEnter, 0, 0),
            new(WaveCommandType.WaveMessage, 200000, 0),
            //
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame150BPM, 150),
            new(WaveCommandType.WaveMessage, 200000, 0),
            //
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame85BPM, 85),
            new(WaveCommandType.WaveMessage, 200000, 0),
            //
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
            { 102000, "画面右から曲者が襲いかかってきました。" },
            { 102010, "曲者をクリックしましょう。" },
            { 102020, "お見事です。" },
            { 103000, "今度は曲者がふたり襲いかかってきました。" },
            { 103010, "昇鯉は同じテンポの敵は一撃で倒せますが、" },
            { 103020, "テンポの異なる敵は倒すことができません。" },
            { 103030, "昇鯉と同じテンポの曲者をクリックしましょう。" },
            { 103040, "昇鯉があざやかに曲者を倒すと" },
            { 103050, "ほかの曲者が昇鯉と同じテンポに変化します。" },
            { 103060, "もうひとりも倒してしまいましょう。" },
            { 103070, "お見事です。" },
            { 104000, "曲者の人数が増えても同じ要領で倒していきます。" },
            { 105000, "忍者のテンポの取り方は他の敵と少し違います。" },
            { 105010, "汚いなさすが忍者きたない。" },
            { 105020, "でも倒し方は同じです。テンポを見切りましょう。" },
            { 106000, "これで遊戯指南は終了です。" },
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

    /// <summary>
    /// ウェーブの敵の定義。
    /// </summary>
    /// <remarks>外部データ化するのが本来は正しい。</remarks>
    public static class WaveEnemies
    {
        private static readonly IReadOnlyDictionary<int, IReadOnlyList<EnemyParam>> enemySetMap = new Dictionary<int, IReadOnlyList<EnemyParam>>()
        {
            // 100000: チュートリアル
            { 102000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 4, 0),
                }
            },
            { 103000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3, 0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5, 0),
                }
            },
            { 104000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5,  0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3,  2),
                }
            },
            { 105000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Fade, 3, -2),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Fade, 5,  0),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Fade, 3,  2),
                }
            },
        };

        public static IReadOnlyList<EnemyParam> GetEnemies(int enemySetID)
        {
            if (enemySetMap.TryGetValue(enemySetID, out IReadOnlyList<EnemyParam> enemies))
            {
                return enemies;
            }
            return new EnemyParam[0];
        }
    }
}
