//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Job.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Experience
    {
        public decimal ExpId { get; set; }
        public string CompName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string PozitionName { get; set; }
        public string ExpDetail { get; set; }
        public Nullable<decimal> CvID { get; set; }
        public Nullable<decimal> UserId { get; set; }
    
        public virtual User User { get; set; }
        public virtual CV CV { get; set; }
    }
}
