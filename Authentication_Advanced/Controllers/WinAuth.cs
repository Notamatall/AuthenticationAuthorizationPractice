using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Authentication_Advanced.Controllers
{

    [ApiController]
    [Route("[controller]")]
  
    public class WinAuth : ControllerBase
    {
        private readonly ILogger<WinAuth> _logger;

        public WinAuth(ILogger<WinAuth> logger)
        {
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public string GetAnon()
        {
            var anonUser = User.Identity.Name;
            return anonUser;
        }
     

        [Authorize]
        [HttpGet]
        public List<string> Home()
        {
            //List<GroupPrincipal> result = new List<GroupPrincipal>();

            //// establish domain context
            //PrincipalContext yourDomain = new PrincipalContext(ContextType.Machine);

            //// find your user
            //UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, User.Identity.Name);

            //// if found - grab its groups
            //if (user != null)
            //{
            //    PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

            //    // iterate over all groups
            //    foreach (Principal p in groups)
            //    {
            //        // make sure to add only group principals
            //        if (p is GroupPrincipal)
            //        {
            //            result.Add((GroupPrincipal)p);
            //        }
            //    }
            //}



            var wi = (WindowsIdentity)User.Identity;
            var result= new List<string>();
            foreach (IdentityReference group in wi.Groups)
            {
                try
                {
                    result.Add(group.Translate(typeof
                    (System.Security.Principal.NTAccount)).ToString());
                }
                catch (Exception ex) { }
            }
            result.Sort();
            return result; 
         

        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("[action]")]
        public string GetPcUsers()
        {
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            string result = string.Empty;

            foreach (ManagementObject envVar in searcher.Get())
            {
                result += $"Username : {envVar["Name"]}\n";
            }

            result += $"Authenticated user username :  {User.Identity.Name}\n";

            return result;

        }

        [Authorize]
        [HttpGet("[action]")]
        public string TimaKrasava()
        {
            return "Tima petux lisiy";

        }


        [Authorize]
        [HttpGet("[action]")]
        public string GetAuth()
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Machine);

            // find your user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, User.Identity.Name);

            // if found - grab its groups
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return "dsa";

        }
    }
}
