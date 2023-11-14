using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Clases.ApiRest
{
    public class DBApi
    {
        public dynamic Post(string url, string json, string autorizacion = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                //client.Authenticator=new http("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C", " ");
                string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

                client.Authenticator = new HttpBasicAuthenticator(key, " ");


                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                if (autorizacion != null)
                {
                    request.AddHeader("Authorization", autorizacion);
                }

                IRestResponse response = client.Execute(request);

                dynamic datos = JsonConvert.DeserializeObject(response.Content);

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }



        public dynamic Putstock(string url, string json, string autorizacion = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var client = new RestClient(url);
                var request = new RestRequest(Method.PUT);
                //client.Authenticator=new http("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C", " ");
                string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

                client.Authenticator = new HttpBasicAuthenticator(key, " ");


                request.AddHeader("content-type", "application/xml");
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                if (autorizacion != null)
                {
                    request.AddHeader("Authorization", autorizacion);
                }

                IRestResponse response = client.Execute(request);

                dynamic datos = JsonConvert.DeserializeObject(response.Content);

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public dynamic PutPrecio(string url, string json, string autorizacion = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var client = new RestClient(url);
                var request = new RestRequest(Method.PUT);
                //client.Authenticator=new http("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C", " ");
                string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

                client.Authenticator = new HttpBasicAuthenticator(key, " ");


                request.AddHeader("content-type", "application/xml");
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                if (autorizacion != null)
                {
                    request.AddHeader("Authorization", autorizacion);
                }

                IRestResponse response = client.Execute(request);

                dynamic datos = JsonConvert.DeserializeObject(response.Content);

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        public dynamic Get(string url)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myWebRequest.Headers.Add("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C"," ");
            string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

            myWebRequest.Credentials = new NetworkCredential(key, " ");
           // myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";
            //myWebRequest.CookieContainer = myCookie;
          //  myWebRequest.Credentials = CredentialCache.DefaultCredentials;
           // myWebRequest.Proxy = null;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

       //     dynamic data = JsonConvert.DeserializeObject(Datos); ///deseareliza

            dynamic data = Datos;


            return data;
        }

        public dynamic GetGV(string url)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myWebRequest.Headers.Add("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C"," ");
            string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

            myWebRequest.Credentials = new NetworkCredential(key, " ");
            // myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";
            //myWebRequest.CookieContainer = myCookie;
            //  myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            // myWebRequest.Proxy = null;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

               dynamic data = JsonConvert.DeserializeObject(Datos); ///deseareliza

          //  dynamic data = Datos;


            return data;
        }

        public dynamic GetGVStock(string url)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myWebRequest.Headers.Add("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C"," ");
            string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

            myWebRequest.Credentials = new NetworkCredential(key, " ");
            // myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";
            //myWebRequest.CookieContainer = myCookie;
            //  myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            // myWebRequest.Proxy = null;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

            dynamic data = JsonConvert.DeserializeObject(Datos); ///deseareliza

            //  dynamic data = Datos;


            return data;
        }

        public dynamic GetGVStockProducts(string url)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //myWebRequest.Headers.Add("2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C"," ");
            string key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();

            myWebRequest.Credentials = new NetworkCredential(key, " ");
            // myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";
            //myWebRequest.CookieContainer = myCookie;
            //  myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            // myWebRequest.Proxy = null;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

            dynamic data = JsonConvert.DeserializeObject(Datos); ///deseareliza

            //  dynamic data = Datos;


            return data;
        }

    }
}
