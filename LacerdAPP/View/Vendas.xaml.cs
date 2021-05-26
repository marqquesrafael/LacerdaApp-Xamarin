using LacerdAPP.ViewModel;
using Mysql;
using Produtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LacerdApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Vendas : ContentPage
    {
        public Vendas()
        {
            InitializeComponent();
        }

        #region Consultar Estoque
        void btConsultarEstoque_Clicked(object sender, System.EventArgs e)
        {
            MysqlCon conexao = new MysqlCon();

            List<Produto> produto = conexao.BuscarProduto(entryCodPeca.Text);
            if (produto != null)
            {
                if (produto.Count > 0)
                {

                    entryPrecoCusto.Text = "💲" + produto[0].precoCusto.ToString("C");
                    entryFornecedor.Text = "🏢" + produto[0].fornecedor;
                    entryDescricao.Text = "👗" + produto[0].descricao;
                    entryTamanho.Text = "🏷" + produto[0].tamanho;
                    entryCor.Text = "🎨" + produto[0].cor;



                    btRegistrarVenda.IsVisible = true;

                    entryCor.IsVisible = true;
                    entryTamanho.IsVisible = true;
                    entryDescricao.IsVisible = true;
                    entryFornecedor.IsVisible = true;
                    entryPrecoCusto.IsVisible = true;
                    entryPrecoVenda.IsVisible = true;
                    entryPrecoVenda.Text = null;
                }
                else
                {
                    DisplayAlert("❌ Erro", "Não há nenhuma peça cadastrada com o código informado!", "OK");
                }
            }
        }
        #endregion


        #region Registrar Venda
        void btRegistrarVenda_Clicked(object sender, System.EventArgs e)
        {
            MysqlCon conexao = new MysqlCon();

            if (string.IsNullOrEmpty(entryPrecoVenda.Text))
            {
                DisplayAlert("❌ Erro:", "Amoor, você esqueceu de inserir o preço de venda da peça", "OK");
            }
            else
            {
                try
                {                   
                  
                    double precusto = double.Parse(entryPrecoCusto.Text.Replace("💲", "").Replace("R$ ",""));
                    double precovenda = double.Parse(entryPrecoVenda.Text);

                    conexao.RealizarVenda(precovenda, precusto, entryCodPeca.Text);

                    double lucro = precovenda - precusto;
                    String resultado = lucro.ToString("C");

                    DisplayAlert("💸 Venda Realizada", "\n Lucro dessa peça foi de " + resultado, "OK");
                    Navigation.PushAsync(new MenuPrincipal());
                }
                catch (Exception ex)
                {
                    DisplayAlert("❌ Erro", "Erro ao se conectar com o banco de dados: \n", "OK");
                }
            }
        }
        #endregion

        private void entryPrecoVenda_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ev = e as TextChangedEventArgs;
            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                entry.TextChanged -= entryPrecoVenda_TextChanged;
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
                entry.TextChanged += entryPrecoVenda_TextChanged;
            }
        }
    }
}