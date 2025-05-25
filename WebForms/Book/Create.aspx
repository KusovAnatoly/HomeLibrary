<%@ Page Title="Добавить книгу" ValidateRequest="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="WebForms.Book.Create" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Добавить книгу</h2>

    <asp:Literal ID="litMessage" runat="server" />

    <div>
        <label class="form-label">Заголовок</label>
        <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server" />
    </div>

    <div>
        <label class="form-label">Автор</label>
        <asp:DropDownList ID="ddlAuthor" CssClass="form-control" runat="server" />
    </div>

    <div>
        <label class="form-label">Издательство</label>
        <asp:DropDownList ID="ddlPublisher" CssClass="form-control" runat="server" />
    </div>

    <div>
        <label class="form-label">Год публикации</label>
        <asp:TextBox ID="txtYear" CssClass="form-control" runat="server" />
    </div>

    <div>
        <label class="form-label">ISBN</label>
        <asp:TextBox ID="txtISBN" CssClass="form-control" runat="server" />
    </div>

    <div>
        <label class="form-label">Жанры</label>
        <asp:ListBox ID="lstGenres" CssClass="form-control" SelectionMode="Multiple" runat="server" />
    </div>

    <div>
        <label class="form-label">Оглавление</label>
        <textarea id="tableOfContents" name="tableOfContents" class="form-control" runat="server"></textarea>
    </div>

    <asp:Button
        ID="btnSave"
        Text="Сохранить"
        CssClass="btn btn-primary mt-3"
        OnClick="btnSave_Click"
        OnClientClick="tinymce.triggerSave();"
        runat="server" />

    <script src="/Scripts/tinymce/tinymce.min.js" referrerpolicy="origin"></script>
    <script>
        tinymce.init({
            selector: '#<%= tableOfContents.ClientID %>',
            plugins: 'link lists code',
            toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright | bullist numlist | link | code'
        });
    </script>
</asp:Content>
