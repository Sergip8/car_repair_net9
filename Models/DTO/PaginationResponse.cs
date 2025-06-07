public record PaginationResponse<T>(
    List<T>? Content = null,
    int Page = 0,
    int Size = 10,
    int TotalPages = 1,
    long TotalElements = 1,
    bool Last = false
);