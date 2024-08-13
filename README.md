# CRUD Produtos

<<<<<<< HEAD
## Tecnologias
=======
## Tecnologias do projeto
>>>>>>> ba6f70cc64962c2830f259bfd3d7b87a96db8e05

- [.NET SDK](https://dotnet.microsoft.com/download) (versão 8.0)
- [Node.js](https://nodejs.org/) (A partir da versao 14)
- [Angular CLI](https://angular.io/cli) (Versao 16)
- [MySQL](https://www.mysql.com/downloads/) (Versao 8)

## Instalação

### Backend (.NET Core)

Clone o repositório:

- Dentro da pasta desejada abra o terminal de seguinte comando: 

   ```bash
   git clone https://github.com/ElisbertoJunior/CRUDProducts.git

- Assim que o repositório for clonado aparecera as estrutura de pastas
    * API
    * ProcuctClient

    ![Imagem](ProductClient/images/extruturaPastas.png)


- Dentro da pasta API está o Server side(Back-end) da aplicação
- Dentro da pasta ProductClient está o Client side(Front-end) da aplicação 
- Dentro da pasta API voce encontrara a as pastas
   * ProductAPI
   * ProductTests
   ![Imagem](ProductClient/images/pastaapi.png)

- Dentro de ProductAPI navegue até o arquivo appsettings.json e configure as duas strings 
de conexão conforme o local e as credenciais do seu MySQL
- Obs: No Program.cs eu explico por que de ter usado 2 strings de conexão

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=;User Id=seu_usuario;Password=sua_senha;Pooling=true;",
    "ConnectionRequests": "Server=localhost;Port=3306;Database=ProductDB;User Id=seu_usuario;Password=sua_senha;Pooling=true;"
}

``` 


* Não e necessário criar o banco ProductDB antes de rodar a aplicação, ela está configurada para criar o banco automaticamente
caso ele não exista assim que se conectar ao MySQL.

### Rodando testes

- Navegue até a pasta ProductTests e digite o seguinte comando para rodar os testes


```code
dotnet test
```
![Imagem](ProductClient/images/apitest.png)

### Rodando a API

- Para rodar a API basta navegar ate a pasta "ProductAPI" e digitar o seguintes comando:
```code
dotnet restore
dotnet build
dotnet run
```
- Obs: A API Rodara na porta 5167 caso ela não esteja disponível configure a porta desejada
e especifique isso no arquivo environment.ts do angular


![API rodando](ProductClient/images/apirodando.png)



### Swagger

- Depois da aplicação rodando para validação no Swagger basta seguir link apresentado no 
console seguido de "/swagger/index.html"
[http://localhost:5167/swagger/index.html](http://localhost:5167/swagger/index.html)

- Os endpoints da API possuem uma ApiKey que deve ser inserida no icone Authorize do Swagger
### ApiKey: Produtos 123456789
![API KEY](ProductClient/images/apikey.png)
<<<<<<< HEAD

### Frontend (Angular 16)

- Para configurar e rodar o front da aplicação navegue para dentro da pasta "ProductClient"
- Instale as dependências necessárias com o comando:
```code
npm install
```
- E confira se a pasta node_modules foi criada:
![Imagem](ProductClient/images/nodemodules.png)

- Com node_modules criada e todas as dependências instaladas basta seguir para os testes e execução 
do projeto

- Caso nao tenha o Angular CLI instalado, instale com o seguinte comando:
```code
npm install -g @angular/cli@16
```

#### Testes Unitários
- Para rodar os testes unitários digite o seguinte comando:
```code
ng test
```

#### Rodando projeto
- Para rodar o projeto digite o seguinte comando:
```code
ng serve
```

- E acesse o projeto em [http://localhost:4200](http://localhost:4200)

### Login e Senha
 - Projeto possui uma autenticação simples e mockada assim descrito na proposta do teste
 - Para acessar a página de produtos, basta inserir o usuário e senha descritos a abaixo
    * Usuario: Admin
    * Senha: 12345

#### Segue exemplo do projeto radando
![Gif](ProductClient/images/crudprodutos.gif)

### Conclusão
Como parte do processo de seleção para uma posição de analista de sistemas C#, este projeto de produtos CRUD foi desenvolvido. O objetivo principal foi desenvolver uma aplicação fullstack usando .NET Core no backend e Angular no frontend, integrando com um banco de dados MySQL

Os conceitos de arquitetura MVC, bem como boas práticas de programação e estruturação de projetos, foram aplicados durante o processo de desenvolvimento. Além disso, a aplicação tem uma interface amigável que facilita a interação do usuário.

Duvidas, sugestões ou criticas estou à disposição!
=======
>>>>>>> ba6f70cc64962c2830f259bfd3d7b87a96db8e05
