CREATE PROC FI_SP_DeleteBeneficiario
    @IdCliente INT
AS
BEGIN
    DELETE BENEFICIARIOS WHERE IdCliente = @IdCliente
END