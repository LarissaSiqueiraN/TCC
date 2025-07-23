const mysql = require('mysql');

const conexao = mysql.createConnection({
  host: 'localhost',
  user: 'Larissa',
  password: 'Larissa.25',
  database: 'tcc_banco',
  port: 3308
});

conexao.connect((erro) => {
  if (erro) {
    console.error('Erro ao conectar:', erro);
    return;
  }
  console.log('Conectado ao MySQL!');

  conexao.query('SELECT * FROM sua_tabela', (erro, resultados) => {
    if (erro) throw erro;

    console.log('Resultados da tabela:');
    console.log(resultados);

    conexao.end();
  });
});
