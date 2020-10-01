//----------Набор глобальных функций для таблиц журналов------------
function sendForm($table, urlAction, dialogId) {
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    var selected = $table.bootstrapTable('getSelections').map(function (v) {
        return v[nameId];
    });
    if (selected.length > 0) {
        var $context = $table.parents('.contextBT');
        if (!dialogId) dialogId = '#modDialog';
        var $form = $context.find(dialogId + ' form');
        clearForm($form);
        let params = { ids: selected };
        showModalForm($form, urlAction, $table, refreshTable, dialogId, params);
    } else MessageBox('Строки не выбраны!');
};

//Создает 
function create($table, urlCreate, dialogId) {
    var $context = $table.parents('.contextBT');
    if (!dialogId) dialogId = '#modDialog';
    var $form = $context.find(dialogId + ' form');
    clearForm($form);
    showModalForm($form, urlCreate, $table, refreshTable, dialogId);
};

function createMany($table, urlCreate, dialogId) {
    var $context = $table.parents('.contextBT');
    if (!dialogId) dialogId = '#modDialog';
    var $form = $context.find(dialogId + ' form');
    clearForm($form);
    showModalForm($form, urlCreate, $table, refreshTable, dialogId);
};

function edit($table, urlGet, urlUpdate, dialogId) {
    var selected = $table.find('tr.success').attr('data-uniqueid');
    if (selected !== undefined) {
        $.get(urlGet, { id: selected }, function (data) {
            $context = $table.parents('.contextBT');
            if (!dialogId) dialogId = '#modDialog';
            var $form = $context.find(dialogId + ' form');

            FillForm(data, $form, showModalForm($form, urlUpdate, $table, refreshTable, dialogId));
           
        });
    } else MessageBox("Строки не выбраны!");
};

function editDetail($table, urlGet, urlUpdate, dialogId) {
    var selected = $table.find('tr.success').attr('data-uniqueid');
    if (selected !== undefined) {
        $.get(urlGet, { id: selected }, function (data) {
            $context = $table.parents('.contextBT');
            if (!dialogId) dialogId = '#modDialog';
            var $form = $context.find(dialogId + ' form');
            FillForm(data, $form, showModalForm($form, urlUpdate, $table, refreshTable, dialogId));
        });
    } else MessageBox("Строки не выбраны!");
};

//Возвращает форму редактирования HTML ()
function editRenderForm($table, urlGet, urlUpdate, callbackAfterSubmit) {
    var selected = $table.find('tr.success').attr('data-uniqueid');
    if (selected !== undefined) {
        $.get(urlGet, { id: selected }, function (data) {
            $context = $table.parents('.contextBT');
            let $modalDialog = $('.modal-dialog', $context);
            $modalDialog.empty().html(data);
            if (!dialogId) dialogId = '#modDialog';
            var $form = $context.find(dialogId + ' form');
            showModalForm($form, urlUpdate, $table, callbackAfterSubmit, dialogId);
        });
    }
};

function remove($table, urlDelete) {
    var selected = $table.find('tr.success').attr('data-uniqueid');
    if (selected !== undefined) {
        MessageBoxConfirm('Вы хотите удалить запись?').done(function () {
            $.post(urlDelete, { id: selected })
           .done(function (data) {
               refreshTable($table, data);
           }).fail(function () {
               MessageBox("Ошибка!");
           });
        });
    }
    else MessageBox("Строки не выбраны!");
};

function removeMany($table, urlRemove) {
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    var selected = $table.bootstrapTable('getSelections').map(function (v) {
        return v[nameId];
    });
    if (selected.length > 0) {
        MessageBoxConfirm('Вы хотите удалить записи?').done(function () {
            $.post(urlRemove, { ids: selected }, function (data) { refreshTable($table, data); });
        });
    } else MessageBox('Строки не выбраны!')
};

function creatAccount($table, url) {
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    var selected = $table.bootstrapTable('getSelections').map(function (v) {
        return v[nameId];
    });
    if (selected.length > 0) {
        $.post(url, { ids: selected }, function (data) { refreshTable($table, data) })
    } else MessageBox('Строки не выбраны!')
};

