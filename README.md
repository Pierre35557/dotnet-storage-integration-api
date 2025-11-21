# Storage Integration API (SharePoint Only – v1)

**Storage Integration API** is a secure .NET 9 REST API that currently provides a controlled, rate-limited integration with **SharePoint Online via Microsoft Graph** for creating folder structures in document libraries.

> ⚠️ **Important**  
> This version of the API is **SharePoint-only**.  
> Support for Azure Blob Storage, AWS S3 and other providers is planned but **not implemented yet**.

This is a focused MVP designed to provide a clean, auditable integration point between external systems and SharePoint.

---

## 🌍 Live Test Endpoint

```
POST https://api.nexusit.dev/storage/directories
```

**Limitations:**

- ✅ SharePoint only
- ✅ Folder creation only
- ✅ Rate limited to **5 requests per minute**
- ❌ No file uploads (yet)
- ❌ No list / delete / move (yet)

---

## ✅ What this API currently does

- Creates folders in a SharePoint document library
- Supports nested directory structures
- Uses Microsoft Graph under the hood
- Requires explicit headers for tenant/app/drive
- Returns the actual SharePoint URL of the created folder

---

## 📦 Request Body (JSON)

```json
{
  "rootDirectory": "Projects/Support/System/",
  "folderName": "Test Folder 01"
}
```

| Property | Description |
|------|------|
| `rootDirectory` | Root folder inside your SharePoint Drive / Library |
| `folderName` | Folder to be created inside the root directory |

✅ Nested folders are supported inside `rootDirectory`

---

## 🔐 Required Headers (SharePoint)

| Header | Description |
|------|------|
| `x-tenant-id` | Microsoft Entra ID (Azure AD) Tenant ID for the SharePoint environment |
| `x-client-id` | SharePoint App Registration (Client ID) |
| `x-client-secret` | SharePoint App Registration Client Secret |
| `x-drive-id` | SharePoint Document Library (Drive) ID |
| `x-system-client-id` | Internal client/system identifier |
| `x-api-key` | API authentication key |
| `Content-Type` | `application/json` |

> Credentials are used only in-memory and are **never stored**.

### Demo values (public testing only)

Use these values when testing against the demo endpoint:

```bash
  x-system-client-id: DEMO
  x-api-key: 30d5b03d-7d77-48f0-8b3e-6e3ec3830b48
```

> These values are **rate limited (5 requests/min)** and **monitored**.  
> They must only be used against: `https://api.nexusit.dev/storage/directories`

---

## 🧪 Example (curl)

```bash
curl -X POST https://api.nexusit.dev/storage/directories \
  -H "Content-Type: application/json" \
  -H "x-tenant-id: <YOUR_TENANT_ID>" \
  -H "x-client-id: <YOUR_CLIENT_ID>" \
  -H "x-client-secret: <YOUR_CLIENT_SECRET>" \
  -H "x-drive-id: <YOUR_DRIVE_ID>" \
  -H "x-system-client-id: DEMO" \
  -H "x-api-key: 30d5b03d-7d77-48f0-8b3e-6e3ec3830b48" \
  -d '{
        "rootDirectory": "Projects/Support/System/",
        "folderName": "Test Folder 01"
      }'
```

---

## ✅ Example Response

```json
{
  "success": true,
  "message": "Record(s) successfully created.",
  "statusCode": 200,
  "data": "https://yourtenant.sharepoint.com/sites/YourSite/Shared%20Documents/Projects/Support/System/Test%20Folder%2001"
}
```

The `data` property contains the **direct URL to the created SharePoint folder**.

---

## 🏗 Current Architecture

The system is built using a clean and extensible design, so that additional storage providers can be added without breaking the API contract.

**Project structure:**

- `Api` – Controllers, middleware, authentication
- `Application` – DTOs, services, use cases
- `Infrastructure` – SharePoint (Microsoft Graph) implementation
- `Domain` – Interfaces and core models

### Future Provider Strategy

A future enhancement will introduce a provider resolver header:

```
x-provider: sharepoint | azure | aws | gcp
```

> ⚠️ This is **not active yet** — currently the API is locked to SharePoint only.

---

## 🗺 Roadmap

| Feature | Status |
|------|------|
| SharePoint folder creation | ✅ Live |
| SharePoint file upload | 🔜 Planned |
| Folder listing | 🔜 Planned |
| Folder deletion / rename | 🔜 Planned |
| Azure Blob Storage | 🔜 Planned |
| AWS S3 support | 🔜 Planned |
| Provider resolver | 🔜 Planned |
| Managed identity support | 🔜 Planned |

---

## 🔒 Security

- No secrets stored
- Headers validated per request
- Rate limiting enforced
- Intended for use behind IP filtering / private networking in production

---

## 🛠 Built With

- .NET 9
- ASP.NET Core Web API
- Microsoft Graph SDK
- Clean Architecture
- Hosted on Railway (current)
- Designed for multi-cloud expansion
