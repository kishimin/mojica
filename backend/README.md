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

## 画像アップロード

`POST /images/frame-file` は `multipart/form-data` を受け付けます。
`frameImage` に画像、`foregroundCharacter`・`foregroundColor`・
`backgroundCharacter`・`backgroundColor` に描画設定を指定します。色は
`#RRGGBB` 形式です。成功時はPNGを返します。

画像の形式や寸法の検証はGlyph Forge側で行います。Glyph Forgeの固定エラー
ヘッダーを利用できる場合、不正画像と出力サイズ超過を区別します。ヘッダーが
ない旧バージョンや未知の値では、422を共通の入力エラーとして扱います。
リクエスト全体は2 MiBと64 KiBのフォーム領域を上限とします。
