<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Coordinators.aspx.cs" Inherits="Fundep.WebApp.Pages.Coordinators" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>FUNDEP - Coordenadores</title>
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
                <h2>Coordenadores</h2>

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

                        <div style="display: grid; grid-template-columns: 1fr 180px; gap: 12px; align-items: end; margin-top: 12px;">
                            <div class="field" style="margin: 0;">
                                <label>Nome do coordenador</label>
                                <asp:TextBox ID="txtCoordName" runat="server" />

                                <asp:RequiredFieldValidator ID="rfvCoordName" runat="server"
                                    ControlToValidate="txtCoordName"
                                    ErrorMessage="Nome do coordenador é obrigatório."
                                    Display="Dynamic" ForeColor="#b00020" />
                            </div>

                            <asp:Button ID="btnAddCoord" runat="server"
                                CssClass="btn secondary"
                                Text="Cadastrar"
                                OnClick="BtnAddCoord_Click" />
                        </div>

                        <hr style="margin: 14px 0; border: none; border-top: 1px solid rgba(0,0,0,.08);" />

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
