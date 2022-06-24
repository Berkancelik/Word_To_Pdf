using Microsoft.AspNetCore.Http;

namespace Word_To_Pdf_Producer.Models
{
    public class WordToPdf
    {
        public string Email { get; set; }
        public IFormFile WordFile { get; set; }
    }
}
