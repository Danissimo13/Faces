'use strict';

(function () {
    function onroutechange(oldMenuTab, newMenuTab) {
        if (oldMenuTab) {
            var oldSelector = '#navbar nav a[menu=' + oldMenuTab + ']';
            $(oldSelector).removeClass('active');
        }

        var newSelector = '#navbar nav a[menu=' + newMenuTab + ']';
        $(newSelector).addClass('active');
    }

    function onhtmlloading(url) {
        var scriptUrl = url;
        $.getScript(scriptUrl);
    }

    function init() {
        var router = new Router([
            new Route('home', 'home.html', 'home', [], true),
            new Route('acc', 'acc.html', 'acc', ['authentication.js']),
            new Route('login', 'login.html', 'acc', ['authentication.js']),
            new Route('about', 'about.html', 'about'),
            new Route('theme', 'theme.html', 'theme', ['themes.js']),
        ]);
        router.onhtmlloading = onhtmlloading;
        router.onroutechange = onroutechange;

        router.init();
    }

    init();
}());