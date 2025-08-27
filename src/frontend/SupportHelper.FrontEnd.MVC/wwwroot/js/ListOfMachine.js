document.addEventListener('DOMContentLoaded', function () {
    $(".btn-detalhes").on("click", () => {
        var id = $(this).attr('idEquipamento');

        $.ajax({
            type: 'GET',
            url: '/Machine/InfoMachine/' + id,
            success: (result) => {
                $("#detalhesEquipamento").html(result);
                $('#modalDetalhesEquipamentos').modal("show");
            }, error: (xhr) => {
                var err = JSON.parse(xhr.responseText);
                alert(err.Message)
            }
        });
    });

    $('.btn-clear').on('click', () => {
        let hostname = $(this).attr('valueHostname');

        
    })
});