using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using server.Interfaces;
using server.Models;

namespace server.Wrappers
{
    public class UrlHelperWrapper : IUrlGenerator
    {
        private readonly IUrlHelper _urlGenerator;
        private readonly HttpContext _httpContext;
        private readonly string _controller;
        
        public UrlHelperWrapper(IUrlHelper urlGenerator, HttpContext httpContext, string controller)
        {
            _urlGenerator = urlGenerator;
            _httpContext = httpContext;
            _controller = controller;
        }
        public string GenerateVerificationLink(string token, string email)
        {
            return _urlGenerator.Action("ConfirmEmail", _controller, new { token, email = email }, _httpContext.Request.Scheme);
        }
    }
}
