using Is.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Domain.Email
{
    public class EmailMessage : BaseEntity
    {
        public string? MailTo { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public Boolean status { get; set; }
    }
}
