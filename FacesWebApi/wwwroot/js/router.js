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
    route: undefined,
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
                var oldRoute = e.oldURL.substring(e.oldURL.lastIndexOf('#'))
                var newRoute = e.newURL.substring(e.newURL.lastIndexOf('#'));

                scope.onroutechange(oldRoute, newRoute);
                scope.hasChanged(scope, r);
            });
        })(this, r);

        this.onroutechange('', window.location.hash);
        this.hasChanged(this, r);
    },
    hasChanged: function (scope, r) {
        if (window.location.hash.length > 0) {
            for (var i = 0, length = r.length; i < length; i++) {
                var route = r[i];
                if (route.isActiveRoute(window.location.hash.substr(1))) {
                    scope.goToRoute(route.htmlName);
                }
            }
        } else {
            for (var i = 0, length = r.length; i < length; i++) {
                var route = r[i];
                if (route.default) {
                    scope.goToRoute(route.htmlName);
                }
            }
        }
    },
    goToRoute: function (htmlName) {
        (function (scope) {
            var url = 'views/' + htmlName;
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    scope.rootElem.innerHTML = this.responseText;
                }
            };
            xhttp.open('GET', url, true);
            xhttp.send();
        })(this);
    }
};