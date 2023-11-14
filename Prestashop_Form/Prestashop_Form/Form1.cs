using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace Prestashop_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string url_image = "http://cargas.nua.pe/nua/0012914095508_01.jpg";
            //string url_image = "https://drive.google.com/file/d/18diyQe1Wuxkqx0erBlktGe7P56Y5wM8e/view";
            string idproducto_prestashop = "6793";
            string filename = "demo_final";
            string fileformat = "jpg";
            DownloadFile_Prestashop_image(url_image, filename, fileformat);
            Upload_Prestashop_image(idproducto_prestashop, filename, fileformat);

        }
        #region Descargar imagen desde una ruta
        public void DownloadFile_Prestashop_image(string origen, string filename, string fileformat)
        {
            try
            {
                string Ruta = @"C:\file_image";
                string destino = Ruta + "\\" + filename + "." + fileformat;

                using (WebClient client = new WebClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    client.DownloadFile(origen, destino);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #region Cargar a prestashop
        public void Upload_Prestashop_image(string idproducto, string filename, string fileformat)
        {
            try
            {
                string postURL = "http://nua.tudominio.pro/api/images/products/" + idproducto;
                string userAgent = "Test";
                string Key = "2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C";
                string Ruta = @"C:\file_image";

                FileStream fs = new FileStream(Ruta + "\\" + filename + "." + fileformat, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("filename", filename + "." + fileformat);
                postParameters.Add("fileformat", fileformat);
                postParameters.Add("image", new FormUpload.FileParameter(data, filename + "." + fileformat, "image/" + fileformat));

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, Key);
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();
            }
            catch (Exception ex)
            {

            }

        }

        #endregion
    }
    #region Metodo Upload
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string Key)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData, Key);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string Key)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            request.PreAuthenticate = true;
            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Key + ":" + "")));
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            return request.GetResponse() as HttpWebResponse;
        }
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;
                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");
                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }
        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
    #endregion
}
