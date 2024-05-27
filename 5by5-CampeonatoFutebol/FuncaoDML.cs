using System.Data;
using Microsoft.Data.SqlClient;

namespace _5by5_CampeonatoFutebol
{
    internal class FuncaoDML
    {
        static Banco b = new Banco();
        SqlConnection conn = new(b.Caminho());
        SqlCommand cmd = new();
        delegate void Funcao();

        public FuncaoDML()
        {
        }

        void InserirTime()
        {
            bool podeInserir = true;
            do
            {
                Console.Write("Quantos times quer inserir: ");
                int opc = retornarInt();

                if (opc > 5)
                {
                    Console.WriteLine("Impossível cadastrar mais de 5 times");
                    podeInserir = false;
                }
                else if (opc < 3)
                {
                    Console.WriteLine("Necessario cadastrar pelo menos 3 times");
                    podeInserir = false;
                }
                else
                {
                    if (ObterQuantidadeTime() < 5)
                    {
                        for (int i = 0; i < opc; i++)
                        {
                            podeInserir = true;

                            Console.Write("Insira o nome do clube: ");
                            string nomeClube = Console.ReadLine();
                            Console.Write("Insira o apelido do clube: ");
                            string apelidoClube = Console.ReadLine();
                            Console.Write("Insira a data de criacao do clube: ");
                            DateOnly dataCriacao = DateOnly.Parse(Console.ReadLine());

                            try
                            {
                                conn.Open();
                                cmd = new("INSERIR_TIME", conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@NOME", SqlDbType.VarChar, 50).Value = nomeClube;
                                cmd.Parameters.Add("@APELIDO", SqlDbType.VarChar, 50).Value = apelidoClube;
                                cmd.Parameters.Add("@DATA_CRIACAO", SqlDbType.Date).Value = dataCriacao;

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine(reader.GetString(0));
                                        i = reader.GetInt32(1);
                                    }
                                    PressioneEnter();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex.Source.ToString());
                                PressioneEnter();
                                return;
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ja existem 5 times cadastrados.");
                        PressioneEnter();
                    }
                }
            } while (podeInserir == false);
        }

        void InserirPartida()
        {
            Random random = new Random();

            int idCasa = 0;
            int idVisita = 0;

            bool limitePartidaAtingido = (ObterQuantidadeTime() * ObterQuantidadeTime()) - ObterQuantidadeTime() == ObterQuantidadePartida();

            if (!limitePartidaAtingido)
            {
                for (int i = 1; i <= ObterQuantidadeTime(); i++)
                {
                    idCasa = i;
                    for (int j = 1; j <= ObterQuantidadeTime(); j++)
                    {
                        if (i != j)
                        {
                            idVisita = j;

                            try
                            {
                                conn.Open();

                                cmd = new("INSERIR_PARTIDA", conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@GOLS_CASA", SqlDbType.Int).Value = random.Next(0, 8);
                                cmd.Parameters.Add("@GOLS_VISITA", SqlDbType.Int).Value = random.Next(0, 8);
                                cmd.Parameters.Add("@ID_CASA", SqlDbType.Int).Value = idCasa;
                                cmd.Parameters.Add("@ID_VISITA", SqlDbType.Int).Value = idVisita;
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erro: " + ex.Source.ToString());
                                PressioneEnter();
                                return;
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Limite de partidas atingido!!!");
                PressioneEnter();
                return;
            }
        }

        void ExcluirDados()
        {
            try
            {
                conn.Open();

                cmd = new("EXCLUIR_DADOS", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterCampeao()
        {
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT TOP 1 C.NOME, C.APELIDO, CC.PONTUACAO FROM CLASSIFICACAO CC JOIN CLUBE C ON CC.ID_CLUBE = C.ID ORDER BY CC.PONTUACAO DESC";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("[Nome do Clube Campeao]: ");
                        Console.WriteLine("{0}", reader.GetString(0));
                        Console.Write("[Apelido do Clube Campeao]: ");
                        Console.WriteLine("{0}", reader.GetString(1));
                        Console.Write("[Pontuacao do Clube Campeao]: ");
                        Console.WriteLine("{0}", reader.GetInt32(2));
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterRankingTimes()
        {
            int cont = 1;
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT C.NOME, C.APELIDO, CC.PONTUACAO FROM CLASSIFICACAO CC JOIN CLUBE C ON CC.ID_CLUBE = C.ID ORDER BY CC.PONTUACAO DESC";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write($"[Nome do {cont}° colocado]: ");
                        Console.WriteLine("{0}", reader.GetString(0));
                        Console.Write($"[Apelido do {cont}° colocado]: ");
                        Console.WriteLine("{0}", reader.GetString(1));
                        Console.Write($"[Pontuacao do {cont}° colocado]: ");
                        Console.WriteLine("{0}", reader.GetInt32(2));
                        Console.WriteLine();
                        cont++;
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterTimeMaisGolsFeitos()
        {
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT TOP 1 C.NOME, C.APELIDO, CC.GOLS_FEITOS FROM CLASSIFICACAO CC JOIN CLUBE C ON CC.ID_CLUBE = C.ID ORDER BY CC.GOLS_FEITOS DESC;";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("[Nome do time que mais fez gols no campeonato]: ");
                        Console.WriteLine("{0}", reader.GetString(0));
                        Console.Write("[Apelido do time que mais fez gols no campeonato]: ");
                        Console.WriteLine("{0}", reader.GetString(1));
                        Console.Write("[Quantidade de gols feitos por esse time]: ");
                        Console.WriteLine("{0}", reader.GetInt32(2));
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterTimeMaisGolsSofridos()
        {
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT TOP 1 C.NOME, C.APELIDO, CC.GOLS_SOFRIDOS FROM CLASSIFICACAO CC JOIN CLUBE C ON CC.ID_CLUBE = C.ID ORDER BY CC.GOLS_SOFRIDOS DESC;";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("[Nome do time que mais tomou gols no campeonato]: ");
                        Console.WriteLine("{0}", reader.GetString(0));
                        Console.Write("[Apelido do time que mais tomou gols no campeonato]: ");
                        Console.WriteLine("{0}", reader.GetString(1));
                        Console.Write("[Quantidade de gols sofridos por esse time]: ");
                        Console.WriteLine("{0}", reader.GetInt32(2));
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterJogoMaisGolsFeitos()
        {
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT TOP 1 JJ.ID_JOGO, JJ.GOLS_TOTAIS_JOGO FROM JOGO JJ ORDER BY JJ.GOLS_TOTAIS_JOGO DESC";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("[ID do jogo que mais teve gols]: ");
                        Console.WriteLine("{0}", reader.GetInt32(0));
                        Console.Write("[Quantidade de gols feitos apenas neste jogo]: ");
                        Console.WriteLine("{0}", reader.GetInt32(1));
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        void ObterMaiorQuantidadeGolsCadaTime()
        {
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT c.NOME, MAX(j.GOLS_CASA), MAX(j.GOLS_VISITA) FROM CLUBE c JOIN JOGO j ON c.ID = j.ID_CASA OR c.ID = j.ID_VISITA GROUP BY c.NOME;";
                cmd.Connection = conn;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("[Nome do clube]: ");
                        Console.WriteLine("{0}", reader.GetString(0));
                        Console.Write("[Maior quantidade gols do clube feitos sendo mandante]: ");
                        Console.WriteLine("{0}", reader.GetInt32(1));
                        Console.Write("[Maior quantidade gols do clube feitos sendo visitante]: ");
                        Console.WriteLine("{0}", reader.GetInt32(2));
                        Console.WriteLine();
                    }
                    PressioneEnter();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        int ObterQuantidadeTime()
        {
            var numeroClube = 0;
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT COUNT(*) FROM CLUBE";
                cmd.Connection = conn;


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numeroClube = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return -1;
            }
            finally
            {
                conn.Close();
            }
            return numeroClube;
        }

        bool QuantidadeTimeZerada()
        {
            return ObterQuantidadeTime() == 0;
        }

        void ValidacaoTimeZerado(Funcao funcao)
        {
            if (!QuantidadeTimeZerada())
            {
                funcao();
            }
            else
            {
                Console.WriteLine("Necessario inserir os times primeiro.");
                PressioneEnter();
                return;
            }
        }

        int ObterQuantidadePartida()
        {
            var numeroPartida = 0;
            try
            {
                conn.Open();

                cmd = new();
                cmd.CommandText = "SELECT COUNT(*) FROM JOGO";
                cmd.Connection = conn;


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numeroPartida = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Source.ToString());
                PressioneEnter();
                return -1;
            }
            finally
            {
                conn.Close();
            }
            return numeroPartida;
        }

        int retornarInt()
        {
            int Inteiro = 0;
            bool ex = false;

            while (!ex)
            {
                if (int.TryParse(Console.ReadLine(), out int varint))
                {
                    Inteiro = varint;
                    ex = true;
                }
                else
                {
                    Console.WriteLine("Formato inválido. Informe números inteiros apenas.");
                }
            }
            return Inteiro;
        }

        void PressioneEnter()
        {
            Console.WriteLine("Pressione enter para continuar...");
            Console.ReadKey();
        }

        int Menu()
        {
            Console.Clear();
            Console.WriteLine("<<<<<<<<<<FUTEBOL>>>>>>>>>>");
            Console.WriteLine("[ 1 ]  Inserir um time");
            Console.WriteLine("[ 2 ]  Inserir as partidas");
            Console.WriteLine("[ 3 ]  Obter o clube campeao do campeonato");
            Console.WriteLine("[ 4 ]  Obter o ranking dos times");
            Console.WriteLine("[ 5 ]  Obter o time com mais gols feitos no campeonato");
            Console.WriteLine("[ 6 ]  Obter o time com mais gols sofridos no campeonato");
            Console.WriteLine("[ 7 ]  Obter o jogo com mais gols do campeonato");
            Console.WriteLine("[ 8 ]  Obter o maior numero de gols de cada time em um unico jogo");
            Console.WriteLine("[ 9 ]  Resetar o campeonato e times");
            Console.WriteLine("[ 0 ]  Sair do programa");
            Console.Write("Insira uma das opcoes validas [ 0 - 9 ]:< > \b\b\b");

            int option = retornarInt();
            return option;
        }

        public void ChamarMenu()
        {
            bool terminouMenu = false;
            do
            {
                switch (Menu())
                {
                    case 1:
                        InserirTime();
                        break;
                    case 2:
                        ValidacaoTimeZerado(InserirPartida);
                        break;
                    case 3:
                        ValidacaoTimeZerado(ObterCampeao);
                        break;
                    case 4:
                        ValidacaoTimeZerado(ObterRankingTimes);
                        break;
                    case 5:
                        ValidacaoTimeZerado(ObterTimeMaisGolsFeitos);
                        break;
                    case 6:
                        ValidacaoTimeZerado(ObterTimeMaisGolsSofridos);
                        break;
                    case 7:
                        ValidacaoTimeZerado(ObterJogoMaisGolsFeitos);
                        break;
                    case 8:
                        ValidacaoTimeZerado(ObterMaiorQuantidadeGolsCadaTime);
                        break;
                    case 9:
                        ExcluirDados();
                        break;
                    case 0:
                        Console.WriteLine("Encerrando o programa.");
                        terminouMenu = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            } while (!terminouMenu);
        }
    }
}