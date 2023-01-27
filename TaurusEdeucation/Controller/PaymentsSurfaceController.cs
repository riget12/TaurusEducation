using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services;
using TaurusEdeucation.Models;
using Umbraco.Web.Mvc;

namespace TaurusEdeucation.Controller
{
  public class PaymentsSurfaceController : SurfaceController
  {
    [HttpGet]
    [ActionName("Payments")]
    public ActionResult PaymentsGet()
    {
      var currentMemberEmail = Members.GetCurrentMember().GetProperty("email").GetValue().ToString();
      var currentLector = Services.MemberService.GetByEmail(currentMemberEmail);
      var data = currentLector.GetValue("tableLessons") == null ? "{}" : currentLector.GetValue("tableLessons").ToString();
      PaymentsModel model = new PaymentsModel();
      model.manipulations = new JSON.Manipulations(data);
      model.pricesDays = model.manipulations.GetPayments(DateTime.Now.Year, DateTime.Now.Month - 1);
      model.year = DateTime.Now.Year;
      model.month = DateTime.Now.Month - 1;

      return PartialView("Payments", model);
    }

    [HttpPost]
    [ActionName("Payments")]
    public ActionResult PaymentsPost(PaymentsModel model)
    {
      SendPaymentsEmail(model);

      return RedirectToCurrentUmbracoPage();
    }

    private void SendPaymentsEmail(PaymentsModel model)
    {
      var lector = Members.GetCurrentMember();
      long amount = new JSON.Manipulations(lector.GetProperty("tableLessons").GetValue().ToString()).GetPaymentsAmount(model.year, model.month);

      Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
      {
        { "amount", amount.ToString() },
        { "account", "DOPLNIT ÚČET" },
        { "dueDate", "DOPLNIT SPLATNOST" },
        { "variableSymbol", "DOPLNIT VARIABILNÍ SYMBOL" }
      };

      string htmlBody = new Email_Sender.Emails().GetEmailHTMLBody("TaurusEdeucation.EmailSender.EmailsHtml.Payments.html", keyValuePairs);

      _ = new Email_Sender.EmailSender(Services.MemberService.GetById(lector.Id).Email, "Informace k platbě", htmlBody);
    }
  }
}