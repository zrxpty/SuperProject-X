using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BLL.Models
{
    public class ServiceResponse<T> : ServiceStatus
    {
        public T? Data { get; set; } = default;
    }

    public class ServiceStatus
    {
        public int Code { get; set; } = 200;
        public string? Message { get; set; } = null;
    }
}
