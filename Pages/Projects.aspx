<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="Fundep.WebApp.Pages.Projects" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>FUNDEP - Projetos</title>
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
                <h2>Cadastro de Projetos</h2>

                <asp:ScriptManager ID="sm1" runat="server" />

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Label ID="lblInfo" runat="server" CssClass="note bad" />
                        <asp:Label ID="lblOk" runat="server" CssClass="note ok" />
                        <h3 style="margin: 0;">Consulta de Projetos</h3>

                        <div style="display: grid; grid-template-columns: 1fr 1fr 170px 170px; gap: 12px; align-items: end; margin-top: 12px;">
                            <div class="field" style="margin: 0;">
                                <label>Número do projeto</label>
                                <asp:TextBox ID="txtFilterNumber" runat="server" onkeypress="return onlyNumbers(event);" />
                            </div>

                            <div class="field" style="margin: 0;">
                                <label>Nome do projeto</label>
                                <asp:TextBox ID="txtFilterName" runat="server" />
                            </div>

                            <asp:Button ID="btnSearch" runat="server"
                                CssClass="btn"
                                Text="Buscar"
                                CausesValidation="false"
                                OnClick="BtnSearch_Click" />

                            <asp:Button ID="btnClearSearch" runat="server"
                                CssClass="btn secondary"
                                Text="Limpar busca"
                                Enabled="false"
                                CausesValidation="false"
                                OnClick="BtnClearSearch_Click" />
                        </div>

                        <hr style="margin: 14px 0; border: none; border-top: 1px solid rgba(0,0,0,.08);" />

                        <h2 style="margin: 0;">Cadastro de Projetos</h2>

                        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-top: 12px;">
                            <div class="field">
                                <label>Número do projeto</label>
                                <asp:TextBox ID="txtProjectNumber" runat="server" onkeypress="return onlyNumbers(event);" />
                                <asp:RequiredFieldValidator ID="rfvProjectNumber" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtProjectNumber"
                                    ErrorMessage="Número do projeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                                <asp:RegularExpressionValidator ID="revProjectNumber" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtProjectNumber"
                                    ValidationExpression="^\d+$"
                                    ErrorMessage="Número do projeto deve conter apenas números."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Número do subprojeto</label>
                                <asp:TextBox ID="txtSubProjectNumber" runat="server" onkeypress="return onlyNumbers(event);" />
                                <asp:RequiredFieldValidator ID="rfvSubProjectNumber" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtSubProjectNumber"
                                    ErrorMessage="Número do subprojeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                                <asp:RegularExpressionValidator ID="revSubProjectNumber" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtSubProjectNumber"
                                    ValidationExpression="^\d+$"
                                    ErrorMessage="Número do subprojeto deve conter apenas números."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field" style="grid-column: 1/-1;">
                                <label>Nome do projeto</label>
                                <asp:TextBox ID="txtName" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtName"
                                    ErrorMessage="Nome do projeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Coordenador</label>
                                <asp:DropDownList ID="ddlCoordinator" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvCoordinator" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="ddlCoordinator"
                                    InitialValue=""
                                    ErrorMessage="Selecione um coordenador."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Saldo do projeto</label>
                                <asp:TextBox ID="txtBalance" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvBalance" runat="server"
                                    ValidationGroup="CreateProject"
                                    ControlToValidate="txtBalance"
                                    ErrorMessage="Saldo é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>
                        </div>

                        <div class="footer-actions">
                            <asp:Button ID="btnSave" runat="server"
                                CssClass="btn"
                                Text="Salvar"
                                ValidationGroup="CreateProject"
                                OnClick="BtnSave_Click" />

                            <asp:Button ID="btnClear" runat="server"
                                CssClass="btn secondary"
                                Text="Limpar"
                                CausesValidation="false"
                                OnClick="BtnClear_Click" />
                        </div>

                        <hr style="margin: 14px 0; border: none; border-top: 1px solid rgba(0,0,0,.08);" />

                        <h3 style="margin: 0;">Resultados</h3>

                        <asp:GridView ID="gvProjects" runat="server"
                            CssClass="table"
                            AutoGenerateColumns="False"
                            DataKeyNames="ProjectNumber,SubProjectNumber"
                            OnRowEditing="Gv_RowEditing"
                            OnRowCancelingEdit="Gv_RowCancelingEdit"
                            OnRowUpdating="Gv_RowUpdating"
                            OnRowDeleting="Gv_RowDeleting">

                            <Columns>
                                <asp:BoundField DataField="ProjectNumber" HeaderText="Projeto" ReadOnly="true" />
                                <asp:BoundField DataField="SubProjectNumber" HeaderText="Subprojeto" ReadOnly="true" />
                                <asp:BoundField DataField="Name" HeaderText="Nome" />

                                <asp:TemplateField HeaderText="Coordenador">
                                    <ItemTemplate><%# Eval("CoordinatorName") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditCoordinator" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Balance" HeaderText="Saldo" />
                                <asp:CommandField ShowEditButton="True" EditText="Editar" UpdateText="Salvar" CancelText="Cancelar" CausesValidation="false" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="Excluir" CausesValidation="false" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnClearSearch" EventName="Click" />

                        <asp:AsyncPostBackTrigger ControlID="gvProjects" EventName="RowEditing" />
                        <asp:AsyncPostBackTrigger ControlID="gvProjects" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="gvProjects" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="gvProjects" EventName="RowCancelingEdit" />
                    </Triggers>

                </asp:UpdatePanel>

            </div>
        </div>

        <script type="text/javascript">
            window.projectSearchIds = {
                number: '<%= txtFilterNumber.ClientID %>',
                name: '<%= txtFilterName.ClientID %>',
                btnSearch: '<%= btnSearch.ClientID %>'
            };
        </script>

    </form>

</body>
</html>
