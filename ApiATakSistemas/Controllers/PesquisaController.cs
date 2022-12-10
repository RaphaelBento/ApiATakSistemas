using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiATakSistemas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PesquisaController : ControllerBase
    {
      

        [HttpGet("Pesquise_Aqui")]
        public string PesquisaNoGoogle(string Pesquisa) 
        {

            var requisicaoWeb = WebRequest.CreateHttp("https://www.google.com.br/search?q=" + Pesquisa);
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var ResponseToString = objResponse.ToString();
                string MarcadorLimpaString = "\"";

                int index;

                string Temp;
                Regex regex = new Regex(@"https://www.*?\s");
                var MatchRegex = regex.Matches(ResponseToString);
                List<string> pedacos = new List<string>();

                foreach (var item in MatchRegex)
                {
                    Temp = item.ToString();
                    index = Temp.IndexOf(MarcadorLimpaString);
                    if (index > 0)
                        pedacos.Add("{" + Temp.Substring(0, index) + "}");

                }
                 var json = JsonSerializer.Serialize(pedacos);
                streamDados.Close();
                resposta.Close();
                if (json != null)
                    return json;
                else 
                {
                    return "Desculpe não foi possivel retornar resultados"; 
                }
            } 

        }

    }
}
