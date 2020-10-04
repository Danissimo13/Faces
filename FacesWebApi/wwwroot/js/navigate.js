$(function () {
    var ui = {
        $body: $('body'),
        $menu: $('#navbar'),
        $app: $('#app')
    };

    let config = {};

    init();
    window.navTo = navTo; 

    function init() {
        $.getJSON('/data/nav-config.json', function (data) {
            config = data;
            var page = (document.location.pathname.substr(1) + document.location.search) || config.mainPage;

            console.log(page);
            navTo(page);
            bindHandlers();
        });
    }

    function bindHandlers() {
        ui.$body.on('click', 'a[link="ajax"]', navigate);
        window.onpopstate = popState;
    }

    function navigate(e) {
        e.stopPropagation();
        e.preventDefault();

        var page = $(this).attr('href');
        navTo(page);
    }

    function navTo(page) {
        var parsedPage = page.substr(0, page.indexOf('?'));
        if (parsedPage) {
            if (!config.pages[parsedPage]) parsedPage = '404';
            loadPage(parsedPage);
        }
        else {
            if (!config.pages[page]) page = '404';
            loadPage(page);
        }

        history.pushState({ page: page }, '', page);
    }

    function popState(e) {
        var page = (e.state && e.state.page) || config.mainPage;
        console.log(e.state);
        loadPage(page);
    }

    function loadPage(page) {
        var url = 'views/' + page + '.html';
        var pageTitle = config.pages[page].title;
        var menu = config.pages[page].menu;

        $.get(url, function (html) {
            document.title = pageTitle + ' | ' + config.siteTitle;
            ui.$menu.find('a').removeClass('active');
            ui.$menu.find('a[menu="' + menu + '"]').addClass('active');
            ui.$app.html(html);
        });
    }
});