using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute()
            : base(typeof(BasicAuthorizationFilter))
        {
        }
    }
    public class BasicAuthorizationFilter : IAuthorizationFilter
    {

        private string _authKey;
        private ILogger _logger;
        private IConfiguration _configuration;

        public BasicAuthorizationFilter(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<BasicAuthorizationFilter>();
            _authKey = _configuration.GetValue<string>("DocConverterApiConfig:Key");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HasValidAuthorization(context);
        }

        private void HasValidAuthorization(AuthorizationFilterContext context)
        {

            string authBase64 = GetBase64String(context);
            if (string.IsNullOrEmpty(authBase64))
            {
                HandleUnathorized(context);
                return;
            }


            byte[] data = Convert.FromBase64String(authBase64);
            string decodedString = Encoding.UTF8.GetString(data);
            if (decodedString != _authKey)
            {
                _logger.LogWarning(1000, $"Sem autorização - DecodedString - {decodedString} - ip: {context.HttpContext.Connection.RemoteIpAddress}");

                HandleUnathorized(context);
                return;
            }
        }

        private void HandleUnathorized(AuthorizationFilterContext context)
        {
            _logger.LogWarning(1000, $"Sem autorização para uso da api - chaveUsada: {context.HttpContext.Request.Headers["Authorization"]} - ip: {context.HttpContext.Connection.RemoteIpAddress}");
            context.Result = new UnauthorizedResult();
        }

        private bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
        private string GetBase64String(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var auth) || auth.Count == 0)
            {
                _logger.LogWarning(1001, $"Não encontrado header de autorization - ip: {context.HttpContext.Connection.RemoteIpAddress}");
                return null;
            }


            string str = auth.ToString();
            if (str.ToLower().Contains("basic"))
            {
                var value = str.Substring(str.ToLower().IndexOf("basic") + 5, str.Length - 5).Trim();
                if (IsBase64String(value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning(1001, $"Não é uma string base64 valida: str -{str}- value: {value} - ip: {context.HttpContext.Connection.RemoteIpAddress}");

                }
                return IsBase64String(value) ? value : null;
            }
            _logger.LogWarning(1001, $"Authorization invalid {str}- ip: {context.HttpContext.Connection.RemoteIpAddress}");

            return null;
        }

    }
}