//
function onClickRowDefault(row, field, $element) {
    $context = $table.parents('.contextBT');
    $('.success', $context).removeClass('success');
    $($element).addClass('success');
    $("#update", $context).attr("disabled", false);
    $("#delete", $context).attr("disabled", false);
    if (!field.isNotEdit) {
        $("#update", $context).attr("disabled", false);
    } else {
        $("#update", $context).attr("disabled", true);
    }
    if (!field.isNotDelete) {
        $("#delete", $context).attr("disabled", false);
    } else {
        $("#delete", $context).attr("disabled", true);
    }
};

function appendTableRow($table, data) {
    if (!$table) return;
    $table.bootstrapTable('append', data);
};

function refreshTable($table, data) {
    $table.bootstrapTable('refresh');
};

function updateTableRow($table, data) {
    if (!$table) return;
    if ((!data) || (data.length == 0)) return;
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    $.each(data, function (index, value) {
        $table.bootstrapTable('updateByUniqueId', { id: value[nameId], row: value });
    });
};

function deleteTableRow(ids, $table) {
    if (!$table) return;
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    $table.bootstrapTable('remove', { field: nameId, values: ids });
};

//ресайз таблицы
function resizeTable($table) {
    var height_container = $table.parents('.contextBT').height();
    if ($table != null)
        $table.bootstrapTable('resetView', { height: height_container });
}

//Работа с формами редактирования
function showModalForm($form, url, $table, callback, dialogId, params) {
    $context = $table.parents('.contextBT');
    if (!dialogId) dialogId = '#modDialog';
    var $form = $context.find(dialogId + ' form');
    $('#submit-Btn', $form).off();
    $('#submit-Btn', $form).click(function (e) {
        e.preventDefault();
        var validator = $form.validate();
        if ($form.valid()) {
            let data = $form.serialize();
            //если есть дополнительные параметры
            if (params !== undefined)
                data = data + '&' + $.param(params);
            $.post(url, data)
                .done(function (answer) {
                    callback($table, answer);
                    $(dialogId, $context).modal('hide');
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    if (textStatus == 'timeout')
                        console.log('The server is not responding');
                    if (textStatus == 'error')
                    {
                        if (jqXHR.responseText != null)
                            MessageBox(jqXHR.responseText);
                    }
                        
                    //addErrorsValidation(parseAjaxRequestErrors(jqXHR.responseText));
                });
        }
        else {
            validator.focusInvalid();
        }
    });
    $(dialogId, $context).modal('show');
};

function clearForm($form) {
    if ($form.length == 0) return;
    $('.validation-summary-valid', $form).empty();
    $('.text-danger', $form).empty();

    var isReset = $form.find('input[isreset=True][type=hidden]');
    isReset.each(function (index, element) {
        setDefaultValue(element);
    });

    $form[0].reset();

    //checkbox
    $('input[type=checkbox]', $form)
        .removeAttr('checked')

    //radio
    $('input[type=radio]', $form)
        .removeAttr('selected')
};

//
function clearForm1($form) {
    if ($form.length == 0) return;
    $('.validation-summary-valid', $form).empty();
    $('.text-danger', $form).empty();

    $('input', $form)
        .not('button, submit, reset, hidden, disabled')
        .not('checkbox, radio').val('');
    
    //checkbox
    $('input[type=checkbox]', $form)
        .removeAttr('checked')

    //radio
    $('input[type=radio]', $form)
        .removeAttr('selected')

        
};

//устанавливает значение value dom элемента по умолчанию
function setDefaultValue(inputElement) {
    if (inputElement.hasAttribute('data-val-required')) {
        //console.log(" is requaired");
        if (inputElement.hasAttribute('data-val-number'))
            inputElement.value = 0;
        else
            inputElement.value = '';
    } else {
        inputElement.value = '';
    }
};

function FillForm(data, $form, afterCallback) {
    data = typeof data === 'string' ? JSON.parse(data) : data;
    clearForm($form);
    var prefix = "";
    fillingForm(data, $form, prefix);
    //for (var name in data) {
    //    if (typeof data[name] === 'object')

    //    $element = $('[name="' + name + '"]', $form[0]).first();
    //    switch ($element.attr("type")) {
    //        case "checkbox":
    //            {
    //                $element.val(true);
    //                if (data[name] == true) $element.attr("checked", "checked");
    //                $('[type="checkbox"]', $form[0]).val(true);
    //            }
    //            break;
    //        case "datetime": fillDatefild($element, data[name]);
    //            break;
    //        case "date": fillDatefild($element, data[name]); 
    //            break;
    //        default: $element.val(data[name]);
    //            break;
    //    }
    //}

    if (afterCallback !== undefined)
        afterCallback();
};

