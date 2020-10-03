function scope(e) {
    var scopedImgHtml = '<div id="scoped-image"></div>';
    var blockImgHtml = '<div id="scoped-image-block"><img class="inverted" src="' + $(e).attr('src') + '" /></div>';
    var scopedImg = $(scopedImgHtml);
    var blockImg = $(blockImgHtml);

    scopedImg.append(blockImg);
    $('body').append(scopedImg);

    scopedImg.on('click', function (e) {
        if (e.target.id == scopedImg.attr('id'))
            $(scopedImg).remove();
    });
};