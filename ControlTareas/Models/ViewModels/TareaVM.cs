using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlTareas.Models.ViewModels
{
    public class TareaVM
    {
        public Tarea oTarea { get; set; }
        public List<SelectListItem> oListaEstado { get; set; }
    }
}
