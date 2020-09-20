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

    function init() {
        var router = new Router([
            new Route('home', 'home.html', 'home',true),
            new Route('about', 'about.html', 'about')
        ]);
        router.onroutechange = onroutechange;

        router.init();
    }

    init();
}());