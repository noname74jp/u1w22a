//https://blog.gigacreation.jp/entry/2020/10/04/223712
//https://ch.nicovideo.jp/lackLucky/blomaga/ar1462757
mergeInto(LibraryManager.library, {
  /**
   * 直接ツイートする(スマホ用)。
   * @param {String} raw_message ツイートするメッセージ。
   */
  // 
  TweetFromUnity: function (raw_message) {
    var mobile_pattern = /android|iphone|ipad|ipod/i;
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.search(mobile_pattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document)) {
      var message = Pointer_stringify(raw_message);
      location.href = "twitter://post?message=" + encodeURIComponent(message)
    }
  },

  /**
   * クリックイベントを購読する(PC用)。
   * @param {String} raw_message ツイートするメッセージ。
   */
  EnableTweetEvent: function(raw_message) {
    var mobile_pattern = /android|iphone|ipad|ipod/i;
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.search(mobile_pattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document)) {
      return;
    }

    var message = Pointer_stringify(raw_message);
    document.onclick = function() {
      window.open("https://twitter.com/intent/tweet?text=" + message, "_blank");
      document.onclick = null;
    } 
  },

  /**
   * クリックイベントの購読を解除する(PC用)。
   * @param {String} key 保存先のキー。
   * @return {String} value 保存した値。
   */
  DisableTweetEvent: function() {
    document.onclick = null;
  }

});
