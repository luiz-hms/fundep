using Fundep.ProjectService.Contracts;
using Fundep.ProjectService.Models;
using System;
using System.Linq;

namespace Fundep.WebApp.Pages
{
    public partial class Coordinators : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["auth"] == null) { Response.Redirect("~/Login.aspx"); return; }
            if (!IsPostBack)
            {

                Bind();
                btnClearCoordSearch.Enabled = false;
            }
        }

        private IFundepProjectService Service() => Fundep.WebApp.ServiceFactory.Create();

        private void Bind()
        {
            lblOk.Text = "";
            lblInfo.Text = "";

            var list = Service().GetCoordinators();

            var filter = (txtFilterCoord.Text ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(filter))
                list = list
                    .Where(c => (c.Name ?? "").IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

            gvCoords.DataSource = list.OrderBy(x => x.Name).ToList();
            gvCoords.DataBind();

            if (list.Count == 0)
                lblInfo.Text = "Nenhum coordenador encontrado.";
        }


        protected void BtnAddCoord_Click(object sender, EventArgs e)
        {
            lblOk.Text = "";
            lblInfo.Text = "";

            if (!Page.IsValid) return;

            try
            {
                Service().CreateCoordinator(new Coordinator
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

        protected void BtnSearchCoord_Click(object sender, EventArgs e)
        {
            btnClearCoordSearch.Enabled = true;
            Bind();
        }

        protected void BtnClearCoordSearch_Click(object sender, EventArgs e)
        {
            txtFilterCoord.Text = "";
            lblInfo.Text = "";
            lblOk.Text = "";
            btnClearCoordSearch.Enabled = false;
            ClearValidators();
            Bind();
        }

        private void ClearValidators()
        {
            foreach (System.Web.UI.IValidator v in Page.Validators)
                v.IsValid = true;
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
            lblOk.Text = "";
            lblInfo.Text = "";

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

                Service().UpdateCoordinator(new Coordinator { Id = id, Name = newName });

                gvCoords.EditIndex = -1;
                lblOk.Text = "Coordenador atualizado.";
                Bind();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void Gv_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            lblOk.Text = "";
            lblInfo.Text = "";

            try
            {
                var id = gvCoords.DataKeys[e.RowIndex].Value.ToString();
                Service().DeleteCoordinator(id);

                lblOk.Text = "Coordenador excluído.";
                Bind();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }
    }
}
