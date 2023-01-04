using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Shared.Enums
{
    /// <summary>
    /// Enumerazione per gestire il tipo del task,
    /// ogni tipo corrisponderà a un numero che verrà inserito
    /// nel database, senza la necessità di andare a creare
    /// una tabella specifica
    /// </summary>
    public enum ToDoType
    {
        IN_CORSO,
        FINITO,
        DA_INIZIARE
    }
}
