using System.Collections.Generic;

namespace u1w22a
{
    /// <summary>
    /// ツイートする情報。
    /// </summary>
    public struct TweetData
    {
        /// <summary>
        /// メッセージ。
        /// </summary>
        public string _message;

        /// <summary>
        /// URL。
        /// </summary>
        public string _url;

        /// <summary>
        /// ハッシュタグ。
        /// </summary>
        public IReadOnlyList<string> _hashTags;

        /// <summary>
        /// 生のメッセージを生成する。
        /// </summary>
        /// <returns>生のメッセージ。</returns>
        public string GenerateRawMessage()
        {
            string rawMessage = $"{(_message != null ? _message : string.Empty)}";

            // ハッシュタグを追加
            if (_hashTags != null && _hashTags.Count != 0)
            {
                // ハッシュタグはコンマ区切りの文字列で設定する
                string hashtags = _hashTags[0];
                for (int i = 0; i < _hashTags.Count; ++i)
                {
                    rawMessage += $" #{_hashTags[i]}";
                }
            }

            // URLを追加
            if (_url != null)
            {
                rawMessage += $" {_url}";
            }

            // 生成したクエリを返す
            return rawMessage;
        }

        /// <summary>
        /// クエリ文字列を生成する。
        /// </summary>
        /// <returns>クエリ文字列。</returns>
        public string GenerateQueryString()
        {
            string query = $"{(_message != null ? _message : string.Empty)}";

            // ハッシュタグを追加
            if (_hashTags != null && _hashTags.Count != 0)
            {
                // ハッシュタグはコンマ区切りの文字列で設定する
                string hashtags = _hashTags[0];
                for (int i = 1; i < _hashTags.Count; ++i)
                {
                    hashtags += $",{_hashTags[i]}";
                }
                query += $"&hashtags={hashtags}";
            }

            // URLを追加
            if (_url != null)
            {
                query += $"&url={_url}";
            }

            // 生成したクエリを返す
            return query;
        }
    }
}
