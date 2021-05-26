using Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace LacerdApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Produtos : ContentPage
    {
        public Produtos()
        {
            InitializeComponent();

            pickerTamanho.Items.Add("Único");
            pickerTamanho.Items.Add("PP");
            pickerTamanho.Items.Add("P");
            pickerTamanho.Items.Add("M");
            pickerTamanho.Items.Add("G");
            pickerTamanho.Items.Add("GG");

        }

        #region Cadastrar peça

        void btCadastro(object sender, System.EventArgs e)
        {
            MysqlCon conexao = new MysqlCon();

            if (string.IsNullOrEmpty(entryDescricao.Text) || string.IsNullOrEmpty(entryFornecedor.Text) || string.IsNullOrEmpty(entryPrecoCusto.Text))
            {
                DisplayAlert("❌ Erro", "Amoor, favor preencher todos os campos", "OK");
            }
            else
            {
                if (entryPrecoCusto.Text.Contains(","))
                {
                    string id = conexao.CadastrarProduto(entryFornecedor.Text, entryDescricao.Text, pickerTamanho.Items[pickerTamanho.SelectedIndex], entryCor.Text, entryPrecoCusto.Text.Replace(",", "."));

                    if (id != null)
                    {
                        DisplayAlert("Produto cadastrado!", "o código do produto é: " + id+ "\n Lembre-se de inserir o código da peça na etiqueta 😉", "OK");
                        Navigation.PushAsync(new MenuPrincipal());
                    }
                }
                else
                {
                    DisplayAlert("❌ Preço Incorreto", "Amor tem que inserir os centavos também por ex: 29,00", "OK");
                }

            }

        }
        #endregion

        private void entryPrecoCusto_TextChanged(object sender, TextChangedEventArgs e)
        {


            var ev = e as TextChangedEventArgs;
            if (ev.NewTextValue != ev.OldTextValue)
            {
                var entry = (Entry)sender;
                entry.TextChanged -= entryPrecoCusto_TextChanged;
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
                entry.TextChanged += entryPrecoCusto_TextChanged;
            }

        }

    }
}