<%@ Page Title="Контакты" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebForms.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <address>
            <strong>GitHub:</strong>   <a href="https://github.com/KusovAnatoly">KusovAnatoly</a><br />
            <strong>Email:</strong> <a href="mailto:kusov.anatoly@gmail.com">kusov.anatoly@gmail.com</a>
        </address>
    </main>
</asp:Content>
