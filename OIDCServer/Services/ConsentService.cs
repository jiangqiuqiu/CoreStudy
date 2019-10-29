using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using OIDCServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCServer.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(
            IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService identityServerInteractionService
            )
        {
            this._clientStore = clientStore;
            this._resourceStore = resourceStore;
            this._identityServerInteractionService = identityServerInteractionService;
        }

        #region Private Method
        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources,InputConsentViewModel model)
        {
            var selectedScope = model?.ScopeConsented ?? Enumerable.Empty<string>();
            var remeberConsent = model?.RememberConsent ?? true ;

            var vm = new ConsentViewModel();


            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.Description = client.Description;
            vm.RememberConsent = remeberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i,selectedScope.Contains(i.Name)||model==null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(x => CreateScopeViewModel(x, selectedScope.Contains(x.Name) || model == null));

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked =check || identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }
        private ScopeViewModel CreateScopeViewModel(Scope scope,bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked =check || scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }
        #endregion

        public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl,InputConsentViewModel model = null)
        {
            //基于传递给登录或同意页面的returnUrl返回AuthorizationRequest
            AuthorizationRequest request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            //request.ClientId  发起请求的客户端标识符
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);

            //request.ScopesRequested 授权请求中请求的范围
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var vm = CreateConsentViewModel(request, client, resources,model);
            vm.ReturnUrl = returnUrl;

            return vm;
        }

        public async Task<ProcessConsentResult> ProcessConsent(InputConsentViewModel model)
        {
            ConsentResponse consentResponse = null;
            var result = new ProcessConsentResult();

            if (model.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (model.Button == "yes")
            {
                if (model.ScopeConsented != null && model.ScopeConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopeConsented
                    };
                }
                else
                {
                    result.ValidationError = "请至少选中一个权限";
                }
                
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                result.RedirectUrl = model.ReturnUrl;
            }

            {
                var consentViewModel =await BuildConsentViewModelAsync(model.ReturnUrl,model);
                result.ViewModel = consentViewModel;
            }

            return result;
        }
    }
}
