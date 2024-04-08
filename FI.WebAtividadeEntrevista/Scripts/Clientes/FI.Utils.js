function ModalDialogWithContent(title, content) {
    const random = Math.random().toString().replace('.', '');
    const dialog = `
        <div id="${random}" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">${title}</h4>
                    </div>
                    <div class="modal-body">
                        ${content}
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                    </div>
                </div>
            </div>
        </div>
     `;
    $('body').append(dialog);
    const modal = $(`#${random}`);
    modal.modal('show');

    modal.on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function ModalDialog(titulo, texto) {
    ModalDialogWithContent(titulo, `<p>${texto}</p>`);
}

function useState(initialState) {
    let state = initialState;

    const setState = (newState) => {
        state = newState;
    }

    const getState = () => {
        return state;
    }

    return [getState, setState];
}