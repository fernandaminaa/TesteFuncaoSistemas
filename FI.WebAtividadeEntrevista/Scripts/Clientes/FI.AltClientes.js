$(document).ready(function () {
    $('#CPF').mask('000.000.000-00', { reverse: true });
    $('#CPFBeneficiario').mask('000.000.000-00', { reverse: true });
    $('#Telefone').mask('(00) 00000-0000');

    if (obj) {
        $('#formCadastroCliente #Nome').val(obj.Nome);
        $('#formCadastroCliente #CEP').val(obj.CEP);
        $('#formCadastroCliente #Email').val(obj.Email);
        $('#formCadastroCliente #Sobrenome').val(obj.Sobrenome);
        $('#formCadastroCliente #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastroCliente #Estado').val(obj.Estado);
        $('#formCadastroCliente #Cidade').val(obj.Cidade);
        $('#formCadastroCliente #Logradouro').val(obj.Logradouro);
        $('#formCadastroCliente #Telefone').val(obj.Telefone).mask('(00) 00000-0000');
        $('#formCadastroCliente #CPF').val(obj.CPF).mask('000.000.000-00');

        $('#listaBeneficiarios tbody').empty();

        $.each(obj.Beneficiarios, function (index, beneficiario) {
            var novaLinha = `
                <tr>
                    <td class="hidden-xs hidden">${beneficiario.Id}</td>
                    <td>${formatarCpf(beneficiario.CPF)}</td>
                    <td>${beneficiario.Nome}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-sm btn-primary btnAlterarBeneficiario" style="margin-right: 0.4rem">Alterar</button>
                        <button type="button" class="btn btn-sm btn-danger btnExcluirBeneficiario">Excluir</button>
                    </td>
                </tr>
            `;

            $('#listaBeneficiarios tbody').append(novaLinha);
        });

        $('#formCadastroCliente').submit(function (event) {
            event.preventDefault();
            alterarCliente();   
        });
    }
})

function alterarCliente() {
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
            const modalId = ModalDialog("Sucesso", "Cliente alterado com sucesso");

            $('#' + modalId).on('hidden.bs.modal', function () {
                window.location.href = urlRetorno;
            });
        },
        error: function (error) {
            const response = JSON.parse(error.responseText);
            ModalDialog("Erro", "Erro ao alterar cliente. Detalhes: " + response.Message);
        }
    });
}