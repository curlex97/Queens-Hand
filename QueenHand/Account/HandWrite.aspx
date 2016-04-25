<%@ Page Title="Генерация почерка" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="HandWrite.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Введите текст в поле ниже</h3>

    <br/>
    

    
    <div class="row">
        <div class="col-md-4">
              <textarea id="TextInput"   name="TextInput" cols="150" rows="22"></textarea>
                
        </div>

        <div class="col-md-4">
         <asp:Image ID="Image1" Width="315" Height="446" runat="server" BorderStyle="Solid" BorderWidth="1" BorderColor="#D4D4D4" /> 
        </div>

        <div class="col-md-4">
            
            <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="WidthBlock" CssClass="col-md-2 control-label">Ширина</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="WidthBlock" CssClass="form-control" />
                        </div>
                    </div>
            
                   <div class="form-group">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="HeightBlock" CssClass="col-md-2 control-label">Высота</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="HeightBlock" CssClass="form-control" />
                        </div>
                    </div>
                
                <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                                <asp:Button ID="Button1" Width="100%"  runat="server" CssClass="btn btn-success" OnClick="Button1_OnClick" Text="Построить" /> 
                        </div>
                    </div>
                
                <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                          <asp:Button ID="Button2" runat="server" Width="100%" OnClick="FontSettings_Button"  CssClass="btn btn-warning"  Text="Настройки" />  
                        </div>
                    </div>
                
                  <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                          <asp:Button ID="Button3" runat="server" Width="100%"  OnClick="SaveFont_OnClick" CssClass="btn btn-primary"  Text="Сохранить" />  
                        </div>
                    </div> 

                </div>


        </div>
    </div>

</asp:Content>
