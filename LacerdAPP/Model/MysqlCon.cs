using LacerdApp;
using LacerdAPP;
using LacerdAPP.ViewModel;
using MySqlConnector;
using Produtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using usuarios;

namespace Mysql
{
    public class MysqlCon
    {

        public MySqlConnection conexao = new MySqlConnection("Persist Security Info = False; server=lacerdapp.mysql.uhserver.com;database=lacerdapp;uid=marqquesrafael;pwd=r14011994@");
        public MySqlDataAdapter Adapter;

        #region lista os usuarios
        public List<Usuario> CarregaUsuarios(string usuario, string senha)
        {
            DataTable dtUsuarios = new DataTable();
            Usuario u = new Usuario();
            List<Usuario> lista = new List<Usuario>();
            string consulta = $"SELECT * FROM usuarios where login = '{usuario}' and senha = '{senha}'";

            try
            {
                conexao.Open();

                Adapter = new MySqlDataAdapter(consulta, conexao);
                Adapter.Fill(dtUsuarios);

                for (int i = 0; i < dtUsuarios.Rows.Count; i++)
                {
                    u.usuario = dtUsuarios.Rows[i]["login"].ToString();
                    u.senha = dtUsuarios.Rows[i]["senha"].ToString();

                    lista.Add(u);
                }

                return lista;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("❌ Erro", "Erro ao se conectar com o banco de dados \n", "OK");
                return null;
            }
        }
        #endregion

        #region Buscar Produto  
        public List<Produto> BuscarProduto(string id)
        {
            DataTable dtProdutos = new DataTable();
            Produto p = new Produto();
            List<Produto> lista = new List<Produto>();

            string consulta = $"select * from produtos where id = {id}";

            try
            {
                conexao.Open();

                Adapter = new MySqlDataAdapter(consulta, conexao);
                Adapter.Fill(dtProdutos);

                for (int i = 0; i < dtProdutos.Rows.Count; i++)
                {

                    p.id = dtProdutos.Rows[i]["id"].ToString();

                    p.descricao = dtProdutos.Rows[i]["descricao"].ToString();

                    p.fornecedor = dtProdutos.Rows[i]["fornecedor"].ToString();

                    p.tamanho = dtProdutos.Rows[i]["tamanho"].ToString();

                    p.cor = dtProdutos.Rows[i]["cor"].ToString();

                    p.precoCusto = double.Parse(dtProdutos.Rows[i]["precocusto"].ToString());

                    lista.Add(p);
                }

                return lista;

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("❌ Erro", "Erro ao se conectar com o banco de dados \n", "OK");
                return null;
            }

        }
        #endregion

        #region Cadastra produtos
        public string CadastrarProduto(string fornecedor, string descricao, string tamanho, string cor, string precocusto)
        {
            string id = string.Empty;
            DataTable dtID = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            string query = $@"INSERT INTO `produtos` (`fornecedor`, `descricao`, `tamanho`,`cor`,`precocusto`,`data_cadastro`, `statusvenda`) VALUES
('{fornecedor}', '{descricao}', '{tamanho}', '{cor}', '{precocusto}', SYSDATE(), 0)";
            string queryReturnID = "SELECT LAST_INSERT_ID();";

            try
            {

                conexao.Open();

                cmd.Connection = conexao;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                Adapter = new MySqlDataAdapter(queryReturnID, conexao);
                Adapter.Fill(dtID);

                for (int i = 0; i < dtID.Rows.Count; i++)
                {
                    id = dtID.Rows[i]["LAST_INSERT_ID()"].ToString();
                }

                return id;

            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("❌ Erro", "Erro ao se conectar com o banco de dados \n", "OK");
                return null;
            }

        }

        #endregion

        #region Realizar Venda
        public void RealizarVenda(double venda, double custo, string id)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlCommand cmdCaixa = new MySqlCommand();
            //string ultimoID = BuscarUltimoID();

            //double precoCusto = double.Parse(custo);
            //double precovenda = double.Parse(venda);
            double lucroVenda = venda - custo;

            List<Valores> caixa = ListarInvestimento_Lucro();

            double InvestimentoAtual = double.Parse(caixa[0].investimentoAtual.ToString());
            double LucroAtual = double.Parse(caixa[0].lucroAtual.ToString());

            InvestimentoAtual += custo;
            LucroAtual += lucroVenda;

            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();
            }
            string query = $"update produtos set precovenda = '{venda.ToString().Replace(",",".")}', statusvenda = 1 where id = {id}";
            string queryAtualizaCaixa = $"insert into caixa (investimento_atual, lucro_atual) values ('{InvestimentoAtual.ToString().Replace(",", ".")}', '{LucroAtual.ToString().Replace(",", ".")}')";

            cmd.Connection = conexao;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();

            cmdCaixa.Connection = conexao;
            cmdCaixa.CommandText = queryAtualizaCaixa;
            cmdCaixa.ExecuteNonQuery();
   
        }
        #endregion

        #region Listar Caixa
        public List<Valores> ListarInvestimento_Lucro()
        {
            DataTable dt = new DataTable();
            Valores v = new Valores();
            List<Valores> lista = new List<Valores>();
            string consulta = $"select * from caixa order by id desc";

            try
            {
                conexao.Open();

                Adapter = new MySqlDataAdapter(consulta, conexao);
                Adapter.Fill(dt);

                v.investimentoAtual = double.Parse(dt.Rows[0]["investimento_atual"].ToString());
                v.lucroAtual = double.Parse(dt.Rows[0]["lucro_atual"].ToString());

                lista.Add(v);

                return lista;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("❌ Erro", "Erro ao se conectar com o banco de dados \n", "OK");
                return null;
            }
        }

        #endregion

        #region Atualizar Caixa
        public void AtualizarCaixa(string investimento, string lucro, string motivo, string valorDebito, int opcaoDebito)
        {
            MySqlCommand cmd = new MySqlCommand();

            string queryInsert = string.Empty;

            if (conexao.State == ConnectionState.Closed)
            {
                conexao.Open();
            }

            if (opcaoDebito == 1)
            {
                queryInsert = $@"insert into caixa (investimento_atual, lucro_atual, motivo_deb_investimento,data_deb_investimento, valor_deb_investimento) values
                ({investimento}, {lucro}, '{motivo}', SYSDATE(), {valorDebito})";
            }
            if (opcaoDebito == 2)
            {
                queryInsert = $@"insert into caixa (investimento_atual, lucro_atual, motivo_deb_lucro,data_deb_lucro, valor_deb_lucro) values
                ({investimento}, {lucro}, '{motivo}', SYSDATE(), {valorDebito})";
            }

            cmd.Connection = conexao;
            cmd.CommandText = queryInsert;
            cmd.ExecuteNonQuery();
        }
        #endregion

    }
}
