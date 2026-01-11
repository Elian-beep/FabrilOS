## Stack utilizada
| ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) | ![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white) |
|--------------|--|
| ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white) | ![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white) |

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Docker Desktop (ou Docker Engine + Compose)](https://www.docker.com/products/docker-desktop/)

## Instalação e execução

Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/FabrilOS.git
cd FabrilOS
```

Execução via Docker
```bash
docker-compose -f devops/docker-compose.yml up -d --build
```

Execução local
```bash
cd backend/FabrilOS.API
dotnet build
dotnet run
```

Executar migrations (apenas local)
```bash
cd FabrilOS.API
dotnet ef database update
```

Obs: A execução local esta configurada na porta 5102 e o container na porta 8080

## Documentação da API
[http://localhost:8080/swagger/index.html](http://localhost:5102/swagger/index.html)

### Registro e login de usuário
```http
  POST /api/Auth/register
```
#### Registro de usuário
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `fullName` | `string` | Nome completo do usuário |
| `email` | `string` | Email do usuário |
| `password` | `string` | Senha |
| `confirmPassword` | `string` | Repetição da senha |

#### Login
```http
  POST /api/Auth/login
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `email` | `string` | Email do usuário |
| `password` | `string` | Senha |

#### Registro de uma Ordem de Serviço
```http
  POST /api/ServiceOrder
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `title` | `string` | Título da OS |
| `description` | `string` | Descrição das atividades |
| `checklistItemIds` | `int[]` | Lista de ids de itens marcados no checklist |

#### Inserção de imagem
```http
  POST /api/ServiceOrder/{id}/images
```
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `id` | `string` | Id da ordem de serviço |
| `file` | `file` | Upload da imagem |

## Trabalhos futuros

- Aplicações de logs e monitoramento utilizando Serilog
- Validações de criação de usuário (senha)
- Exportação DOC do documento