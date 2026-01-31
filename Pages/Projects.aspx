<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="Fundep.WebApp.Pages.Projects" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>FUNDEP - Projects</title>
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
                <h2>Cadastro de Projetos</h2>

                <asp:ScriptManager ID="sm1" runat="server" />

                <asp:UpdatePanel ID="up1" runat="server">
                    <ContentTemplate>

                        <asp:UpdateProgress ID="prog1" runat="server">
                            <ProgressTemplate>
                                <div class="loader-wrap">
                                    <div class="spinner"></div>
                                    <div>Carregando...</div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                        <asp:Label ID="lblInfo" runat="server" CssClass="note bad" />
                        <asp:Label ID="lblOk" runat="server" CssClass="note ok" />

                        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-top: 12px;">
                            <div class="field">
                                <label>Número do projeto</label>
                                <asp:TextBox ID="txtProjectNumber" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvProjectNumber" runat="server"
                                    ControlToValidate="txtProjectNumber"
                                    ErrorMessage="Número do projeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Número do subprojeto</label>
                                <asp:TextBox ID="txtSubProjectNumber" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvSubProjectNumber" runat="server"
                                    ControlToValidate="txtSubProjectNumber"
                                    ErrorMessage="Número do subprojeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field" style="grid-column: 1/-1;">
                                <label>Nome do projeto</label>
                                <asp:TextBox ID="txtName" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                    ControlToValidate="txtName"
                                    ErrorMessage="Nome do projeto é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Coordenador</label>
                                <asp:DropDownList ID="ddlCoordinator" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvCoordinator" runat="server"
                                    ControlToValidate="ddlCoordinator"
                                    InitialValue=""
                                    ErrorMessage="Selecione um coordenador."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <div class="field">
                                <label>Saldo do projeto</label>
                                <asp:TextBox ID="txtBalance" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvBalance" runat="server"
                                    ControlToValidate="txtBalance"
                                    ErrorMessage="Saldo é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>
                        </div>

                        <div class="footer-actions">
                            <asp:Button ID="btnSave" runat="server"
                                CssClass="btn"
                                Text="Salvar"
                                OnClick="BtnSave_Click" />

                            <asp:Button ID="btnClear" runat="server"
                                CssClass="btn secondary"
                                Text="Limpar"
                                OnClick="BtnClear_Click"
                                CausesValidation="false" />
                        </div>

                        <hr style="margin: 14px 0; border: none; border-top: 1px solid rgba(0,0,0,.08);" />

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
                                    <ItemTemplate>
                                        <%# Eval("CoordinatorName") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditCoordinator" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Balance" HeaderText="Saldo" />
                                <asp:CommandField ShowEditButton="True" EditText="Editar" UpdateText="Salvar" CancelText="Cancelar" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="Excluir" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

    </form>

</body>
</html>
