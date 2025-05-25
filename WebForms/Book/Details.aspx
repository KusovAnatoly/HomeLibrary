<%@ Page Title="Карточка книги" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="WebForms.Book.Details" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card mt-4 shadow-sm p-4 border rounded" style="max-width: 700px;">
        <h2 class="mb-3"><asp:Label ID="lblTitle" runat="server" /></h2>

        <p><strong>Автор:</strong> <asp:Label ID="lblAuthor" runat="server" /></p>
        <p><strong>Издательство:</strong> <asp:Label ID="lblPublisher" runat="server" /></p>
        <p><strong>Год издания:</strong> <asp:Label ID="lblYear" runat="server" /></p>
        <p><strong>ISBN:</strong> <asp:Label ID="lblISBN" runat="server" /></p>
        <p><strong>Жанры:</strong> <asp:Label ID="lblGenres" runat="server" /></p>

        <hr />
        <h4>Оглавление:</h4>
        <div class="toc-content border p-3" style="background-color:#f9f9f9;">
            <asp:Literal ID="litTableOfContents" runat="server" />
        </div>

        <div class="mt-4">
            <asp:HyperLink ID="lnkEdit" runat="server" CssClass="btn btn-primary">Редактировать</asp:HyperLink>
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Index.aspx" CssClass="btn btn-secondary">Назад к списку</asp:HyperLink>
        </div>
    </div>
</asp:Content>
