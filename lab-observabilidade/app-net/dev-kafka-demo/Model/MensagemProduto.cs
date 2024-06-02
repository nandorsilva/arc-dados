namespace dev_kafka_demo.Model
{
    public class MensagemProduto
    {
        //public int IDPEDIDO { get; set; }
        //public string NOME { get; set; }

        public int Id { get; set; }
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public decimal? Valor { get; set; }

        public int? Quantidade { get; set; }

        public decimal ValorTotal
        {
            get
            {
                decimal ret = 0;

                if (this.Valor.HasValue && this.Quantidade.HasValue)
                    ret = this.Valor.Value * this.Quantidade.Value;

                return ret;
            }
        }
    }
}
