using LacerdAPP.ViewModel;
using Microcharts;
using Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using Acr.UserDialogs;
using System.Text.RegularExpressions;
using Rg.Plugins.Popup.Services;
using LacerdAPP.View;

namespace LacerdApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Caixa : ContentPage
    {
        public Caixa()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() //metodo executado nativamente na chamada da pagina
        {
            #region Carregar dados do Caixa
            float investimentoAtual = 0;
            float lucroAtual = 0;
            MysqlCon conexao = new MysqlCon();
            List<Valores> caixa = conexao.ListarInvestimento_Lucro();

            if (caixa != null)
            {
                if (caixa.Count > 0)
                {

                    investimentoAtual = float.Parse(caixa[0].investimentoAtual.ToString());
                    lucroAtual = float.Parse(caixa[0].lucroAtual.ToString());

                }

                lblInvestimento.Text = "📈 " + investimentoAtual.ToString("C");
                lblLucro.Text = "💵 " + lucroAtual.ToString("C");

            }

            List<Entry> entradas = new List<Entry>
            {

                new Entry(investimentoAtual)
                {
                  Color=SKColor.Parse("#fa7f72"),
                  Label ="Investimento",
                  ValueLabelColor = SKColor.Parse("#fa7f72")

                },

                new Entry(lucroAtual)
                {
                  Color=SKColor.Parse("#228b22"),
                  Label ="Lucro",
                  ValueLabelColor = SKColor.Parse("#228b22")
                }


            };


            graficoPizza.Chart = new DonutChart { Entries = entradas };
            graficoPizza.Chart.LabelTextSize = 30;
            #endregion
        }

        public static string tipoDebito;
        private async void btnDebitar_Clicked(object sender, EventArgs e)
        {

            tipoDebito = await DisplayActionSheet("Selecione:", null, null, "💵 Lucro", "📈 Investimento");
            

            if (!string.IsNullOrEmpty(tipoDebito))
            {
              UserDialogs.Instance.Toast("O valor será debitado do " + tipoDebito + ".\n", TimeSpan.FromSeconds(5));
              await PopupNavigation.Instance.PushAsync(new TipoDebitoPopup());
            }
            else
            {
              UserDialogs.Instance.Toast("❌ Erro: \n Nenhuma opção selecionada!", TimeSpan.FromSeconds(5));
            }
            

            //conexao.AtualizarCaixa(totalCusto.ToString("F").Replace(",", "."), totalVenda.ToString("F").Replace(",", "."), "motivo investimento","50.00",1);
        }

        public static string RetornaTipoDebito()
        {
            string teste = tipoDebito;

            return teste;
        }

   
    }

}