using System;
using System.Globalization;
using System.Linq;
using Fundep.ProjectService.Models;

namespace Fundep.WebApp.Pages
{
    public partial class Projects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["auth"] == null) { Response.Redirect("~/Login.aspx"); return; }

            if (!IsPostBack)
            {
                EnsureCoordinatorExistsOrRedirect();
                BindCoordinators();
                BindProjects();
            }
        }

        private void EnsureCoordinatorExistsOrRedirect()
        {
            var service = Fundep.WebApp.ServiceFactory.Create();
            if (service.GetCoordinators().Count == 0)
                Response.Redirect("~/Pages/Coordinators.aspx");
        }

        private void BindCoordinators()
        {
            var service = Fundep.WebApp.ServiceFactory.Create();
            var coords = service.GetCoordinators();

            ddlCoordinator.Items.Clear();
            ddlCoordinator.Items.Add(new System.Web.UI.WebControls.ListItem("-- selecione --", ""));
            foreach (var c in coords)
                ddlCoordinator.Items.Add(new System.Web.UI.WebControls.ListItem(c.Name, c.Id));
        }

        private void BindProjects()
        {
            var service = Fundep.WebApp.ServiceFactory.Create();
            var coords = service.GetCoordinators().ToDictionary(c => c.Id, c => c.Name);
            var projects = service.GetProjects().Select(p => new
            {
                p.ProjectNumber,
                p.SubProjectNumber,
                p.Name,
                CoordinatorName = coords.ContainsKey(p.CoordinatorId) ? coords[p.CoordinatorId] : "(não encontrado)",
                Balance = p.Balance.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"))
            }).ToList();

            gvProjects.DataSource = projects;
            gvProjects.DataBind();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            lblInfo.Text = ""; lblOk.Text = "";

            if (string.IsNullOrWhiteSpace(txtProjectNumber.Text) ||
                string.IsNullOrWhiteSpace(txtSubProjectNumber.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(ddlCoordinator.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtBalance.Text))
            {
                lblInfo.Text = "Preencha todos os campos do projeto.";
                return;
            }

            decimal balance;
            if (!decimal.TryParse(txtBalance.Text, NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), out balance) &&
                !decimal.TryParse(txtBalance.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out balance))
            {
                lblInfo.Text = "Saldo inválido. Use um número (ex.: 1500,00).";
                return;
            }

            try
            {
                var service = Fundep.WebApp.ServiceFactory.Create();
                service.CreateProject(new Project
                {
                    ProjectNumber = txtProjectNumber.Text.Trim(),
                    SubProjectNumber = txtSubProjectNumber.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    CoordinatorId = ddlCoordinator.SelectedValue,
                    Balance = balance
                });

                lblOk.Text = "Projeto cadastrado com sucesso.";
                BtnClear_Click(null, null);
                BindProjects();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtProjectNumber.Text = "";
            txtSubProjectNumber.Text = "";
            txtName.Text = "";
            txtBalance.Text = "";
            ddlCoordinator.SelectedIndex = 0;
        }

        protected void Gv_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvProjects.EditIndex = e.NewEditIndex;
            RebindForEditRow();
        }

        private void RebindForEditRow()
        {
            var service = Fundep.WebApp.ServiceFactory.Create();
            var coords = service.GetCoordinators();
            var coordMap = coords.ToDictionary(c => c.Id, c => c.Name);

            var projects = service.GetProjects().Select(p => new
            {
                p.ProjectNumber,
                p.SubProjectNumber,
                p.Name,
                CoordinatorId = p.CoordinatorId,
                CoordinatorName = coordMap.ContainsKey(p.CoordinatorId) ? coordMap[p.CoordinatorId] : "(não encontrado)",
                Balance = p.Balance.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"))
            }).ToList();

            gvProjects.DataSource = projects;
            gvProjects.DataBind();

            if (gvProjects.EditIndex >= 0 && gvProjects.EditIndex < gvProjects.Rows.Count)
            {
                var row = gvProjects.Rows[gvProjects.EditIndex];
                var ddl = (System.Web.UI.WebControls.DropDownList)row.FindControl("ddlEditCoordinator");
                ddl.Items.Clear();
                foreach (var c in coords)
                    ddl.Items.Add(new System.Web.UI.WebControls.ListItem(c.Name, c.Id));

                var coordId = projects[gvProjects.EditIndex].CoordinatorId;
                var item = ddl.Items.FindByValue(coordId);
                if (item != null) item.Selected = true;
            }
        }

        protected void Gv_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvProjects.EditIndex = -1;
            BindProjects();
        }

        protected void Gv_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            lblInfo.Text = ""; lblOk.Text = "";

            try
            {
                var keys = gvProjects.DataKeys[e.RowIndex];
                var projectNumber = keys.Values[0].ToString();
                var subProjectNumber = keys.Values[1].ToString();

                var row = gvProjects.Rows[e.RowIndex];
                var nameTextBox = (System.Web.UI.WebControls.TextBox)row.Cells[2].Controls[0];
                var ddl = (System.Web.UI.WebControls.DropDownList)row.FindControl("ddlEditCoordinator");
                var balanceTextBox = (System.Web.UI.WebControls.TextBox)row.Cells[4].Controls[0];

                var newName = (nameTextBox.Text ?? "").Trim();
                var newCoordId = ddl.SelectedValue;
                var balanceRaw = (balanceTextBox.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newCoordId) || string.IsNullOrWhiteSpace(balanceRaw))
                {
                    lblInfo.Text = "Nome, coordenador e saldo são obrigatórios.";
                    return;
                }

                decimal balance;
                if (!decimal.TryParse(balanceRaw, NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), out balance) &&
                    !decimal.TryParse(balanceRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out balance))
                {
                    lblInfo.Text = "Saldo inválido.";
                    return;
                }

                var service = Fundep.WebApp.ServiceFactory.Create();
                service.UpdateProject(new Project
                {
                    ProjectNumber = projectNumber,
                    SubProjectNumber = subProjectNumber,
                    Name = newName,
                    CoordinatorId = newCoordId,
                    Balance = balance
                });

                gvProjects.EditIndex = -1;
                lblOk.Text = "Projeto atualizado.";
                BindProjects();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void Gv_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            lblInfo.Text = ""; lblOk.Text = "";
            try
            {
                var keys = gvProjects.DataKeys[e.RowIndex];
                var projectNumber = keys.Values[0].ToString();
                var subProjectNumber = keys.Values[1].ToString();

                var service = Fundep.WebApp.ServiceFactory.Create();
                service.DeleteProject(projectNumber, subProjectNumber);

                lblOk.Text = "Projeto excluído.";
                BindProjects();
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }
    }
}
