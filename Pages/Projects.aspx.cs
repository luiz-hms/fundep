using Fundep.ProjectService.Contracts;
using Fundep.ProjectService.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Fundep.WebApp.Pages
{
    public partial class Projects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["auth"] == null) { Response.Redirect("~/Login.aspx"); return; }

            if (!IsPostBack)
            {
                EnsureDefaultCoordinator();
                LoadCoordinatorsDropdown();
                BindProjects();

                btnClearSearch.Enabled = false;
            }
        }

        private IFundepProjectService Service() => Fundep.WebApp.ServiceFactory.Create();

        private void EnsureDefaultCoordinator()
        {
            var service = Service();
            var coords = service.GetCoordinators();
            if (coords.Count == 0)
            {
                service.CreateCoordinator(new Coordinator
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "Coordenador Padrão"
                });
            }
        }

        private void LoadCoordinatorsDropdown()
        {
            var service = Service();
            var coords = service.GetCoordinators();

            ddlCoordinator.Items.Clear();
            ddlCoordinator.Items.Add(new ListItem("-- Selecione --", ""));

            foreach (var c in coords.OrderBy(x => x.Name))
                ddlCoordinator.Items.Add(new ListItem(c.Name, c.Id));
        }

        private void BindProjects()
        {
            lblInfo.Text = "";
            lblOk.Text = "";

            var service = Service();

            var list = service.GetProjects();
            var coords = service.GetCoordinators();

            foreach (var p in list)
            {
                var c = coords.FirstOrDefault(x => x.Id == p.CoordinatorId);
                p.CoordinatorName = c != null ? c.Name : "";
            }

            var fNumber = (txtFilterNumber.Text ?? "").Trim();
            var fName = (txtFilterName.Text ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(fNumber))
                list = list.Where(p => p.ProjectNumber == fNumber).ToList();

            if (!string.IsNullOrWhiteSpace(fName))
                list = list.Where(p => (p.Name ?? "").IndexOf(fName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            gvProjects.DataSource = list;
            gvProjects.DataBind();

            if (list.Count == 0)
                lblInfo.Text = "Nenhum projeto encontrado.";
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            lblOk.Text = "";

            if (!Page.IsValid) return;

            try
            {
                var service = Service();
                var coordId = ddlCoordinator.SelectedValue;

                var coord = service.GetCoordinators().FirstOrDefault(c => c.Id == coordId);
                if (coord == null) { lblInfo.Text = "Selecione um coordenador válido."; return; }

                service.CreateProject(new Project
                {
                    ProjectNumber = txtProjectNumber.Text.Trim(),
                    SubProjectNumber = txtSubProjectNumber.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    CoordinatorId = coord.Id,
                    CoordinatorName = coord.Name,
                    Balance = decimal.Parse(txtBalance.Text.Trim()),
                });

                lblOk.Text = "Projeto cadastrado com sucesso.";
                ClearCreateForm();
                BindProjects();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            lblOk.Text = "";
            ClearCreateForm();
            ClearValidators();
        }

        private void ClearCreateForm()
        {
            txtProjectNumber.Text = "";
            txtSubProjectNumber.Text = "";
            txtName.Text = "";
            txtBalance.Text = "";
            if (ddlCoordinator.Items.Count > 0) ddlCoordinator.SelectedIndex = 0;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            lblOk.Text = "";
            btnClearSearch.Enabled = true;

            BindProjects();
        }

        protected void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtFilterNumber.Text = "";
            txtFilterName.Text = "";
            lblInfo.Text = "";
            lblOk.Text = "";

            btnClearSearch.Enabled = false;

            ClearValidators();
            BindProjects();
        }

        private void ClearValidators()
        {
            foreach (System.Web.UI.IValidator v in Page.Validators)
                v.IsValid = true;
        }

        protected void Gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProjects.EditIndex = e.NewEditIndex;
            BindProjects();

            var row = gvProjects.Rows[e.NewEditIndex];
            var ddl = (DropDownList)row.FindControl("ddlEditCoordinator");
            if (ddl != null)
            {
                ddl.Items.Clear();
                var coords = Service().GetCoordinators().OrderBy(x => x.Name).ToList();
                foreach (var c in coords) ddl.Items.Add(new ListItem(c.Name, c.Id));

                var keys = gvProjects.DataKeys[e.NewEditIndex];
                var projectNumber = keys.Values["ProjectNumber"].ToString();
                var subProjectNumber = keys.Values["SubProjectNumber"].ToString();

                var current = Service().GetProjects()
                    .FirstOrDefault(p => p.ProjectNumber == projectNumber && p.SubProjectNumber == subProjectNumber);

                if (current != null && !string.IsNullOrWhiteSpace(current.CoordinatorId))
                    ddl.SelectedValue = current.CoordinatorId;
            }
        }

        protected void Gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProjects.EditIndex = -1;
            BindProjects();
        }

        protected void Gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var keys = gvProjects.DataKeys[e.RowIndex];
                var projectNumber = keys.Values["ProjectNumber"].ToString();
                var subProjectNumber = keys.Values["SubProjectNumber"].ToString();

                var row = gvProjects.Rows[e.RowIndex];

                var nameTextBox = (TextBox)row.Cells[2].Controls[0];
                var ddl = (DropDownList)row.FindControl("ddlEditCoordinator");
                var balanceTextBox = (TextBox)row.Cells[4].Controls[0];

                var newName = (nameTextBox.Text ?? "").Trim();
                var newBalance = (balanceTextBox.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    lblInfo.Text = "Nome do projeto é obrigatório.";
                    return;
                }

                var service = Service();
                var coord = service.GetCoordinators().FirstOrDefault(c => c.Id == (ddl?.SelectedValue ?? ""));
                if (coord == null) { lblInfo.Text = "Selecione um coordenador válido."; return; }

                service.UpdateProject(new Project
                {
                    ProjectNumber = projectNumber,
                    SubProjectNumber = subProjectNumber,
                    Name = newName,
                    CoordinatorId = coord.Id,
                    CoordinatorName = coord.Name,
                    Balance = decimal.Parse(newBalance)
                });

                gvProjects.EditIndex = -1;
                lblOk.Text = "Projeto atualizado.";
                BindProjects();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void Gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                var keys = gvProjects.DataKeys[e.RowIndex];
                var projectNumber = keys.Values["ProjectNumber"].ToString();
                var subProjectNumber = keys.Values["SubProjectNumber"].ToString();

                Service().DeleteProject(projectNumber, subProjectNumber);

                lblOk.Text = "Projeto excluído.";
                BindProjects();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }
    }
}
