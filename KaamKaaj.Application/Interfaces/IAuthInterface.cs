using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaamKaaj.Domain.DTOs;

namespace KaamKaaj.Application.Interfaces
{
    public interface IAuthInterface
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<string> LoginAsync(UserDto userDto);
        Task<string> SignupAsync(UserDto userDto);
    }
}
