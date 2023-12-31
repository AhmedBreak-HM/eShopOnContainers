using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identity_Demo.Pages;
using IdentityModel;
using Matgr.Identity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Matgr.IdentityServer.Pages.Account.Register
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        public ViewModel View { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [ViewData]
        public object Message { get; set; }

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IClientStore _clientStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleInManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;

        public Index(
                    IIdentityServerInteractionService interaction,
                    IAuthenticationSchemeProvider schemeProvider,
                    IClientStore clientStore,
                    UserManager<ApplicationUser> userManager,
                    RoleManager<IdentityRole> roleInManager,
                    SignInManager<ApplicationUser> signInManager,
                    IEventService events)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _userManager = userManager;
            _roleInManager = roleInManager;
            _signInManager = signInManager;
            _events = events;
        }

        public async Task<IActionResult> OnGet(string? returnUrl=null)
        {
            await BuildModelAsync(returnUrl);

            if (View.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToPage("/ExternalLogin/Challenge/Index", new { scheme = View.ExternalLoginScheme, returnUrl });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");

                    await _userManager.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name,Input.FirstName+" "+ Input.LastName),
                            new Claim(JwtClaimTypes.Email, Input.Email),
                            new Claim(JwtClaimTypes.FamilyName, Input.LastName),
                            new Claim(JwtClaimTypes.GivenName, Input.FirstName),
                            new Claim(JwtClaimTypes.WebSite, "http://"+Input.Username+".com"),
                            new Claim(JwtClaimTypes.Role,"Customer") });

                    await _signInManager.PasswordSignInAsync(Input.Username, Input.Password,
                        true, lockoutOnFailure: false);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect("https://localhost:7146/Account/Login");
                }
            }
            // something went wrong, show form with error
            await BuildModelAsync(Input.ReturnUrl);
            return Page();
        }



        private async Task BuildModelAsync(string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                View = new ViewModel
                {
                    EnableLocalLogin = local,

                };

                Input.Username = context?.LoginHint;
                Input.ReturnUrl = returnUrl;

                if (!local)
                {
                    View.ExternalProviders = new[] { new ViewModel.ExternalProvider { AuthenticationScheme = context.IdP } };
                }
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ViewModel.ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();



            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            View = new ViewModel
            {
                AllowRememberLogin = RegisterOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && RegisterOptions.AllowLocalLogin,
                ExternalProviders = providers.ToArray()
            };
        }
    }
}
