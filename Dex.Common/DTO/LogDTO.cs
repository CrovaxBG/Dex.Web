namespace Dex.Common.DTO
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    public class LogDTO
    {
        public String Message
        {
            get;
            set;
        }

        public String StackTrace
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Int32 Id
        {
            get;
            set;
        }
    }
}