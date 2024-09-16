$(document).ready(function () {
    let myChart;

    // Preset SQL Queries Change Event
    $("#presetQueries").change(function () {
        var selectedQuery = $(this).val();
        if (selectedQuery) {
            $("#sqlQuery").val(selectedQuery);
        }
    });

    // Chart Form Submit Event
    $("#selectedFunction").change(function () {
        var selectedFunction = $(this).val();
        $("#dynamicParams").empty();

        if (selectedFunction === "GetSalesWithinDateRange") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="startDate">Başlangıç Tarihi:</label>
                    <input type="date" class="form-control" id="startDate" required>
                </div>
                <div class="form-group">
                    <label for="endDate">Bitiş Tarihi:</label>
                    <input type="date" class="form-control" id="endDate" required>
                </div>
            `);

        }else if (selectedFunction === "GetSalesByProduct") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="productId">Product Id:</label>
                    <input type="number" class="form-control" id="productId" required>
                </div>
               
            `);
        } 
    });
    $("#chartForm").submit(function (event) {
        event.preventDefault();
        var server = $("#server").val();
        var database = $("#database").val();
        var trustedConnection = $("#trustedConnection").val();
        var chartType = $("#chartType").val();
        var selectedProcedure = $("#procedureSelect").val();
        var selectedView = $("#viewSelect").val();
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();
        var selectedFunction = $("#selectedFunction").val(); // Function seçimi
        var sqlQuery = selectedProcedure ? null : $("#sqlQuery").val();
        var selectedProcedure = $("#selectedProcedure").val();
        var amount = $("#amount").val();
        var dbConnection = `Server = ${server}; Database = ${database}; Trusted_Connection = ${trustedConnection };`;
        var productId = $("#productId").val(); // Eklenen yeni parametre
        console.log(productId);
        var requestData = {
            chartRequest: {
                dbConnection: dbConnection,
                chartType: chartType,
                selectedProcedure: selectedProcedure,
                selectedView: selectedView,
                selectedFunction: selectedFunction, // Function request'e ekliyoruz
                startDate: startDate,
                endDate: endDate,
                amount: amount ? parseFloat(amount) : null, // Boş ise null olarak gönder
                productId: productId ? parseInt(productId) : null // Yeni parametre: Boş ise null olarak gönder

            }
        };
        $.ajax({
            url: "/api/chartrequests/getchartdata",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(
                requestData
            )
            ,
            success: 
                function (response) {
                    console.log(response)
                    if (myChart) {
                        myChart.destroy();
                    }


                    var ctx = document.getElementById('myChart').getContext('2d');
                    myChart = new Chart(ctx, {
                        type: chartType,
                        data: {
                            labels: response.labels,
                            datasets: [{
                                label: 'Veri Kümesi',
                                data: response.data,
                                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1
                            }]
                        },
                        options: {
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    });
                },            
            error: function (error) {
                console.log("Bir hata oluştu: " + error.responseText);

            }
        });
    });
    // View parametreleri için dinamik alan ekleme
    $("#viewSelect").change(function () {
        var selectedView = $(this).val();
        $("#dynamicParams").empty();

       
    });


    $("#selectedProcedure").change(function () {
        var selectedProcedure = $(this).val();
        $("#dynamicParams").empty();


        if (selectedProcedure === "GetSalesDataByDateRange") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="startDate">Başlangıç Tarihi:</label>
                    <input type="date" class="form-control" id="startDate" required>
                </div>
                <div class="form-group">
                    <label for="endDate">Bitiş Tarihi:</label>
                    <input type="date" class="form-control" id="endDate" required>
                </div>
            `);
        }
        else if (selectedProcedure === "GetSalesAboveAmount") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="amount">Minimum Satış Tutarı:</label>
                    <input type="number" class="form-control" id="amount" required>
                </div>
            `);
        }
        else if (selectedProcedure === "GetSalesCountByDateRange") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="startDate">Başlangıç Tarihi:</label>
                    <input type="date" class="form-control" id="startDate" required>
                </div>
                <div class="form-group">
                    <label for="endDate">Bitiş Tarihi:</label>
                    <input type="date" class="form-control" id="endDate" required>
                </div>
            `);
        }
        else if (selectedProcedure === "GetSalesByProductId") {
            $("#dynamicParams").append(`
                <div class="form-group">
                    <label for="productId">Product Id:</label>
                    <input type="number" class="form-control" id="productId" required>
                </div>
               
            `);

        };
    });

    // Server, Database, Trusted Connection Change Event
    $("#server, #database, #trustedConnection").change(function () {
        var server = $("#server").val();
        var database = $("#database").val();
        var trustedConnection = $("#trustedConnection").val();

        // Eğer Server ve Database bilgileri girildiyse
        if (server && database) {
            var dbConnection = `Server=${server};Database=${database};Trusted_Connection=${trustedConnection};`;

            // AJAX İsteği ile Stored Procedures ve Views Listesini Al
            $.ajax({
                url: "/api/chartrequests/getProcedureViewsAndFunctions",
                method: "GET",
                data: { dbConnection: dbConnection }, 
                success: function (response) {
                    console.log(response);
                    // Stored Procedures Dropdown'ını Doldur
                    $("#selectedProcedure").empty().append('<option value="">Select Procedure</option>');
                    $.each(response.procedures, function (index, value) {
                        $("#selectedProcedure").append(`<option value="${value}">${value}</option>`);
                    });

                    // Views Dropdown'ını Doldur
                    $("#viewSelect").empty().append('<option value="">Select View</option>');
                    $.each(response.views, function (index, value) {
                        $("#viewSelect").append(`<option value="${value}">${value}</option>`);
                    });

                    // Functions Dropdown'ını Doldur
                    $("#selectedFunction").empty().append('<option value="">Select Function</option>');
                    $.each(response.functions, function (index, value) {
                        $("#selectedFunction").append(`<option value="${value}">${value}</option>`);
                    });
                },
                error: function (error) {
                    alert("Bir hata oluştu: " + error.responseText);
                }
            });
        }
    });
});
