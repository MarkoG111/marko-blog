using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.UseCases
{
    public class GetUserUseCaseDto
    {
        public int IdUseCase { get; set; }
        public string UseCaseName { get; set;}
    }
}