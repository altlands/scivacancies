﻿jQuery.extend(jQuery.expr[":"], {
    attrStartsWith: function (el, _, b) {
        for (var i = 0, atts = el.attributes, n = atts.length; i < n; i++) {
            if (atts[i].nodeName.indexOf(b[3]) === 0) {
                return true;
            }
        }

        return false;
    },
    attrValueStartsWith: function (el, _, b) {
        for (var i = 0, atts = el.attributes, n = atts.length; i < n; i++) {
            if (atts[i].nodeValue.indexOf(b[3]) === 0) {
                return true;
            }
        }

        return false;
    }
});

$(document).ready(function () {
    //select
    var params = {
        changedEl: "select",
        visRows: 12,
        scrollArrows: true
    }
    cuSel(params);

    // Checkbox
    $(".checkbox").not(".disabled").click(function () {
        if ($(this).parent().find("input").attr("checked")) {
            $(this).removeClass("checked");
            $(this).parent().find("input").attr("checked", false);
        } else {
            $(this).addClass("checked");
            $(this).parent().find("input").attr("checked", true);
        }
    });

    $(".radio").click(function () {
        if ($(this).hasClass("checked")) {
            return false;
        } else {
            $(this).parents(".radio-list:first").children("li").find(".radio:first").removeClass("checked");
            $(this).closest("li").siblings("li").find("input").attr("checked", false);
            $(this).find("input").attr("checked", true);
            $(this).addClass("checked");
        }
    });
    $(".radio-list label").click(function () {
        if ($(this).siblings("span").hasClass("checked")) {
            return false;
        } else {
            $(this).closest(".radio-list").find(".radio").removeClass("checked");
            $(this).closest("li").siblings("li").find("input").attr("checked", false);
            $(this).siblings("span").find("input").attr("checked", true);
            $(this).siblings("span").addClass("checked");
        }
    });
    // Popup
    //$('.window-popup, .popup-bg, .bg-window').hide(); 
    $(".popup-bg").css({ opacity: .2 });

    function alignCenter(elem) {
        var modalHeight = ($(window).height() - 40 * 2);
        elem.css({
            height: modalHeight + "px",
            left: ($(window).width() - elem.outerWidth()) / 2 + "px",
            top: /*($(window).height() - elem.outerHeight()) / 2*/ 20 + "px"
        });
        $(elem).find("div.content-popup").css({
            height: (modalHeight - 100 - 50) + "px"
        });
    }

    alignCenter($(".window-popup"));
    $(window).resize(function () {
        alignCenter($(".window-popup"));
    });
    $(".open-popup").click(function () {
        var source = this;
        if ($(source).attr("data-modal") != undefined) {
            $(".window-popup[data-name=\"" + $(source).attr("data-modal") + "\"]").fadeIn(300);
        }
        else {
            $(".popup-bg, .window-popup, .bg-window").fadeIn(300);
        }
        return false;
    });
    $(".close-popup").click(function () {
        $(".popup-bg, .window-popup, .bg-window").fadeOut(300);
    });
    $("#cancellogin").click(function () {
        $($(this).parents("form")[0]).find("input[type!=\"submit\"]").val(null);
        $(".close-popup").click();
    });
    // function centering
    $("#subscribe").popover({
        'placement': "bottom",
        'trigger': "hover",
        html: true,
        content: function () {
            return $("#subscribe-popover").html();
        }
    });
    $("#subscribed").popover({
        'placement': "bottom",
        'trigger': "hover",
        html: true,
        content: function () {
            return $("#subscribed-popover").html();
        }
    });
    $("#lk").popover({
        'placement': "bottom",
        'trigger': "hover",
        html: true,
        content: function () {
            return $("#lk-popover").html();
        }
    });
    //tabs
    $(".nav-tabs li:first a, #tab.nav-tabs li:first a").tab("show");
    $(".nav-tabs a").click(function (e) {
        e.preventDefault();
        $(this).tab("show");
    });

    //carousel
    var slider = $(".bxslider").bxSlider({
        mode: "horizontal",
        autoControls: true,
        minSlides: 1,
        pager: false,
        adaptiveHeight: true
    });

    //open block solutions
    $(".solutions").click(function () {
        $(this).parents(".content-slide").find(".b-solution").slideToggle("fast", function () {
            var current = slider.getCurrentSlide();
            /*slider.reloadSlider();
			slider.goToSlide(current);*/
            //slider.find("li").eq(current).css("height", "100%");
            $(".bx-viewport").css("height", "100%");

        });
        if ($(this).hasClass("op")) {
            $(this).parent("div").hide().next("div").show();
        } else {
            $(this).parent("div").hide().prev("div").show();
        }
    });

    findRemovableCreateriaItem($(".has-removable-items").find("li"));
    recountCriteriaItem();

    /*
        this code need for navigate by new pager or filter values
     */
    // changeaction
    $("input[changeaction=true]").keypress(function (e) {
        if (e.which == 13) {
            var input = this;
            var form = $(input).parents("form")[0];
            var uri = updateQueryStringParameter($(form).attr("action"), e.currentTarget.name, e.currentTarget.value);
            console.log(uri);
            window.location = uri;
        }
    });
    $("input[changeaction=true]").parents("form").submit(function (e) {
        e.preventDefault();
    });

    /*
     * показать все элементы
     */
    $("div.filter-contents span.show-all-list").click(function () {
        var source = this;
        $(source).siblings("ul").find("li").show(500, function () {
            $(source).hide();
            $(source).siblings("span.hide-unselected").show();
        });
    });
    /*
     * скрыть невыбранные элементы
     */
    $("div.filter-contents span.hide-unselected").click(function () {
        var source = this;
        $(source).siblings("ul").find("li").find("span.checkbox:not(.checked)").parents("li").hide(300, function () {
            $(source).hide();
            $(source).siblings("span.show-all-list").show();
        });
    });
    /*
     * Исследователь редактирует аккаунт: удалить интерес из списка
     */
    $(".list-tags.research-interests").find("a.link-remove").click(function () {
        var source = this;
        $($(source).parents("li")[0]).remove();
        return false;
    });
    /*
     *
     */
    $('.collapsible-filter-header').click(function() {
        var source = this;
        if ($(source).hasClass('open')) {
            $(source).children('ul').hide();
            $(source).removeClass('open');
        }
        else {
            $(source).addClass('open');
            $(source).children('ul').show();
        }
    });
    /*
    end of the code
    */
});


