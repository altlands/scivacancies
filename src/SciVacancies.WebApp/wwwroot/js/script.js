var graphs = {}; //глобальная переменная для графиков
jQuery.extend(jQuery.expr[":"], {
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

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

$(document).ready(function () {
    //select
    var params = {
        changedEl: "select:not(.skip)",
        visRows: 12,
        scrollArrows: true
    }
    cuSel(params);

    //jquery datepicker
    $(function () {
        $.datepicker.setDefaults(
            $.extend($.datepicker.regional["ru"])
        );
        $(".datepicker-vacancy").datepicker({
            changeMonth: true,
            changeYear: true,
            minDate: "+1D",
            showButtonPanel: true
        });
    });

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
        } else {
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
    //закладки в личном кабинете
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

    /*
 *  this code need for navigate by new pager values
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
 * показать все элементы в группе значений в поиске
 */
    $("div.filter-contents span.show-all-list").click(function () {
        var source = this;
        var parentContainer = $(source).parents('.cat-filter')[0];
        //plain checkbox list
        $(parentContainer).find("li").show(500, function () {
            $(source).hide();
            $(source).siblings("span.hide-unselected").show();
        });
    });
    /*
 * скрыть невыбранные элементы в группе значений в поиске
 */
    $("div.filter-contents span.hide-unselected").click(function () {
        var source = this;
        var parentContainer = $(source).parents('.cat-filter')[0];
        //plain checkbox list
        $(parentContainer).find("span.checkbox:not(.checked)").parents("li").hide(300, function () {
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
 * развернуть/свернуть список значений в фильтре
 */
    $('.collapsible-filter-header').click(function (event) {
        event.stopPropagation();
        var source = this;
        if ($(source).hasClass('open')) {
            $(source).children('ul').hide();
            $(source).removeClass('open');
        } else {
            $(source).addClass('open');
            $(source).children('ul').show();
        }
    });
    $('li.collapsible-filter-header.open').children('ul').show(); //показать вложенные элементы если добавлен класс .open
    /*
 * исправление распространения событий для списка фильтров при поиске
 */
    $('li.li-checkbox').click(function (event) {
        event.stopPropagation();
    });
    /*
 * Управление внутренними вкладками в Областях науки
 */
    $('.jshelper-sub-research-directions').click(function () {
        var source = this;

        $(source).siblings().removeClass('active');
        $(source).addClass('active');
        var parentContainer = $(source).parents('.jshelper-parent-of-tabs')[0];
        $(parentContainer).siblings('.jshelper-list-sections-science').hide();
        $(parentContainer).siblings('.jshelper-list-sections-science[id="' + $(source).attr('data-tabname') + '"]').show();
    });
    /*
 * закрывать модальное окно при нажатии кнопки Отмены
 */
    $('span.icon-close').click(function () {
        var source = this;
        var parent = $(source).parents('.window-popup')[0];
        $(parent).find('span.close-popup').click();
    });
    /*
 * редактирование Вакансии (развернуть/скрыть Критерии)
 */
    //b-publication open
    $('span.name-section').click(function () {
        var source = this;
        var parent = null;
        try {
            parent = $(source).parents('.b-publication')[0];
        } catch (e) {
        }
        if (parent != null) {
            $(parent).toggleClass('open');
        }
    });
    /*
 * показать все элементы (в Вакансии развернуть/скрыть Критерии)
 */
    $("div.lnk-container span.icon-hsm-eye").click(function () {
        var source = this;
        var parentContainer = $(source).parents('.right-cell')[0];
        $(parentContainer).find("div.b-publication").addClass('open');
        $(source).hide();
        $(source).siblings("span.icon-sm-eye").show();
    });
    /*
 * скрыть все элементы (в Вакансии развернуть/скрыть Критерии)
 */
    $("div.lnk-container span.icon-sm-eye").click(function () {
        var source = this;
        var parentContainer = $(source).parents('.right-cell')[0];
        $(parentContainer).find("div.b-publication").removeClass('open');
        $(source).hide();
        $(source).siblings("span.icon-hsm-eye").show();
    });
    ///*
    // * дизайн для кнопки выбора Фотографии
    // */
    //var buttonFile = $('a[data-selectorphoto="true"]');
    var buttonFile = document.getElementById('buttonFile');
    //var file = $('input[data-selectphoto="true"]');
    var file = document.getElementById('files');
    $(buttonFile).click(function () {
        file.click();
        file.onchange = function () {
            $("#fileName").remove();
            $(file).prev().after("<div class='italic mt10' id='fileName'>" + file.value + "</div>");
        }
        return false;
    });

    /*
         * переключатели для главной страницы
         */
    function toggleChevron(e) {
        $(e.target).prev('.panel-heading').parent().toggleClass('selected');
    }

    $('#accordion').on('hidden.bs.collapse', toggleChevron);
    $('#accordion').on('show.bs.collapse', toggleChevron);

    $('.tabs-toggle__item:first a').tab('show');
    $('.tabs-toggle__item a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
    /*
 * «срок трудового договора» поля должны показываться только, если выбираешь «срочный»
 */
    $('#cusel-scroll-' + 'ContractTypeValue').find('span').click(toggleContractTime);
    toggleContractTime();
    /*
 * сброс фильтра
 */
    $(".filter-link-uncheck-all").click(function () {
        var source = this;
        var parent = $(source).parents('.filter-contents')[0];

        $(parent).find('input[type="checkbox"]').attr('checked', false);
        $(parent).find('span.checkbox.checked').removeClass('checked');

        $(parent).find('input[type="text"]').val(null);

        //$(parent).parents('form').submit();
    });
    /*
 * сброс фильтра
 */
    $(".filter-link-check-all").click(function () {
        var source = this;
        var parent = $(source).parents('.filter-contents')[0];

        $(parent).find('input[type="checkbox"]').attr('checked', true);
        $(parent).find('span.checkbox').not('.checked').addClass('checked');

        $(parent).find('input[type="text"]').val(null);

        //$(parent).parents('form').submit();
    });
    /*
 * Временно для переключателя
 */
    $(".tabs_after_title > li").click(function () {
        $(".tabs_after_title > li").siblings().removeClass("active");
        $(this).addClass("active");
    });
    /*
 * на форме редактирваония добавить объектыв 
 */
    $('.property-list-container').each(function () {
        var parentDiv = this;
        var prefix = $(parentDiv).attr('data-property-respocible');

        if (prefix !== undefined && prefix != null) {
            var count = $(parentDiv).find(":visible.property-list-item").length;
            if (count === 0) {
                addingNewItemToList(parentDiv, prefix);
            }
        }
    });
    /*
        end of the code
        */
});
/*
 * обработка каскадных выпадающих списков для Cusel (год окончания периода не может быть меньше года начала периода)
 */
function cuselValueChanged(source, key) {
    var newValue = $(source).val();
    //console.log(key + ' - ' + newValue);

    //найти Зависимый элемент
    var children = $('[data-cusel-number-child="' + key + '"');
    if (children != undefined) {
        //если есть Зависимый
        //console.log(children);

        var children_cuselText = $(children).find('.cuselText');
        var childrenCurrentValue = children_cuselText[0].innerText;
        if (childrenCurrentValue != undefined) {
            //console.log(childrenCurrentValue);

            if (childrenCurrentValue < newValue)
                //если если значение Зависимого меньше значения Родительского
            {
                //то поставить родительское значение как новое значение Зависимого
                //console.log("it is less then parent");
                $(children_cuselText).html(newValue);

                $(children).find('.cusel-scroll-pane').find('span.cuselActive').removeClass('cuselActive');
                $(children).find('.cusel-scroll-pane').find('span[val="' + newValue + '"]').addClass('cuselActive');
            }
            /*
            //урезать (скрыть) неподходящие значения потомков
            //обновить вид потомка
            */
        }

    }
}
/*
 * Графики. запросить новые данные
 */
function refreshAllGraphicData() {
    graphs = graphs || {}; //глобальная переменная для графиков    

    if (graphs.chart === undefined || graphs.chart2 === undefined)
    { return; }

    //refreshData
    graphs.regionId = $('#RegionId').val();

    graphs.period = $('#graphicPeriod').find('.active').attr('data-pariod-value');
    //end: refreshData


    if (graphs.chart !== null) {

        var dataGraph1 = {
            regionId: graphs.regionId,
            interval: graphs.period
        };
        $.get("/analytics/VacancyPositions", dataGraph1, function (data) {
            graphs.chart.options.data = data; // Set Array of dataSeries
            graphs.chart.render();
        });
    }

    if (graphs.chart2 != null) {
        var dataGraph2 = {
            regionId: graphs.regionId,
            interval: graphs.period
        };
        $.get("/analytics/VacancyPayments", dataGraph2, function (data) {
            graphs.chart2.options.data = data; // Set Array of dataSeries
            graphs.chart2.render();
        });
    }
}
/*
 * каптча
 */
function reloadImg(captchaImageFieldName) {
    var d = new Date();
    $('#' + captchaImageFieldName).attr("src", "/captcha/fetch?w=164&h=50" + d.getTime());
}
/*
 * сделать некоторую обработку перед отправкой формы Регистрации
 */
function beforeFormSubmitRegister(source, captchaImageFieldName, captchaInputFieldName, captchaEmptyFieldName, captchaInvalidFieldName, event) {
    var form = $('#' + captchaImageFieldName).parents('form')[0];
    if (!$(form).hasClass('validated')) {
        event.preventDefault();


        var validData = {
            captchaText: $('#' + captchaInputFieldName).val()
        };

        $('#' + captchaInvalidFieldName).hide();
        if (validData.captchaText === undefined || validData.captchaText === null || validData.captchaText === "") {
            $('#' + captchaEmptyFieldName).show();
            return false;
        }
        $('#' + captchaEmptyFieldName).hide();

        $.ajax({
            url: '/captcha/isvalid',
            type: 'POST' /*'GET'*/,
            data: validData,
            success: function (isCaptureValid) {
                if (isCaptureValid) {
                    $('#' + captchaInvalidFieldName).hide();
                    $(form).addClass('validated');
                    $(form).submit();
                    return true;
                } else {
                    reloadImg(captchaImageFieldName);
                    $('#' + captchaInvalidFieldName).show();
                    return false;
                }
            },
            error: function (error) {
                reloadImg(captchaImageFieldName);
                $('#' + captchaInvalidFieldName).show();
                return false;
            }
        });
    }
};
/*
 * на форме Регистрации регистрации нажатие по кнопке "Согласие на обработку персональным данных"
 */
function vacancySaveOptions(options) {
    if (options.publish !== undefined && options.publish) {
        $("form").find("input[type=\"hidden\"][name=\"ToPublish\"]").val(true);
        return true;
    }
    if (options.saveDraft !== undefined && options.saveDraft) {
        $("form").find("input[type=\"hidden\"][name=\"ToPublish\"]").val(false);
        return true;
    }
    return false;
};

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
 * Выбрать значение из словаря
 */
function selectedItemFromModalDictionary(hiddenInputName, textInput, newValue, displayText) {
    $("#" + hiddenInputName).val(newValue);
    $("#" + textInput).val(displayText);
    $("[data-setnewvalue=\"" + textInput + "\"]").val(displayText);
    $("div[data-name=\"" + hiddenInputName + "\"]").find(".close-popup").click();
    return false;
};
/**
 * перед отправкой формы удалить шаблоны пополнения списков
 */
function beforeFormSubmit(source) {
    var form = $(source).parents("form")[0];
    $(form).find("[data-list-template=\"true\"]").remove();
    return true;
};
/**
 * добавить новую форму к списку с объектами
 */
function addNewItemToList(source, prefix) {
    //<div class="table-form mt15" data-innercount="@(Model.Educations.Count+1)">
    var parentDiv = $(source).parents(".property-list-container")[0];
    addingNewItemToList(parentDiv, prefix);
    return false;
};
/**
 * удалить форму из списка с объектами
 */
function removeItemFromList(source, prefix) {
    if (confirm("Вы уверены что хотите удалить эту запись?")) {
        var parentDiv = $(source).parents(".property-list-item")[0];

        var parentDivContainer = $(source).parents(".property-list-container")[0];

        $(parentDiv).remove();

        if (prefix !== undefined && prefix != null) {
            var count = $(parentDivContainer).find(":visible.property-list-item").length;
            if (count === 0) {
                addingNewItemToList(parentDivContainer, prefix);
            }
        }

        return false;
    }
    return false;
};
/**
 * добавление формы к списку с объектами
 */
function addingNewItemToList(parentDiv, prefix) {
    //получаем текущий индекс количества строк
    var newIndex = parseInt($(parentDiv).attr("data-innercount")) + 1;
    var oldPrefixDash = prefix + "_0__",
        oldPrefixSharp = prefix + "##0##",
        oldPrefixBracket = prefix + "\\[0\\]",
        newPrefixDash = prefix + "_" + newIndex + "__",
        newPrefixBracket = prefix + "[" + newIndex + "]",
        newPrefixSharp = prefix + newIndex;

    //находим шаблон для добавления строк
    var templateDiv = $(parentDiv).find("[data-list-template=\"true\"]").clone();
    //меняем индексы в новом шаблоне
    var templateDivInnerHtml = templateDiv
        .html()
        .replace(new RegExp(oldPrefixBracket, 'g'), newPrefixBracket)
        .replace(new RegExp(oldPrefixDash, 'g'), newPrefixDash)
        .replace(new RegExp(oldPrefixSharp, 'g'), newPrefixSharp)
        .replace(new RegExp('js_add_guid_here', 'g'), guid()) //генерим новые гуиды для работы select'ов. (по этим Guid работают "каскады" для отработки правила "окончания периода не может быть раньше его начала")
    ;
    //обновляем шаблон
    templateDiv.html(templateDivInnerHtml);

    $(templateDiv).find("select.skip").removeClass("skip").addClass("newselect");

    var lastItemRow = $(parentDiv).find(".property-list-item").last();
    $(templateDiv).removeAttr('data-list-template');
    $(templateDiv).find("input[name=\"" + prefix + '.Index"]').val(newIndex);
    $(lastItemRow).after(templateDiv);
    $(templateDiv).fadeIn("fast");

    var paramsSelect = {
        changedEl: "select.newselect",
        visRows: 12,
        scrollArrows: true
    }
    cuSel(paramsSelect);

    //сохранить новый индекс
    $(parentDiv).attr("data-innercount", newIndex);
}
/**
  * добавить метку о том что поискаовый запрос нужно сохранить в качестве подписки
  */
function isNullOrWhitespace(input) {

    if (typeof input === 'undefined' || input == null) return true;

    return input.replace(/\s/g, '').length < 1;
}
function addNewSubscription(source) {
    var parentForm = $(source).parents("form")[0];

    var titleValue = $(parentForm).find("input[name=\"NewSubscriptionTitle\"]").val();
    if (isNullOrWhitespace(titleValue)) {
        confirm("Укажите новый заголовок Подписки");
        return false;
    }
    $(parentForm).find("input[name=\"NewSubscriptionAdd\"]").val(true);
    return true;
}
/*
 * «срок трудового договора» поля должны показываться только, если выбираешь «срочный»
 */
function toggleContractTime(e) {
    var parent = $('.contract-time-period');

    if (parent.length === 0) {
        return;
    }

    var currentValue = "";
    if (e != undefined) {
        currentValue = e.target.innerText;
    } else {
        currentValue = $('#cusel-scroll-' + 'ContractTypeValue').find('span.cuselActive')[0].textContent;
    }
    if (currentValue.indexOf("ессроч") > -1) {
        $(parent).fadeOut("slow");
        $('#ContractTimeYears').val(null);
        $('#ContractTimeMonths').val(null);
    } else {
        $(parent).fadeIn("slow");
    }
}