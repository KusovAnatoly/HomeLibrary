# Home Library ASP.NET MVC App

**Home Library** — это веб-приложение для управления книгами, разработанное на ASP.NET MVC. Поддерживает CRUD-операции с использованием редактора TinyMCE.

---

## 🔧 Возможности

* Просмотр списка книг
* Добавление, редактирование и удаление книг
* Выбор автора и издательства из выпадающих списков
* Поддержка множественного выбора жанров
* Оглавление в формате HTML с визуальным редактором TinyMCE
* Просмотр карточки книги (детальный просмотр)

---

## 📁 Структура проекта

```
/Controllers
  └── BookController.cs
  └── HomeController.cs

/Views
  └── Book/
      ├── Create.cshtml
      ├── Details.cshtml
      ├── Edit.cshtml
      └── Index.cshtml
  └── Book/
      └──Index.cshtml

/Models
  ├── AuthorViewModel.cs
  ├── BookViewModel.cs
  ├── ErrorViewModel.cs
  └── GenreViewModel.cs

wwwroot/
  └── lib/
      └── tinymce/
```

---

## 🧩 Технологии

* ASP.NET Core MVC
* ADO.NET + SQL Server
* TinyMCE (HTML-редактор)
* Bootstrap (для базовой стилизации)
* Хранимые процедуры SQL: `GetBooks`, `InsertBook`, `UpdateBook`, `GetGenres`, `InsertBookGenre` и т.д.

---

## ⚙️ Установка и запуск

1. Создайте базу данных, и инициализируйте скрипты с данными и хранимыми процедурами из `Database Scripts`.

2. Склонируйте репозиторий:

   ```bash
   git clone https://github.com/KusovAnatoly/BookCatalog.git
   cd BookCatalog
   ```

3. Настройте строку подключения в `appsettings.json`:

   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=HomeLibrary;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. Проверьте наличие нужных хранимых процедур в базе данных.

5. Запустите проект в Visual Studio или через .NET CLI:

   ```bash
   dotnet run
   ```

6. Перейдите в браузере по адресу:

   ```
   http://localhost:7030/
   ```

---

## Примечания

* TinyMCE встроен локально в `wwwroot/lib/tinymce`.
* В проекте реализованы все операции с использованием хранимых процедур.

---

## Автор

Разработано на ASP.NET MVC
**\[Анатолием Кусовым]**