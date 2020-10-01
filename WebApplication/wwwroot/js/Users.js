$(document).ready(function () { init() })

$(window).resize(function () {
    var $table = $('#Users_table');
    resizeTable($table);
});

function buildUsersTable($table, urlData) {
    $table.bootstrapTable('destroy').bootstrapTable({
        toolbar: "#toolbar_Users",
        uniqueId: 'id_user',
        idField: 'id_user',
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
                title: 'Логин',
                field: 'login',
                halign: 'center',
                align: 'left',
                valign: 'middle',
                sortable: true,
            },
            {
                title: 'Пароль',
                field: 'password',
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

    var $table = $('#Users_table');
    $context = $table.parents('.contextBT');
    $table.attr("data-unique-id", "id_user");
    buildUsersTable($table, '/Users/GetData');
    $form = $context.find('#modDialog form');

    resizeTable($table);
    //Навешивание обработчиков событий на кнопки
    $("#create_Users").on("click", function () {
        create($table, '/Users/Create');     
    });

    $("#update_Users").on("click", function () {  
        edit($table, '/Users/Get', '/Users/Update')     
    });

    $("#delete_Users").on("click", function () {
        removeMany($table, '/Users/DeleteMany')      
    });
     
    $("#update_Users").attr("disabled", true);
    $("#delete_Users").attr("disabled", true);
    
    $table.on('click-row.bs.table', onClickRowUsers);

    formInit($context);

    
}


function onClickRowUsers(row, field, $element) {
    $('.success').removeClass('success');
    $($element).addClass('success');

    $("#update_Users").attr("disabled", false);
    $("#delete_Users").attr("disabled", false);
    if (!field.ProtectEdit) {
        $("#update_Users").attr("disabled", false);
        $("#delete_Users").attr("disabled", false);
    } else {
        $("#update_Users").attr("disabled", true);
        $("#delete_Users").attr("disabled", true);
    };

    let IdUser = $("#Users_table").find('tr.success').attr('data-uniqueid');
    //
    if (IdUser !== undefined) {
        $('#down_panel_container').attr('hidden', false);
        $('form [name="id_user"]', '#down_panel').val(IdUser);

                      
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













