using SimuladorPrestamos.BO;
using SimuladorPrestamos.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using System.IO;

namespace SimuladorPrestamos.Controllers
{
    public class PrestamosController : Controller
    {
        SimulacionesDAO SimulacionDAO = new SimulacionesDAO();
        ClientesDAO Clientes = new ClientesDAO();
        // GET: Prestamos

        public ActionResult Simulaciones()
        {
            return View(SimulacionDAO.ListaSimulaciones().Tables[0]);
        }

        public ActionResult Editar()
        {

            return View(new SimulacionBO());
        }


        public ActionResult ListaNombres()
        {
            return Json(Clientes.ListaNombreClientes(), JsonRequestBehavior.AllowGet);
        } 

        public ActionResult Guardar(SimulacionBO Simulacion)
        {
            SimulacionDAO.Agregar(Simulacion);
            return RedirectToAction("Simulaciones");
        }

        public ActionResult Archivo(int id)
        {
            SimulacionDAO.GenerarPDF(id, Server.MapPath("~/Reportes/Reporte" + id + ".pdf"));
            
            return File(Server.MapPath("~/Reportes/Reporte" + id + ".pdf"), "application/pdf", "Reporte"+id+".pdf");
        }
    }
}