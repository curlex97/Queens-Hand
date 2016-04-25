<%@ Page Title="Панель управления" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MainPanel.aspx.cs" Inherits="MainPanel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1><%: Title %></h1>
    <br/>
    <div class="row">
        <div class="col-md-6">
    <asp:Label ID="UserLabel" runat="server" Text="Label" class="lead"></asp:Label>  <br/> <br/>
    <asp:Button ID="Button2" OnClick="DownLoadForms" Width="100%" runat="server" CssClass="btn btn-primary"  Text="Скачивание форм заполнения" /> <h1></h1>
      <asp:Button ID="Button3" OnClick="UploadForms" Width="100%" runat="server" CssClass="btn btn-primary"  Text="Загрузка шрифта на сервер" /> <h1></h1>
      <asp:Button ID="Button4" OnClick="HandWriting" Width="100%" runat="server" CssClass="btn btn-success"  Text="Генерация изображения почерка" />  <h1></h1>
     <asp:Button ID="Button5" runat="server" OnClick="FontSettings_Button" Width="100%" CssClass="btn btn-warning"  Text="Настройка параметров почерка" />  <h1></h1>  
    <asp:Button ID="Button6" OnClick="ManageProfile" Width="100%" runat="server" class="btn btn-warning"  Text="Настройка профиля" />  <h1></h1>   <br/><br/>
    <asp:Button  ID="Button1" Width="100%"  class="btn btn-danger" OnClick="SignOut_Button" runat="server" Text="Выход" />
        </div>
        <div class="col-md-12">
            <asp:ListView  ID="UserList" runat="server"></asp:ListView>
            </div>
        </div>
</asp:Content>
