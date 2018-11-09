function matchCustom(params, data) {

    var searchTerm = $.trim(params.term).toLowerCase();

    if (searchTerm === '') {
        return data;
    }

    if (typeof data.text === 'undefined') {
        return null;
    }

    if (data.text.toLowerCase().indexOf(searchTerm) > -1) {
        var modifiedData = $.extend({}, data, true);

        return modifiedData;
    }

    return null;
}