using HtmlAgilityPack;

namespace Tomates.Models
{
    public class WebScraper
    {
        public static async Task<string> call_url(string url)
        {

            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);

            //Retorna todo el html
            return response;
        }

        //quitar los espacios de los comentarios
        public static List<List<string>> ActoresPrincipales(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            //actores principales

            var actores_nodos = document.DocumentNode.SelectSingleNode("//*[@id=\"cast-and-crew\"]/div/div[1]");

            //Todos los comentarios descienden en divs pero su informacion esta en un anchor 
            var actores_nodos_list = actores_nodos.Descendants("div").Where(node => node.GetAttributeValue("data-qa", "").Contains("cast-crew-item")).ToList();

            List<List<string>> actores = new List<List<string>>();

            foreach (var actor in actores_nodos_list)
            {
                var foto = actor.Descendants("img").ToList()[0].GetAttributeValue("src", "");
                var nombres = actor.Descendants("p").ToList();

                List<string> info_actor = new List<string>() { foto };
                foreach (var nombre in nombres)
                {
                    string valor = nombre.InnerText;
                    info_actor.Add(TrimDato(valor));
                }
                actores.Add(info_actor);
            }
            return actores;

        }

        public static List<List<string>> ComentariosCriticos(string url)
        {

            try
            {
                //Se le agrega a la url para que vaya a la pagina donde están los comentarios de los críticos

                string criticos_url = url + "/reviews?type=top_critics";


                //Comentarios de los usuarios verificados
                var html_criticos = call_url(criticos_url).Result;


                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html_criticos);

                //Todos los comentarios descienden en divs 
                var parse_data = document.DocumentNode.SelectSingleNode("//*[@id=\"reviews\"]/div[2]").Descendants("div").Where(node => node.GetAttributeValue("class", "").Contains("review-row")).ToList();

                List<List<string>> datos = new List<List<string>>();


                foreach (var comentario in parse_data)
                {

                    var foto = comentario.Descendants("img").Where(node => node.GetAttributeValue("class", "").Contains("critic-picture")).ToList()[0].GetAttributeValue("src", "");
                    var nombre = comentario.Descendants("a").Where(node => node.GetAttributeValue("class", "").Contains("display-name")).ToList()[0].InnerText;

                    var comentario_txt = comentario.Descendants("p").Where(node => node.GetAttributeValue("class", "").Contains("review-text")).ToList()[0].InnerText;

                    datos.Add(new List<string> { foto, TrimDato(nombre), TrimDato(comentario_txt) });

                }


                return datos;


            }
            catch (Exception e)
            {
                Console.WriteLine("No hay comentarios de los criticos");
                List<List<string>> datos = new List<List<string>>();

                return datos;
            }


        }


        public static List<List<string>> ComentariosUsuarios(string url, bool pelicula_true = true)
        {


            try
            {
                string usuario_url;
                if (pelicula_true)
                {
                    usuario_url = url + "/reviews?type=verified_audience";
                }
                else
                {
                    usuario_url = url + "/reviews?type=user";
                }

                //Se le agrega a la url para que vaya a la pagina donde están los comentarios de los usuarios verificados


                //Comentarios de los usuarios verificados
                var html_usuarios = call_url(usuario_url).Result;

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html_usuarios);

                //Todos los comentarios descienden en divs 
                var parse_data = document.DocumentNode.SelectSingleNode("//*[@id=\"reviews\"]/div[2]/div[2]").Descendants("div").Where(node => node.GetAttributeValue("class", "").Contains("audience-review-row")).ToList();

                List<List<string>> datos = new List<List<string>>();


                foreach (var comentario in parse_data)
                {
                    string nombre;
                    if (pelicula_true)
                    {
                        nombre = comentario.Descendants("span").Where(node => node.GetAttributeValue("data-qa", "").Contains("review-name")).ToList()[0].InnerText;

                    }
                    else
                    {
                        nombre = comentario.Descendants("a").Where(node => node.GetAttributeValue("data-qa", "").Contains("review-name")).ToList()[0].InnerText;

                    }
                    var comentario_txt = comentario.Descendants("p").Where(node => node.GetAttributeValue("data-qa", "").Contains("review-text")).ToList()[0].InnerText;


                    datos.Add(new List<string> { TrimDato(nombre), TrimDato(comentario_txt) });

                }

                return datos;

            }
            catch (Exception ex)
            {
                List<List<string>> datos = new List<List<string>>();

                Console.WriteLine("no hay comentarios de los usuarios verificados");

                return datos;

            }



        }


        public static List<List<string>> PlataformasDisponibles(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            /* las plataformas donde está disponible,*/

            var parsed_data = document.DocumentNode.Descendants("where-to-watch-meta").Where(node => node.GetAttributeValue("data-qa", "").Contains("affiliate-item")).ToList();

            List<List<string>> datos = new List<List<string>>();

            //Pueden haber varias platadormas y estas descienden en varios <where-to-watch-bubble>
            //Se extrae el nombre del atributo image
            foreach (var node in parsed_data)
            {
                var link_img = node.GetAttributeValue("href", "href");
                var plataforma = node.SelectSingleNode("where-to-watch-bubble").GetAttributeValue("image", "");
                plataforma = plataforma.Replace("-", " ");
                List<string> list = new List<string>() { link_img, plataforma };

                datos.Add(list);

            }

            return datos;

        }


        public static string TrimDato(string valor)
        {

            char[] charsToTrim = { ' ', '\n' };
            //Eliminando los espacios inecesarios

            valor = valor.Replace("  ", "");
            valor = valor.Replace("\n", "");
            valor.Trim(charsToTrim);


            return valor;

        }

        public static List<List<string>> Calificacion(string html, bool pelicula_temporada = true)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            List<string> datos = new List<string>();

            //datos principales

            //div del cual se comienza la busqueda
            var informacion = document.DocumentNode.SelectSingleNode("//*[@id=\"topSection\"]");

            //<score-board> del que descienden las calificaciones
            var score = informacion.Descendants("score-board").Where(node => node.Id.Contains("scoreboard")).ToList()[0];
            string tomatometro_reviews = "";
            string audiencia_reviews = "";

            if (pelicula_temporada)
            {
                tomatometro_reviews = score.Descendants("a").Where(node => node.GetAttributeValue("data-qa", "").Contains("tomatometer-review-count")).ToList()[0].InnerText;
                audiencia_reviews = score.Descendants("a").Where(node => node.GetAttributeValue("data-qa", "").Contains("audience-rating-count")).ToList()[0].InnerText;


            }


            var audience = score.GetAttributeValue("audiencescore", "");


            var tomatometro = score.GetAttributeValue("tomatometerscore", "");


            List<List<string>> pelicula_info = new List<List<string>>();

            pelicula_info.Add(new List<string> { "audiencia", TrimDato(audience) });
            pelicula_info.Add(new List<string> { "audiencia reviews", TrimDato(audiencia_reviews) });
            pelicula_info.Add(new List<string> { "tomatometro", TrimDato(tomatometro) });
            pelicula_info.Add(new List<string> { "tomatometro reviews", TrimDato(tomatometro_reviews) });


            return pelicula_info;

        }


        public static List<List<string>> Descripcion(string html, bool pelicula_true = true, bool temporada = false)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var descripcion = document.DocumentNode.Descendants("drawer-more").Where(node => node.GetAttributeValue("status", "").Contains("closed")).ToList();

            //Si es una pelicula o la pagina principal de una serie solamente tiene una sinopsis

            if (pelicula_true || !temporada)
            {


                return new List<List<string>>() { new List<string>() { "sinopsis:", TrimDato(descripcion[0].InnerText) } };
            }

            //Si es una serie trae la siniopsis de cada capitulo
            List<List<string>> sinopsis_capitulos = new List<List<string>>();

            int capitulo = 1;

            foreach (var sinopsis in descripcion)
            {

                sinopsis_capitulos.Add(new List<string>() { $"Capitulo {capitulo}", TrimDato(descripcion[capitulo - 1].InnerText) });

                capitulo++;
            }


            return sinopsis_capitulos;

        }
        public static List<List<string>> DatosPrincipales(string html, bool pelicula = true, bool temporada = false)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            List<string> datos = new List<string>();

            HtmlNode informacion;
            string nombre_pelicula;
            string movie_serie;
            //Son importantes en algunos datos el esapcio al final 
            string[] datos_solicitados = new string[11] {"Rating:","Genre:","Original Language:"
                ,"Release Date (Streaming):","Release Date (Theaters):","Runtime:","Distributor:","Production Co:", "TV Network: ","Premiere Date: ","Genre: "};

            if (pelicula)
            {

                informacion = document.DocumentNode.SelectSingleNode("//*[@id=\"topSection\"]");
                nombre_pelicula = informacion.Descendants("h1").Where(node => node.GetAttributeValue("data-qa", "").Contains("score-panel-movie-title")).ToList()[0].InnerText;
                movie_serie = "//*[@id=\"movie-info\"]";
            }
            else
            {

                informacion = document.DocumentNode.SelectSingleNode("//*[@id=\"topSection\"]");
                movie_serie = "//*[@id=\"series-info\"]";

                //cambia de p a h1 y el node de inicio para buscar la informacion
                if (temporada)
                {
                    nombre_pelicula = informacion.Descendants("p").Where(node => node.GetAttributeValue("slot", "").Contains("title")).ToList()[0].InnerText;
                    movie_serie = "//*[@id=\"tv-season-info\"]";
                }
                else
                {
                    nombre_pelicula = informacion.Descendants("h1").Where(node => node.GetAttributeValue("slot", "").Contains("title")).ToList()[0].InnerText;
                }


            }
            //datos principales

            //div del cual se parte a buscar la imagen y el nombre

            var imagen = informacion.Descendants("img").Where(node => node.GetAttributeValue("alt", "").Contains(nombre_pelicula)).ToList()[0].GetAttributeValue("src", "");


            //lista que contiene la info de la pelicula
            var info = document.DocumentNode.SelectSingleNode(movie_serie).Descendants("ul").ToList()[0].Descendants("li").ToList();


            List<List<string>> pelicula_info = new List<List<string>>();

            pelicula_info.Add(new List<string> { "imagen", imagen });
            pelicula_info.Add(new List<string> { "nombre", TrimDato(nombre_pelicula) });



            foreach (var li in info)
            {
                List<string> datos_pelicula = new List<string>();

                //nombre del dato
                var tipo_dato = li.Descendants("b").ToList()[0].InnerText;

                //Si el tipo de dato es uno de los que está en la base de datos
                if (datos_solicitados.Contains(tipo_dato))
                {



                    datos_pelicula.Add(tipo_dato);

                    var a_descendientes = li.Descendants("span").ToList()[0].Descendants("a").ToList();

                    if (a_descendientes.Count == 0)
                    {
                        //Si no hay mas datos que desciendan del spam
                        //el valor de ese dato está en el innertext del span
                        var valor = li.Descendants("span").ToList()[0].InnerText;
                        valor = TrimDato(valor);


                        //Hay algunos anchor que solo tienen el valor de &nbsp; por lo que se omiten 
                        if (!valor.StartsWith("&nbsp;"))
                        {
                            datos_pelicula.Add(valor);

                        }

                        //Si hay mas datos que descienden del span
                    }
                    else
                    {
                        //Estos datos están en un anchor entonces se itera y se toma solo el innertext
                        foreach (var a in a_descendientes)
                        {

                            var valor = a.InnerText;

                            valor = TrimDato(valor);

                            //Hay algunos anchor que solo tienen el valor de &nbsp; por lo que se omiten 
                            if (!valor.StartsWith("&nbsp;"))
                            {
                                datos_pelicula.Add(valor);

                            }
                        }

                    }

                    pelicula_info.Add(datos_pelicula);
                }
            }


            return pelicula_info;

        }
    }
}
