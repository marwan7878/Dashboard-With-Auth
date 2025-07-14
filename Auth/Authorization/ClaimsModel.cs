namespace Auth.Authorization
{
    public class ClaimsModel
    {
        public int RoleId { get; set; }

        public List<ClaimSelection> FirstClaimsList { get; set; } = new();
       
        public ClaimsModel()
        {
            FirstClaimsList = [];
            
        }

    }

    public class ClaimSelection
    {
        public string ClaimType { get; set; }
        public string? Label { get; set; }
        public bool IsSelected { get; set; }
    }
}