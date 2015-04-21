using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    public class CardExpiry
    {
        private int _month;

        public int Month
        {
            get
            {
                return this._month;
            }

            set
            {
                if (value < 1 || value > 12)
                {
                    throw new ArgumentException("Invalid month", "ExpiryDate");
                }

                this._month = value;
            }
        }

        private int _year;

        public int Year
        {
            get
            {
                return this._year;
            }

            set
            {
                if (value < DateTime.Now.Year)
                {
                    throw new ArgumentException("Invalid year", "ExpiryDate");
                }

                this._year = value;
            }
        }
    }
}
