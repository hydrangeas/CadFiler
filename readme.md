# Features

���̃A�v���P�[�V������[�����[�g BLOB �X�g�A (RBS)](https://docs.microsoft.com/ja-jp/sql/relational-databases/blob/remote-blob-store-rbs-sql-server)�� Azure SQL Database �Ŏg�p�ł��Ȃ����߁A���l�̋@�\���������邽�߂�POC�ł��BDDD�̃e�X�g�����˂Ă���̂ŁA�\�����[�V�����\����������ɂȂ��Ă��܂��B

# Quick Start

## Debug�r���h/�f�o�b�O����ꍇ
`Microsoft Azure Storage Emulator`�𗧂��グ�ALocalDB�Ɉȉ��̃e�[�u�����쐬���Ă����B
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
Storage Emulator/LocalDB�ȊO���g�p����ꍇ�́A`CadFiler.Infrastructure/applicationsettings.debug.json`������������B

## Release�r���h����ꍇ
`CadFiler.Infrastructure/applicationsettings.json`���쐬���A�u�o�̓f�B���N�g���ɃR�s�[�F��ɃR�s�[����v��ݒ肷��B�ݒ���e�͈ȉ��̒ʂ�B

```json:applicationsettings.json(CadFiler.Infrastructure)
{
  "ConnectionStrings": {
    "Azure.BlobStorage": "AccountName=devstoreaccount1;AccountKey=...",
    "Azure.SQLDatabase":  "Server=tcp:...",
  }
}
```
�������Azure��`Blob Storage`��`SQL Database`�œ���m�F�����Ă��܂��B


