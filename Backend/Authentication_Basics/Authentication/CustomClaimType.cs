namespace Authentication_Basics.Authentication
{
    public static class CustomClaimType
    {
        public const string ClaimNamespace = "http://schemas.mycompany.com/ws/2020/01/identity/claims";

        public const string Permission = ClaimNamespace + "/permission";
        public const string UserId = ClaimNamespace + "/userid";
        public const string UserLanguageCode = ClaimNamespace + "/userlanguagecode";
        public const string UserCultureCode = ClaimNamespace + "/userculturecode";
        public const string UserActiveDirectoryId = ClaimNamespace + "/useractivedirectoryid";
        public const string UserFirstName = ClaimNamespace + "/userfirstname";
        public const string UserLastName = ClaimNamespace + "/userlastname";
        public const string UserEmail = ClaimNamespace + "/useremail";
        public static string ApplyNamespace(string claim)
        {
            return string.Concat(ClaimNamespace, "/user/", claim.ToLowerInvariant());
        }
    }
}
