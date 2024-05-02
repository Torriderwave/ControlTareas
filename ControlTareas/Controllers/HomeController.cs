using ControlTareas.Models;
using ControlTareas.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ClosedXML.Excel;

namespace ControlTareas.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DbcontrolTContext _DBContext;

        public HomeController(DbcontrolTContext context)
        {
            _DBContext = context;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            
            
            List<Tarea> lista = _DBContext.Tareas.Include(c => c.oEstado).ToList();
            
            return View(lista);

        }

        [HttpGet]
        public async Task<FileResult> ExportarTareasAExcel()
        {
            var lista = await _DBContext.Tareas.ToListAsync();
            var lista2 = await _DBContext.Estados.ToListAsync();
            var nombreArchivo = $"Reporte de Tareas.xlsx";
            return GenerarExcel(nombreArchivo, lista, lista2);
        }


        private FileResult GenerarExcel(string nombreArchivo, IEnumerable<Tarea> lista, IEnumerable<Estado> lista2)
        {
            DataTable dataTable = new DataTable("Reporte de control de Tareas");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Fecha"),
                new DataColumn("Descripcion"),
                new DataColumn("Estado")
            });

            foreach ((var listas,var listas2) in lista.Zip(lista2))
            {
                DataRow dataRow = dataTable.Rows.Add(listas.Fecha,
                            listas.Descripcion, listas2.Estado1);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo);
                }

            }
        }


        [HttpGet]
        public IActionResult Tarea_Detalle( int idTarea)
        {
            TareaVM oTareaVM = new TareaVM()
            {
                oTarea = new Tarea(),
                oListaEstado = _DBContext.Estados.Select(estado => new SelectListItem()
                {
                    Text = estado.Estado1,
                    Value = estado.IdEstado.ToString()
                }).ToList()
                
            
            };

            if (idTarea!=0)
            {
                oTareaVM.oTarea = _DBContext.Tareas.Find(idTarea);
            }

            return View(oTareaVM);

        }


        [HttpPost]
        public IActionResult Tarea_Detalle(TareaVM oTareaVM)
        {
            if(oTareaVM.oTarea.IdTarea == 0) {
            _DBContext.Tareas.Add(oTareaVM.oTarea);

            }
            else
            {
                _DBContext.Tareas.Update(oTareaVM.oTarea);
            }

            _DBContext.SaveChanges();   

            return RedirectToAction("Index","Home");

        }


        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("IniciarSesion", "Inicio");
        }

        [HttpGet]
        public IActionResult Eliminar(int idTarea)
        {
            Tarea oTarea = _DBContext.Tareas.Include(c=>c.oEstado).Where(e => e.IdTarea==idTarea)
                .FirstOrDefault();

            return View(oTarea);

        }

        [HttpPost]
        public IActionResult Eliminar(Tarea oTarea)
        {
            _DBContext.Tareas.Remove(oTarea);
            _DBContext.SaveChanges();


            return RedirectToAction("Index", "Home");

        }

    }
}
