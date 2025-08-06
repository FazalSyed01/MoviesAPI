using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaamKaaj.Domain.DTOs
{
    public record UserDto(string Username, string Password, string Role);
}
