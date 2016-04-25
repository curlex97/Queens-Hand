<%@ Page Title="Загрузка формы заполнения" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="UploadFont.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <br/>    <br/>
    <div class="row">
     <div class="form-group">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="FileUpload1" CssClass="col-md-2 control-label">Прописные буквы</asp:Label>
                        <div class="col-md-10">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                    </div>
        <br/>
        <div class="form-group">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="FileUpload2" CssClass="col-md-2 control-label">Заглавные буквы</asp:Label>
                        <div class="col-md-10">
                            <asp:FileUpload ID="FileUpload2" runat="server" />
                        </div>
                    </div>
        <br/>
        <div class="form-group">
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="FileUpload3" CssClass="col-md-2 control-label">Специальные символы</asp:Label>
                        <div class="col-md-10">
                            <asp:FileUpload ID="FileUpload3" runat="server" />
                        </div>
                    </div>
        <br/>
          <br/>  <br/>
         <div class="form-group">
        <asp:Button ID="Button3" OnClick="UploadFont_Button" Width="100%" runat="server" CssClass="btn btn-primary"  Text="Загрузить" /> <h1></h1>
</div>
        </div>
</asp:Content>
