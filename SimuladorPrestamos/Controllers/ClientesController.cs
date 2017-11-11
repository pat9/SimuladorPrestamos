using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimuladorPrestamos.DAO;
using SimuladorPrestamos.BO;

namespace SimuladorPrestamos.Controllers
{
    public class ClientesController : Controller
    {
        ClientesDAO Clientes = new ClientesDAO();
        // GET: Clientes
        
        public ActionResult Index()
        {
            return View(Clientes.ListaClientes().Tables[0]);
        }

        public ActionResult Editar(int id = 0)
        {
            ClientesBO ObjDev = (id == 0) ?  (new ClientesBO()) : (Clientes.BuscarCliente(id));
            return View(ObjDev);
        }

        public ActionResult Guardar(ClientesBO Cliente)
        {
            int res = Cliente.CodigoCliente > 1 ? Clientes.Modificar(Cliente) : Clientes.Agregar(Cliente);
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            if(id >0)
            {
                Clientes.Eliminar(Clientes.BuscarCliente(id));
            }
            return new EmptyResult();
        }

    }
}