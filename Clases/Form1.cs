using Clases.ApiRest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Data.Common;
using System.Net;
using System.Text;





namespace Clases
{
    public partial class Form1 : Form
    {
        DBApi dBApi = new DBApi();
        private string mensaje;
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());


        private void ConvertToXMLActStock(IDataReader reader, string recordName)
        {
            XElement resultNode = new XElement("prestashop");
            while (reader.Read())
            {
                XElement row = new XElement(recordName);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(new XElement(reader.GetName(i), reader.GetValue(i)));
                }
                resultNode.Add(row);
            }
            string s = resultNode.ToString();
          //  string rrr = resultNode.ToString();
            txtPut.Text = s.ToString();

        }

        private void btnPrueba_Click(object sender, EventArgs e)
        {
            label9.Text = ("Actualizar stock prestashop...");

            Persona persona = new Persona
            {
                //job = txtTrabajador.Text,
                //name = txtNombresPost.Text
                producto = txtPut.Text
            };
            
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
             //   using (con)
                    {


                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spActualizaStock_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dataReader = cmd.ExecuteReader();
                    ConvertToXMLActStock(dataReader, "stock_available");
                    /*using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            txtPut.Text = s.ToString();
                            // do something with s
                        }
                    }*/
                    
                }
            }
            catch (SqlException ex)
            {
               
                MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();

            }
            string json = txtPut.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

            dynamic respuesta = dBApi.Putstock($"http://147.182.202.218/connection/saveStocksXml?api_key=02fd57f2da404a59aad7c4e79e2ea1fe", json);
           // MessageBox.Show(respuesta.ToString());
            txtPut.Clear();
        }

        public void actualizarStockPrestashop()

        {
            label9.Text = ("Actualizar stock prestashop...");

            Persona persona = new Persona
            {
                //job = txtTrabajador.Text,
                //name = txtNombresPost.Text
                producto = txtPut.Text
            };

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //   using (con)
                {


                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spActualizaStock_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dataReader = cmd.ExecuteReader();
                    ConvertToXMLActStock(dataReader, "stock_available");
                    /*using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            txtPut.Text = s.ToString();
                            // do something with s
                        }
                    }*/

                }
            }
            catch (SqlException ex)
            {
                //  Console.WriteLine("error sql");
                //  MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();
            }
            string json = txtPut.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

           // dynamic respuesta = dBApi.Putstock($"http://{dominio}/api/stock_availables?display=full&output_format=JSON", json);
            dynamic respuesta = dBApi.Putstock($"http://147.182.202.218/connection/saveStocksXml?api_key=02fd57f2da404a59aad7c4e79e2ea1fe", json);
            //   MessageBox.Show(respuesta.ToString());
            txtPut.Clear();

        }

        private void ConvertToXML(IDataReader reader,string recordName)
        {
            XElement resultNode = new XElement("prestashop");
            while (reader.Read())
            {
                XElement row = new XElement(recordName);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(new XElement(reader.GetName(i), reader.GetValue(i)));
                }
                resultNode.Add(row);
            }
            string s = resultNode.ToString();
          //  string rrr = resultNode.ToString();
            s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");
            s = s.Replace("<id1>", "<associations><categories><category><id1>").Replace("</id1><id2>", "</id></category><category><id>").Replace("</id2><id3>", "</id></category><category><id>").Replace("</id3><id4>", "</id></category><category><id>").Replace("</id4><id5>", "</id></category><category><id>").Replace("</id5><id6>", "</id></category><category><id>").Replace("</id6>", "</id6></category></categories></associations>");
            s = s.Replace("<id1>", "<id>").Replace("</id1>", "</id></category>").Replace("<id2>", "<category><id>").Replace("</id2>", "</id></category>").Replace("<id3>", "<category><id>").Replace("</id3>", "</id></category>").Replace("<id4>", "<category><id>").Replace("</id4>", "</id></category>").Replace("<id5>", "<category><id>").Replace("</id5>", "</id></category>").Replace("<id6>", "<category><id>").Replace("</id6>", "</id>");
            s = s.Replace("T00:00:00", "");
            s = s.Replace("00:00:00.000", "");
            txtXml.Text = s.ToString();

        }

        private void btnPost_Click(object sender, EventArgs e)
        {

            label9.Text = ("Exportando Productos a prestashop...");

            Persona persona = new Persona
            {
                //job = txtTrabajador.Text,
                //name = txtNombresPost.Text
                producto = txtXml.Text
            };
                      
            
            try
            {

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //using (con)

                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spProduct_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*****/
                    var dataReader = cmd.ExecuteReader();
                    ConvertToXML(dataReader, "product");
                    /*****/
                    /*    using (XmlReader reader = cmd.ExecuteXmlReader())
                        {
                            while (reader.Read())
                            {
                                string s = reader.ReadOuterXml();
                                con.Close();
                                s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");
                                s = s.Replace("<id1>","<associations><categories><category><id>").Replace("</id1><id2>", "</id></category><category><id>").Replace("</id2><id3>", "</id></category><category><id>").Replace("</id3><id4>", "</id></category><category><id>").Replace("</id4><id5>", "</id></category><category><id>").Replace("</id5><id6>", "</id></category><category><id>").Replace("</id6>", "</id></category></categories></associations>");
                                txtXml.Text = s.ToString();
                                // do something with s

                            }

                        }*/


                    //txtXml.Text = reader.ToString();

                }
            }
            catch (SqlException ex)
            {

              //  MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();

            }
            
            // string json = JsonConvert.SerializeObject(persona);
            string json = txtXml.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

            dynamic respuesta = dBApi.Post($"http://{dominio}/api/products?display=full&output_format=JSON", json);

           MessageBox.Show(respuesta.ToString());
            txtXml.Clear();

        }

        public void exportarProductosPrestashop()
        {
            label9.Text = ("Exportando Productos a prestashop...");

            Persona persona = new Persona
            {
                //job = txtTrabajador.Text,
                //name = txtNombresPost.Text
                producto = txtXml.Text
            };


            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //using (con)

                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spProduct_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    /*****/
                    var dataReader = cmd.ExecuteReader();
                    ConvertToXML(dataReader, "product");
                    /*****/
                    /*    using (XmlReader reader = cmd.ExecuteXmlReader())
                        {
                            while (reader.Read())
                            {
                                string s = reader.ReadOuterXml();
                                con.Close();
                                s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");
                                s = s.Replace("<id1>","<associations><categories><category><id>").Replace("</id1><id2>", "</id></category><category><id>").Replace("</id2><id3>", "</id></category><category><id>").Replace("</id3><id4>", "</id></category><category><id>").Replace("</id4><id5>", "</id></category><category><id>").Replace("</id5><id6>", "</id></category><category><id>").Replace("</id6>", "</id></category></categories></associations>");
                                txtXml.Text = s.ToString();
                                // do something with s

                            }

                        }*/


                    //txtXml.Text = reader.ToString();

                }
            }
            catch (SqlException ex)
            {

                //  MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();
            }

            // string json = JsonConvert.SerializeObject(persona);
            string json = txtXml.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

            dynamic respuesta = dBApi.Post($"http://{dominio}/api/products?display=full&output_format=JSON", json);

          //  MessageBox.Show(respuesta.ToString());
            txtXml.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void rxtGetVentas_Click(object sender, EventArgs e)
        {
            
            label9.Text = ("recuperando ventas cabecera...");

            try
            {
                string d1 ;
            string d2  = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            d1 = txtFecha.Text;
            d1 = d1.Replace("/", "-");

                // dynamic response = dBApi.GetGV("http://nua.tudominio.pro/api/orders?date=1&output_format=JSON&display=full");
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

                dynamic response = dBApi.GetGV($"http://21B6ANU6SMTJ7DBAG2GEN4JXHDX6G71C@{dominio}/api/orders?date=1&filter[date_add]=[{d1},{d2}]&output_format=JSON&display=full");
            dataGridView1.DataSource = response.orders;
             }
           catch(Exception ex)
            {
             MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }


        public  void getVentasCabecera()
        {
            label9.Text = ("recuperando ventas cabecera...");

            try
            {
                string d1;
                string d2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                d1 = txtFecha.Text;
                d1 = d1.Replace("/", "-");

                // dynamic response = dBApi.GetGV("http://nua.tudominio.pro/api/orders?date=1&output_format=JSON&display=full");
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

                dynamic response = dBApi.GetGV($"http://2KA34A9IR61ZQ4W5I6YUER9GU2V37Q9C@{dominio}/api/orders?date=1&filter[date_add]=[{d1},{d2}]&output_format=JSON&display=full");
                dataGridView1.DataSource = response.orders;
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            label9.Text = ("Guardando ventas cabecera...");
            string sql = @"insert into VENTAS_PRESTASHOP_TEMP 
            (id,
            id_address_delivery,
            id_address_invoice, 
            id_cart,
            id_currency,
            id_lang,
            id_customer,
            id_carrier,
            current_state,
            module,
            invoice_number,
            invoice_date,
            delivery_number,
            delivery_date,
            valid,
            date_add,
            date_upd,
            shipping_number,
            id_shop_group,
            id_shop,
            secure_key,
            payment,
            recyclable,
            gift,
            gift_message,
            mobile_theme,
            total_discounts,
            total_discounts_tax_incl,
            total_discounts_tax_excl,
            total_paid,
            total_paid_tax_incl,
            total_paid_tax_excl,
            total_paid_real,
            total_products,
            total_products_wt,
            total_shipping,
            total_shipping_tax_incl,
            total_shipping_tax_excl,
            carrier_tax_rate,
            total_wrapping,
            total_wrapping_tax_incl,
            total_wrapping_tax_excl,
            round_mode,
            round_type,
            conversion_rate,
            reference,
            associations) 
            
            values (@id,
            @id_address_delivery,
            @id_address_invoice,
            @id_cart,
            @id_currency,
            @id_lang,
            @id_customer,
            @id_carrier,
            @current_state,
            @module,
            @invoice_number,
            @invoice_date,
            @delivery_number,
            @delivery_date,
            @valid,
            @date_add,
            @date_upd,
            @shipping_number,
            @id_shop_group,
            @id_shop,
            @secure_key,
            @payment,
            @recyclable,
            @gift,
            @gift_message,
            @mobile_theme,
            @total_discounts,
            @total_discounts_tax_incl,
            @total_discounts_tax_excl,
            @total_paid,
            @total_paid_tax_incl,
            @total_paid_tax_excl,
            @total_paid_real,
            @total_products,
            @total_products_wt,
            @total_shipping,
            @total_shipping_tax_incl,
            @total_shipping_tax_excl,
            @carrier_tax_rate,
            @total_wrapping,
            @total_wrapping_tax_incl,
            @total_wrapping_tax_excl,
            @round_mode,
            @round_type,
            @conversion_rate,
            @reference,
            @associations
            ) ";
            SqlCommand spg = new SqlCommand("dbo.spINSERTAR_VENTAS_PRESTASHOP",con);
            spg.CommandType = CommandType.StoredProcedure;
            SqlCommand guardar = new SqlCommand(sql,con);
            con.Open();

            try
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["Column1"].Value));
                    guardar.Parameters.AddWithValue("@id_address_delivery", Convert.ToString(row.Cells["Column2"].Value));
                    guardar.Parameters.AddWithValue("@id_address_invoice", Convert.ToString(row.Cells["Column3"].Value));
                    //guardar.Parameters.AddWithValue("@id_address_invoice", Convert.ToString(row.Cells["Column3"].Value));
                    guardar.Parameters.AddWithValue("@id_cart", Convert.ToString(row.Cells["Column4"].Value));
                    guardar.Parameters.AddWithValue("@id_currency", Convert.ToString(row.Cells["Column5"].Value));
                    guardar.Parameters.AddWithValue("@id_lang", Convert.ToString(row.Cells["Column6"].Value));
                    guardar.Parameters.AddWithValue("@id_customer", Convert.ToString(row.Cells["Column7"].Value));
                    guardar.Parameters.AddWithValue("@id_carrier", Convert.ToString(row.Cells["Column8"].Value));
                    guardar.Parameters.AddWithValue("@current_state", Convert.ToString(row.Cells["Column9"].Value));
                    guardar.Parameters.AddWithValue("@module", Convert.ToString(row.Cells["Column10"].Value));
                    guardar.Parameters.AddWithValue("@invoice_number", Convert.ToString(row.Cells["Column11"].Value));
                    guardar.Parameters.AddWithValue("@invoice_date", Convert.ToString(row.Cells["Column12"].Value));
                    guardar.Parameters.AddWithValue("@delivery_number", Convert.ToString(row.Cells["Column13"].Value));
                    guardar.Parameters.AddWithValue("@delivery_date", Convert.ToString(row.Cells["Column14"].Value));
                    guardar.Parameters.AddWithValue("@valid", Convert.ToString(row.Cells["Column15"].Value));
                    guardar.Parameters.AddWithValue("@date_add", Convert.ToString(row.Cells["Column16"].Value));
                    guardar.Parameters.AddWithValue("@date_upd", Convert.ToString(row.Cells["Column17"].Value));
                    guardar.Parameters.AddWithValue("@shipping_number", Convert.ToString(row.Cells["Column18"].Value));
                    guardar.Parameters.AddWithValue("@id_shop_group", Convert.ToString(row.Cells["Column19"].Value));
                    guardar.Parameters.AddWithValue("@id_shop", Convert.ToString(row.Cells["Column20"].Value));
                    guardar.Parameters.AddWithValue("@secure_key", Convert.ToString(row.Cells["Column21"].Value));
                    guardar.Parameters.AddWithValue("@payment", Convert.ToString(row.Cells["Column22"].Value));
                    guardar.Parameters.AddWithValue("@recyclable", Convert.ToString(row.Cells["Column23"].Value));
                    guardar.Parameters.AddWithValue("@gift", Convert.ToString(row.Cells["Column24"].Value));
                    guardar.Parameters.AddWithValue("@gift_message", Convert.ToString(row.Cells["Column25"].Value));

                    guardar.Parameters.AddWithValue("@mobile_theme", Convert.ToString(row.Cells["Column26"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts", Convert.ToDecimal(row.Cells["Column27"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts_tax_incl", Convert.ToDecimal(row.Cells["Column28"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts_tax_excl", Convert.ToDecimal(row.Cells["Column29"].Value));
                    guardar.Parameters.AddWithValue("@total_paid", Convert.ToDecimal(row.Cells["Column30"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_tax_incl", Convert.ToDecimal(row.Cells["Column31"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_tax_excl", Convert.ToDecimal(row.Cells["Column32"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_real", Convert.ToDecimal(row.Cells["Column33"].Value));
                    guardar.Parameters.AddWithValue("@total_products", Convert.ToDecimal(row.Cells["Column34"].Value));
                    guardar.Parameters.AddWithValue("@total_products_wt", Convert.ToDecimal(row.Cells["Column35"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping", Convert.ToDecimal(row.Cells["Column36"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping_tax_incl", Convert.ToDecimal(row.Cells["Column37"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping_tax_excl", Convert.ToDecimal(row.Cells["Column38"].Value));
                    guardar.Parameters.AddWithValue("@carrier_tax_rate", Convert.ToDecimal(row.Cells["Column39"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping", Convert.ToDecimal(row.Cells["Column40"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping_tax_incl", Convert.ToDecimal(row.Cells["Column41"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping_tax_excl", Convert.ToDecimal(row.Cells["Column42"].Value));
                    guardar.Parameters.AddWithValue("@round_mode", Convert.ToString(row.Cells["Column43"].Value));
                    guardar.Parameters.AddWithValue("@round_type", Convert.ToString(row.Cells["Column44"].Value));
                    guardar.Parameters.AddWithValue("@conversion_rate", Convert.ToString(row.Cells["Column45"].Value));
                    guardar.Parameters.AddWithValue("@reference", Convert.ToString(row.Cells["Column46"].Value));
                    guardar.Parameters.AddWithValue("@associations", Convert.ToString(row.Cells["Column47"].Value));



                    guardar.ExecuteNonQuery();
                }
                spg.ExecuteNonQuery();
                MessageBox.Show("Datos guardados");
                //    dataGridView1.DataSource = null;
                //  dataGridView1.Refresh();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                
              //MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            finally { con.Close(); }

        }

        public void guardarVentaCabecera()
        {
            label9.Text = ("Guardando ventas cabecera...");

            string sql = @"insert into VENTAS_PRESTASHOP_TEMP 
            (id,
            id_address_delivery,
            id_address_invoice, 
            id_cart,
            id_currency,
            id_lang,
            id_customer,
            id_carrier,
            current_state,
            module,
            invoice_number,
            invoice_date,
            delivery_number,
            delivery_date,
            valid,
            date_add,
            date_upd,
            shipping_number,
            id_shop_group,
            id_shop,
            secure_key,
            payment,
            recyclable,
            gift,
            gift_message,
            mobile_theme,
            total_discounts,
            total_discounts_tax_incl,
            total_discounts_tax_excl,
            total_paid,
            total_paid_tax_incl,
            total_paid_tax_excl,
            total_paid_real,
            total_products,
            total_products_wt,
            total_shipping,
            total_shipping_tax_incl,
            total_shipping_tax_excl,
            carrier_tax_rate,
            total_wrapping,
            total_wrapping_tax_incl,
            total_wrapping_tax_excl,
            round_mode,
            round_type,
            conversion_rate,
            reference,
            associations) 
            
            values (@id,
            @id_address_delivery,
            @id_address_invoice,
            @id_cart,
            @id_currency,
            @id_lang,
            @id_customer,
            @id_carrier,
            @current_state,
            @module,
            @invoice_number,
            @invoice_date,
            @delivery_number,
            @delivery_date,
            @valid,
            @date_add,
            @date_upd,
            @shipping_number,
            @id_shop_group,
            @id_shop,
            @secure_key,
            @payment,
            @recyclable,
            @gift,
            @gift_message,
            @mobile_theme,
            @total_discounts,
            @total_discounts_tax_incl,
            @total_discounts_tax_excl,
            @total_paid,
            @total_paid_tax_incl,
            @total_paid_tax_excl,
            @total_paid_real,
            @total_products,
            @total_products_wt,
            @total_shipping,
            @total_shipping_tax_incl,
            @total_shipping_tax_excl,
            @carrier_tax_rate,
            @total_wrapping,
            @total_wrapping_tax_incl,
            @total_wrapping_tax_excl,
            @round_mode,
            @round_type,
            @conversion_rate,
            @reference,
            @associations
            ) ";
            SqlCommand spg = new SqlCommand("dbo.spINSERTAR_VENTAS_PRESTASHOP", con);
            spg.CommandType = CommandType.StoredProcedure;
            SqlCommand guardar = new SqlCommand(sql, con);
            con.Open();

            try
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["Column1"].Value));
                    guardar.Parameters.AddWithValue("@id_address_delivery", Convert.ToString(row.Cells["Column2"].Value));
                    guardar.Parameters.AddWithValue("@id_address_invoice", Convert.ToString(row.Cells["Column3"].Value));
                    //guardar.Parameters.AddWithValue("@id_address_invoice", Convert.ToString(row.Cells["Column3"].Value));
                    guardar.Parameters.AddWithValue("@id_cart", Convert.ToString(row.Cells["Column4"].Value));
                    guardar.Parameters.AddWithValue("@id_currency", Convert.ToString(row.Cells["Column5"].Value));
                    guardar.Parameters.AddWithValue("@id_lang", Convert.ToString(row.Cells["Column6"].Value));
                    guardar.Parameters.AddWithValue("@id_customer", Convert.ToString(row.Cells["Column7"].Value));
                    guardar.Parameters.AddWithValue("@id_carrier", Convert.ToString(row.Cells["Column8"].Value));
                    guardar.Parameters.AddWithValue("@current_state", Convert.ToString(row.Cells["Column9"].Value));
                    guardar.Parameters.AddWithValue("@module", Convert.ToString(row.Cells["Column10"].Value));
                    guardar.Parameters.AddWithValue("@invoice_number", Convert.ToString(row.Cells["Column11"].Value));
                    guardar.Parameters.AddWithValue("@invoice_date", Convert.ToString(row.Cells["Column12"].Value));
                    guardar.Parameters.AddWithValue("@delivery_number", Convert.ToString(row.Cells["Column13"].Value));
                    guardar.Parameters.AddWithValue("@delivery_date", Convert.ToString(row.Cells["Column14"].Value));
                    guardar.Parameters.AddWithValue("@valid", Convert.ToString(row.Cells["Column15"].Value));
                    guardar.Parameters.AddWithValue("@date_add", Convert.ToString(row.Cells["Column16"].Value));
                    guardar.Parameters.AddWithValue("@date_upd", Convert.ToString(row.Cells["Column17"].Value));
                    guardar.Parameters.AddWithValue("@shipping_number", Convert.ToString(row.Cells["Column18"].Value));
                    guardar.Parameters.AddWithValue("@id_shop_group", Convert.ToString(row.Cells["Column19"].Value));
                    guardar.Parameters.AddWithValue("@id_shop", Convert.ToString(row.Cells["Column20"].Value));
                    guardar.Parameters.AddWithValue("@secure_key", Convert.ToString(row.Cells["Column21"].Value));
                    guardar.Parameters.AddWithValue("@payment", Convert.ToString(row.Cells["Column22"].Value));
                    guardar.Parameters.AddWithValue("@recyclable", Convert.ToString(row.Cells["Column23"].Value));
                    guardar.Parameters.AddWithValue("@gift", Convert.ToString(row.Cells["Column24"].Value));
                    guardar.Parameters.AddWithValue("@gift_message", Convert.ToString(row.Cells["Column25"].Value));

                    guardar.Parameters.AddWithValue("@mobile_theme", Convert.ToString(row.Cells["Column26"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts", Convert.ToDecimal(row.Cells["Column27"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts_tax_incl", Convert.ToDecimal(row.Cells["Column28"].Value));
                    guardar.Parameters.AddWithValue("@total_discounts_tax_excl", Convert.ToDecimal(row.Cells["Column29"].Value));
                    guardar.Parameters.AddWithValue("@total_paid", Convert.ToDecimal(row.Cells["Column30"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_tax_incl", Convert.ToDecimal(row.Cells["Column31"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_tax_excl", Convert.ToDecimal(row.Cells["Column32"].Value));
                    guardar.Parameters.AddWithValue("@total_paid_real", Convert.ToDecimal(row.Cells["Column33"].Value));
                    guardar.Parameters.AddWithValue("@total_products", Convert.ToDecimal(row.Cells["Column34"].Value));
                    guardar.Parameters.AddWithValue("@total_products_wt", Convert.ToDecimal(row.Cells["Column35"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping", Convert.ToDecimal(row.Cells["Column36"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping_tax_incl", Convert.ToDecimal(row.Cells["Column37"].Value));
                    guardar.Parameters.AddWithValue("@total_shipping_tax_excl", Convert.ToDecimal(row.Cells["Column38"].Value));
                    guardar.Parameters.AddWithValue("@carrier_tax_rate", Convert.ToDecimal(row.Cells["Column39"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping", Convert.ToDecimal(row.Cells["Column40"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping_tax_incl", Convert.ToDecimal(row.Cells["Column41"].Value));
                    guardar.Parameters.AddWithValue("@total_wrapping_tax_excl", Convert.ToDecimal(row.Cells["Column42"].Value));
                    guardar.Parameters.AddWithValue("@round_mode", Convert.ToString(row.Cells["Column43"].Value));
                    guardar.Parameters.AddWithValue("@round_type", Convert.ToString(row.Cells["Column44"].Value));
                    guardar.Parameters.AddWithValue("@conversion_rate", Convert.ToString(row.Cells["Column45"].Value));
                    guardar.Parameters.AddWithValue("@reference", Convert.ToString(row.Cells["Column46"].Value));
                    guardar.Parameters.AddWithValue("@associations", Convert.ToString(row.Cells["Column47"].Value));



                    guardar.ExecuteNonQuery();
                }
                spg.ExecuteNonQuery();
                //MessageBox.Show("Datos guardados");
                
                //    dataGridView1.DataSource = null;
                //  dataGridView1.Refresh();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();

            }
            finally { con.Close(); }
        }

        public void funcionPincipal()
        {


            

            /* ventas*/
            //getVentasCabecera();
           // guardarVentaCabecera();
           // getVentaDetalle();
           // guardarDetalleVenta();

            /*productos*/
           //exportarProductosPrestashop();
           // importarProductos();
           // guardarProductos();

          //  importarStock();
         //   guardarStock();

            actualizarStockPrestashop();

            actualizarPrecioPrestashop();

            /* archivo*/
            //cargarInfo();
            //generarArchivo();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
           
            try { 
            string d1;
//            lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            lblFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaDesde.Text=DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");

                //  fechh
                con.Open();
            //MessageBox.Show("Conectado correctamente");

            SqlCommand cmd = new SqlCommand("dbo.spFechaMax_Prestashop", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader leer = cmd.ExecuteReader();
            if (leer.Read())
            {
                string date = leer["fecha"].ToString();
                    DateTime dat;
                    dat = DateTime.Parse(date);
                   // txtFecha.Text = leer["fecha"].ToString();
                    txtFecha.Text = dat.ToString("yyyy-MM-dd");

                }

            
                con.Close();


            dataGridView1.AllowUserToAddRows = false;
            dgvStock.AllowUserToAddRows = false;
            dgvStockProductos.AllowUserToAddRows = false;
            dgvVentaDetalle.AllowUserToAddRows = false;

             funcionPincipal();
            //    timer2.Start();
               this.Close();
            }

            catch (Exception ex)
            {
                //MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

     
        private void btnTraerStock_Click(object sender, EventArgs e)
        {
            label9.Text = ("Importando stock...");

            try
            {
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

                dynamic response = dBApi.GetGVStock($"http://@{dominio}/api/stock_availables?display=full&output_format=JSON");
                dgvStock.DataSource = response.stock_availables;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }

        }

        public void importarStock()
        {
            label9.Text = ("Importando stock...");

            try
            {
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();
                dynamic response = dBApi.GetGVStock($"http://@{dominio}/api/stock_availables?display=full&output_format=JSON");
                dgvStock.DataSource = response.stock_availables;
            }
            catch(Exception ex)
            {
                //  MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }


        private void btnGuardarStock_Click(object sender, EventArgs e)
        {
            label9.Text = ("Guardando stock...");

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sqldel = " delete from STOCK_PRESTASHOP";
            string sql = @"insert into STOCK_PRESTASHOP 
            (id,
            id_product,
            id_product_attribute, 
            id_shop,
            id_shop_group,
            quantity,
            depends_on_stock,
            out_of_stock) 
            
            values (@id,
            @id_product,
            @id_product_attribute,
            @id_shop,
            @id_shop_group,
            @quantity,
            @depends_on_stock,
            @out_of_stock )";
            SqlCommand setTabla = new SqlCommand(sqldel, con);

            SqlCommand guardar = new SqlCommand(sql, con);
            con.Open();

            try
            {
                setTabla.ExecuteNonQuery();

                foreach (DataGridViewRow row in dgvStock.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["id"].Value));
                    guardar.Parameters.AddWithValue("@id_product", Convert.ToString(row.Cells["id_product"].Value));
                    guardar.Parameters.AddWithValue("@id_product_attribute", Convert.ToString(row.Cells["id_product_attribute"].Value));
                    guardar.Parameters.AddWithValue("@id_shop", Convert.ToString(row.Cells["id_shop"].Value));
                    guardar.Parameters.AddWithValue("@id_shop_group", Convert.ToString(row.Cells["id_shop_group"].Value));
                    guardar.Parameters.AddWithValue("@quantity", Convert.ToString(row.Cells["quantity"].Value));
                    guardar.Parameters.AddWithValue("@depends_on_stock", Convert.ToString(row.Cells["depends_on_stock"].Value));
                    guardar.Parameters.AddWithValue("@out_of_stock", Convert.ToString(row.Cells["out_of_stock"].Value));

                    guardar.ExecuteNonQuery();
                }
                MessageBox.Show("Datos guardados");
                dgvStock.Rows.Clear();
                dgvStock.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();
            }
            finally { con.Close(); }
        }

        public void guardarStock()
        {
            label9.Text = ("Guardando stock...");

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sqldel = " delete from STOCK_PRESTASHOP";
            string sql = @"insert into STOCK_PRESTASHOP 
            (id,
            id_product,
            id_product_attribute, 
            id_shop,
            id_shop_group,
            quantity,
            depends_on_stock,
            out_of_stock) 
            
            values (@id,
            @id_product,
            @id_product_attribute,
            @id_shop,
            @id_shop_group,
            @quantity,
            @depends_on_stock,
            @out_of_stock )";
            SqlCommand setTabla = new SqlCommand(sqldel, con);

            SqlCommand guardar = new SqlCommand(sql, con);
            con.Open();

            try
            {
                setTabla.ExecuteNonQuery();

                foreach (DataGridViewRow row in dgvStock.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["id"].Value));
                    guardar.Parameters.AddWithValue("@id_product", Convert.ToString(row.Cells["id_product"].Value));
                    guardar.Parameters.AddWithValue("@id_product_attribute", Convert.ToString(row.Cells["id_product_attribute"].Value));
                    guardar.Parameters.AddWithValue("@id_shop", Convert.ToString(row.Cells["id_shop"].Value));
                    guardar.Parameters.AddWithValue("@id_shop_group", Convert.ToString(row.Cells["id_shop_group"].Value));
                    guardar.Parameters.AddWithValue("@quantity", Convert.ToString(row.Cells["quantity"].Value));
                    guardar.Parameters.AddWithValue("@depends_on_stock", Convert.ToString(row.Cells["depends_on_stock"].Value));
                    guardar.Parameters.AddWithValue("@out_of_stock", Convert.ToString(row.Cells["out_of_stock"].Value));

                    guardar.ExecuteNonQuery();
                }
               // MessageBox.Show("Datos guardados");
                dgvStock.Rows.Clear();
                dgvStock.Refresh();
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();
            }
            finally { con.Close(); }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dgvStock.DataSource = null;
            dgvStock.Refresh();
        }

        private void btnTraeProdu_Click(object sender, EventArgs e)
        {
            label9.Text = ("Importando Productos...");

            try
            {
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();
                dynamic response = dBApi.GetGVStockProducts($"http://{dominio}/api/products?display=full&output_format=JSON");
                dgvStockProductos.DataSource = response.products;
            }
            catch(Exception ex)
            {
                MessageBox.Show("error:"+ex);
                string err = ex.ToString();
            }
        }

        public void importarProductos()
        {
            label9.Text = ("Importando Productos...");

            try
            {
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();
                dynamic response = dBApi.GetGVStockProducts($"http://{dominio}/api/products?display=full&output_format=JSON");
                dgvStockProductos.DataSource = response.products;
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }


        private void btnGuardarProdu_Click(object sender, EventArgs e)
        {
            label9.Text = ("Guardando productos...");

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sqldel = " delete from PRODUCTO_STOCK_PRESTASHOP";
            string sql = @"insert into PRODUCTO_STOCK_PRESTASHOP  
            (id,
            name,
            reference,
            id_default_combination)
            
            values (@id,
            @name,
            @reference,
            @id_default_combination)";

          
            SqlCommand setTabla = new SqlCommand(sqldel, con);

            SqlCommand guardar = new SqlCommand(sql, con);
            con.Open();

            try
            {
                setTabla.ExecuteNonQuery();

                foreach (DataGridViewRow row in dgvStockProductos.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["idprodu"].Value));
                    guardar.Parameters.AddWithValue("@name", Convert.ToString(row.Cells["nameprodu"].Value));
                    guardar.Parameters.AddWithValue("@reference", Convert.ToString(row.Cells["referenceprodu"].Value));
                    guardar.Parameters.AddWithValue("@id_default_combination", Convert.ToString(row.Cells["id_default_combinationprodu"].Value));



                    guardar.ExecuteNonQuery();
                }
                MessageBox.Show("Datos guardados");
                dgvStockProductos.Rows.Clear();
                dgvStockProductos.Refresh();
                enviarFotos();
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                MessageBox.Show("Error: Datos no guardados");
            }
            finally { con.Close(); }
        }/*************/


        public void guardarProductos()
        {
            label9.Text = ("Guardando productos...");

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sqldel = " delete from PRODUCTO_STOCK_PRESTASHOP";
            string sql = @"insert into PRODUCTO_STOCK_PRESTASHOP  
            (id,
            name,
            reference,
            id_default_combination)
            
            values (@id,
            @name,
            @reference,
            @id_default_combination)";


            SqlCommand setTabla = new SqlCommand(sqldel, con);

            SqlCommand guardar = new SqlCommand(sql, con);
            con.Open();

            try
            {
                setTabla.ExecuteNonQuery();

                foreach (DataGridViewRow row in dgvStockProductos.Rows)
                {
                    guardar.Parameters.Clear();
                    guardar.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["idprodu"].Value));
                    guardar.Parameters.AddWithValue("@name", Convert.ToString(row.Cells["nameprodu"].Value));
                    guardar.Parameters.AddWithValue("@reference", Convert.ToString(row.Cells["referenceprodu"].Value));
                    guardar.Parameters.AddWithValue("@id_default_combination", Convert.ToString(row.Cells["id_default_combinationprodu"].Value));


                    guardar.ExecuteNonQuery();
                }
               // MessageBox.Show("Datos guardados");
                dgvStockProductos.Rows.Clear();
                dgvStockProductos.Refresh();
               // enviarFotos();
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
               // MessageBox.Show("Error: Datos no guardados");
            }
            finally { con.Close(); }
        }

        public static void enviarFotos()
            {
            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))

                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spFotosPrestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dt = new DataTable();
                    var da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 120;
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                       string id = row["id"].ToString();
                        string url = row["url_web"].ToString();
                        string reference = row["reference"].ToString();

                        string url_image = url;
                        string idproducto_prestashop = id;
                        string filename = "demo_final";
                        string fileformat = "jpg";
                        DownloadFile_Prestashop_image(url_image, filename, fileformat);
                        Upload_Prestashop_image(idproducto_prestashop, filename, fileformat);

                        string update = "update item set fecha_creacion_prestashop=(select getdate())   where id_item = '" + reference + "' ";
                        SqlCommand cmd2 = new SqlCommand(update, con);
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                string error = ex.ToString();
                Console.WriteLine(ex);
            }

        }
        #region gestion imagenes
        public static void DownloadFile_Prestashop_image(string origen, string filename, string fileformat)
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


        public static void Upload_Prestashop_image(string idproducto, string filename, string fileformat)
        {
            try
            {
                string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();
                string postURL = $"http://{dominio}/api/images/products/" + idproducto;
                string userAgent = "Test";
                string Key = System.Configuration.ConfigurationSettings.AppSettings["key"].ToString();
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






        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void ConvertToXMLActPrecios(IDataReader reader, string recordName)
        {
            XElement resultNode = new XElement("prestashop");
            while (reader.Read())
            {
                XElement row = new XElement(recordName);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(new XElement(reader.GetName(i), reader.GetValue(i)));
                }
                resultNode.Add(row);
            }
            string s = resultNode.ToString();
          //  string rrr = resultNode.ToString();

            s = s.Replace("T00:00:00", "");
            s = s.Replace("00:00:00.000", "");
            s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");

            txtActualizaPrecios.Text = s.ToString();
             

        }

        private void btnActualizaPrecio_Click(object sender, EventArgs e)
        {
            label9.Text = ("Actualizar precios prestashop...");

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //using (con)
                {


                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spActualizaPrecios_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dataReader = cmd.ExecuteReader();
                    ConvertToXMLActPrecios(dataReader,"product");

                    /*using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");

                            txtActualizaPrecios.Text = s.ToString();
                            // do something with s
                        }
                    }*/

                }
            }
            catch (SqlException ex)
            {
                //  Console.WriteLine("error sql");
                MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();
            }

            string json = txtActualizaPrecios.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

            //dynamic respuesta = dBApi.PutPrecio($"http://{dominio}/api/products?display=full&output_format=JSON", json);
            dynamic respuesta = dBApi.PutPrecio($"http://147.182.202.218/connection/savePricesXml?api_key=02fd57f2da404a59aad7c4e79e2ea1fe", json);
           // MessageBox.Show(respuesta.ToString());
            txtActualizaPrecios.Clear();

        }

        public void actualizarPrecioPrestashop()
        {
            label9.Text = ("Actualizar precios prestashop...");

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //using (con)
                {


                    con.Open();
                    SqlCommand cmd = new SqlCommand("dbo.spActualizaPrecios_Prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var dataReader = cmd.ExecuteReader();
                    ConvertToXMLActPrecios(dataReader, "product");

                    /*using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            s = s.Replace("<link_rewrite>", "<link_rewrite><language id = '1'>").Replace("</link_rewrite>", "</language></link_rewrite >").Replace("<name>", "<name><language id='1'>").Replace("</name>", "</language></name>");

                            txtActualizaPrecios.Text = s.ToString();
                            // do something with s
                        }
                    }*/

                }
            }
            catch (SqlException ex)
            {
                //  Console.WriteLine("error sql");
                //  MessageBox.Show("Error: No se pudo conectar a la base de datos InventarioSQL");
                string err = ex.ToString();
            }
            string json = txtActualizaPrecios.Text;
            string dominio = System.Configuration.ConfigurationSettings.AppSettings["dominio"].ToString();

           // dynamic respuesta = dBApi.PutPrecio($"http://{dominio}/api/products?display=full&output_format=JSON", json);
            dynamic respuesta = dBApi.PutPrecio($"http://147.182.202.218/connection/savePricesXml?api_key=02fd57f2da404a59aad7c4e79e2ea1fe", json);

            //  MessageBox.Show(respuesta.ToString());
            txtActualizaPrecios.Clear();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void ConvertToXMLTreVentaDatalle(IDataReader reader, string recordName)
        {
            XElement resultNode = new XElement("prestashop");
            while (reader.Read())
            {
                XElement row = new XElement(recordName);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(new XElement(reader.GetName(i), reader.GetValue(i)));
                }
                resultNode.Add(row);
            }
            string s = resultNode.ToString();
            s = s.Replace("<prestashop>\r\n  <orders>\r\n    <VD>", "").Replace("]\r\n}</VD>\r\n  </orders>\r\n  <orders>\r\n    <VD>{\r\n  \"order_rows\":[", ",").Replace("</VD>\r\n  </orders>\r\n</prestashop>", "");
            
                 txtVentaDetalle.Text = s.ToString();
            //            s = s.Replace("<prestashop><orders><VD>", "").Replace("</VD></orders><orders><VD>", "").Replace("</VD></orders></prestashop>", "").Replace("]\r\n}{", "101").Replace("\"order_rows\":[", "102").Replace("101\r\n  102", ",").Replace("102", "\"order_rows\":["); //."    .Replace("m1\r\nm2", "xxx");

            dynamic data = JsonConvert.DeserializeObject(s);
            txtVentaDetalle.Text = s.ToString();

            dgvVentaDetalle.DataSource = data.order_rows;

            
        }

        private void btnTreVentaDetalle_Click(object sender, EventArgs e)
        {
            label9.Text = ("Recuperando ventas detalle...");

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
              //  using (con)

                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("dbo.spVenta_detalle_prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                   var datareader = cmd.ExecuteReader();
                    ConvertToXMLTreVentaDatalle(datareader, "orders");
                   
                    
                    /*
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            s = s.Replace("<prestashop><orders><VD>", "").Replace("</VD></orders><orders><VD>", "").Replace("</VD></orders></prestashop>", "").Replace("]\r\n}{", "101").Replace("\"order_rows\":[", "102").Replace("101\r\n  102", ",").Replace("102", "\"order_rows\":["); //."    .Replace("m1\r\nm2", "xxx");

                            dynamic data = JsonConvert.DeserializeObject(s);
                            
                            txtVentaDetalle.Text = s.ToString();

                            dgvVentaDetalle.DataSource = data.order_rows;

                            // do something with s
                        }

                    }*/
                    
                    //txtXml.Text = reader.ToString();

                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();

            }
        }

        public void getVentaDetalle()
        {
            label9.Text = ("Recuperando ventas detalle...");

            try
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
                //  using (con)

                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("dbo.spVenta_detalle_prestashop", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var datareader = cmd.ExecuteReader();
                    ConvertToXMLTreVentaDatalle(datareader, "orders");


                    /*
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            string s = reader.ReadOuterXml();
                            con.Close();
                            s = s.Replace("<prestashop><orders><VD>", "").Replace("</VD></orders><orders><VD>", "").Replace("</VD></orders></prestashop>", "").Replace("]\r\n}{", "101").Replace("\"order_rows\":[", "102").Replace("101\r\n  102", ",").Replace("102", "\"order_rows\":["); //."    .Replace("m1\r\nm2", "xxx");

                            dynamic data = JsonConvert.DeserializeObject(s);
                            
                            txtVentaDetalle.Text = s.ToString();

                            dgvVentaDetalle.DataSource = data.order_rows;

                            // do something with s
                        }

                    }*/

                    //txtXml.Text = reader.ToString();

                }
            }
            catch (SqlException ex)
            {

              //  MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();

            }
        }

        private void btnGuardarVD_Click(object sender, EventArgs e)
        {
            label9.Text = ("Guardando ventas detalle...");


            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sql = @"insert into VENTA_DETALLE_PRESTASHOP_TEMP 
            (id_vc,
            id,
            product_id, 
            product_attribute_id,
            product_quantity,
            product_name,
            product_reference,
            product_ean13,
            product_upc,
            product_price,
            unit_price_tax_incl,
            unit_price_tax_excl) 
            
            values (@id_vc,
            @id,
            @product_id,
            @product_attribute_id,
            @product_quantity,
            @product_name,
            @product_reference,
            @product_ean13,
            @product_upc,
            @product_price,
            @unit_price_tax_incl,
            @unit_price_tax_excl)";
           

            SqlCommand spg = new SqlCommand("dbo.spINSERTAR_VENTA_DETALLE_PRESTASHOP", con);
            spg.CommandType = CommandType.StoredProcedure;

            //SqlCommand spifmovimi = new SqlCommand("dbo.spInsertar_Ventas_ifmovimi_prestashop", con);
            //spifmovimi.CommandType = CommandType.StoredProcedure;

            // actualiza sa
           // SqlCommand spactsa = new SqlCommand("dbo.spActualiza_SAP", con);
            //spactsa.CommandType = CommandType.StoredProcedure;

            SqlCommand spactestado= new SqlCommand("dbo.spActualiza_Estado_VDP", con);
            spactestado.CommandType = CommandType.StoredProcedure;

            //-----------

            SqlCommand guardarvd = new SqlCommand(sql, con);
            con.Open();

            try
            {
                foreach (DataGridViewRow row in dgvVentaDetalle.Rows)
                {
                    guardarvd.Parameters.Clear();
                    guardarvd.Parameters.AddWithValue("@id_vc", Convert.ToString(row.Cells["id_vc"].Value));
                    guardarvd.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["idvc"].Value));
                    guardarvd.Parameters.AddWithValue("@product_id", Convert.ToString(row.Cells["product_id"].Value));
                    guardarvd.Parameters.AddWithValue("@product_attribute_id", Convert.ToString(row.Cells["product_attribute_id"].Value));
                    guardarvd.Parameters.AddWithValue("@product_quantity", Convert.ToString(row.Cells["product_quantity"].Value));
                    guardarvd.Parameters.AddWithValue("@product_name", Convert.ToString(row.Cells["product_name"].Value));
                    guardarvd.Parameters.AddWithValue("@product_reference", Convert.ToString(row.Cells["product_reference"].Value));
                    guardarvd.Parameters.AddWithValue("@product_ean13", Convert.ToString(row.Cells["product_ean13"].Value));
                    guardarvd.Parameters.AddWithValue("@product_upc", Convert.ToString(row.Cells["product_upc"].Value));
                    guardarvd.Parameters.AddWithValue("@product_price", Convert.ToDecimal(row.Cells["product_price"].Value));
                    guardarvd.Parameters.AddWithValue("@unit_price_tax_incl", Convert.ToDecimal(row.Cells["unit_price_tax_incl"].Value));
                    guardarvd.Parameters.AddWithValue("@unit_price_tax_excl", Convert.ToDecimal(row.Cells["unit_price_tax_excl"].Value));

               
                    guardarvd.ExecuteNonQuery();
                }
                spg.ExecuteNonQuery();
               // spifmovimi.ExecuteNonQuery();

                //spactsa.ExecuteNonQuery();
                spactestado.ExecuteNonQuery();

                MessageBox.Show("Datos guardados");
                dgvVentaDetalle.Rows.Clear();
                dgvVentaDetalle.Refresh();
                txtVentaDetalle.Clear();
                

            }
            catch (Exception ex)
            {

               MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();
                dgvVentaDetalle.Rows.Clear();
                dgvVentaDetalle.Refresh();
                txtVentaDetalle.Clear();
            }
            finally { con.Close(); }
        }
        public void guardarDetalleVenta()
        {
            label9.Text = ("Guardando ventas detalle...");

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString());

            string sql = @"insert into VENTA_DETALLE_PRESTASHOP_TEMP 
            (id_vc,
            id,
            product_id, 
            product_attribute_id,
            product_quantity,
            product_name,
            product_reference,
            product_ean13,
            product_upc,
            product_price,
            unit_price_tax_incl,
            unit_price_tax_excl) 
            
            values (@id_vc,
            @id,
            @product_id,
            @product_attribute_id,
            @product_quantity,
            @product_name,
            @product_reference,
            @product_ean13,
            @product_upc,
            @product_price,
            @unit_price_tax_incl,
            @unit_price_tax_excl)";


            SqlCommand spg = new SqlCommand("dbo.spINSERTAR_VENTA_DETALLE_PRESTASHOP", con);
            spg.CommandType = CommandType.StoredProcedure;

            //SqlCommand spifmovimi = new SqlCommand("dbo.spInsertar_Ventas_ifmovimi_prestashop", con);
            //spifmovimi.CommandType = CommandType.StoredProcedure;

            // actualiza sa
            //SqlCommand spactsa = new SqlCommand("dbo.spActualiza_SAP", con);
            //spactsa.CommandType = CommandType.StoredProcedure;

            SqlCommand spactestado = new SqlCommand("dbo.spActualiza_Estado_VDP", con);
            spactestado.CommandType = CommandType.StoredProcedure;

            //-----------

            SqlCommand guardarvd = new SqlCommand(sql, con);
            con.Open();

            try
            {
                foreach (DataGridViewRow row in dgvVentaDetalle.Rows)
                {
                    guardarvd.Parameters.Clear();
                    guardarvd.Parameters.AddWithValue("@id_vc", Convert.ToString(row.Cells["id_vc"].Value));
                    guardarvd.Parameters.AddWithValue("@id", Convert.ToString(row.Cells["idvc"].Value));
                    guardarvd.Parameters.AddWithValue("@product_id", Convert.ToString(row.Cells["product_id"].Value));
                    guardarvd.Parameters.AddWithValue("@product_attribute_id", Convert.ToString(row.Cells["product_attribute_id"].Value));
                    guardarvd.Parameters.AddWithValue("@product_quantity", Convert.ToString(row.Cells["product_quantity"].Value));
                    guardarvd.Parameters.AddWithValue("@product_name", Convert.ToString(row.Cells["product_name"].Value));
                    guardarvd.Parameters.AddWithValue("@product_reference", Convert.ToString(row.Cells["product_reference"].Value));
                    guardarvd.Parameters.AddWithValue("@product_ean13", Convert.ToString(row.Cells["product_ean13"].Value));
                    guardarvd.Parameters.AddWithValue("@product_upc", Convert.ToString(row.Cells["product_upc"].Value));
                    guardarvd.Parameters.AddWithValue("@product_price", Convert.ToDecimal(row.Cells["product_price"].Value));
                    guardarvd.Parameters.AddWithValue("@unit_price_tax_incl", Convert.ToDecimal(row.Cells["unit_price_tax_incl"].Value));
                    guardarvd.Parameters.AddWithValue("@unit_price_tax_excl", Convert.ToDecimal(row.Cells["unit_price_tax_excl"].Value));


                    guardarvd.ExecuteNonQuery();
                }
                spg.ExecuteNonQuery();
               // spifmovimi.ExecuteNonQuery();

               // spactsa.ExecuteNonQuery();
                spactestado.ExecuteNonQuery();

              //  MessageBox.Show("Datos guardados");
                dgvVentaDetalle.Rows.Clear();
                dgvVentaDetalle.Refresh();
                txtVentaDetalle.Clear();

            }
            catch (Exception ex)
            {

           //     MessageBox.Show("Error: Datos no guardados");
                string err = ex.ToString();
            }
            finally { con.Close(); }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }

        private void dgvVentaDetalle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void lblFecha_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
           


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
          // button1.PerformClick();

          //  timer2.Stop();
          //this.Close();
        }

       

        private void btnCargar_Click(object sender, EventArgs e)
        {
            
            try
            {
                
             
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))
               
                {
                 
                    string fd;
                    string fh;
                    fd = txtFechaDesde.Text;
                    fh = txtFechaHasta.Text;
                    con.Open();


                    SqlCommand cmd3 = new SqlCommand(@"select '0001' as BBSI,IDENTITY(int, 1,1) as registros,vc.id,'20553224145' as NR,
                    '20553224145' as NRuc,convert(datetime, replace(vc.invoice_date,'-',''))as invoice_date ,ci.Codigo_Item,sum(vd.product_quantity) as cantidad INTO #tempVentas
                    from VENTAS_PRESTASHOP vc
                    inner join VENTA_DETALLE_PRESTASHOP vd on vd.id_vc=vc.id
                    left join vMaxId_ItemEnCS_Barra ci on ci.id_item=vd.product_reference
                    group by vd.product_quantity,vc.id,vc.invoice_date,ci.Codigo_Item", con);
                    cmd3.ExecuteNonQuery();


                    SqlCommand cmd2 = new SqlCommand(@"select 'BBSI' as BBSI,'BEBEMAS' AS BEBEMAS,RIGHT ('0000'+CAST (t1.reg AS varchar(4)),4) as PEDIDOS,'0001' AS ID,'OD' AS OD
                    FROM (  select   count (registros) as reg from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh) t1", con);
                    cmd2.Parameters.AddWithValue("@fd", fd);
                    cmd2.Parameters.AddWithValue("@fh", fh);
                    SqlDataAdapter adaptador2 = new SqlDataAdapter();
                    adaptador2.SelectCommand = cmd2;
                    DataTable tabla2 = new DataTable();
                    adaptador2.Fill(tabla2);
                    dgvCab.DataSource = tabla2;


                    SqlCommand cmd = new SqlCommand(@"select t2.BBSI,RIGHT ('0000'+CAST (t1.reg AS varchar(4)),4) as pedidos,t2.id as NumDesp,t2.NR,t2.NRuc,convert(varchar,t2.invoice_date) as fecha,RIGHT('0000'+cast(t2.registros as varchar(4)),4) as registros,
                    t2.Codigo_Item,t2.cantidad from 
                   (  select   count (registros) as reg from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh) t1,
                   (select * from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh)t2", con);
                    cmd.Parameters.AddWithValue("@fd", fd);
                    cmd.Parameters.AddWithValue("@fh", fh);
                    SqlDataAdapter adaptador = new SqlDataAdapter();
                    adaptador.SelectCommand = cmd;
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    dgvArchivo.DataSource = tabla;


                    SqlCommand droptable = new SqlCommand(@"drop table #tempVentas", con);
                    droptable.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {

             //   MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }
        public void cargarInfo()
        {
            try
            {


                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["conexion"].ToString()))

                {

                    string fd;
                    string fh;
                    fd = txtFechaDesde.Text;
                    fh = txtFechaHasta.Text;
                    con.Open();


                    SqlCommand cmd3 = new SqlCommand(@"select '0001' as BBSI,IDENTITY(int, 1,1) as registros,vc.id,'20553224145' as NR,
                    '20553224145' as NRuc,convert(datetime, replace(vc.invoice_date,'-',''))as invoice_date ,ci.Codigo_Item,sum(vd.product_quantity) as cantidad INTO #tempVentas
                    from VENTAS_PRESTASHOP vc
                    inner join VENTA_DETALLE_PRESTASHOP vd on vd.id_vc=vc.id
                    left join vMaxId_ItemEnCS_Barra ci on ci.id_item=vd.product_reference
                    group by vd.product_quantity,vc.id,vc.invoice_date,ci.Codigo_Item", con);
                    cmd3.ExecuteNonQuery();


                    SqlCommand cmd2 = new SqlCommand(@"select 'BBSI' as BBSI,'BEBEMAS' AS BEBEMAS,RIGHT ('0000'+CAST (t1.reg AS varchar(4)),4) as PEDIDOS,'0001' AS ID,'OD' AS OD
                    FROM (  select   count (registros) as reg from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh) t1", con);
                    cmd2.Parameters.AddWithValue("@fd", fd);
                    cmd2.Parameters.AddWithValue("@fh", fh);
                    SqlDataAdapter adaptador2 = new SqlDataAdapter();
                    adaptador2.SelectCommand = cmd2;
                    DataTable tabla2 = new DataTable();
                    adaptador2.Fill(tabla2);
                    dgvCab.DataSource = tabla2;


                    SqlCommand cmd = new SqlCommand(@"select t2.BBSI,RIGHT ('0000'+CAST (t1.reg AS varchar(4)),4) as pedidos,t2.id as NumDesp,t2.NR,t2.NRuc,convert(varchar,t2.invoice_date) as fecha,RIGHT('0000'+cast(t2.registros as varchar(4)),4) as registros,
                    t2.Codigo_Item,t2.cantidad from 
                   (  select   count (registros) as reg from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh) t1,
                   (select * from #tempVentas WHERE invoice_date>=@fd and invoice_date<=@fh)t2", con);
                    cmd.Parameters.AddWithValue("@fd", fd);
                    cmd.Parameters.AddWithValue("@fh", fh);
                    SqlDataAdapter adaptador = new SqlDataAdapter();
                    adaptador.SelectCommand = cmd;
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    dgvArchivo.DataSource = tabla;


                    SqlCommand droptable = new SqlCommand(@"drop table #tempVentas", con);
                    droptable.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {

                //   MessageBox.Show("Error:No se ha podido establecer conexión con el WS Prestashop");
                string err = ex.ToString();
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            TextWriter writer = new StreamWriter(@"C:\Rec\OD-VentasWeb.txt");
            
          writer.Write(dgvCab.Rows[0].Cells[0].Value.ToString() + "|"+ dgvCab.Rows[0].Cells[1].Value.ToString() + "|"+ dgvCab.Rows[0].Cells[2].Value.ToString() + "|"+ dgvCab.Rows[0].Cells[3].Value.ToString() + "|"+ dgvCab.Rows[0].Cells[4].Value.ToString() + "|");
          writer.WriteLine("");

        

            for (int i=0; i<dgvArchivo.Rows.Count;i++)
            {
                for (int j = 0; j < dgvArchivo.Columns.Count; j++)
                {
                    writer.Write(dgvArchivo.Rows[i].Cells[j].Value.ToString()+"|");
                }
                writer.WriteLine("");
            }
            writer.Close();
          //  MessageBox.Show("Datos exportados correctamente");
        }

        public void generarArchivo()
        {
            TextWriter writer = new StreamWriter(@"C:\Rec\OD-VentasWeb.txt");

            writer.Write(dgvCab.Rows[0].Cells[0].Value.ToString() + "|" + dgvCab.Rows[0].Cells[1].Value.ToString() + "|" + dgvCab.Rows[0].Cells[2].Value.ToString() + "|" + dgvCab.Rows[0].Cells[3].Value.ToString() + "|" + dgvCab.Rows[0].Cells[4].Value.ToString() + "|");
            writer.WriteLine("");



            for (int i = 0; i < dgvArchivo.Rows.Count; i++)
            {
                for (int j = 0; j < dgvArchivo.Columns.Count; j++)
                {
                    writer.Write(dgvArchivo.Rows[i].Cells[j].Value.ToString() + "|");
                }
                writer.WriteLine("");
            }
            writer.Close();
          //  MessageBox.Show("Datos exportados correctamente");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_4(object sender, EventArgs e)
        {
            //funcionPincipal();

            try
            {
                funcionPincipal();
                //rxtGetVentas.PerformClick();

                //btnGuardar.PerformClick();
                //btnTreVentaDetalle.PerformClick();
                //btnGuardarVD.PerformClick();

                //btnPost.PerformClick();
                //btnTraeProdu.PerformClick();
                //btnGuardarProdu.PerformClick();
                //btnTraerStock.PerformClick();
                //btnGuardarStock.PerformClick();
                //btnPrueba.PerformClick();
                //btnActualizaPrecio.PerformClick();


            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            //finally 
            { 
                this.Close();
            }

        }

        private void dgvStockProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    public class Persona
    {
        
        public string producto { get; set; }

    }

    #region de referencia de fotos 
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

