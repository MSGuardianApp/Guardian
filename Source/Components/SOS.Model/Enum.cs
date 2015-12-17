namespace SOS.Model
{
    public enum BuddyState
    {
        Active = 1,
        Suspended = 2,
        Blocked = 3,
        Marshal = 4
    }

    public enum DeviceType
    {
        WindowsPhone = 1,
        Andoid = 2,
        iOS = 3
    }

    public enum Command
    {
        DEFAULT = 1,
        BEGIN = 2,
        STOPSOSONLY = 3,
        STOPALL = 4,
        STOP = 5
    }

    public enum ExtendedCommand
    {
        WAIT = 1,
        PROCESS = 2,
        STOP = 3
    }
}
