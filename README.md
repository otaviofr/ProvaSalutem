# ProvaSalutem
O projeto foi desenvolvido utilizando .NET 3.1.402, estrutura MVC, Ract, Typescript, MongoDB, Bootstrap, React, HTML, CSS. 
Para o BackEnd foi utilizado a estrutura MVC, linguagem C# e framework .NET 3.1.402.
Para o FrontEnd foi utilizado HTML, React, Typescript. Para a estilização foi utilizado Bootstrap e Reactstrap.
O projeto é uma API RESTFULL.

Para executar, deve executar o backend primeiro e o frontend depois.

Antes de tudo, é necessário criar um banco de dados no mongo db chamado 'Salutem', somente isso. Pois as collections serão criadas automaticamente dentro deste banco.

Caso não saiba como, instale o mongo db na maquina, baixe o mongo db compass, que é uma interface para utilização do mongo. COnecte localmente e no canto inferior direito, 
tem um + para criar um novo banco de dados. O nome deve ser exatamente 'Salutem', e a conexão local deve ser a padrão do mongo : localhost:27017.

Para o backend rodar corretamente, é necessário ter o mongo db instalado, para assim gerar uma base local e serem salvos os dados.

Para executar o backend:
    -Abra a pasta do projeto (Prova Salutem) no VS Code.
    -Vá até a aba DEBUG
    -Clique no ícone do play
 Após estes passos irá abrir uma pagina do Swagger, onde é possível visualizar os endpoints da API. Se quiser, pode fechar esta página.

 Após os passos acima é necessário executar o frontend.

 Para executar o frontend:
    -Abra a pasta prova_salutem_front em outra janela do VS Code
    -Digite o comando 'npm run start' e aguarde o navegador abrir

Após estes passos é só executar os testes desejados. 

OBS: 
Eu não consegui realizar a parte de exportação no front end, e não realizei a parte de executar distribuição, por tempo mesmo, pois se a faço nao teria tempo de estudar pro frontend.
Não era muito familiarizado com o frontend e precisei estudar para fazer este projeto.
O backend funciona completo até onde foi desenvolvido, para testar a parte de exportação, tem uma rota disponível no swagger, pagina que abre quando
roda o backend. É so colocar os possiveis filtros e realizar o download do arquivo. 
