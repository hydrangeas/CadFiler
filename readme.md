# Features

このアプリケーションは[リモート BLOB ストア (RBS)](https://docs.microsoft.com/ja-jp/sql/relational-databases/blob/remote-blob-store-rbs-sql-server)が Azure SQL Database で使用できないため、同様の機能を実現するためのPOCです。DDDのテストも兼ねているので、ソリューション構成が少し大仰になっています。

# Quick Start

## Debugビルド/デバッグする場合
`Microsoft Azure Storage Emulator`を立ち上げ、LocalDBに以下のテーブルを作成しておく。
```sql
CREATE TABLE [dbo].[metadata] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [logical_name]  NVARCHAR (255)   NOT NULL,
    [physical_name] UNIQUEIDENTIFIER NOT NULL,
    [file_size]     INT              NOT NULL,
    [display_order] INT              NOT NULL,
    [created]       DATETIME2 (7)    NOT NULL,
    [updated]       DATETIME2 (7)    NOT NULL
);
```
Storage Emulator/LocalDB以外を使用する場合は、`CadFiler.Infrastructure/applicationsettings.debug.json`を書き換える。

## Releaseビルドする場合
`CadFiler.Infrastructure/applicationsettings.json`を作成し、「出力ディレクトリにコピー：常にコピーする」を設定する。設定内容は以下の通り。

```json:applicationsettings.json(CadFiler.Infrastructure)
{
  "ConnectionStrings": {
    "Azure.BlobStorage": "AccountName=devstoreaccount1;AccountKey=...",
    "Azure.SQLDatabase":  "Server=tcp:...",
  }
}
```
いずれもAzureの`Blob Storage`と`SQL Database`で動作確認をしています。


