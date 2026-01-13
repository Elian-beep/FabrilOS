-- Create de tabelas
CREATE TABLE Produto (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, descricao VARCHAR(255) NOT NULL, unidadeMedida VARCHAR(10), valor DECIMAL(10, 2), tamanho DECIMAL(10, 4));
CREATE TABLE Armazem (id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY, nome VARCHAR(100) NOT NULL, espacoDisponivel DECIMAL(10, 4));
CREATE TABLE ProdxArmazem ( idProduto INT NOT NULL, idArmazem INT NOT NULL, qtd DECIMAL(12, 3),
	PRIMARY KEY (idProduto, idArmazem),
	CONSTRAINT FK_Produto FOREIGN KEY (IdProduto) REFERENCES Produto(Id),
	CONSTRAINT FK_Armazem FOREIGN KEY (IdArmazem) REFERENCES Armazem(Id)
);

-- Seeds
INSERT INTO Produto (descricao, unidadeMedida, valor, tamanho) VALUES 
('Cimento CP II', 'saco', 35.00, 0.05),
('Tijolo 8 furos', 'milheiro', 850.00, 2.00),
('Piso Porcelanato', 'cx', 120.00, 1.50);
INSERT INTO Produto (descricao, unidadeMedida, valor, tamanho) VALUES ('Torneiras metálicas', 'cx', 75.00, 4.00);

INSERT INTO Armazem (nome, espacoDisponivel) VALUES 
('Galpão Zona Norte', 500.00),
('Galpão Centro', 200.00);
INSERT INTO Armazem (nome, espacoDisponivel) VALUES ('Galpão Zona Sul', 100.00);

INSERT INTO ProdxArmazem (idProduto, idArmazem, qtd) VALUES 
(1, 1, 100),
(2, 1, 50),
(3, 2, 20),
(1, 2, 10);

-- Listagem de totais por armazém
SELECT A.nome, SUM(PA.qtd * P.tamanho) AS TotalOcupado FROM Armazem A
	JOIN ProdxArmazem PA ON A.id = PA.idArmazem
	JOIN Produto P ON PA.idProduto = P.id
	GROUP BY A.nome;

-- Função para busca de armazém por produto
CREATE OR REPLACE FUNCTION Top5ArmazensPorProduto(target_produto_id INT)
RETURNS TABLE (
    NomeArmazem VARCHAR, 
    Quantidade DECIMAL
) 
AS $$
BEGIN
    RETURN QUERY
    SELECT A.nome, PA.qtd FROM Armazem A
    	JOIN ProdxArmazem PA ON A.id = PA.idArmazem WHERE PA.idProduto = target_produto_id ORDER BY PA.qtd DESC LIMIT 5;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM Top5ArmazensPorProduto(1);

-- Relatório dos produtos espalhados
SELECT P.descricao, COUNT(PA.idArmazem) AS Presenca FROM Produto P
	JOIN ProdxArmazem PA ON P.id = PA.idProduto GROUP BY P.id, P.descricao ORDER BY Presenca DESC;

-- Busca de produto sem armazém
SELECT P.id, P.descricao FROM Produto P
	LEFT JOIN ProdxArmazem PA ON P.id = PA.idProduto
	WHERE PA.idProduto IS NULL;

-- Listagem de armazém com valores
SELECT A.nome AS Armazem, SUM(PA.qtd * P.valor) AS Valor FROM Armazem A
	JOIN ProdxArmazem PA ON A.id = PA.idArmazem
	JOIN Produto P ON PA.idProduto = P.id
	GROUP BY A.id, A.nome ORDER BY Valor DESC;