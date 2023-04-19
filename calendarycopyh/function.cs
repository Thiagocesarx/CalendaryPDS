using System;
using System.Data;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        [HttpPost]
        public ActionResult NewEvent(string nomeEvento, DateTime dataHora, string cargaHoraria, string descricao, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string uf)
        {
            // Verifica se já existe algum evento registrado no mesmo dia, com pelo menos 5 horas de diferença.
            DateTime dataInicio = dataHora.AddHours(-5);
            DateTime dataFim = dataHora.AddHours(5);
            string connectionString = "server=thepression.mysql.database.azure.com;user id=tester;password=AiderHUB23;database=AiderHUBazure";
            string sql = "SELECT COUNT(*) FROM events WHERE data_hora BETWEEN @inicio AND @fim;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inicio", dataInicio);
                    command.Parameters.AddWithValue("@fim", dataFim);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        // Já existe um evento registrado no mesmo dia, com menos de 5 horas de diferença.
                        return Content("Não é possível registrar um evento neste período. Já existe um evento registrado no mesmo dia, com pelo menos 5 horas de diferença.");
                    }
                }
            }

            // Insere o evento no banco de dados.
            sql = "INSERT INTO events (nome_evento, data_hora, carga_horaria, descricao, cep, logradouro, numero, complemento, bairro, cidade, uf) VALUES (@nomeEvento, @dataHora, @cargaHoraria, @descricao, @cep, @logradouro, @numero, @complemento, @bairro, @cidade, @uf);";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nomeEvento", nomeEvento);
                    command.Parameters.AddWithValue("@dataHora", dataHora);
                    command.Parameters.AddWithValue("@cargaHoraria", cargaHoraria);
                    command.Parameters.AddWithValue("@descricao", descricao);
                    command.Parameters.AddWithValue("@cep", cep);
                    command.Parameters.AddWithValue("@logradouro", logradouro);
                    command.Parameters.AddWithValue("@numero", numero);
                    command.Parameters.AddWithValue("@complemento", complemento);
                    command.Parameters.AddWithValue("@bairro", bairro);
                    command.Parameters.AddWithValue("@cidade", cidade);
                    command.Parameters.AddWithValue("@uf", uf);
                    command.ExecuteNonQuery();
                }
            }

            // Exibe mensagem de sucesso.
            return Content("Evento registrado com sucesso!");
        }
    }
}
