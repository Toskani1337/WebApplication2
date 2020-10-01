$(document).ready(function () { init() })

$(window).resize(function () {
    var $table = $('#Subsysteams_table');
    resizeTable($table);
});

function buildSubsysteamsTable($table, urlData) {
    $table.bootstrapTable('destroy').bootstrapTable({
        toolbar: "#toolbar_Subsysteams",
        uniqueId: 'id_subsystem',
        idField: 'id_subsystem',
        method: 'post',
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
                title: 'Модуль',
                field: 'name',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            }
           
        ],
    })
}

//инициализация главной таблицы
function init() {

    var $table = $('#Subsysteams_table');
    $context = $table.parents('.contextBT');
    $table.attr("data-unique-id", "id_subsystem");
    buildSubsysteamsTable($table, '/Subsysteams/GetData');
    $form = $context.find('#modDialog form');

    resizeTable($table);
    //Навешивание обработчиков событий на кнопки
    $("#create_Subsysteams").on("click", function () {
        create($table, '/Subsysteams/Create');     
    });

    $("#update_Subsysteams").on("click", function () {  
        edit($table, '/Subsysteams/Get', '/Subsysteams/Update')     
    });

    $("#delete_Subsysteams").on("click", function () {
        removeMany($table, '/Subsysteams/DeleteMany')      
    });
     
    $("#update_Subsysteams").attr("disabled", true);
    $("#delete_Subsysteams").attr("disabled", true);
    
    $table.on('click-row.bs.table', onClickRowSubsysteams);

    formInit($context);

    
}


function onClickRowSubsysteams(row, field, $element) {
    $('.success').removeClass('success');
    $($element).addClass('success');

    $("#update_Subsysteams").attr("disabled", false);
    $("#delete_Subsysteams").attr("disabled", false);
    if (!field.ProtectEdit) {
        $("#update_Subsysteams").attr("disabled", false);
        $("#delete_Subsysteams").attr("disabled", false);
    } else {
        $("#update_Subsysteams").attr("disabled", true);
        $("#delete_Subsysteams").attr("disabled", true);
    };

    let IdSubsystem = $("#Subsysteams_table").find('tr.success').attr('data-uniqueid');
    //
    if (IdSubsystem !== undefined) {
        $('#down_panel_container').attr('hidden', false);
        $('form [name="id_subsystem"]', '#down_panel').val(IdSubsystem);

                      
    };
}


//инициализация формы
function formInit($context) {

    $form = $context.find('#modDialog form');
    optionsDatepicker = {

        format: "M yyyy",
        viewMode: "months",
        minViewMode: "months",
        todayHighlight: true,
        autoclose: true,
        language: 'ru',
    }
   
}













