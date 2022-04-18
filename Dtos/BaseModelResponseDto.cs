using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class BaseModelResponseDto
    {
        public ApiResponseCode Code { get; set; }
        public string Message { get; set; }
    }

    public class BaseModelResponseDto<T> : BaseModelResponseDto where T : class
    {
        public T Data { get; set; }
    }
}
