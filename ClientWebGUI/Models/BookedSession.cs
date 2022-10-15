namespace ClientWebGUI.Models
{
    public class BookedSession
    {
        public int Id;
        public DateTime EndDate;
        public string? ClientName;
        public int CentreId;
        public Centre? Centre;
    }
}
