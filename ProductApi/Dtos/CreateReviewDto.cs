namespace ProductApi.Dtos
{
    public class CreateReviewDto
    {
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
    }
}
