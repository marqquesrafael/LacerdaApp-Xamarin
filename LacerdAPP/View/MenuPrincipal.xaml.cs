using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using static LacerdApp.MenuPrincipal;

namespace LacerdApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() //metodo executado nativamente na chamada da página
        {
            base.OnAppearing();         
        }

        protected async override void OnDisappearing() //executado nativamente na saida da pagina
        {
            if (Navigation.NavigationStack.Count == 2)
            {
                bool isSaida = await DisplayAlert("📲 Logoff", "Deseja realmente deixar o LacerdAPP ?", "SIM", "NÃO");
                if (isSaida)
                {
                    System.Environment.Exit(0);
                }
            }        
        }

        void btAddProduto(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Produtos());
        }

        void btVendas(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Vendas());
        }

        void btCaixa(object sender, System.EventArgs e)
        {
            //DisplayAlert("🛠 MANUTENÇÃO 🛠", "Tela de caixa ainda se encontra em fase de desenvolvimento.", "OK");
             Navigation.PushAsync(new Caixa());
        }

    }
}