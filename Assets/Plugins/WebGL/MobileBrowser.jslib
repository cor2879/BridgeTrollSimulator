mergeInto(LibraryManager.library, {
  OldSkool_IsMobileTouchBrowser: function () {
    var userAgent = navigator.userAgent || "";
    var platform = navigator.platform || "";
    var userAgentDataMobile =
      navigator.userAgentData && navigator.userAgentData.mobile === true;
    var appleMobile =
      /iPhone|iPad|iPod/i.test(userAgent) ||
      (platform === "MacIntel" && navigator.maxTouchPoints > 1);
    var androidMobile = /Android/i.test(userAgent);

    return userAgentDataMobile || appleMobile || androidMobile ? 1 : 0;
  }
});
