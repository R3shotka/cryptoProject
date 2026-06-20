# Налаштування проєкту

## Backend (API)

1. Встановити залежності:
```bash
cd api
dotnet restore
```

2. Налаштувати секрети (для локальної розробки):
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=cryptomathew;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;Encrypt=False"
dotnet user-secrets set "JWT:SigningKey" "YOUR_GENERATED_KEY_AT_LEAST_256_BITS"
dotnet user-secrets set "JWT:Issuer" "http://localhost:5144"
dotnet user-secrets set "JWT:Audience" "http://localhost:5144"
```

Або згенерувати безпечний JWT ключ автоматично:
```bash
NEW_KEY=$(openssl rand -hex 64)
dotnet user-secrets set "JWT:SigningKey" "$NEW_KEY"
```

3. Запустити міграції БД:
```bash
dotnet ef database update
```

4. Запустити API:
```bash
dotnet run
```

## Frontend

1. Встановити залежності:
```bash
cd frontend
npm install
```

2. Створити `.env` файл:
```bash
cp .env.example .env
```

Відредагувати `.env` якщо API запущений не на `http://localhost:5144`.

3. Запустити фронтенд:
```bash
npm start
```

## Production

Для production не використовуйте `user-secrets`. Налаштуйте environment variables або Azure Key Vault.
