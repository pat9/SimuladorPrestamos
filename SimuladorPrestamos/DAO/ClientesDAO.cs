using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SimuladorPrestamos.BO;

namespace SimuladorPrestamos.DAO
{
    public class ClientesDAO
    {
        ConexionDAO Conexion;

        public ClientesDAO()
        {
            Conexion = new ConexionDAO();
        }

        public int Agregar(ClientesBO Cliente)
        {
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Clientes VALUES(@nombre, @apellido,@telefono)");
            sqlCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = Cliente.Nombre;
            sqlCommand.Parameters.Add("@apellido", SqlDbType.VarChar).Value = Cliente.Apellido;
            sqlCommand.Parameters.Add("@telefono", SqlDbType.VarChar).Value = Cliente.Telefono;
            return Conexion.EjecutarComando(sqlCommand);
        }

        public int Modificar(ClientesBO Cliente)
        {
            SqlCommand sqlCommand = new SqlCommand("UPDATE Clientes SET Nombre = @nombre, Apellido = @apellido, Telefono = @telefono where idCliente = @id");
            sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = Cliente.CodigoCliente;
            sqlCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = Cliente.Nombre;
            sqlCommand.Parameters.Add("@apellido", SqlDbType.VarChar).Value = Cliente.Apellido;
            sqlCommand.Parameters.Add("@telefono", SqlDbType.VarChar).Value = Cliente.Telefono;
            return Conexion.EjecutarComando(sqlCommand);
        }
        public int Eliminar(ClientesBO Cliente)
        {
            Conexion.EjecutarComando(new SqlCommand("DELETE FROM Simulaciones where idCliente1='" + Cliente.CodigoCliente + "'"));
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM Clientes where idCliente = @id");
            sqlCommand.Parameters.Add("@id", SqlDbType.VarChar).Value = Cliente.CodigoCliente;
            sqlCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = Cliente.Nombre;
            sqlCommand.Parameters.Add("@apellido", SqlDbType.VarChar).Value = Cliente.Apellido;
            sqlCommand.Parameters.Add("@telefono", SqlDbType.VarChar).Value = Cliente.Telefono;
            return Conexion.EjecutarComando(sqlCommand);
        }

        public DataSet ListaClientes()
        {
            return Conexion.EjecutarSentencia(new SqlCommand("Select * from Clientes"));
        }

        public ClientesBO BuscarCliente(int id)
        {
            ClientesBO Cliente = new ClientesBO();
            SqlCommand Comando = new SqlCommand("Select  * from Clientes where idCliente='" + id + "'");
            SqlDataReader Reader;
            Comando.Connection = Conexion.ConectarBD();
            Conexion.Abrir();
            Reader = Comando.ExecuteReader();
            if(Reader.Read())
            {
                Cliente.CodigoCliente = int.Parse(Reader[0].ToString());
                Cliente.Nombre = Reader[1].ToString();
                Cliente.Apellido = Reader[2].ToString();
                Cliente.Telefono = Reader[3].ToString();
            }
            Conexion.Cerrar();
            return Cliente;
        }

        public List<ClientesBO> ListaNombreClientes()
        {
            List<ClientesBO> Lista = new List<ClientesBO>();
            SqlCommand Comando = new SqlCommand("Select  * from Clientes");

            SqlDataReader Reader;
            Comando.Connection = Conexion.ConectarBD();
            Conexion.Abrir();
            Reader = Comando.ExecuteReader();
            while(Reader.Read())
            {
                ClientesBO Cliente = new ClientesBO();
                Cliente.CodigoCliente = int.Parse(Reader[0].ToString());
                Cliente.Nombre = Reader[1].ToString();
                Lista.Add(Cliente);
            }
            Conexion.Cerrar();
            return Lista;
        }


    }
}