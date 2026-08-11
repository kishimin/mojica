# Mojica backend

ASP.NET Core 8 Web API と xUnit のTDD環境です。

## 実行

```powershell
dotnet run --project backend/Mojica.Api
```

## テスト

```powershell
dotnet test backend/Mojica.Api.Tests/Mojica.Api.Tests.csproj
```

## カバレッジ

```powershell
dotnet test backend/Mojica.Api.Tests/Mojica.Api.Tests.csproj --collect:"XPlat Code Coverage"
```

APIには動作確認用の `GET /health` エンドポイントを含みます。
