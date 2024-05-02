using System;
using System.Collections.Generic;

namespace ControlTareas.Models;

public partial class Tarea
{
    public int IdTarea { get; set; }

    public string? Fecha { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? oEstado { get; set; }
}
