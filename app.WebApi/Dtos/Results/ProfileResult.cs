namespace app.WebApi.Dtos.Results
{
    public class ProfileResult : BaseModelResult
    {        
        public string Name { get; set; }
        public List<PermissionResult> Permissions { get; set; }
    }
}