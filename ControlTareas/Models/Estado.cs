using System;
using System.Collections.Generic;

namespace ControlTareas.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Estado1 { get; set; }

    public virtual ICollection<Tarea> Tareas { get; } = new List<Tarea>();
}
