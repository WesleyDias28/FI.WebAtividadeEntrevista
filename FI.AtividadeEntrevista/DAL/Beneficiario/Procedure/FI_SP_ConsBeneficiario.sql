CREATE PROC FI_SP_ConsBeneficiario
    @IdCliente INT
AS
BEGIN
    IF (ISNULL(@IdCliente, 0) = 0)
    SELECT 
            ID,
            IDCLIENTE,
            NOME,
            CPF
    FROM BENEFICIARIOS WITH (NOLOCK)
    ELSE
    SELECT 
            ID,
            IDCLIENTE,
            NOME,
            CPF
    FROM BENEFICIARIOS WITH (NOLOCK)
    WHERE IDCLIENTE = @IdCliente
END