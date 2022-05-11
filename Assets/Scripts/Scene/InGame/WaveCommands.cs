#define ENABLE_WAVE1
#define ENABLE_WAVE2
#define ENABLE_WAVE3

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
        Ranking,
        End,
    }

    /// <summary>
    /// 敵種類。
    /// </summary>
    public enum EnemyType
    {
        Unity,
        Father,
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
            case EnemyType.XinobiWhite:
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
            new(WaveCommandType.EnemyEnter, 103000, 30),
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
            new(WaveCommandType.EnemyEnter, 104000, 25),
            new(WaveCommandType.WaveMessage, 104000, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // 忍者のテンポの取り方は他の敵と少し違います。
            new(WaveCommandType.EnemyEnter, 105000, 25),
            new(WaveCommandType.WaveMessage, 105000, 0),
            new(WaveCommandType.WaveMessage, 105010, 0),
            new(WaveCommandType.WaveMessage, 105020, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // これで遊戯指南は終了です。
            new(WaveCommandType.WaveMessage, 106000, 0),
            new(WaveCommandType.WaveMessage, 106010, 0),
            new(WaveCommandType.WaveMessage, 106020, 0),
            new(WaveCommandType.SamuraiExit, 0, 0),
            new(WaveCommandType.End, 0, 0),
        };
        private static readonly IReadOnlyList<WaveCommand> InGameWaveCommands = new WaveCommand[]
        {
            new(WaveCommandType.Begin, 0, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame85BPM, 85),
            new(WaveCommandType.SamuraiEnter, 0, 0),

#if ENABLE_WAVE1 // WAVE1
            // ？？「おうおう、そこ行くお侍さんよお」
            new(WaveCommandType.WaveMessage, 200000, 0),
            new(WaveCommandType.WaveMessage, 200010, 0),
            new(WaveCommandType.WaveMessage, 200020, 0),
            new(WaveCommandType.EnemyEnter, 200000, 30),
            new(WaveCommandType.WaveMessage, 200030, 0),
            new(WaveCommandType.WaveMessage, 200040, 0),
            new(WaveCommandType.WaveMessage, 200050, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 200010, 25),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 200020, 20),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 200030, 20),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // 辰雄「ふん、たわいもない。」
            new(WaveCommandType.WaveMessage, 201000, 0),
            new(WaveCommandType.WaveMessage, 201010, 0),
            new(WaveCommandType.WaveMessage, 201020, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame128BPM, 128),
            new(WaveCommandType.WaveMessage, 201030, 0),
#else
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame128BPM, 128),
#endif

#if ENABLE_WAVE2 // WAVE2
            // 故国に入った昇鯉辰雄は、殿にお目通りするため城を目指す
            //new(WaveCommandType.WaveMessage, 300000, 0),
            new(WaveCommandType.EnemyEnter, 300000, 27),
            new(WaveCommandType.WaveMessage, 300010, 0),
            new(WaveCommandType.WaveMessage, 300020, 0),
            new(WaveCommandType.WaveMessage, 300030, 0),
            new(WaveCommandType.WaveMessage, 300040, 0),
            new(WaveCommandType.WaveMessage, 300050, 0),
            new(WaveCommandType.WaveMessage, 300060, 0),
            new(WaveCommandType.WaveMessage, 300070, 0),
            new(WaveCommandType.WaveMessage, 300080, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 300010, 24),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 300020, 21),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 300030, 18),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // ？？「息子よ、なぜ戻って参った」
            new(WaveCommandType.WaveMessage, 301000, 0),
            new(WaveCommandType.EnemyEnter, 300040, 15),
            new(WaveCommandType.WaveMessage, 301010, 0),
            new(WaveCommandType.WaveMessage, 301020, 0),
            new(WaveCommandType.WaveMessage, 301030, 0),
            new(WaveCommandType.WaveMessage, 301040, 0),
            new(WaveCommandType.WaveMessage, 301050, 0),
            new(WaveCommandType.WaveMessage, 301060, 0),
            new(WaveCommandType.WaveMessage, 301070, 0),
            new(WaveCommandType.WaveMessage, 301080, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // 父「この国を、頼んだ、ぞ……ぐふっ」
            new(WaveCommandType.WaveMessage, 302000, 0),
            new(WaveCommandType.WaveMessage, 302010, 0),
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame150BPM, 150),
            new(WaveCommandType.WaveMessage, 302020, 0),
