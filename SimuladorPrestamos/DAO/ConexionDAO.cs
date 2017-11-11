using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SimuladorPrestamos.DAO
{
    public class ConexionDAO
    {
        SqlConnection Conexion;
        SqlCommand Comando;
        SqlDataAdapter Adaptador;
        DataSet Datos;

        public ConexionDAO()
        {
            Comando = new SqlCommand();
            Adaptador = new SqlDataAdapter();
        }

        public SqlConnection ConectarBD()
        {
            Conexion = new SqlConnection("server=DANIEL-PC ; database=SimuladorPrestamos ; integrated security = true;");
            return Conexion;
        }

        public void Abrir()
        {
            Conexion.Open();
        }

        public void Cerrar()
        {
            Conexion.Close();
        }

        public int EjecutarComando(SqlCommand command)
        {
            //try
            {
                Comando = new SqlCommand();
                Comando = command;
                Comando.Connection = ConectarBD();
                Abrir();
                Comando.ExecuteNonQuery();
                Cerrar();
                return 1;
            }
            //catch
            //{
              //  return 0;
            //}
        }

        public DataSet EjecutarSentencia(SqlCommand command)
        {
            //try
            {
                Datos = new DataSet();
                Comando = command;
                Comando.Connection = ConectarBD();
                Adaptador.SelectCommand = Comando;
                Abrir();
                Adaptador.Fill(Datos);
                Cerrar();
                return Datos;
            }
            //catch
            //{
                //return Datos;
            //}
        }


    }
}