'use strict';

$(function () {
    function onroutechange(oldMenuTab, newMenuTab) {
        if (oldMenuTab) {
            var oldSelector = '#navbar nav a[menu=' + oldMenuTab + ']';
            $(oldSelector).removeClass('active');
        }

        var newSelector = '#navbar nav a[menu=' + newMenuTab + ']';
        $(newSelector).addClass('active');
    }

    function init() {
        var router = new Router([
            new Route('home', 'home.html', 'home', [], true),
            new Route('acc', 'acc.html', 'acc', ['authorize.js']),
            new Route('login', 'login.html', 'acc', ['authorize.js', 'login.js']),
            new Route('reg', 'reg.html', 'acc', ['authorize.js']),
            new Route('about', 'about.html', 'about'),
            new Route('theme', 'theme.html', 'theme', ['themes.js']),
        ]);

        router.onroutechange = onroutechange;

        router.init();
    }

    init();
});