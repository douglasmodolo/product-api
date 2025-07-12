# 📦 Product API

API RESTful desenvolvida em ASP.NET Core para gerenciamento de produtos e categorias.  
Este projeto foi criado como parte do curso **"Web API ASP .NET Core Essencial (.NET 8 / .NET 9)"** do professor **José Carlos Macoratti** na Udemy.

## 🚀 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- ASP.NET Core Web API
- Entity Framework Core (EF Core)
- [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) (provider para MySQL)
- Swagger / Swashbuckle (para documentação da API)
- MySQL Server

## 📁 Funcionalidades

- CRUD completo de **Produtos**
- CRUD completo de **Categorias**
- Relacionamento entre produtos e categorias
- Validações básicas
- Documentação interativa com Swagger

## 📂 Estrutura do Projeto

```
ProductApi/
├── Controllers/
│   ├── ProductsController.cs
│   └── CategoriesController.cs
├── Models/
│   ├── Product.cs
│   └── Category.cs
├── Data/
│   └── AppDbContext.cs
├── Program.cs
└── ...
```

## 🔧 Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/douglasmodolo/product-api.git
   cd product-api
   ```

2. Restaure os pacotes:
   ```bash
   dotnet restore
   ```

3. Configure a **string de conexão** no `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;port=3306;database=DataBaseNameAqui;user=root;password=SuaSenhaAqui;"
   }
   ```

4. Aplique as migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Execute a aplicação:
   ```bash
   dotnet run
   ```

6. Acesse a documentação Swagger:
   ```
   https://localhost:xxxx/swagger
   ```

## 📚 Fonte de Estudo

Este projeto segue o conteúdo do curso:

- [Curso Web API ASP .NET Core Essencial (.NET 8 / .NET 9)](https://www.udemy.com/course/curso-web-api-asp-net-core-essencial/) — José Carlos Macoratti

## 📄 Licença

Este projeto é livre para fins educacionais.
