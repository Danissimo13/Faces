'use strict';

function Router(routes) {
    try {
        if (!routes) {
            throw 'error: routes param is mandatory';
        }
        this.constructor(routes);
    } catch (e) {
        console.error(e);
    }
}

Router.prototype = {
    tab: undefined,
    routes: undefined,
    rootElem: undefined,
    onroutechange: undefined,
    constructor: function (routes) {
        this.routes = routes;
        this.rootElem = document.getElementById('app');
    },
    init: function () {
        var r = this.routes;
        (function (scope, r) {
            window.addEventListener('hashchange', function (e) {
                scope.hasChanged(scope, r);
            });
        })(this, r);

        this.hasChanged(this, r);
    },
    hasChanged: function (scope, r) {
        if (window.location.hash.length > 0) {
            for (var i = 0, length = r.length; i < length; i++) {
                var route = r[i];
                if (route.isActiveRoute(window.location.hash.substr(1))) {
                    scope.goToRoute(route.htmlName);

                    scope.loadJs(route);

                    scope.onroutechange(scope.tab, route.menuTab);
                    scope.tab = route.menuTab;
                }
            }
        } else {
            for (var i = 0, length = r.length; i < length; i++) {
                var route = r[i];
                if (route.default) {
                    scope.goToRoute(route.htmlName);

                    scope.loadJs(route);

                    scope.onroutechange("", route.menuTab);
                    scope.tab = route.menuTab;
                }
            }
        }
    },
    goToRoute: function (htmlName) {
        (function (scope) {
            var url = 'views/' + htmlName;
            $(scope.rootElem).load(url);
        })(this);
    },
    loadJs: function (route) {
        (function ()
        {
            if (route.jsNames) {
                var allScripts = "";
                for (var jsName of route.jsNames) {
                    var str = '<script src="js/' + jsName + '"></script>'
                    allScripts += str;
                }

                $('#scripts').html(allScripts);
            }
        })();
    }
};