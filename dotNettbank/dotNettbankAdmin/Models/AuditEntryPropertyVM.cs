using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNettbankAdmin.Models
{
    public class AuditEntryPropertyVM
    {
        public DateTime Date { get; set; }
        public string EntityName { get; set; }
        private string _stateName;
        public string State
        {
            get
            {
                return _stateName;
            }
            set
            {
                switch (value)
                {
                    case "EntityAdded":
                        _stateName = "Insert";
                        break;
                    case "EntityDeleted":
                        _stateName = "Delete";
                        break;
                    case "EntityModified":
                        _stateName = "Update";
                        break;
                    default:
                        break;
                }
            }
        }

        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

    }
}