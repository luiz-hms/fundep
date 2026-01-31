using System;
using System.Globalization;
using System.Linq;

namespace Fundep.WebApp.Pages
{
    public partial class SearchProjects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["auth"] == null) { Response.Redirect("~/Login.aspx"); return; }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            lblInfo.Text = ""; lblOk.Text = ""; gvResult.Visible = false;

            var number = (txtFilterNumber.Text ?? "").Trim();
            var name = (txtFilterName.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(number) && string.IsNullOrWhiteSpace(name))
            {
                lblInfo.Text = "Informe ao menos um filtro (número do projeto ou nome do projeto).";
                return;
            }

            try
            {
                var service = Fundep.WebApp.ServiceFactory.Create();
                var coords = service.GetCoordinators().ToDictionary(c => c.Id, c => c.Name);

                var list = service.SearchProjects(number, name)
                    .Select(p => new
                    {
                        p.ProjectNumber,
                        p.SubProjectNumber,
                        p.Name,
                        CoordinatorName = coords.ContainsKey(p.CoordinatorId) ? coords[p.CoordinatorId] : "(não encontrado)",
                        Balance = p.Balance.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"))
                    }).ToList();

                if (list.Count == 0)
                {
                    lblInfo.Text = "Nenhum projeto encontrado para os filtros informados.";
                    return;
                }

                gvResult.DataSource = list;
                gvResult.DataBind();
                gvResult.Visible = true;
                lblOk.Text = string.Format("{0} projeto(s) encontrado(s).", list.Count);
            }
            catch (Exception ex) { lblInfo.Text = ex.Message; }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtFilterNumber.Text = "";
            txtFilterName.Text = "";
            gvResult.Visible = false;
            lblInfo.Text = ""; lblOk.Text = "";
        }
    }
}
