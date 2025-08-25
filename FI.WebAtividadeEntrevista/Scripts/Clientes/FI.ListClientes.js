﻿
$(document).ready(function () {

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlClienteList,
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '50%'
                },
                Email: {
                    title: 'Email',
                    width: '35%'
                },
                Alterar: {
                    title: '',
                    display: function (data) {
                        return '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },
                Excluir: {
                    title: '',
                    display: function (data) {
                        return `<button class="btn btn-danger btn-sm btnExcluirCliente" data-id="${data.record.Id}">Excluir</button>`;
                    }
                }
            }
        });

    //Load student list from server
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');

    $('#gridClientes').on('click', '.btnExcluirCliente', function () {
        const id = $(this).data('id');

        if (!confirm("Tem certeza que deseja excluir este cliente?"))
            return;

        $.ajax({
            url: urlExclusao,
            type: 'POST',
            data: { id: id },
            success: function (response) {
                if (response.Result === "OK") {
                    ModalDialog("Sucesso", response.Message);
                    location.reload();
                } else {
                    ModalDialog("Erro: " + response.Message);
                }
            },
            error: function (response) {
                if (response.status === 400) {
                    const mensagem = response.responseJSON?.Message || "Erro ao tentar excluir o cliente";
                    ModalDialog("Erro", mensagem);
                }
                else
                    ModalDialog("Erro", "Ocorreu um erro inesperado.");
            }
        });
    });
})