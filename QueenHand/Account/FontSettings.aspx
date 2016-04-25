<%@ Page Title="Настрока почерка" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="FontSettings.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <br/>
    <div align="left" class="row">
        
        <div class="col-md-4">
              
                 <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="TextBox0" >Длина пробела</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox0" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="TextBox1" >Минимальная доп. длина пробела</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="TextBox2" >Максимальная доп. длина пробела</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="TextBox3" >Порог зависимости от длины слова</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label5" runat="server" AssociatedControlID="TextBox4" >Высота строки</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox4" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label6" runat="server" AssociatedControlID="TextBox5" >Минимальная доп. высота строки</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox5" CssClass="form-control" />
                        </div>
                    </div>
                     
                   
                </div>
        </div>

   <div class="col-md-4">
              
                 <div  class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" AssociatedControlID="TextBox7" >Максимальная  доп. высота строки</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox7" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="TextBox8">Нижний порог вариативности</asp:Label>
                         <br/>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox8" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label10" runat="server" AssociatedControlID="TextBox9" >Верхний порог вариативности</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox9" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label11" runat="server" AssociatedControlID="TextBox10" >Отступ от верхнего края</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox10" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label12" runat="server" AssociatedControlID="TextBox11" >Отступ от правого края</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox11" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label13" runat="server" AssociatedControlID="TextBox12" >Минимальный доп. отступ от левого края</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox12" CssClass="form-control" />
                        </div>
                    </div>
                     

                </div>
        </div>
        
         <div class="col-md-4">
              
                 <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label15" runat="server" AssociatedControlID="TextBox14" >Максимальный доп. отступ от левого края</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox14" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label16" runat="server" AssociatedControlID="TextBox15" >Масштаб X</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox15" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label17" runat="server" AssociatedControlID="TextBox16" >Масштаб Y</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox16" CssClass="form-control" />
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="Label18" runat="server" AssociatedControlID="TextBox17" >Общий масштаб %</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox17" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label19" runat="server" AssociatedControlID="TextBox18" >Сдвиг по Y от</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox18" CssClass="form-control" />
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <asp:Label ID="Label20" runat="server" AssociatedControlID="TextBox19" >Сдвиг по Y до</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TextBox19" CssClass="form-control" />
                        </div>
                    </div>

                      <div class="form-group">
                        <div class="col-md-10">
                            <br/>
                          <asp:Button ID="Button2" runat="server" Width="100%" OnClick="ApplySettings_Button"  CssClass="btn btn-default btn-primary"  Text="Сохранить" />  
                        </div>
                    </div>
                </div>
        </div>
         

    </div>
</asp:Content>
