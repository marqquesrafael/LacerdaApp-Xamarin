using System;
using System.Collections.Generic;
using System.Text;

namespace Produtos
{
    public class Produto
    {
        public string id { get; set; }
        public string fornecedor { get; set; }
        public string descricao { get; set; }
        public string tamanho { get; set; }
        public string cor { get; set; }
        public double precoCusto { get; set; }
        public string precoVenda { get; set; }

    }
}
