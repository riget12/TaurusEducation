using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models
{
    public class ModalDialogModel
    {
        public ModalDialogButtons ModalDialogButtons { get; set; }
        public string ButtonText { get; set; }
    }

    public enum ModalDialogButtons
    {
        Ok
    }
}