'use strict';

(function () {
    function onroutechange(oldRoute, newRoute) {
        var oldSelector = '#navbar nav a[href="' + oldRoute + '"]';
        var newSelector = '#navbar nav a[href="' + newRoute + '"]';

        $(oldSelector).removeClass('active');
        $(newSelector).addClass('active');
    }

    function init() {
        var router = new Router([
            new Route('home', 'home.html', true),
            new Route('about', 'about.html')
        ]);
        router.onroutechange = onroutechange;

        router.init();
    }

    init();
}());