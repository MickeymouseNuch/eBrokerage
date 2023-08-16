using Inspinia_MVC5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UVG_Main.Models.EMSG
{
    public class cEMSG
    {
        public class cGetContWork
        {
            public int CountWork { get; set; }
            public int CountAllWork { get; set; }
            public Decimal Percent
            {
                get
                {
                    return (CountWork * 100.00 / CountAllWork).ToString().ToDecimal();

                }
            }

        }

        public class cStackedWorkStatus
        {
            public string Display { get; set; }
            public int Darft { get; set; }
            public int เปิดงาน { get; set; }
            public int รับเรื่อง { get; set; }
            public int ไม่สำเร็จ { get; set; }
            public int จบงาน { get; set; }
            public int ยกเลิก { get; set; }
        }

        public class cStackedWorkType
        {
            public string Display { get; set; }
            public int รับเช็ค { get; set; }
            public int วางบิล { get; set; }
            public int จัดส่งเอกสาร { get; set; }
            public int รับเอกสาร { get; set; }
            public int อื่นๆ { get; set; }
        }

        public class cSummaryWorkAll
        {
            public string DocumentStatusTH { get; set; }
            public int DocumentStatusID { get; set; }
            public DateTime WorkDate { get; set; }
            public int CntWork { get; set; }
          
        }

        public class cGetTop10
        {
            public string Display { get; set; }
            public int CountWork { get; set; }
          
        }
    }
}