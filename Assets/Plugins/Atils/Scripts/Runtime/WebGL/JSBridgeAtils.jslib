mergeInto(LibraryManager.library, {

    isMobile: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /android|iphone|ipad|ipod/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    isAndroid: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /android/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    isIOS: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /iphone|ipad|ipod/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    isIPhone: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /iphone/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    isIPad: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /ipad/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

    isIPod: function() {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /ipod/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },

});