//Fill part form
function fillingForm(data, $form, prefix) {
    for (var name in data) {
        var obj = data[name];
        if (jQuery.type(data[name]) === "object") {
            var prefixNext = prefix + name + ".";
            fillingForm(data[name], $form, prefixNext)
        } else {
            $element = $('[name="' + prefix + name + '"]', $form[0]).first();
            switch ($element.attr("type")) {
                case "checkbox":
                    {
                        $element.val(true);
                        if (data[name] == true) $element.attr("checked", "checked");
                        $('[type="checkbox"]', $form[0]).val(true);
                    }
                    break;
                case "datetime": fillDatefild($element, data[name]);
                    break;
                case "date": fillDatefild($element, data[name]);
                    break;
                default: $element.val(data[name]);
                    break;
            }
        }
    }
};

function setValueInput($element, value) {
    switch ($element.attr("type")) {
        case "checkbox":
            {
                $element.val(true);
                if (data[name] == true) $element.attr("checked", "checked");
                $('[type="checkbox"]', $form[0]).val(true);
            }
            break;
        case "datetime": fillDatefild($element, data[name]);
            break;
        case "date": fillDatefild($element, data[name]);
            break;
        default: $element.val(data[name]);
            break;
    }

};

function fillDatefild($element, date) {
    if ($element.hasClass("datepicker")) {    
        let  datetime = parseJsonDate(date);       
        $element.datepicker("setDate", datetime);
    } else {
        $element.val(date);
    }
};

/*summaryValidation*/
//messages - это Массив
function addErrorsValidation(messages) {
    messages.forEach(function (mes) {
        $('.validationSummary').append("<p> ●" + mes);
        console.log(mes);
    });
};

function addErrorsRequestValidation(str) {
    var messages = parseAjaxRequestErrors(e);
    addErrorsValidation(messages);
};

//Парсинг строки с ошибками
function parseAjaxRequestErrors(str) {
    return str.split(" | ");
};

function getContext1() {
    var $context = $(this).closest('.contextBT');
    return $context;
};

function getContext($element) {
    var $context = $element.parents('.contextBT');
    if ($context == null)
        $context = $table.parents('.bootstrap-table');
    return $context;
};

function getTable($element) {
    var $context = getContext($element);
    var $table = $context.find('.table').last();
    return $table;
};

function getParentTable($childTable) {
    var parentTableId = $childTable.attr('data-perent-table');
    var $parentTable = $(parentTableId);
    return $parentTable;
};

function getToolbar($element) {
    var $context = getContext($element);
    var $table = getTable($element);
    var toolbarId = $table.attr('data-toolbar');
    var $toolbar = $context.find(toolbarId);
    return $toolbar;
};

function getForms($element) {
    var $context = getContext($element);
    var $form = $context.find('#modDialog form');
    return $form;
};

//Печать таблицы
function printPageBuilder(table, title) {
    var months = ["Января", "Февраля", "Марта", "Апреля", "Мая", "Июня",
            "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря"];
    var currentDate = new Date();
    var fullDate = currentDate.getDate() + " " + months[currentDate.getMonth()] + " " + currentDate.getFullYear();
    if (title === undefined) title = 'Журнал учителя';
    var header = '<div style="margin-left: 3%;margin-right: 3%;"><h3 style="float:left;">' + title + '</h3><h3 style="float:right;">"Облачный элемент" ' + fullDate + '</h3></div>';
    var style = "<style type=\"text/css\" media=\"print\"> @page { size: auto; margin: 25px 0 25px 0;  }  </style>  <style type=\"text/css\" media=\"all\"> table {  border-collapse: collapse; font-size: 12px; }  table, th, td { border: 1px solid grey; } th, td { text-align: center; vertical-align: middle; }  p { font-weight: bold; margin-left:20px; } table { width:94%;  margin-left:3%;    margin-right:3%;\n  } div.bs-table-print { text-align:center;  } </style>";
    var body = header + "<div class=\"bs-table-print\">".concat(table, "</div>");
    return "<html><head> </head>" + style + " <title>Print Table</title> <body>" + body + " </body> </html>";
};

