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
- **Atualização parcial de produtos com PATCH (JSON Patch)**

## 📂 Estrutura do Projeto

```
ProductApi/
├── Controllers/
│   ├── ProductsController.cs
│   └── CategoriesController.cs
├── Models/
│   ├── Product.cs
│   └── Category.cs
├── Context/
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

---

## ✏️ Atualização Parcial de Produtos (`PATCH`)

A API permite atualizar parcialmente um produto usando o formato [JSON Patch](https://datatracker.ietf.org/doc/html/rfc6902).  
O endpoint recebe operações como `replace`, `add` e `remove` para modificar apenas os campos desejados.

### **Endpoint**
```http
PATCH /api/products/{id}/UpdatePartial
```

### **Exemplo de requisição**
```json
[
  {
    "path": "/stock",
    "op": "replace",
    "value": 16
  },
  {
    "path": "/price",
    "op": "replace",
    "value": 87.6
  }
]
```

- **path**: caminho da propriedade a ser alterada (respeitando a capitalização usada no DTO).
- **op**: operação (`replace`, `add` ou `remove`).
- **value**: valor a ser atribuído.

> É possível enviar **mais de uma alteração no mesmo request**.

### **Exemplo com cURL**
```bash
curl -X PATCH "https://localhost:5001/api/products/1/UpdatePartial" -H "Content-Type: application/json-patch+json" -d '[
  { "path": "/stock", "op": "replace", "value": 16 },
  { "path": "/price", "op": "replace", "value": 87.6 }
]'
```

### **Resposta**
```json
{
  "id": 1,
  "name": "Produto A",
  "price": 87.6,
  "stock": 16,
  "categoryName": "Categoria X"
}
```

---

## 📚 Fonte de Estudo

Este projeto segue o conteúdo do curso:

- [Curso Web API ASP .NET Core Essencial (.NET 8 / .NET 9)](https://www.udemy.com/course/curso-web-api-asp-net-core-essencial/) — José Carlos Macoratti

## 📄 Licença

Este projeto é livre para fins educacionais.
