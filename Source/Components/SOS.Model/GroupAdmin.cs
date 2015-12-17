using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOS.Model
{
    [Serializable]
    public class GroupAdmin 
    {
        public int GroupAdminID { get; set; }

        [MaxLength(50)]
        public string GroupIDCSV { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(4000)]
        public string MobileNumber { get; set; }

        [MaxLength(4000)]
        public string LiveAuthID { get; set; }

        [MaxLength(4000)]
        public string LiveUserID { get; set; }

    }
}
