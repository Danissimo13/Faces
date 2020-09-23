'use strict';

function Route(name, htmlName, menuTab, defaultRoute) {
    try {
        if (!name || !htmlName) {
            throw 'error: name and htmlName params are mandatories';
        }
        this.constructor(name, htmlName, menuTab,defaultRoute);
    } catch (e) {
        console.error(e);
    }
}

Route.prototype = {
    name: undefined,
    htmlName: undefined,
    menuTab: undefined,
    jsNames: undefined,
    default: undefined,
    constructor: function (name, htmlName, menuTab, jsNames, defaultRoute) {
        this.name = name;
        this.htmlName = htmlName;
        this.menuTab = menuTab;
        this.jsNames = jsNames;
        this.default = defaultRoute;
    },
    isActiveRoute: function (hashedPath) {
        return hashedPath.replace('#', '') === this.name;
    }
}