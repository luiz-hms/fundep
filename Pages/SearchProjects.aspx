<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchProjects.aspx.cs" Inherits="Fundep.WebApp.Pages.SearchProjects" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <script src="../Scripts/WebForms/common.js"></script>
    <meta charset="utf-8" />
    <title>FUNDEP - Consulta</title>
    <link rel="stylesheet" href="../Content/site.css" />
</head>
<body>

    <form id="form1" runat="server">

        <div class="topbar">
            <div class="brand">FUNDEP • Sistema de Projetos</div>
            <div class="nav">
                <a href="Projects.aspx">Projetos</a>
                <a href="SearchProjects.aspx">Consulta</a>
                <a href="Coordinators.aspx">Coordenadores</a>
                <a href="../Login.aspx">Sair</a>
            </div>
        </div>

        <div class="container">
            <div class="card">
                <h2>Consulta de Projetos</h2>

                <asp:ScriptManager ID="sm1" runat="server" />

                <asp:UpdateProgress ID="prog1" runat="server"
                    AssociatedUpdatePanelID="up1"
                    DisplayAfter="0">
                    <ProgressTemplate>
                        <div class="loader-wrap">
                            <div class="spinner"></div>
                            <div>Carregando...</div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Label ID="lblInfo" runat="server" CssClass="note bad" />
                        <asp:Label ID="lblOk" runat="server" CssClass="note ok" />

                        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-top: 12px;">
                            <div class="field">
                                <label>Número do projeto</label>
                                <asp:TextBox ID="txtFilterNumber" runat="server" onkeypress="return onlyNumbers(event);" />
                            </div>

                            <div class="field">
                                <label>Nome do projeto</label>
                                <asp:TextBox ID="txtFilterName" runat="server" />
                            </div>
                        </div>

                        <div class="footer-actions">
                            <asp:Button ID="btnSearch" runat="server"
                                CssClass="btn"
                                Text="Buscar"
                                OnClick="BtnSearch_Click" />

                            <asp:Button ID="btnClear" runat="server"
                                CssClass="btn secondary"
                                Text="Limpar"
                                OnClick="BtnClear_Click"
                                CausesValidation="false" />
                        </div>

                        <asp:GridView ID="gvResult" runat="server"
                            CssClass="table"
                            AutoGenerateColumns="False"
                            Visible="false">
                            <Columns>
                                <asp:BoundField DataField="ProjectNumber" HeaderText="Projeto" />
                                <asp:BoundField DataField="SubProjectNumber" HeaderText="Subprojeto" />
                                <asp:BoundField DataField="Name" HeaderText="Nome" />
                                <asp:BoundField DataField="CoordinatorName" HeaderText="Coordenador" />
                                <asp:BoundField DataField="Balance" HeaderText="Saldo" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    </Triggers>

                </asp:UpdatePanel>

            </div>
        </div>

    </form>

</body>
</html>
