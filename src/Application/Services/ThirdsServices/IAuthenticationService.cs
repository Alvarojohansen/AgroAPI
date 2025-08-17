using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ThirdsServices
{
    public interface IAuthenticationService
    {
        string? Authenticate(CredentialsRequest credentials);
    }
}
