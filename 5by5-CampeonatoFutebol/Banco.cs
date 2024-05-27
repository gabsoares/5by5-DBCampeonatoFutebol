namespace _5by5_CampeonatoFutebol
{
    internal class Banco
    {
        readonly string conexao = "Data Source = 127.0.0.1; Initial Catalog=DBCampeonatoAmador; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=True;";

        public Banco()
        {

        }

        public string Caminho()
        {
            return conexao;
        }
    }
}
