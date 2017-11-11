using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SimuladorPrestamos.BO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace SimuladorPrestamos.DAO
{
    public class SimulacionesDAO
    {
        ConexionDAO Conexion;
        public SimulacionesDAO()
        {
            Conexion = new ConexionDAO();
        }

        public int Agregar(SimulacionBO simulacion)
        {
            SqlCommand command = new SqlCommand("insert into Simulaciones values(@idCliente, @Monto, @Taza, @Plazo, @FechaIni)");
            command.Parameters.Add("@idCliente", SqlDbType.Int).Value = simulacion.CodigoCliente;
            command.Parameters.Add("@Monto", SqlDbType.Real).Value = simulacion.Monto;
            command.Parameters.Add("@Taza", SqlDbType.Real).Value = simulacion.TazaInteres;
            command.Parameters.Add("@Plazo", SqlDbType.Int).Value = simulacion.PlazoPago;
            command.Parameters.Add("@FechaIni", SqlDbType.Date).Value = simulacion.FechaIncio.ToString("yyyy/MM/dd");
            return Conexion.EjecutarComando(command);
        }

        public int Modificar(SimulacionBO simulacion)
        {
            SqlCommand command = new SqlCommand("update Simulaciones set idCliente = @idCliente, Monto = @Monto, Taza = @Taza, Plazo = @Plazo, FechaInicio = @FechaIni where idPrestamo = @idPrestamo");
            command.Parameters.Add("@idPrestamo", SqlDbType.Int).Value = simulacion.CodigoPrestamo;
            command.Parameters.Add("@idCliente", SqlDbType.Int).Value = simulacion.CodigoCliente;
            command.Parameters.Add("@Monto", SqlDbType.Real).Value = simulacion.Monto;
            command.Parameters.Add("@Taza", SqlDbType.Real).Value = simulacion.TazaInteres;
            command.Parameters.Add("@Plazo", SqlDbType.Int).Value = simulacion.PlazoPago;
            command.Parameters.Add("@FechaIni", SqlDbType.Date).Value = simulacion.FechaIncio.ToString("yyyy/MM/dd");
            return Conexion.EjecutarComando(command);
        }

        public int Eliminar(SimulacionBO simulacion)
        {
            SqlCommand command = new SqlCommand("delete from Simulaciones where idPrestamo = @idPrestamo");
            command.Parameters.Add("@idPrestamo", SqlDbType.Int).Value = simulacion.CodigoPrestamo;
            command.Parameters.Add("@idCliente", SqlDbType.Int).Value = simulacion.CodigoCliente;
            command.Parameters.Add("@Monto", SqlDbType.Real).Value = simulacion.Monto;
            command.Parameters.Add("@Taza", SqlDbType.Real).Value = simulacion.TazaInteres;
            command.Parameters.Add("@Plazo", SqlDbType.Int).Value = simulacion.PlazoPago;
            command.Parameters.Add("@FechaIni", SqlDbType.Date).Value = simulacion.FechaIncio.ToString("yyyy/MM/dd");
            return Conexion.EjecutarComando(command);
        }

        public DataSet ListaSimulaciones()
        {
            return Conexion.EjecutarSentencia(new SqlCommand("select idPrestamo, Clientes.idCliente ,Nombre, Monto, TazaInteres, Plazo, FechaInicio from Simulaciones inner join Clientes on Simulaciones.idCliente1 = Clientes.idCliente"));
        }

        public SimulacionBO BuscarSimulacion(int id)
        {
            SimulacionBO simulacion = new SimulacionBO();
            SqlCommand Comando = new SqlCommand("Select  * from Simulaciones where idPrestamo ='" + id + "'");
            SqlDataReader Reader;
            Comando.Connection = Conexion.ConectarBD();
            Conexion.Abrir();
            Reader = Comando.ExecuteReader();
            if(Reader.Read())
            {
                simulacion.CodigoPrestamo = int.Parse(Reader[0].ToString());
                simulacion.CodigoCliente = int.Parse(Reader[1].ToString());
                simulacion.Monto = double.Parse(Reader[2].ToString());
                simulacion.TazaInteres = double.Parse(Reader[3].ToString());
                simulacion.PlazoPago = int.Parse(Reader[4].ToString());
                simulacion.FechaIncio = DateTime.Parse(Reader[5].ToString());
            }
            Conexion.Cerrar();
            return simulacion;
        }

        public void GenerarPDF(int id, string ruta)
        {


                FileStream PFD = new FileStream(ruta, FileMode.Create);
              

                //Instancia de la clase clientes DAO para poder buscar al cliente que hace el prestamo
                ClientesDAO clientes = new ClientesDAO();
                //Buscamos el registro de la simulacion
                SimulacionBO simulacion = BuscarSimulacion(id);
                //Buscamos el cliente al que se le hizo la simulacion
                ClientesBO cliente = clientes.BuscarCliente(simulacion.CodigoCliente);

                //Creamos un nuevo documento de ITextSharp y le pasamos como parametro que queremos que sea de tamaño carta
                Document Reporte = new Document(PageSize.LETTER);
                PdfWriter Writer = PdfWriter.GetInstance(Reporte, PFD);
                //Agregamos un titulo
                Reporte.AddTitle("Simulacion Prestamo " + simulacion.CodigoPrestamo);
                //Agregarmos Al creador del documento
                Reporte.AddCreator("PrestamosUTM");
                //Y lo abrimos para editar
                Reporte.Open();
                //Intanciamos la fuente que tendra nuestro documento
                Font Fuente = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK);
                //Agregamos un parrafo nuevo para que sea nuestra cabecera
                Reporte.Add(new Paragraph("Simulacion de prestamo N." + simulacion.CodigoPrestamo));
                Reporte.Add(Chunk.NEWLINE);
                //Hacemos lo mismp para los datos del cliente
                Reporte.Add(new Paragraph("Numero de cliente: " + cliente.CodigoCliente + "\nCliente: " + cliente.Nombre + " " + cliente.Apellido));
                Reporte.Add(Chunk.NEWLINE);
                //Creamos una nueva tabla y le pasamos como parametro el numero de columnas
                PdfPTable tablaPrestamo = new PdfPTable(6);
                tablaPrestamo.WidthPercentage = 100;

                //Configuaramos el titulo de las columnas
                PdfPCell CellNumeroPago = new PdfPCell(new Phrase("No. Pago", Fuente));
                CellNumeroPago.BorderWidth = 0;
                CellNumeroPago.BorderWidthBottom = 0.75f;

                PdfPCell CellFecha = new PdfPCell(new Phrase("Fecha", Fuente));
                CellFecha.BorderWidth = 0;
                CellFecha.BorderWidthBottom = 0.75f;

                PdfPCell CellPrincial = new PdfPCell(new Phrase("Principal", Fuente));
                CellPrincial.BorderWidth = 0;
                CellPrincial.BorderWidthBottom = 0.75f;

                PdfPCell CellInteres = new PdfPCell(new Phrase("Interés", Fuente));
                CellInteres.BorderWidth = 0;
                CellInteres.BorderWidthBottom = 0.75f;

                PdfPCell CellIVA = new PdfPCell(new Phrase("IVA(16%)", Fuente));
                CellIVA.BorderWidth = 0;
                CellIVA.BorderWidthBottom = 0.75f;

                PdfPCell CellPago = new PdfPCell(new Phrase("Pago Total", Fuente));
                CellPago.BorderWidth = 0;
                CellPago.BorderWidthBottom = 0.75f;

                //Agregamos las celdas a la tabla
                tablaPrestamo.AddCell(CellNumeroPago);
                tablaPrestamo.AddCell(CellFecha);
                tablaPrestamo.AddCell(CellPrincial);
                tablaPrestamo.AddCell(CellInteres);
                tablaPrestamo.AddCell(CellIVA);
                tablaPrestamo.AddCell(CellPago);

                //Pasamos el total a una variable
                double Monto = simulacion.Monto;
            
                    DateTime Fecha = simulacion.FechaIncio;

                for (int i = simulacion.PlazoPago, j = 1; i > 0; i--, j++)
                {
                    //calculamos los datos que se va a usar
                    double InteresMensual = Monto * simulacion.TazaInteres;
                    double IVA = (Monto + InteresMensual) * .16;
                    double TotalPago = (Monto + InteresMensual + IVA) / i;

                    CellNumeroPago = new PdfPCell(new Phrase(j.ToString(), Fuente));
                    CellNumeroPago.BorderWidth = 1;

                    CellFecha = new PdfPCell(new Phrase(Fecha.ToShortDateString(), Fuente));
                    CellFecha.BorderWidth = 1;

                    CellPrincial = new PdfPCell(new Phrase(Math.Round(Monto, 2).ToString(), Fuente));
                    CellPrincial.BorderWidth = 1;

                    CellInteres = new PdfPCell(new Phrase(Math.Round(InteresMensual,2).ToString(), Fuente));
                    CellInteres.BorderWidth = 1;

                    CellIVA = new PdfPCell(new Phrase(Math.Round(IVA,2).ToString(), Fuente));
                    CellIVA.BorderWidth = 1;

                    CellPago = new PdfPCell(new Phrase(Math.Round(TotalPago,2).ToString(), Fuente));
                    CellPago.BorderWidth = 1;

                    //Añadimos las celdas a la tabla
                    tablaPrestamo.AddCell(CellNumeroPago);
                    tablaPrestamo.AddCell(CellFecha);
                    tablaPrestamo.AddCell(CellPrincial);
                    tablaPrestamo.AddCell(CellInteres);
                    tablaPrestamo.AddCell(CellIVA);
                    tablaPrestamo.AddCell(CellPago);
                Monto = Monto - TotalPago;
                Fecha = Fecha.AddMonths(1);

                }
                Reporte.Add(tablaPrestamo);
                Reporte.Close();
                PFD.Close();
                Writer.Close();
            
        }

    }
}