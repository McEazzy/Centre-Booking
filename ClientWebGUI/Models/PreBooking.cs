using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClientWebGUI.Models
{
    public class PreBooking
    {
        public int Id { get; set; }

        [UIHint("BookDate")]
        public DateTime EndDate { get; set; }
        public string? ClientName { get; set; }
        public int CentreId { get; set; }
    }
}
