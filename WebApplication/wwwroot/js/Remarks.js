$(document).ready(function () { init() })

$(window).resize(function () {
    var $table = $('#Remarks_table');
    resizeTable($table);
});

function buildRemarksTable($table, urlData) {
    $table.bootstrapTable('destroy').bootstrapTable({
        toolbar: "#toolbar_Remarks",
        uniqueId: 'id_remark',
        idField: 'id_remark',
        method: 'post',
        rowStyle: 'rowStyle2',
        url: urlData,
        showRefresh: true,       
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
                title: 'Модуль',
                field: 'subsystem',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
            {
                title: 'Создатель замечания',
                field: 'user_create',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
            {
                title: 'Ошибка',
                field: 'error',
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
                title: 'Текст замечания',
                field: 'text_remark',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
           
        ],
    })
}


function rowStyle2(row, index) {
    if (row.status == 'Закрыт')
        return { classes: 'weekend' }
    else if (row.status == 'Открыт')
        return { classes: 'work-gainsboro' }
    else return { classes: 'work' }
} 

//инициализация главной таблицы
function init() {

    var $table = $('#Remarks_table');
    $context = $table.parents('.contextBT');
    $table.attr("data-unique-id", "id_remark");
    buildRemarksTable($table, '/Remarks/GetData');
    $form = $context.find('#modDialog form');

    resizeTable($table);
    //Навешивание обработчиков событий на кнопки
    $("#create_Remarks").on("click", function () {
        create($table, '/Remarks/Create');     
    });

    $("#update_Remarks").on("click", function () {  
        edit($table, '/Remarks/Get', '/Remarks/Update')     
    });

    $("#delete_Remarks").on("click", function () {
        removeMany($table, '/Remarks/DeleteMany')      
    });
     
    $("#update_Remarks").attr("disabled", true);
    $("#delete_Remarks").attr("disabled", true);
    
    $table.on('click-row.bs.table', onClickRowRemarks);

    formInit($context);

    $('#Remarks_table').on('refresh.bs.table', function (params) {

     refreshRemarksDetail(-1);
    });
}

var tree = {
    masterTable: '#Remarks_table',
    detailTables: [
        {
            id: '#Tasks_table',
        },

    ],
};


function onClickRowRemarks(row, field, $element) {
    
    $('#RemarksTableContainer .success').removeClass('success');

    $($element).addClass('success');

    $("#update_Remarks").attr("disabled", false);
    $("#delete_Remarks").attr("disabled", false);
    if (!field.ProtectEdit) {
        $("#update_Remarks").attr("disabled", false);
        $("#delete_Remarks").attr("disabled", false);
    } else {
        $("#update_Remarks").attr("disabled", true);
        $("#delete_Remarks").attr("disabled", true);
    };

    let IdRemark = $("#Remarks_table").find('tr.success').attr('data-uniqueid');
    //
    if (IdRemark !== undefined) {
        $('#down_panel_container').attr('hidden', false);
        $('form [name="id_remark"]', '#down_panel').val(IdRemark);


        //Инициализация детальной части
        refreshRemarksDetail(IdRemark);
    };
}


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













