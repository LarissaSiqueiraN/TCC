use cegonhasBrincos;

CREATE TABLE produtos_encomenda (
    fk_produto VARCHAR(55),
    fk_Encomenda INT,
    PRIMARY KEY (fk_produto, fk_Encomenda),
    FOREIGN KEY (fk_produto) REFERENCES produto(produtoId),
    FOREIGN KEY (fk_Encomenda) REFERENCES encomenda(numeroEncmenda)
);