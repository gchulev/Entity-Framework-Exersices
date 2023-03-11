namespace ProductShop.DTOs.Import
{
    public class ImportUsersDto
    {
        public string? FirstName { get; set; }

        public string LastName { get; set; } = null!;

        public int? Age { get; set; }
    }
}
