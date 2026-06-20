# 🪙 Crypto Portfolio Tracker

A full-stack web application for tracking cryptocurrency portfolios in real-time. Users can search for cryptocurrencies, manage their portfolio, and view live market prices through integration with the CoinGecko API.

## 🎯 Project Overview

This project demonstrates a modern full-stack application with:
- **Backend**: ASP.NET Core 8.0 Web API with JWT authentication
- **Frontend**: React 19 with TypeScript and Tailwind CSS
- **Database**: SQL Server with Entity Framework Core
- **External API**: CoinGecko API integration for real-time crypto data

## ✨ Features

### Authentication & Authorization
- User registration and login with JWT tokens
- Secure password requirements (uppercase, lowercase, digits, minimum length)
- Protected routes requiring authentication
- Token-based API authorization

### Cryptocurrency Management
- Search cryptocurrencies using CoinGecko API
- View real-time market data (price, 24h change)
- Add cryptocurrencies to personal portfolio
- Track multiple crypto assets
- Add comments/notes for each asset

### Technical Features
- **Rate Limiting**: Caching for external API calls to prevent hitting rate limits
- **Global Error Handling**: Consistent error responses across the API
- **Input Validation**: DataAnnotations on all DTOs
- **CORS Configuration**: Proper cross-origin setup for frontend-backend communication
- **Repository Pattern**: Clean separation of concerns
- **Unit Tests**: Comprehensive test coverage for repositories

## 🛠️ Tech Stack

### Backend
- **ASP.NET Core 8.0** - Web API framework
- **Entity Framework Core 8.0** - ORM for database access
- **ASP.NET Identity** - User authentication and authorization
- **JWT Bearer Authentication** - Secure token-based auth
- **SQL Server** - Relational database
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking library for tests

### Frontend
- **React 19** - UI library
- **TypeScript** - Type-safe JavaScript
- **React Router 7** - Client-side routing
- **Axios** - HTTP client
- **Tailwind CSS 3** - Utility-first CSS framework
- **Context API** - State management

## 📁 Project Structure

```
CryptoProject/
├── api/                          # Backend ASP.NET Core API
│   ├── Controllers/              # API endpoints
│   ├── Services/                 # Business logic (CoinGecko integration)
│   ├── Repository/               # Data access layer
│   ├── Models/                   # Entity models
│   ├── Dtos/                     # Data transfer objects
│   ├── Interfaces/               # Repository interfaces
│   ├── InterfacesService/        # Service interfaces
│   ├── Middleware/               # Custom middleware (exception handling)
│   ├── Data/                     # Database context
│   ├── Migrations/               # EF Core migrations
│   └── Program.cs                # Application entry point
├── api.Tests/                    # Unit tests
│   ├── CryptoAssetRepositoryTests.cs
│   └── CryptoAssetControllerIntegrationTests.cs
└── frontend/                     # React TypeScript frontend
    └── src/
        ├── Components/           # Reusable UI components
        ├── Pages/                # Page components (Login, Register, Search)
        ├── Context/              # React Context (AuthContext)
        ├── Services/             # API service layer
        └── Routes/               # Route configuration
```

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) (or SQL Server Express)
- [CoinGecko API Key](https://www.coingecko.com/en/api) (free tier)

### Backend Setup

1. **Clone the repository**
```bash
git clone <your-repo-url>
cd CryptoProject/api
```

2. **Configure appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CryptoDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JWT": {
    "Issuer": "http://localhost:5000",
    "Audience": "http://localhost:5000",
    "SigningKey": "YourSecretKeyHereMustBeAtLeast32CharactersLong"
  },
  "CoinGecko": {
    "DemoApiKey": "your-coingecko-api-key"
  }
}
```

3. **Run database migrations**
```bash
dotnet ef database update
```

4. **Run the API**
```bash
dotnet run
```

The API will be available at `http://localhost:5000`
Swagger documentation: `http://localhost:5000/swagger`

### Frontend Setup

1. **Navigate to frontend directory**
```bash
cd ../frontend
```

2. **Install dependencies**
```bash
npm install
```

3. **Configure API URL**
Update `frontend/src/Services/api.service.ts` if your backend runs on a different port:
```typescript
const API_BASE_URL = 'http://localhost:5000/api';
```

4. **Run the development server**
```bash
npm start
```

The app will open at `http://localhost:3000`

## 🧪 Running Tests

### Backend Unit Tests
```bash
cd api.Tests
dotnet test
```

Expected output: `Passed: 10, Failed: 0`

## 📡 API Endpoints

### Authentication
- `POST /api/Account/register` - Register new user
- `POST /api/Account/login` - Login and get JWT token

### Crypto Assets
- `GET /api/CryptoAsset` - Get all crypto assets (requires auth)
- `GET /api/CryptoAsset/{id}` - Get asset by ID
- `GET /api/CryptoAsset/{id}/live` - Get asset with live market data
- `POST /api/CryptoAsset` - Create new asset
- `PUT /api/CryptoAsset/{id}` - Update asset
- `DELETE /api/CryptoAsset/{id}` - Delete asset

### Search
- `GET /api/Crypto/search?query={query}` - Search cryptocurrencies

### Comments
- `GET /api/Comment/{assetId}` - Get comments for asset
- `POST /api/Comment/{assetId}` - Add comment
- `PUT /api/Comment/{id}` - Update comment
- `DELETE /api/Comment/{id}` - Delete comment

## 🔐 Security Features

- **JWT Authentication**: Secure token-based authentication
- **Password Requirements**: Enforced strong password policy
- **CORS Configuration**: Restricted to frontend origin only
- **Input Validation**: All DTOs validated with DataAnnotations
- **Secure Secret Management**: Secrets stored in appsettings (not hardcoded)
- **Rate Limiting**: Prevents abuse of external API calls

## 🎨 Screenshots

### Login Page
Clean authentication interface with form validation

### Search Page
Real-time cryptocurrency search with live market data

### Portfolio View
Track your crypto assets with current prices and 24h changes

## 🤝 Contributing

This is a portfolio project, but feedback and suggestions are welcome!

## 📝 License

This project is open source and available under the [MIT License](LICENSE).

## 👤 Author

**Your Name**
- GitHub: [@your-github](https://github.com/your-github)
- LinkedIn: [Your LinkedIn](https://linkedin.com/in/your-profile)
- Email: mrkudik368@gmail.com

## 🙏 Acknowledgments

- [CoinGecko API](https://www.coingecko.com/) for cryptocurrency data
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [React Documentation](https://react.dev)

---

**Built with ❤️ as a portfolio project to demonstrate full-stack development skills**
