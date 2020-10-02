$(document).ready(function () {

    $("#subsystem").val(-1);

    $('#submit_remark').on('click', function () {

        $.ajax({
            url: '/Reports/GetRemarksData',
            data: {
                subsystem: $("#subsystem").val(),
            },
            method: 'POST',
            dataType: "html",
            success: (function (data) {
                if (data!="null")
                $("#remark_data_table").html(data);
            })
        })
            .fail(function () {
                $.MessageBox("Ошибка получения данных");
            });
    });


       $(document).on("click", "#print", function () {
                              
           var mywindow = window.open('', '', 'left=50,top=50,width=800,height=640,toolbar=0,scrollbars=1');
           var style = ".table_header > div:nth-child(3), #result_tabel tbody tr td:nth-child(3) {min-width:5em;}.table_header > div:nth-child(4), #result_tabel tbody tr td:nth-child(4) {min-width:3em;}table {border-collapse: collapse; width:100%;} table td, table th {border: 1px solid black;} table>tbody>tr>td:nth-child(n+4){text-align:center;}";

           var body = $("#remark_data_table").html();
      
           mywindow.document.write('<style>' + style + '</style></head><body >');
           mywindow.document.write('<div style="display:block;"><h3 style="text-align:center">Незакрытые замечания по отделу: ' + $("#subsystem option:selected").text()  + '</h3></div>');
          
           mywindow.document.write('<div><table id="subsystem">' + body + '</table></div>');
           mywindow.document.write('</body></html>');
           mywindow.document.close(); // necessary for IE >= 10
           mywindow.focus(); // necessary for IE >= 10
           mywindow.print();
           mywindow.close();
       });



       $('#export').on('click', function () {

                          
               $.download("/Reports/ExportRemarksData", "subsystem=" + $("#subsystem").val() );

       }); 

});