namespace SOS.Service.Interfaces
{
    public enum TokenTypes
    {
        Invalid,
        Auth,
        Track,
        SOS
    }

    public enum SubscribeBuddyActionType
    { 
        UnSubscribe,
        Subscribe,
        Block
    }

    public enum BuddyState
    {
        Active = 1,
        Suspended = 2,
        Blocked = 3,
        Marshal = 4
    }
}