#else
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame150BPM, 150),
#endif

#if ENABLE_WAVE3 // WAVE3
            // 使えない鯉だ、息子も殺せんとは
            new(WaveCommandType.WaveMessage, 400000, 0),
            new(WaveCommandType.WaveMessage, 400010, 0),

            new(WaveCommandType.EnemyEnter, 400000, 18),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 400010, 16),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 400020, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
            new(WaveCommandType.EnemyEnter, 400030, 14),
            new(WaveCommandType.EnemyBeat, 0, 0),

            // 湯煮亭！　見つけたぞ！
            new(WaveCommandType.EnemyEnter, 400040, 12),
            new(WaveCommandType.WaveMessage, 401000, 0),
            new(WaveCommandType.WaveMessage, 401010, 0),
            new(WaveCommandType.WaveMessage, 401020, 0),
            new(WaveCommandType.WaveMessage, 401030, 0),
            new(WaveCommandType.EnemyBeat, 0, 0),
#endif

            // 辰雄「父上……湯煮亭は打ち果たしました」
            new(WaveCommandType.PlayBgm, (int)SoundBgmId.InGame85BPM, 85),
            new(WaveCommandType.WaveMessage, 402000, 0),
            new(WaveCommandType.WaveMessage, 402010, 0),
            new(WaveCommandType.WaveMessage, 402020, 0),
            new(WaveCommandType.WaveMessage, 402030, 0),
            new(WaveCommandType.Ranking, 0, 0),
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
            { 103040, "昇鯉があざやかに曲者を倒すと、" },
            { 103050, "ほかの曲者が昇鯉と同じテンポに変化します。" },
            { 103060, "もうひとりも倒してしまいましょう。" },
            { 103070, "お見事です。" },
            { 104000, "曲者の人数が増えても同じ要領で倒していきます。" },
            { 105000, "忍者のテンポの取り方は他の敵と少し違います。" },
            { 105010, "汚いなさすが忍者きたない。" },
            { 105020, "でも倒し方は同じです。テンポを見切りましょう。" },
            { 106000, "これで遊戯指南は終了です。" },
            { 106010, "「物語」ではストーリモードが、" },
            { 106020, "「腕試し」ではスピードランモードが遊べます。" },

            // 200000: WAVE1
            { 200000, "昇鯉辰雄はひとり故国への道を急いでいた。"},
            { 200010, "<color=#ff8080ff>？？「おうおう、そこ行くお侍さんよお」</color>"},
            { 200020, "<color=#ff8080ff>？？「命が惜しけりゃみぐるみを置いていきな」</color>"},
            { 200030, "<color=#40c040ff>辰雄「何かと思えば野盗か」</color>"},
            { 200040, "<color=#40c040ff>辰雄「貴様らでは相手にならん。出直せ。」</color>"},
            { 200050, "<color=#ff8080ff>野盗「なんだと！　やっちまえ！」</color>"},

            { 201000, "<color=#40c040ff>辰雄「ふん、たわいもない。」</color>"},
            { 201010, "<color=#40c040ff>辰雄「しかしなぜこのようなところにまで野盗が？」</color>"},
            { 201020, "<color=#40c040ff>辰雄「やはり何かがあったのだろうか」</color>"},
            { 201030, "国を思う心でテンポがアップした。"},

            // 300000: WAVE2
            //{ 300000, "故国に入った昇鯉辰雄は、殿にお目通りするため城を目指す。"},
            { 300010, "<color=#ff8080ff>侍「そこもと、この先への立ち入りはまかりならん」</color>"},
            { 300020, "<color=#40c040ff>辰雄「我が名は昇鯉辰雄、この国の武士である」</color>"},
            { 300030, "<color=#40c040ff>辰雄「何があったかは知らぬが通して頂きたい」</color>"},
            { 300040, "<color=#ff8080ff>侍「昇鯉辰雄！？　殿を弑した謀反人か！？」</color>"},
            { 300050, "<color=#40c040ff>辰雄「なっ、どういうことだ！」</color>"},
            { 300060, "<color=#ff8080ff>侍「昇鯉辰雄は見つけ次第殺せとの命令だ！」</color>"},
            { 300070, "<color=#ff8080ff>侍「であえ！　であえ！」</color>"},
            { 300080, "<color=#40c040ff>辰雄「くっ、問答無用か！」</color>"},

            { 301000, "<color=#ff8080ff>？？「息子よ、よくぞ戻って参った」</color>"},
            { 301010, "<color=#40c040ff>辰雄「父上、これはどういうことですか？」</color>"},
            { 301020, "<color=#ff8080ff>父「家老のひとりである湯煮亭（ゆにてい）が」</color>"},
            { 301030, "<color=#ff8080ff>父「殿を弑したのだ」</color>"},
            { 301040, "<color=#ff8080ff>父「そなたに罪を被せてな」</color>"},
            { 301050, "<color=#40c040ff>辰雄「父上はそれを黙ってみていたのですか！」</color>"},
            { 301060, "<color=#ff8080ff>父「悲しいかな、わしには流れに逆らう度胸も力もない」</color>"},
            { 301070, "<color=#ff8080ff>父「だが息子よ、そなたにはその力がある」</color>"},
            { 301080, "<color=#ff8080ff>父「わしを打ち倒し、しかばねを越えてゆけ」</color>"},

            { 302000, "<color=#ff8080ff>父「この国を、頼んだ、ぞ……ぐふっ」</color>"},
            { 302010, "<color=#40c040ff>辰雄「父上……」</color>"},
            { 302020, "怒りの心でテンポがアップした。"},

            // 400000: WAVE3
            { 400000, "<color=#ff8080ff>？？「使えない鯉だ、反逆者の息子も殺せんとは」</color>"},
            { 400010, "<color=#40c040ff>辰雄「湯煮亭！　どこだ！」</color>"},

            { 401000, "<color=#40c040ff>辰雄「湯煮亭！　見つけたぞ！」</color>"},
            { 401010, "<color=#ff8080ff>湯煮亭「くっ、鯉ごときに我が野望がっ！」</color>"},
            { 401020, "<color=#40c040ff>辰雄「違うぞ湯煮亭。父も私もただの鯉ではない」</color>"},
            { 401030, "<color=#40c040ff>辰雄「昇り鯉、龍になるものだっ！」</color>"},

            { 402000, "<color=#ff8080ff>湯煮亭「うぼぁー」</color>"},
            { 402010, "<color=#40c040ff>辰雄「父上……湯煮亭は打ち果たしました」</color>"},
            { 402020, "そうつぶやくと昇鯉辰雄は真っ直ぐ駆けていった。"},
            { 402030, "故国の未来へと向かって。"},
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
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 5,  0),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 3,  2),
                }
            },
            // 200000: WAVE1
            { 200000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3, -1),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5, 1),
                }
            },
            { 200010, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5,  0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3,  2),
                }
            },
            { 200020, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5, -2),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 3,  0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 5,  2),
                }
            },
            { 200030, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 2,  0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 4, -2),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 6,  0),
                    new EnemyParam(EnemyType.Rohnin, EnemyAppearType.Move, 4,  2),
                }
            },
            // WAVE2
            { 300000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 3,  2),
                }
            },
            { 300010, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 3,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5,  2),
                }
            },
            { 300020, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 2,  0),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 4, -2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 6,  0),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 4,  2),
                }
            },
            { 300030, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 0,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 2,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 3,  2),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 6,  0),
                }
            },
            { 300040, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Father, EnemyAppearType.Move, 4, 0),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 2, 1),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 6, -1),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 5, 2),
                }
            },
            // WAVE3
            { 400000, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 5,  0),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 0, 0),
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, 3,  2),
                }
            },
            { 400010, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 3,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5,  2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 0, 2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 0, -2),
                }
            },
            { 400020, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, -4, 0),
                }
            },
            { 400030, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 0,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 5, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 2,  0),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 3,  2),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 6,  0),
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, 1,  2),
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, 1, -2),
                }
            },
            { 400040, new EnemyParam[]
                {
                    new EnemyParam(EnemyType.Unity, EnemyAppearType.Move, 4, 0),
                    new EnemyParam(EnemyType.XinobiBlack, EnemyAppearType.Move, 2, 1),
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, 6, -1),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 3, -2),
                    new EnemyParam(EnemyType.SamuraiHigh, EnemyAppearType.Move, 5, 2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 0, 2),
                    new EnemyParam(EnemyType.SamuraiLow, EnemyAppearType.Move, 0, -2),
                    new EnemyParam(EnemyType.XinobiWhite, EnemyAppearType.Fade, -4, 0),
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
