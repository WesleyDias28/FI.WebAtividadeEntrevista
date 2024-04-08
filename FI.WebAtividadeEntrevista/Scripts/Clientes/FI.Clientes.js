$(document).ready(function () {
    const [beneficiarios, setBeneficiarios] = useState([]);

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CPF": $(this).find("#CPF").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "Beneficiarios": beneficiarios()
            },
            error:
                function (r) {
                    switch (r.status) {
                        case 400:
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                            break;
                        case 500:
                        default:
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                            break;
                    }
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    })

    const Beneficiario = $('#formCadastro #beneficiario')[0];

    beneficiario.onclick = function (e) {
        e.preventDefault();

        ModalDialogWithContent("Beneficiário", `
            <form id="formBeneficiario">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="CPFBeneficiario">CPF</label>
                            <input type="text" class="form-control" id="cpf-input" placeholder="CPF" maxlength="14">
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="NomeBeneficiario">Nome</label>
                            <input type="text" class="form-control" id="name-input" placeholder="Nome">
                        </div>
                    </div>
                    <div class="col-md-4">
                        <button type="submit" id="addBeneficiario" class="btn btn-success btn-block" style="margin-top: 25px;">Incluir</button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>CPF</th>
                                    <th>Nome</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="beneficiarios">
                            </tbody>
                        </table>
                    </div>
                </div>
            </form>
        `);

        const beneficiariosElementsList = document.getElementById('beneficiarios');
        const addBeneficiario = document.getElementById('addBeneficiario');
        const form = document.getElementById('formBeneficiario');

        const cpfInput = document.getElementById('cpf-input');
        const nameInput = document.getElementById('name-input');

        let id = 0;

        cpfInput.addEventListener('keyup', () => {
            const cpf = cpfInput.value;
            cpfInput.value = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        });

        cpfInput.addEventListener('keypress', (e) => {
            if (isNaN(e.key)) e.preventDefault();
        });

        const addRow = (cpf, nome, id) => {
            if (!Number.isInteger(id)) throw new Error("Id must be an integer.");

            const rowElement = document.createElement('tr');
            rowElement.setAttribute('data-cpf', cpf);
            rowElement.setAttribute('data-nome', nome);
            rowElement.setAttribute('data-id', id || ++id);

            const cpfColumn = document.createElement('td');
            cpfColumn.innerText = cpf;
            rowElement.append(cpfColumn);

            const nomeColumn = document.createElement('td');
            nomeColumn.innerText = nome;
            rowElement.append(nomeColumn);

            const actionsColumn = document.createElement('td');
            rowElement.append(actionsColumn);

            const editButton = document.createElement('button');
            editButton.innerText = 'Editar';
            editButton.classList.add('btn', 'btn-primary', 'btn-sm', 'mr-1');
            actionsColumn.append(editButton);

            const removeButton = document.createElement('button');
            removeButton.innerText = 'Remover';
            removeButton.classList.add('btn', 'btn-danger', 'btn-sm');
            actionsColumn.append(removeButton);

            editButton.addEventListener('click', () => {
                const id = rowElement.getAttribute('data-id');
                const cpf = rowElement.getAttribute('data-cpf');
                const nome = rowElement.getAttribute('data-nome');

                setBeneficiarios(beneficiarios().filter(b => b.Id === id));

                cpfInput.value = cpf;
                nameInput.value = nome;

                rowElement.parentNode.removeChild(rowElement);
            });

            removeButton.addEventListener('click', () => {
                const id = rowElement.dataset.id;

                setBeneficiarios(beneficiarios().filter(b => b.Id !== id));

                rowElement.parentNode.removeChild(rowElement);
            });

            return rowElement;
        }

        const populateTable = (target, data) => {
            if (!Array.isArray(data)) {
                throw new Error("Data must be an array.");
            }

            target.append(...data.map(b => addRow(b.CPF, b.Nome, b.Id)));
        }

        populateTable(beneficiariosElementsList, beneficiarios());

        form.onsubmit = (e) => {
            e.preventDefault();
        }

        console.log(beneficiarios());

        addBeneficiario.addEventListener("click", () => {
            e.stopPropagation();

            if (cpfInput.value === "" || nameInput.value === "") {
                ModalDialog("Erro", "Preencha todos os campos.");
                return;
            }

            if (beneficiarios().find(b => b.CPF === cpfInput.value)) {
                ModalDialog("Erro", "CPF já cadastrado.");
                return;
            }

            setBeneficiarios([...beneficiarios(), { Id: id, CPF: cpfInput.value, Nome: nameInput.value }]);

            beneficiariosElementsList.append(addRow(cpfInput.value, nameInput.value, id));

            form.reset();
        });
    }
});