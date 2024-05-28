CREATE OR ALTER PROC
INSERIR_TIME
@NOME VARCHAR(50),
@APELIDO VARCHAR(50),
@DATA_CRIACAO DATE
AS
BEGIN
	IF (SELECT COUNT(*) FROM CLUBE) >= 5
	BEGIN
		SELECT 'ERRO: LIMITE MAXIMO DE CLUBES ATINGIDO', 10
	END
	ELSE
	BEGIN
		INSERT INTO CLUBE
		VALUES(@NOME, @APELIDO, @DATA_CRIACAO);

		INSERT INTO CLASSIFICACAO
		VALUES(0,0,0,0,0,0,0);
		SELECT 'TIME CRIADO COM SUCESSO!'
	END;
END;

CREATE OR ALTER PROC
INSERIR_PARTIDA
@GOLS_CASA INT,
@GOLS_VISITA INT,
@ID_CASA INT,
@ID_VISITA INT
AS
BEGIN
	INSERT INTO JOGO (GOLS_CASA, GOLS_VISITA, ID_CASA, ID_VISITA)
	VALUES(@GOLS_CASA, @GOLS_VISITA, @ID_CASA, @ID_VISITA);

	UPDATE JOGO
	SET GOLS_TOTAIS_JOGO = @GOLS_CASA + @GOLS_VISITA
    WHERE ID_CASA = @ID_CASA AND ID_VISITA = @ID_VISITA;

--ATUALIZAR PONTUACAO DEPENDENDO SE O TIME � DA CASA OU VISITA (SE FOR CASA + 3 PONTOS, SE FOR VISITA + 5)
	IF(@GOLS_CASA > @GOLS_VISITA)
	BEGIN
		UPDATE CLASSIFICACAO
		SET PONTUACAO = PONTUACAO + 3,
		    VITORIAS = VITORIAS + 1
		WHERE ID_CLUBE = @ID_CASA
		UPDATE CLASSIFICACAO
		SET DERROTAS = DERROTAS + 1
		WHERE ID_CLUBE = @ID_VISITA
	END
	ELSE IF(@GOLS_VISITA > @GOLS_CASA)
	BEGIN
		UPDATE CLASSIFICACAO
		SET PONTUACAO = PONTUACAO + 5,
		    VITORIAS = VITORIAS + 1
		WHERE ID_CLUBE = @ID_VISITA
		UPDATE CLASSIFICACAO
		SET DERROTAS = DERROTAS + 1
		WHERE ID_CLUBE = @ID_CASA
	END
	ELSE
	BEGIN
		UPDATE CLASSIFICACAO
		SET PONTUACAO = PONTUACAO + 1
		WHERE ID_CLUBE = @ID_VISITA OR ID_CLUBE = @ID_CASA
		UPDATE CLASSIFICACAO
		SET EMPATES = EMPATES + 1
		WHERE ID_CLUBE = @ID_VISITA OR ID_CLUBE = @ID_CASA
	END;

	UPDATE CLASSIFICACAO
	SET GOLS_SOFRIDOS = GOLS_SOFRIDOS + @GOLS_CASA
	WHERE ID_CLUBE = @ID_VISITA

	UPDATE CLASSIFICACAO
	SET GOLS_SOFRIDOS = GOLS_SOFRIDOS + @GOLS_VISITA
	WHERE ID_CLUBE = @ID_CASA

	UPDATE CLASSIFICACAO
	SET GOLS_FEITOS = GOLS_FEITOS + @GOLS_CASA
	WHERE ID_CLUBE = @ID_CASA

	UPDATE CLASSIFICACAO
	SET GOLS_FEITOS = GOLS_FEITOS + @GOLS_VISITA
	WHERE ID_CLUBE = @ID_VISITA

	UPDATE CLASSIFICACAO
	SET SALDO_GOLS = GOLS_FEITOS - GOLS_SOFRIDOS
	WHERE ID_CLUBE = @ID_CASA

	UPDATE CLASSIFICACAO
	SET SALDO_GOLS = GOLS_FEITOS - GOLS_SOFRIDOS
	WHERE ID_CLUBE = @ID_VISITA

	SELECT 'PARTIDAS INSERIDAS COM SUCESSO!!!'
END;

CREATE OR ALTER PROC
EXCLUIR_DADOS
AS
BEGIN
	DELETE FROM CLASSIFICACAO;
	DELETE FROM JOGO;
	DELETE FROM CLUBE;

	DBCC CHECKIDENT ('CLASSIFICACAO', RESEED, 0);
	DBCC CHECKIDENT ('JOGO', RESEED, 0);
	DBCC CHECKIDENT ('CLUBE', RESEED, 0);

	SELECT 'PARTIDAS E TIMES RESETADOS COM SUCESSO!!!'
END;