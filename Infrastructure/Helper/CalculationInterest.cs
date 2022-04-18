using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Enums;

namespace Infrastructure.Helper
{
    public static class Helper
    {
        public static decimal CaculationInterest(decimal amount, InterestType type, DateTime fromDate, float interestRate = 0, decimal InterestAmount = 0){
            decimal res = 0;
            DateTime now = DateTime.Now;
            System.TimeSpan diffResult = now.Subtract(fromDate); 
            if(type == InterestType.Daily){
                res = InterestAmount*diffResult.Days;
            }else if(type == InterestType.DayPerMilion){
                var c = amount/1000000;
                res = c*InterestAmount*diffResult.Days;
            }else if(type == InterestType.Yearly){
                res = amount*((decimal)interestRate/100)*(diffResult.Days/30);
            }
            return res;
        }
    }
}
