## 🚀 Como Executar

### 📋 Pré-requisitos
- .NET 8.0 SDK
- Docker & Docker Compose
- PostgreSQL (via Docker)

### 🐳 1. Configurar Banco de Dados (Docker)
```bash
# Navegar para o diretório de infraestrutura
cd MotorRentService/Infra

# Iniciar PostgreSQL via Docker Compose
docker-compose up -d
```

### 🗃️ 2. Configurar Entity Framework
```bash
# Navegar para o projeto principal
cd ../../MotorRentService

# Instalar EF Tools (se não instalado)
dotnet tool install --global dotnet-ef

# Aplicar migrações
dotnet ef database update
```

### ⚡ 3. Executar a Aplicação
```bash
# Compilar e executar
dotnet build
dotnet run

# Ou executar diretamente
dotnet run --project MotorRentService
```

### 🔍 4. Verificar Execução
- **API:** `https://localhost:5100`
- **Swagger:** `https://localhost:5100/swagger`
- **Logs:** `MotorRentService/logs/MotorRental-{data}.txt`

### 🐰 5. Configurar RabbitMQ (Opcional)
Para funcionalidades de notificação:
```bash
# Adicionar RabbitMQ ao docker-compose.yml ou executar:
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### 🛠️ Comandos Úteis

**Entity Framework:**
```bash
# Criar nova migração
dotnet ef migrations add <NomeDaMigracao>

# Reverter migração
dotnet ef database update <MigracaoAnterior>

# Remover última migração
dotnet ef migrations remove
```

**Docker:**
```bash
# Parar containers
docker-compose down

# Reiniciar com rebuild
docker-compose up -d --build

# Ver logs do PostgreSQL
docker-compose logs postgres

# Listar os contêineres em execução
docker ps
```

**Aplicação:**
```bash
# Executar em modo desenvolvimento
dotnet run --environment Development

# Executar testes
dotnet test

# Publicar aplicação
dotnet publish -c Release
```

---

## 🔧 Configuração de Ambiente

### appsettings.json
```json
{
  "ConnectionStrings": {
    "MotorRentConnection": "Host=localhost;Database=motorrent;Username=postgres;Password=yourpassword"
  },
  "RabbitMqHost": "localhost",
  "RabbitMqPort": "5672"
}
```

### docker-compose.yml (Infra)
```yaml
version: '3.8'
services:
  postgres:
    image: postgres
    ports:
      - '5433:5432'
    environment:
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_USER=postgres
      - POSTGRES_DB=MotorRentDB
    volumes:
      - MotorRentDB_pg_data:/bitnami/postgresql

  rabbitmq:
    image: rabbitmq:4-management
    ports:
      - "15672:15672"
      - "5672:5672"
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest

volumes:
    MotorRentDB_pg_data:
```

---

## 🏗️ Estrutura do Projeto