function setIsResearcher(value) {
    $("form").find("input[type=\"hidden\"][name=\"IsResearcher\"]").val(value);
};

function vacancySaveOptions(options) {

    if (options.publish !== undefined && options.publish) {
        $("form").find("input[type=\"hidden\"][name=\"ToPublish\"]").val(true);
        return true;
    }

    if (options.saveDraft !== undefined && options.saveDraft) {
        $("form").find("input[type=\"hidden\"][name=\"ToPublish\"]").val(false);
        return true;
    }

};

function addNewCriteriaItem(ulName) {
    var $ul = $("#" + ulName);
    var newItem = $ul.find("li:hidden").clone().show();
    findRemovableCreateriaItem(newItem);
    $ul.append(newItem);
    recountCriteriaItem();
};
function findRemovableCreateriaItem($lis) {
    $lis.find("span.big-link-remove").click(function () {
        var removeMe = $(this).parents("li")[0];
        $(removeMe).remove();
        recountCriteriaItem();
    });
};
function recountCriteriaItem() {
    var inputs = $("#CriteriaInputs").find("li:visible").find("input");
    for (var i = 1; i <= inputs.length; i++) {
        $(inputs[i - 1]).attr("id", "Criteria[" + i + "]");
    }
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf("?") !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, "$1" + key + "=" + value + "$2");
    }
    else {
        return uri + separator + key + "=" + value;
    }
}
/*
 * Выбрать отрасли науки
 */
function selectedItemFromModalDictionary(hiddenInputName, newValue, displayText) {
    $("#" + hiddenInputName).val(newValue);
    $("#" + hiddenInputName).siblings("a")[0].innerText = displayText;
    $("div[data-name=\"" + hiddenInputName + "\"]").find(".close-popup").click();
    return false;
};
/**
 * перед отправкой формы удалить шаблоны пополнения списков
 * @returns {} 
 */
function beforeFormSubmit(source) {
    var form = $(source).parents("form")[0];
    $(form).find("[data-list-template=\"true\"]").remove();
    return true;
};
/**
 * добавить новый элемент к списку при редактировании
 * @returns {} 
 */
function addNewItemToList(source, prefix) {
    //<div class="table-form mt15" data-innercount="@(Model.Educations.Count+1)">
    var parentDiv = $(source).parents(".property-list-container")[0];

    //получаем текущий индекс количества строк
    var newIndex = parseInt($(parentDiv).attr("data-innercount")) + 1;
    var oldPrefixDash = prefix + "_0__",
    oldPrefixBracket = prefix + "\\[0\\]",
    newPrefixDash = prefix + "_" + newIndex + "__",
        newPrefixBracket = prefix + "[" + newIndex + "]";

    //находим шаблон для добавления строк
    var templateDiv = $(parentDiv).find("[data-list-template=\"true\"]").clone();
    //меняем индексы в новом шаблоне
    var templateDivInnerHtml = templateDiv
        .html()
        .replace(new RegExp(oldPrefixBracket, 'g'), newPrefixBracket)
        .replace(new RegExp(oldPrefixDash, 'g'), newPrefixDash);
    //обновляем шаблон
    templateDiv.html(templateDivInnerHtml);

    var lastItemRow = $(parentDiv).find(".property-list-item").last();
    $(templateDiv).removeAttr('data-list-template');
    $(templateDiv).find("input[name=\""+ prefix + '.Index"]').val(newIndex);
    $(lastItemRow).after(templateDiv);
    $(templateDiv).fadeIn("slow");

    //сохранить новый индекс
    $(parentDiv).attr("data-innercount", newIndex);
    return false;
};
/**
 * добавить новый элемент к списку при редактировании
 * @returns {} 
 */
function removeItemFromList(source) {
    if (confirm("Вы уверены что хотите удалить эту запись?")) {
        var parentDiv = $(source).parents(".property-list-item")[0];
        $(parentDiv).fadeOut(500, function () {
            $(parentDiv).remove();
            return false;
        });
        return false;
    }
    return false;
};