document.addEventListener('DOMContentLoaded', function () {
    $('.btn-detalhes').on('click', function () {
        var id = $(this).data('id');
        console.log('Capturado id:', id);

        $.ajax({
            type: 'GET',
            url: 'InfoMaquina/' + id,
            success: (result) => {
                $('#infoPane').html(result);
            }, error: (xhr) => {
                var err = JSON.parse(xhr.responseText);
                alert(err.Message)
            }
        });

    });
});
        /*$('#infoPane').load('/Maquinas/InfoMaquina', { id: id });*/