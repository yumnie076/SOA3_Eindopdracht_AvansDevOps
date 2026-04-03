namespace AvansDevOps.Domain;

// FACTORY METHOD PATTERN: Concrete product - ReviewSprint.
// A sprint that ends with a sprint review.
// The scrum master must upload a review summary before closing.
public class ReviewSprint : Sprint
{
    public bool HasReviewSummary { get; private set; }
    public string? ReviewSummaryDocument { get; private set; }

    public ReviewSprint(string name, DateTime startDate, DateTime endDate)
        : base(name, startDate, endDate)
    {
        HasReviewSummary = false;
    }

    public void UploadReviewSummary(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            throw new ArgumentException("Review summary document cannot be empty.");
        ReviewSummaryDocument = document;
        HasReviewSummary = true;
    }
}
