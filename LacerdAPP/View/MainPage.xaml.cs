using Mysql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using usuarios;
using Xamarin.Forms;

namespace LacerdApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }



        protected override void OnAppearing() //metodo executado nativamente na chamada da pagina
        {
            base.OnAppearing();
            entryUsuario.Text = null;
            entrySenha.Text = null;

            if (Navigation.NavigationStack.Count > 1)
            {
                tela.IsVisible = false;
                Navigation.PushAsync(new MenuPrincipal());
            }


        }





        void btLogin(object sender, System.EventArgs e)
        {
            MysqlCon conexao = new MysqlCon();
            DataTable dtUsuario = new DataTable();

            if (string.IsNullOrEmpty(entryUsuario.Text))
            {
                DisplayAlert("❌ Erro", "Tem que preencher o campo de usuário amoor!!", "OK");
                entryUsuario.Focus();
            }
            else if (string.IsNullOrEmpty(entrySenha.Text))
            {
                DisplayAlert("❌ Erro", "Tem que preencher o campo de senha amoor!!", "OK");
                entrySenha.Focus();
            }
            else
            {

                List<Usuario> usuarios = conexao.CarregaUsuarios(entryUsuario.Text, entrySenha.Text);

                if (usuarios != null)
                {
                    if (usuarios.Count > 0)
                    {
                        DisplayAlert("🔓 Login realizado com sucesso!", "Seja bem vinda ao 📲 LacerdAPP", "OK");
                        Navigation.PushAsync(new MenuPrincipal());
                    }
                    else
                    {
                        DisplayAlert("❌ Erro", "Usuário ou senha inválido, qualquer coisa liga pro amor da sua vida 🙈💗", "OK");
                    }
                }
            }
        }
    }
}
