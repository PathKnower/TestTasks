using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask2
{
    public class FileModel
    {
        public string FileVersion { get; set; }

        [Key]
        public string Name { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
