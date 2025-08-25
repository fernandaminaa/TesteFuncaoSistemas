
$(document).ready(function () {
    $('#CPF').mask('000.000.000-00', { reverse: true });
    $('#CPFBeneficiario').mask('000.000.000-00', { reverse: true });
    $('#Telefone').mask('(00) 00000-0000');

    $('#formCadastroCliente').submit(function (event) {
        event.preventDefault();
        incluirCliente();
    });
});

function incluirCliente() {
    var cliente = {
        Nome: $('#formCadastroCliente #Nome').val(),
        Sobrenome: $('#formCadastroCliente #Sobrenome').val(),
        CPF: $('#formCadastroCliente #CPF').val(),
        Nacionalidade: $('#formCadastroCliente #Nacionalidade').val(),
        CEP: $('#formCadastroCliente #CEP').val(),
        Estado: $('#formCadastroCliente #Estado').val(),
        Cidade: $('#formCadastroCliente #Cidade').val(),
        Logradouro: $('#formCadastroCliente #Logradouro').val(),
        Email: $('#formCadastroCliente #Email').val(),
        Telefone: $('#formCadastroCliente #Telefone').val(),
        Beneficiarios: []
    };

    $('#listaBeneficiarios tbody tr').each(function () {
        var beneficiario = {
            Id: $(this).find('td:eq(0)').text().trim(),
            CPF: $(this).find('td:eq(1)').text().trim(),
            Nome: $(this).find('td:eq(2)').text().trim()
        };

        cliente.Beneficiarios.push(beneficiario);
    });

    $.ajax({
        url: urlPost,
        type: 'POST',
        data: JSON.stringify(cliente),
        contentType: 'application/json',
        success: function (result) {
            const modalId = ModalDialog("Sucesso", "Cliente incluído com sucesso");

            $('#' + modalId).on('hidden.bs.modal', function () {
                window.location.href = urlRetorno;
            });
        },
        error: function (error) {
            const response = JSON.parse(error.responseText);
            ModalDialog("Erro", "Erro ao incluir cliente. Detalhes: " + response.Message);
        }
    });
}
