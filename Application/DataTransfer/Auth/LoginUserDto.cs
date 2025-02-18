using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer.Auth
{
  public class LoginUserDto
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }
}