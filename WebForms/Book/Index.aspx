<%@ Page Title="Книги" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebForms.Book.Index" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <h2>Книги</h2>
        <a href="Create.aspx" class="btn btn-primary">Добавить книгу</a>
        <asp:GridView ID="gvBooks" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="BookID">
            <Columns>
                <asp:BoundField DataField="Title" HeaderText="Название" />
                <asp:BoundField DataField="AuthorName" HeaderText="Автор" />
                <asp:BoundField DataField="PublisherName" HeaderText="Издательство" />
                <asp:BoundField DataField="PublishYear" HeaderText="Год" />
                <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                <asp:TemplateField HeaderText="Действия">
                    <ItemTemplate>
                        <a href='<%# Eval("BookID", "Details.aspx?id={0}") %>' class="btn btn-sm btn-primary">Посмотреть</a>
                        <a href='<%# Eval("BookID", "Edit.aspx?id={0}") %>' class="btn btn-sm btn-primary">Редактировать</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </main>
</asp:Content>