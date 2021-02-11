$(function () {
	const switcher = $('#theme-switcher');

	if ($.cookie('theme')) {
		document.documentElement.setAttribute("theme", $.cookie('theme'));
		switcher.addClass('active');
    }

	switcher.on('click', () => {
		if (document.documentElement.hasAttribute('theme')) {
			document.documentElement.removeAttribute("theme");
			$.removeCookie('theme');
			switcher.removeClass('active');
		}
		else {
			document.documentElement.setAttribute("theme", "dark");
			$.cookie('theme', 'dark');
			switcher.addClass('active');
		}

		$('.theme').removeClass('active');
	});
});