CREATE PROC FI_SP_AltBeneficiario
    @IdCliente  INT,
    @Nome       VARCHAR(50),
    @CPF        VARCHAR(11)
AS
BEGIN
    UPDATE BENEFICIARIOS
    SET 
        Nome = @Nome,
        CPF  = @CPF
    WHERE IdCliente = @IdCliente

SELECT SCOPE_IDENTITY()
END