# FrontEnd MotorRent

Este é o frontend do sistema MotorRent, desenvolvido em Blazor Server para gerenciamento de locação de motocicletas.

## Funcionalidades

### 🏍️ Gestão de Motocicletas
- Cadastro de novas motocicletas
- Listagem de motocicletas cadastradas
- Visualização de detalhes
- Exclusão de motocicletas
- Destaque especial para motos do ano 2024

### 👥 Gestão de Entregadores
- Cadastro de entregadores
- Busca por CNPJ
- Atualização de imagem da CNH
- Validação de documentos

### 📋 Gestão de Locações
- Criação de novas locações
- Consulta de locações por ID
- Processo de devolução
- Cálculo automático de valores e multas

### 📊 Planos de Locação
- Visualização dos planos disponíveis
- Informações detalhadas sobre preços e multas
- 4 planos: 7, 15, 30 e 45 dias

## Tecnologias Utilizadas

- **ASP.NET Core 8.0**
- **Blazor Server**
- **Bootstrap 5** para styling
- **Bootstrap Icons** para ícones
- **Entity Framework Core** para persistência
- **System.Text.Json** para serialização

## Configuração

### Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code

### Configuração da API
No arquivo `appsettings.json`, configure a URL da API backend:

```json
{
  "ConnectionStrings": {
    "ApiUrl": "https://localhost:7001/"
  }
}
```

### Executando o Projeto

1. Clone o repositório
2. Navegue até a pasta do projeto FrontEndMotorRent
3. Execute o comando:
   ```bash
   dotnet run
   ```
4. Acesse https://localhost:7002 (ou a porta configurada)

## Estrutura do Projeto

```
FrontEndMotorRent/
├── Components/
│   ├── Layout/          # Layouts da aplicação
│   └── Pages/           # Páginas Blazor
├── Models/              # DTOs e modelos
├── Services/            # Serviços para comunicação com API
└── wwwroot/            # Arquivos estáticos
```

## Páginas Disponíveis

- **/** - Dashboard principal
- **/motorcycles** - Gerenciamento de motocicletas
- **/delivery-persons** - Gerenciamento de entregadores
- **/rentals** - Gerenciamento de locações
- **/rental-plans** - Visualização dos planos

## Comunicação com a API

O frontend se comunica com a API backend através de serviços HTTP:

- `MotorcycleService` - Operações com motocicletas
- `DeliveryPersonService` - Operações com entregadores
- `RentalService` - Operações com locações

Todos os serviços herdam de `ApiService` que fornece métodos básicos para HTTP requests.

## Interface do Usuário

- **Design Responsivo** - Funciona em desktop, tablet e mobile
- **Bootstrap Components** - Interface moderna e profissional
- **Feedback Visual** - Loading states, modais e alertas
- **Navegação Intuitiva** - Menu lateral com ícones

## Validação de Dados

- Validação client-side usando Data Annotations
- Mensagens de erro em português
- Validação em tempo real nos formulários

## Tratamento de Erros

- Mensagens de erro amigáveis ao usuário
- Feedback visual para operações bem-sucedidas
- Handling gracioso de falhas de comunicação com a API 