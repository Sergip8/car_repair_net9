  public enum AppointmentStatus
    {
        Scheduled = 1,
        Confirmed = 2,
        InProgress = 3,
        Completed = 4,
        Cancelled = 5,
        NoShow = 6
    }

    public enum WorkOrderStatus
    {
        Created = 1,
        InProgress = 2,
        AwaitingParts = 3,
        AwaitingApproval = 4,
        Completed = 5,
        Cancelled = 6
    }

public enum InvoiceStatus
{
    Draft = 1,
    Sent = 2,
    Paid = 3,
    Overdue = 4,
    Cancelled = 5,
        Pending = 6
    }

    public enum PaymentMethod
    {
        Cash = 1,
        Check = 2,
        CreditCard = 3,
        DebitCard = 4,
        BankTransfer = 5,
        Other = 6
    }