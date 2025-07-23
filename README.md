# Финальная проектная работа: Усиление безопасности BionicPRO

## Описание проекта

BionicPRO — российская компания, производящая бионические протезы. В результате утечки данных через уязвимость в SSO-механизме (Authorization Code Grant) компании потребовалось срочно улучшить безопасность системы.

### Цель проекта:
- Реализовать безопасный вход с использованием **PKCE**.
- Создать защищённый API `/reports`, доступный только пользователям с ролью `prothetic_user`.
- Обеспечить проверку JWT и корректную обработку ошибок 401 и 403.
- Обновить инфраструктуру через `docker-compose`.

## Что было сделано

### 1. Реализован PKCE в Keycloak и frontend-приложении

PKCE был включён в настройках клиента в Keycloak:

![alt text](<Screenshot 2025-07-23 192442.png>)

Клиент `reports-frontend` теперь использует PKCE (код-верификатор + code challenge) для авторизации в Keycloak.

---

### 2. API `/reports`, защищённый ролью

Бэкенд написан на C# с использованием ASP.NET и JWT middleware. Реализована RBAC-политика:

```csharp
options.AddPolicy("ProtheticOnly", policy =>
    policy.RequireClaim("roles", "prothetic_user"));
```

### 3. Примеры успешных и неуспешных обращений

✅ **Успешный запрос от пользователя с ролью prothetic_user:**

![alt text](<Screenshot 2025-07-23 191725.png>)

❌ **Запрос от пользователя без роли prothetic_user (403 Forbidden):**

![alt text](<Screenshot 2025-07-23 191901.png>)

❌ **Запрос с невалидным токеном (401 Unauthorized):**

![alt text](<Screenshot 2025-07-23 192412.png>)


## Как запустить

```bash
git clone <репозиторий>
cd <папка>
docker-compose up --build
```

### Доступные сервисы:
- **Frontend:** http://localhost:3000
- **Keycloak:** http://localhost:8080
- **API:** http://localhost:5000/reports

## Структура

- `frontend/` — приложение на React с PKCE.
- `reports-api/` — защищённый API с проверкой токенов.
- `docker-compose.yml` — сборка всей системы.
- `keycloak/realm-export.json` — конфигурация Keycloak.