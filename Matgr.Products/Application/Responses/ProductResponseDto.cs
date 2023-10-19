namespace Matgr.Products.Application.Responses
{
    // Dont use it
    public class ProductResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
        public string? DisplayMessage { get; set; }
        public List<string>? ErrorMessage { get; set; }


    }
}
