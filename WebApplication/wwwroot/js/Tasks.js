$(document).ready(function () { initTask() })

$(window).resize(function () {
    var $table = $('#Tasks_table');
    resizeTable($table);
});

function buildTaskTable($table, data) {
    $table.bootstrapTable('destroy').bootstrapTable({
        toolbar: "#toolbar_Tasks",
        uniqueId: 'id_task',
        rowStyle: 'rowStyle',
        idField: 'id_task',
        method: 'post',
        data: data,
        queryParams: 'queryParamsTasks',
        showRefresh: false,       
        locale: 'ru-RU',
        
        showColumns: true,
        showPrint: false,       
        clickToSelect: true,
        buttonsClass: 'primary',

        columns: [
            {
                checkbox: true,
            },
            {
                title: 'Дата создания',
                field: 'doc_date_string',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },           
            {
                title: 'Автор задачи',
                field: 'user_create',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },            
            {
                title: 'Статус',
                field: 'status',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
            {
                title: 'Текст задачи',
                field: 'text_task',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
           
        ],
    })
}

function queryParamsTasks(params) {

    params.id_remark = $("#Remarks_table").find('tr.success').attr('data-uniqueid');
    return params
}

function rowStyle(row, index) {
    if (row.status == 'Не выполнен')
        return { classes: 'weekend table-sm' }
    else if (row.status == 'Выполнен')
        return { classes: 'work-gainsboro table-sm' }
    else if (row.status == 'Черновик')
        return { classes: 'work table-sm' }
    else return { classes: 'work table-sm' }
} 

//инициализация главной таблицы
function initTask() {

    var $table = $('#Tasks_table');
    $context = $table.parents('.contextBT');
    $table.attr("data-unique-id", "id_task");
    //buildTaskTable($table, '/Task/GetData');
    $form = $context.find('#modDialog form');

    resizeTable($table);
    //Навешивание обработчиков событий на кнопки
    $("#create_Tasks").on("click", function () {
        create1($table, '/Tasks/Create');     
    });

    $("#update_Tasks").on("click", function () {  
        edit1($table, '/Tasks/Get', '/Tasks/Update')     
    });

    $("#delete_Tasks").on("click", function () {
        removeMany($table, '/Tasks/DeleteMany')      
    });

    $("#status2_Tasks").on("click", function () { editDetailMany($table, '/Tasks/UpdateMany', 2) });
    $("#status1_Tasks").on("click", function () { editDetailMany($table, '/Tasks/UpdateMany', 1) });
    $("#status0_Tasks").on("click", function () { editDetailMany($table, '/Tasks/UpdateMany', 0) });
     
    $("#update_Tasks").attr("disabled", true);
    $("#delete_Tasks").attr("disabled", true);
    
    $table.on('click-row.bs.table', onClickRowTask);

    formInit($context);
 
}


function onClickRowTask(row, field, $element) {
   
    $('#TasksTableContainer .success').removeClass('success');
  
    $($element).addClass('success');

    $("#update_Tasks").attr("disabled", false);
    $("#delete_Tasks").attr("disabled", false);
    
       
    };



//инициализация формы
function formInit($context) {

    $form = $context.find('#modDialog form');
    optionsDatepicker = {

        format: "dd.mm.yyyy",
        viewMode: "days",    
        todayHighlight: true,
        autoclose: true,
        language: 'ru',
    }

    $("#doc_date", $form).datepicker(optionsDatepicker);
    $("#doc_date", $form).datepicker("setDate", new Date());
   
}

function refreshRemarksDetail(id_remark) {

  
    if (id_remark == undefined)
        id_remark = $("#Remarks_table").find('tr.success').attr('data-uniqueid');

    $.post('/Tasks/GetData', {

        id_remark: id_remark
    })
        .done(function (data) {
            var $table = $('#Tasks_table');
            buildTaskTable($table, data);
            resizeTable($table);           
        });
};

$('#Tasks_table').on('refresh.bs.table', function (params) {

    let id_remark = $("#Remarks_table").find('tr.success').attr('data-uniqueid');

    refreshRemarksDetail(id_remark);
});

function editDetailMany($table, urlUpdate, status) {
    var nameId = $table.bootstrapTable('getOptions')["uniqueId"];
    var selected = $table.bootstrapTable('getSelections').map(function (v) {
        return v[nameId];
    });
    if (selected.length > 0) {
        $.post(urlUpdate, {
            status: status,
            id_tasks: selected
        }, function (data) {
                refreshRemarksDetail();
        });
    } else MessageBox("Строки не выбраны!");
};

//переопределение создания 
function create1($table, urlCreate, dialogId) {
    var $context = $table.parents('.contextBT');
    if (!dialogId) dialogId = '#modDialog';
    var $form = $context.find(dialogId + ' form');
    clearForm($form);
    showModalForm1($form, urlCreate, $table, refreshRemarksDetail, dialogId);
};


//Работа с формами редактирования
function showModalForm1($form, url, $table, callback, dialogId, params) {
    let id_remark = $("#Remarks_table").find('tr.success').attr('data-uniqueid');
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
                    callback(id_remark);
                    $(dialogId, $context).modal('hide');
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    if (textStatus == 'timeout')
                        console.log('The server is not responding');
                    if (textStatus == 'error') {
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


function edit1($table, urlGet, urlUpdate, dialogId) {
    var selected = $table.find('tr.success').attr('data-uniqueid');   
    if (selected !== undefined) {
        $.get(urlGet, { id: selected }, function (data) {
            $context = $table.parents('.contextBT');
            if (!dialogId) dialogId = '#modDialog';
            var $form = $context.find(dialogId + ' form');

            FillForm(data, $form, showModalForm1($form, urlUpdate, $table, refreshRemarksDetail, dialogId));

        });
    } else MessageBox("Строки не выбраны!");
};










