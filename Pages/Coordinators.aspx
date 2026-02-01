<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coordinators.aspx.cs" Inherits="Fundep.WebApp.Pages.Coordinators" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>FUNDEP - Coordenadores</title>
    <link rel="stylesheet" href="../Content/site.css" />
    <script src="../Scripts/WebForms/common.js"></script>
</head>
<body>

    <form id="form1" runat="server">
        <div id="globalLoader" class="loader-wrap" style="display: none;">
            <div class="spinner"></div>
            <div>Carregando...</div>
        </div>

        <div class="topbar">
            <div class="brand">FUNDEP • Sistema de Projetos</div>
            <div class="nav">
                <a href="Projects.aspx" onclick="showGlobalLoader()">Projetos</a>
                <a href="Coordinators.aspx" onclick="showGlobalLoader()">Coordenadores</a>
                <a href="../Login.aspx" onclick="showGlobalLoader()">Sair</a>
            </div>
        </div>

        <div class="container">
            <div class="card">
                <h2>Coordenadores</h2>

                <asp:ScriptManager ID="sm1" runat="server" />

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Label ID="lblInfo" runat="server" CssClass="note bad" />
                        <asp:Label ID="lblOk" runat="server" CssClass="note ok" />
                        <div style="display: grid; grid-template-columns: 1fr 180px; gap: 12px; align-items: end; margin-top: 12px;">
                            <div class="field" style="margin: 0;">
                                <label>Nome do coordenador</label>
                                <asp:TextBox ID="txtCoordName" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvCoordName" runat="server"
                                    ValidationGroup="CreateCoordinator"
                                    ControlToValidate="txtCoordName"
                                    ErrorMessage="Nome do coordenador é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <asp:Button ID="btnAddCoord" runat="server"
                                ValidationGroup="CreateCoordinator"
                                CssClass="btn secondary"
                                Text="Cadastrar"
                                OnClick="BtnAddCoord_Click" />
                        </div>

                        <hr style="margin: 14px 0; border: none; border-top: 1px solid rgba(0,0,0,.08);" />


                        <div style="display: grid; grid-template-columns: 1fr 180px; gap: 12px; align-items: end; margin-top: 12px;">
                            <div class="field" style="margin: 0;">
                                <label>Pesquisar coordenador</label>
                                <asp:TextBox ID="txtFilterCoord" runat="server" />
                            </div>

                            <asp:Button ID="btnSearchCoord" runat="server"
                                CssClass="btn"
                                Text="Buscar"
                                CausesValidation="false"
                                OnClick="BtnSearchCoord_Click" />
                        </div>

                        <div class="footer-actions">
                            <asp:Button ID="btnClearCoordSearch" runat="server"
                                CssClass="btn secondary"
                                Enabled="false"
                                Text="Limpar busca"
                                CausesValidation="false"
                                OnClick="BtnClearCoordSearch_Click" />
                        </div>

                        <asp:GridView ID="gvCoords" runat="server"
                            CssClass="table"
                            AutoGenerateColumns="False"
                            DataKeyNames="Id"
                            OnRowEditing="Gv_RowEditing"
                            OnRowCancelingEdit="Gv_RowCancelingEdit"
                            OnRowUpdating="Gv_RowUpdating"
                            OnRowDeleting="Gv_RowDeleting">

                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Nome" />
                                <asp:CommandField ShowEditButton="True" EditText="Editar" UpdateText="Salvar" CancelText="Cancelar" CausesValidation="false" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="Excluir" CausesValidation="false" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddCoord" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearchCoord" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClearCoordSearch" EventName="Click" />

                        <asp:AsyncPostBackTrigger ControlID="gvCoords" EventName="RowEditing" />
                        <asp:AsyncPostBackTrigger ControlID="gvCoords" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="gvCoords" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="gvCoords" EventName="RowCancelingEdit" />
                    </Triggers>

                </asp:UpdatePanel>

            </div>
        </div>
        <script type="text/javascript">
            window.coordSearchIds = {
                text: '<%= txtFilterCoord.ClientID %>',
                btnSearch: '<%= btnSearchCoord.ClientID %>',
                btnClear: '<%= btnClearCoordSearch.ClientID %>'
            };
        </script>

    </form>

</body>
</html>
