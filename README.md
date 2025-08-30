# 🏍️ MotorRent - Sistema de Locação de Motocicletas

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-purple.svg)](https://blazor.net/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-blue.svg)](https://www.postgresql.org/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.8+-orange.svg)](https://www.rabbitmq.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-blue.svg)](https://www.docker.com/)

Sistema completo para gerenciamento de locação de motocicletas, desenvolvido com arquitetura de microserviços usando .NET 8, Blazor Server, PostgreSQL e RabbitMQ.

## 🚀 Como Executar a Aplicação

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### 1️⃣ Clone o Repositório

```bash
git clone https://github.com/gabrielms96/MotorRent.git
cd MotorRent
```

### 2️⃣ Configuração com Docker (Recomendado)

```bash
# Subir todos os serviços
docker-compose up -d

# Verificar se os containers estão rodando
docker-compose ps
```

**Serviços disponíveis:**
- **Frontend:** http://localhost:7002
- **API Backend:** https://localhost:7001
- **PostgreSQL:** localhost:5432
- **RabbitMQ Management:** http://localhost:15672 (guest/guest)

### 3️⃣ Configuração Manual (Desenvolvimento)

#### Backend (MotorRentService)
```bash
cd MotorRentService

# Restaurar dependências
dotnet restore

# Executar migrações do banco
dotnet ef database update

# Executar a API
dotnet run --urls="https://localhost:7001"
```

#### Frontend (FrontEndMotorRent)
```bash
cd FrontEndMotorRent

# Restaurar dependências
dotnet restore

# Executar o frontend
dotnet run --urls="https://localhost:7002"
```

#### Serviço de Notificações
```bash
cd NotificationMotorRentService

# Restaurar dependências
dotnet restore

# Executar o serviço
dotnet run
```

### 4️⃣ Configuração do Banco de Dados

#### PostgreSQL Local
```bash
# Criar banco de dados
createdb motorrent_db

# String de conexão no appsettings.json
"DefaultConnection": "Host=localhost;Database=motorrent_db;Username=postgres;Password=sua_senha"
```

#### RabbitMQ Local
```bash
# Instalar RabbitMQ
# Windows: https://www.rabbitmq.com/install-windows.html
# Linux: sudo apt-get install rabbitmq-server
# macOS: brew install rabbitmq

# Iniciar serviço
sudo systemctl start rabbitmq-server
```

## 📊 Endpoints da API

### Motocicletas
```http
GET    /api/Motorcycle/GetAllMotorcycles
GET    /api/Motorcycle/{id}
POST   /api/Motorcycle
DELETE /api/Motorcycle/{id}
```

### Entregadores
```http
POST   /api/DeliveryPerson/CreateDeliveryPerson
GET    /api/DeliveryPerson/{cnpj}
POST   /api/DeliveryPerson/UpdateCNHImage/{cnpj}
```

### Locações
```http
POST   /api/teste/Rental/CreateRental
GET    /api/teste/Rental/{id}
PUT    /api/teste/Rental/ReturnRental
```

## 🐳 Docker Compose

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:13
    environment:
      POSTGRES_DB: motorrent_db
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
    ports:
      - "5432:5432"

  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "5672:5672"
      - "15672:15672"

  motorrent-api:
    build: ./MotorRentService
    ports:
      - "7001:80"
    depends_on:
      - postgres
      - rabbitmq

  motorrent-frontend:
    build: ./FrontEndMotorRent
    ports:
      - "7002:80"
    depends_on:
      - motorrent-api
```


## 🔧 Configurações

### appsettings.json (Backend)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=motorrent_db;Username=postgres;Password=postgres123"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  }
}
```

### appsettings.json (Frontend)
```json
{
  "ConnectionStrings": {
    "ApiUrl": "https://localhost:7001/"
  }
}
```

## 👨‍💻 Autor

**Gabriel MS** - [gabrielms96](https://github.com/gabrielms96)

---

⭐ Se este projeto te ajudou, considere dar uma estrela no repositório!
