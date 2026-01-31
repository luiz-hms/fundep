using System;
using Fundep.ProjectService.Models;

namespace Fundep.WebApp.Pages
{
    public partial class Coordinators : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["auth"] == null) { Response.Redirect("~/Login.aspx"); return; }
            if (!IsPostBack) Bind();
        }

        private void Bind()
        {
            lblOk.Text = "";
            lblInfo.Text = "";

            var service = Fundep.WebApp.ServiceFactory.Create();
            var list = service.GetCoordinators();

            gvCoords.DataSource = list;
            gvCoords.DataBind();

            if (list.Count == 0)
                lblInfo.Text = "Nenhum coordenador cadastrado. Cadastre pelo menos um para continuar.";
        }

        protected void BtnAddCoord_Click(object sender, EventArgs e)
        {
            lblOk.Text = ""; lblInfo.Text = "";

            if (string.IsNullOrWhiteSpace(txtCoordName.Text))
            {
                lblInfo.Text = "Nome do coordenador é obrigatório.";
                return;
            }

            try
            {
                var service = Fundep.WebApp.ServiceFactory.Create();
                service.CreateCoordinator(new Coordinator
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = txtCoordName.Text.Trim()
                });

                txtCoordName.Text = "";
                lblOk.Text = "Coordenador cadastrado com sucesso.";
                Bind();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void Gv_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCoords.EditIndex = e.NewEditIndex;
            Bind();
        }

        protected void Gv_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCoords.EditIndex = -1;
            Bind();
        }

        protected void Gv_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            lblOk.Text = ""; lblInfo.Text = "";

            try
            {
                var id = gvCoords.DataKeys[e.RowIndex].Value.ToString();
                var nameTextBox = (System.Web.UI.WebControls.TextBox)gvCoords.Rows[e.RowIndex].Cells[0].Controls[0];
                var newName = (nameTextBox.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    lblInfo.Text = "Nome do coordenador é obrigatório.";
                    return;
                }

                var service = Fundep.WebApp.ServiceFactory.Create();
                service.UpdateCoordinator(new Coordinator { Id = id, Name = newName });

                gvCoords.EditIndex = -1;
                lblOk.Text = "Coordenador atualizado.";
                Bind();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void Gv_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            lblOk.Text = ""; lblInfo.Text = "";

            try
            {
                var id = gvCoords.DataKeys[e.RowIndex].Value.ToString();
                var service = Fundep.WebApp.ServiceFactory.Create();
                service.DeleteCoordinator(id);

                lblOk.Text = "Coordenador excluído.";
                Bind();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }
    }
}
