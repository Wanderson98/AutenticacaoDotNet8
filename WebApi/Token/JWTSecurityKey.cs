﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Token
{
    public class JWTSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
