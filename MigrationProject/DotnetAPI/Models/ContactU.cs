using System;

namespace DotnetAPI.Models
{
    public partial class ContactU
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string content { get; set; }
    }
}
