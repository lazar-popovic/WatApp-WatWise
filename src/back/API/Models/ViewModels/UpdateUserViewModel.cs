namespace API.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public required string? Email { get; set; } = string.Empty;
        public required string? Firstname { get; set; } = string.Empty;
        public required string? Lastname { get; set; } = string.Empty;
        public required LocationViewModel Location { get; set; }
    }
}
