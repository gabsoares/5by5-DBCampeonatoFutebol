CREATE TABLE CLUBE
(
	ID INT IDENTITY(1,1),
	NOME VARCHAR(50),
	APELIDO VARCHAR(50),
	DATA_CRIACAO DATE,
	CONSTRAINT PKCLUBE PRIMARY KEY (ID),
	CONSTRAINT UKCLUBE UNIQUE (NOME, APELIDO)
);

CREATE TABLE JOGO
(
	ID_JOGO INT IDENTITY(1,1),
	GOLS_CASA INT,
	GOLS_VISITA INT,
	GOLS_TOTAIS_JOGO INT,
	ID_CASA INT,
	ID_VISITA INT,
	CONSTRAINT PKCAMP PRIMARY KEY (ID_JOGO),
	CONSTRAINT FK_CLUBE_CASA FOREIGN KEY (ID_CASA) REFERENCES CLUBE(ID),
	CONSTRAINT FK_CLUBE_VISITA FOREIGN KEY (ID_VISITA) REFERENCES CLUBE(ID)
);

CREATE TABLE CLASSIFICACAO
(
	ID_CLUBE INT IDENTITY(1,1),
	GOLS_FEITOS INT,
	GOLS_SOFRIDOS INT,
	SALDO_GOLS INT,
	VITORIAS INT,
	DERROTAS INT,
	EMPATES INT,
	PONTUACAO INT,
	CONSTRAINT PKCLASSIFICACAO PRIMARY KEY (ID_CLUBE),
	CONSTRAINT FK_CLASSIFICACAO_CLUBE FOREIGN KEY (ID_CLUBE) REFERENCES CLUBE(ID)
);

DROP TABLE CLASSIFICACAO;
DROP TABLE JOGO;
DROP TABLE CLUBE;