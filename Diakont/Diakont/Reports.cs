//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diakont
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reports
    {
        public System.Guid report_id { get; set; }
        public System.Guid j_id { get; set; }
        public System.Guid d_id { get; set; }
        public System.DateTime date_from { get; set; }
        public System.DateTime date_to { get; set; }
        public decimal monthly_pay { get; set; }
    
        public virtual Departments Departments { get; set; }
        public virtual Jobs Jobs { get; set; }
    }
}