using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Developer_Test_Task_v4._6_by_Leo_Software.Models
{
    public class Appointment
    {
        public string AppointmentUid { get; set; }  
        public DateTime BookedFrom { get; set; }    
        public DateTime BookedTill { get; set; }    
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }     
        public string CustomerName { get; set; }    
        public string CustomerPhone { get; set; }   
    }


}