using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OIDCServer.ViewModels;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace OIDCServer.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentController(
            IClientStore clientStore, 
            IResourceStore resourceStore, 
            IIdentityServerInteractionService identityServerInteractionService
            )
        {
            this._clientStore = clientStore;
            this._resourceStore = resourceStore;
            this._identityServerInteractionService = identityServerInteractionService;
        }


        private async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            //基于传递给登录或同意页面的returnUrl返回AuthorizationRequest
            AuthorizationRequest request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request==null)
            {
                return null;
            }

            //request.ClientId  发起请求的客户端标识符
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);

            //request.ScopesRequested 授权请求中请求的范围
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            return CreateConsentViewModel(request,client,resources);
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request,Client client,Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.Description = client.Description;
            vm.AllowRemeberConsent = client.AllowRememberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i=>i.Scopes).Select(x=>CreateScopeViewModel(x));

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel 
            { 
                Name=identityResource.Name,
                DisplayName=identityResource.DisplayName,
                Description = identityResource.Description,
                Checked=identityResource.Required,
                Required= identityResource.Required,
                Emphasize=identityResource.Emphasize
            };
        }
        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            ConsentViewModel model = await BuildConsentViewModel(returnUrl);
            if (model==null)
            {

            }

            return View(model);
        }
    }
}