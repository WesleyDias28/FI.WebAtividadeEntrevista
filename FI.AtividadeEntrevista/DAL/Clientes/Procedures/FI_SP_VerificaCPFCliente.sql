﻿CREATE PROC FI_SP_VerificaCPFCliente
	@CPF VARCHAR(11),
	@ID BIGINT
AS
BEGIN
	SELECT 1 FROM CLIENTES WITH(NOLOCK) WHERE CPF = @CPF AND ID != @ID
END