//При выборе таба делать ресайз таблицы
$(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
    var tab = $(e.target);
    var contentId = tab.attr("href");
    var $table = $(contentId).find('.table').last();
    $table.bootstrapTable('refresh');
    resizeTable($table);
});

function refreshDetail() {
    //найти навигацию
    var tabDetail = $('#tabDetail').find('.active');
    if (tabDetail.length !== 0) {
        //найти главную таблицу во вкладке
        let contentId = tabDetail.attr("href");
        let $table = $(contentId).find('.table').last();
        //let idName = tabDetail.attr('for');
        //let $table = $(contentId).find(idName);
        //$table = tabDetail.find(idName);
        //перезагрузить ее
        $table.bootstrapTable('refresh');
        resizeTable($table);
    }
};

function updateTree(tree) {
    var $table = $(tree.masterTable);
    let uniqueId = $("#Pupils_table").find('tr.success').attr('data-uniqueid');
    if (uniqueId !== undefined) {
        refreshDetail();
    };
}

function buildTable(idTable, options) {
    let $table = $(idTable);
    $table.bootstrapTable('destroy').bootstrapTable(options)
};

function destroyTable(idTable) {
    let $table = $(idTable);
    $table.bootstrapTable('destroy');
};

/*Функции формата данных*/
function indexFormatter(value, row, index) {
    return index;
};

function charToBoolFormatter(value, row, index) {
    if (value == "1") return "Да";
    if (value == "0") return "Нет";
    return "Нет";
};

function inputDecimalFormatter(value, row, index, field) {
    return ['<input type="number" class="table-td-textbox inputCell"',
        " data-name=\"".concat(field, "\""), " data-value=\"".concat(value, "\""), " value=\"".concat(value, "\""), ' autofocus="autofocus">'].join('');
};

function dateFormatter(value, row, index) {
    return formatDate(parseJsonDate(value))
};

function priceFormatter(value, row, index) {
    if (value == null) value = 0;
    var str_val = value.toFixed(2) + '';
    var str = str_val.replace('.', ',');
    return str;
};

/*Форматирование данных*/
function parseJsonDate(jsonDateString) {
    if (jsonDateString == null) return null;
    if (jsonDateString == "") return "";
    //return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    return new Date(jsonDateString);
};

function formatDate(date) {
    if (date == null) return null;
    if (date == "") return "";

    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();

    return day + '.' + month + '.' + year;
};

function formatDate_input(date) {
    if (date == null) return null;
    if (date == "") return "";

    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();

    return year + '-' + month + '-' + day;
};

//подгрузка данных в селект
function loadSelect($element, url, data, emptyFild) {
    $.post(url, data,
        function (res) {
            $element.empty();
            var options = '';
            if (emptyFild) options += '<option value="">' + emptyFild + '</option>';
            for (var i = 0; i < res.length; i++) {
                options += '<option value="' + res[i].Value + '">' + res[i].Text + '</option>';
            }
            $element.html(options);
        })
};

function clearSelect($element, emptyFild) {
    $element.empty();
    var options = '';
    if (emptyFild) options += '<option value="">' + emptyFild + '</option>';
    $element.html(options);
};

//Cookie
function setCookie(cookieName, cookieValue, nDays) {
    var today = new Date();
    var expire = new Date();

    if (nDays == null || nDays == 0)
        nDays = 62;

    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + escape(cookieValue) + ";expires=" + expire.toGMTString();
};

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
};

//dialog modal
function MessageBoxConfirm(message) {
    return $.MessageBox({
        buttonDone: "Да",
        buttonFail: "Нет",
        message: message
    });
};

function MessageBox(message) {
    return $.MessageBox({
        message: message
    });
};

function GetColumsData($table, colums) {
    var dataTable = $table.bootstrapTable('getData');
    var data = new Array();
    $.each(dataTable, function (item, value) {
        let newValue = {};
        $.each(colums, function (i, param) {
            newValue[param] = value[param];
        });
        data.push(newValue);
    });

    return data;
};