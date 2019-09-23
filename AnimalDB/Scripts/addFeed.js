function loadModalImage(id, name) {
    $('.animal-photo-name').text(name);
    $('.animal-picture-modal').html('<img src="../../Content/AnimalImages/' + id + '.jpg" alt="Animal Picture" />');
    $('#PhotoModal').modal();
    return false;
}

$('#btnFinish,#btnPrevious,#btnGroup').attr('disabled', 'disabled');
$('#visualCheck').change(function () {
    if ($('#visualCheck')[0].checked) {
        $('#btnFinish,#btnPrevious,#btnGroup').removeAttr('disabled');
        $('#btnFinish,#btnPrevious,#btnGroup').removeClass('btn-outline-danger');
        $('#btnFinish,#btnPrevious,#btnGroup').addClass('btn-success');

    } else {
        $('#btnFinish,#btnPrevious,#btnGroup').attr('disabled', 'disabled');
        $('#btnFinish,#btnPrevious,#btnGroup').addClass('btn-outline-danger');
        $('#btnFinish,#btnPrevious,#btnGroup').removeClass('btn-success');
    }
});

function vm() {
    var self = this;

    self.FeedGroup = function () {
        var feedvalue = $('#feedgroupvalue').val();
        var groupname = $('#groupdropdown').val();
        if (groupname !== '') {
            $('div.col-md-2').each(function (index, column) {
                if ($(column).find('.groupname td span').html().replace('&nbsp;', '').trim() === groupname) {
                    $('input[id^="Animals_' + index + '__Feeds_' + $('#groupdatedropdown').val() + '__FeedAmount"]').val(feedvalue);
                    $('input[id^="Animals_' + index + '__Feeds_' + $('#groupdatedropdown').val() + '__CombinedFeed"]').val('true');
                }
            });
            self.Finish();
        } else {
            $('#feedgroupmodal').modal('hide');
        }
    };

    self.RemoveFeedGroup = function () {
        var groupname = $('#removegroupdropdown').val();
        if (groupname !== '') {
            $('div.col-md-2').each(function (index, column) {
                if ($(column).find('.groupname td span').html().replace('&nbsp;', '').trim() === groupname) {
                    $('input[id^="Animals_' + index + '__Feeds_' + $('#removegroupdatedropdown').val() + '__FeedAmount"]').val('');
                    $('input[id^="Animals_' + index + '__Feeds_' + $('#removegroupdatedropdown').val() + '__CombinedFeed"]').val('false');
                }
            });
            self.Finish();
        } else {
            $('#removefeedgroupmodal').modal('hide');
        }
    };

    self.FeedPrevious = function () {
        var feedamount, feedtype = false, feedinput;

        $.each($('.col-md-2 .feedingtable table'), function (i, el) {
            $.each($(el).find('input[id$=__FeedAmount]'), function (j, el2) {
                if ($(el2).val() !== '') {
                    feedamount = $(el2).val();
                    feedtype = $(el2).siblings('input').val();
                }
            });
            $(el).find('input[id$=__FeedAmount]').last().val(feedamount);
            $(el).find('input[id$=__CombinedFeed]').last().val(feedtype);
        });
        self.Finish();
    };

    self.LowWeightAnimals = ko.observableArray();

    self.SaveAndFinish = function () {
        self.LowWeightAnimals.removeAll();
        $.each($('div.col-md-2'), function (index, column) {
            var animalid = $(column).find('.uniqueanimalid b').html().trim();
            var animalweight = $(column).find('.baseweight').html().replace('g', '').trim();
            var result = false;

            var lastWeight = $(column).find('input[id$=__Weight]').filter(function (i, el) { return $(el).val() !== "";}).last().val();
            if (lastWeight !== "" && parseInt(animalweight, 10) * .85 > parseInt(lastWeight, 10)) {
                var animal = { name: animalid };
                if (self.LowWeightAnimals.indexOf(animal) < 0) {
                    self.LowWeightAnimals.push(animal);
                }
            }
        });
        if (self.LowWeightAnimals().length === 0) {
            self.Finish();
        } else {
            $('#lowweightalert').modal('show');
        }
    };

    self.Finish = function () {
        $('#state').val('finish');
        // convert all ff's into -1's
        $('input[name$="FeedAmount"]').each(function (i, j) {
            if ($(j).val() === 'ff') {
                $(j).val(-1);
            }
        });
        document.forms[1].submit();
    };
}

ko.applyBindings(new vm());

$('.combinedfeed .feed').keyup(function (e) {
    var newvalue = e.target.value;
    var feedid = e.target.id;
    feedid = feedid.slice(0, feedid.lastIndexOf("_") - 1);
    feedid = feedid.slice(feedid.lastIndexOf("_") + 1);
    var currentgroupname = $(e.target).parents('.col-md-2').find('.groupname td span').html().replace('&nbsp;', '').trim();
    $.each($('.col-md-2'), function (i, j) {
        if ($(j).find('.groupname td span').html().replace('&nbsp;', '').trim() === currentgroupname) {
            $(j).find($('input[id$="__Feeds_' + feedid + '__FeedAmount"]')).val(newvalue);
        }
    });
});

var currentGroup = '';
var previousGroup = '';
var previousElement = null;
$.each($('.groupname td'), function (i, j) {
    currentGroup = $(j).find('span').html().replace('&nbsp;', '').trim();
    if (currentGroup !== '') {
        if (currentGroup !== previousGroup) {
            $(j).addClass('start');
        } else if (currentGroup === previousGroup) {
            $(j).addClass('middle');
        }
    }
    if (currentGroup !== '' && currentGroup !== previousGroup && previousGroup !== '' && previousElement !== null) {
        $(previousElement).addClass('end');
    }
    previousElement = j;
    previousGroup = currentGroup;
});
if (currentGroup !== '' && previousElement !== null) {
    $(previousElement).addClass('end');
}

// convert all -1's into -ff's
$('input[name$="FeedAmount"]').each(function (i, j) {
    if ($(j).val() === '-1') {
        $(j).val('ff');
    }
});

$('.col-md-2 .start').parents('.col-md-2').find('.feed').css('display', 'inline');
$('.col-md-2 .start').parents('.col-md-2').find('.combinedfeedmsg').css('display', 'none');
$('.col-md-2 .start').parents('.col-md-2').addClass('start-group-separator');
$('.col-md-2 .end').parents('.col-md-2').addClass('end-group-separator');

$('#groupdatedropdown option:last, #removegroupdatedropdown option:last').attr('selected', 'selected');

$("html, body").animate({ scrollTop: $(document).height() }, 1000);