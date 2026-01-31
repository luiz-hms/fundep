using System;

namespace Fundep.WebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                lblMsg.Text = "Usuário e senha são obrigatórios.";
                return;
            }

            Session["auth"] = true;
            Response.Redirect("~/Pages/Projects.aspx");
        }
    }
}
