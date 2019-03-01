using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarteleraMvc.Models;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Vse.Web.Serialization;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.YouTube.v3.Data;

namespace CarteleraMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About Cartelera: ";
            ViewData["Description"] = "Cartelera is just a simple APP to find differents movies or TV shows and see a trailer.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "My personal info.";
            ViewData["Name"] = "Ruiz, Maximiliano";
            ViewData["MobilePhone"] = "1566698304";
            ViewData["Address"] = "Calle 462, Berazategui";
            ViewData["Mail"] = "maxi.ruiz.1991@gmail.com";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string nombre)
        {
            //token para utilizar la API
            string apiKey = "ede6e2a";
            string baseUri = $"http://www.omdbapi.com/?apikey={apiKey}";

            string name = nombre;

            var sb = new StringBuilder(baseUri);
            sb.Append($"&s={name}");

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(sb.ToString());
            getRequest.Method = "GET";

            var getResponse = (HttpWebResponse)getRequest.GetResponse();
            Stream newStream = getResponse.GetResponseStream();
            StreamReader sr = new StreamReader(newStream);
            var result = sr.ReadToEnd();
            var responseApi = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (responseApi.Response)
            {
                //Filtro de la lista que obtengo de la API por aquellas que solo sean peliculas o series.
                var MovieList = responseApi.Search.Where(x => x.Type == "movie" || x.Type == "series").ToList();
                return View("Index", MovieList);
            }
            else
            {
                ViewBag.error = "Movie not found.";
                return View("Error");
            }

        }

        public IActionResult MovieInfo(string imdbID)
        {
            string apiKey = "ede6e2a";
            string Uri = $"http://www.omdbapi.com/?apikey={apiKey}&i={imdbID}&plot=full";


            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(Uri);
            getRequest.Method = "GET";

            var getResponse = (HttpWebResponse)getRequest.GetResponse();
            Stream newStream = getResponse.GetResponseStream();
            StreamReader sr = new StreamReader(newStream);
            var result = sr.ReadToEnd();
            var responseApi = JsonConvert.DeserializeObject<MovieDetails>(result);

            if (responseApi.Response)
            {
                return View("MovieInfo", responseApi);
            }
            else
            {
                ViewBag.error = "Movie not found.";
                return View("Error");
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorApi()
        {
            ViewBag.error = "Daily Limit for Unauthenticated Use Exceeded from youtube API.";
            return View("Error");
        }
    }
}
