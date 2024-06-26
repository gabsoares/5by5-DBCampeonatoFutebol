CREATE OR ALTER TRIGGER VERIFICA_TIME_DUPLICADO
ON JOGO
INSTEAD OF INSERT
AS
BEGIN
	DECLARE
	@ID_CASA INT,
	@ID_VISITA INT,
	@GOLS_CASA INT,
	@GOLS_VISITA INT

	SELECT @ID_CASA = ID_CASA, @ID_VISITA = ID_VISITA, @GOLS_CASA = GOLS_CASA, @GOLS_VISITA = GOLS_VISITA FROM INSERTED

	IF(@ID_CASA = @ID_VISITA)
	BEGIN
		PRINT('NAO PODE CADASTRAR O MESMO TIME PRA JOGAR CONTRA ELE MESMO');
		RETURN;
	END
	ELSE
	BEGIN
		INSERT INTO JOGO
		VALUES(@GOLS_CASA, @GOLS_VISITA, @ID_CASA, @ID_VISITA);
	END
END;