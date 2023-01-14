using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoProject.Models
{
    public class WebApiResponse<T>
    {
        public T? Result { get; set; }
        public string? Error { get; set; }
        public bool IsSuccesful { get; set; }
    }
}
