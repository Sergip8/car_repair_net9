
public class PaginationRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string Query { get; set; } = "";
    public string Direction { get; set; } = "ASC";
    public string Sort { get; set; } = "id";
}


