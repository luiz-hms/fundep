<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Fundep.WebApp.Login" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <script src="Scripts/common.js"></script>
    <meta charset="utf-8" />
    <title>FUNDEP - Login</title>
    <link rel="stylesheet" href="Content/site.css" />
</head>
<body>
    <div class="topbar">
        <div class="brand">FUNDEP • Sistema de Projetos</div>
    </div>

    <div class="container">
        <div class="card" style="max-width: 520px; margin: 0 auto;">
            <h2>Login</h2>
            <form id="form1" runat="server">
                <div class="field">
                    <label>Usuário</label>
                    <asp:TextBox ID="txtUser" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" ControlToValidate="txtUser"
                        ErrorMessage="Usuário é obrigatório." Display="Dynamic" ForeColor="#b00020" />
                </div>

                <div class="field">
                    <label>Senha</label>
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPass"
                        ErrorMessage="Senha é obrigatória." Display="Dynamic" ForeColor="#b00020" />
                </div>

                <div class="footer-actions">
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn" Text="Entrar" OnClick="BtnLogin_Click" />
                </div>

                <asp:Label ID="lblMsg" runat="server" CssClass="note bad" />
            </form>
        </div>
    </div>
</body>
</html>
