using Acr.UserDialogs;
using LacerdApp;
using LacerdAPP.ViewModel;
using Mysql;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LacerdAPP.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TipoDebitoPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public TipoDebitoPopup()
        {
            InitializeComponent();
        }

        private void entryValorDebito_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;
            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                entry.TextChanged -= entryValorDebito_TextChanged;
                string text = Regex.Replace(ev.NewTextValue.Trim(), @"[^0-9]", "");


                if (text.Length > 2)
                {
                    text = text.Insert(text.Length - 2, ",");
                }

                if (text.Length > 6)
                {
                    text = text.Insert(text.Length - 6, ".");
                }
                if (text.Length > 10)
                {
                    text = text.Insert(text.Length - 10, ".");
                }

                if (entry.Text != text)
                    entry.Text = text;
                entry.TextChanged += entryValorDebito_TextChanged;
            }
        }

        private void btndebitar_Clicked(object sender, EventArgs e)
        {
            string tipoDebito = Caixa.RetornaTipoDebito();
            double valor = double.Parse(entryValorDebito.Text);


            MysqlCon mysql = new MysqlCon();
            List<Valores> caixa = mysql.ListarInvestimento_Lucro();

            double InvestimentoAtual = double.Parse(caixa[0].investimentoAtual.ToString());
            double LucroAtual = double.Parse(caixa[0].lucroAtual.ToString());

            if (tipoDebito.Contains("Investimento"))
            {
                InvestimentoAtual -= valor;
                mysql.AtualizarCaixa(InvestimentoAtual.ToString("F").Replace(",", "."), LucroAtual.ToString("F").Replace(",", "."), entryDescricao.Text, valor.ToString("F").Replace(",", "."), 1);
            }
            if (tipoDebito.Contains("Lucro"))
            {
                LucroAtual -= valor;
                mysql.AtualizarCaixa(InvestimentoAtual.ToString("F").Replace(",", "."), LucroAtual.ToString("F").Replace(",", "."), entryDescricao.Text, valor.ToString("F").Replace(",", "."), 2);
            }

            

            UserDialogs.Instance.Toast($"Foi Debitado {valor.ToString("C")} da conta de  {tipoDebito}", TimeSpan.FromSeconds(5));

            PopupNavigation.Instance.PopAsync();
            Navigation.PushAsync(new Caixa());




        }
    }
}