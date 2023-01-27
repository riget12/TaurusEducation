using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web.Http;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace TaurusEdeucation.Controller
{
    /// <summary>
    /// Controller pro stránku s podmínkami
    /// REST architektura
    /// </summary>
    [Route("api/conditions")]
    public class ConditionsController : UmbracoApiController
    {
        /// <summary>
        /// Stáhnutí podmínek jako PDF
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public HttpResponseMessage SaveConditionsAsPdf()
        {
            IPublishedContent page = Umbraco.Content(Guid.Parse("5a2916d7-fb97-420f-a3d5-8c4457f98bdf"));
            string conditions = page.Value<string>("conditions");

            Regex regex = new Regex("<[^>]*>");
            conditions = regex.Replace(conditions, "");

            byte[] bytes = null;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StringReader sr = new StringReader(conditions))
                {
                    using (PdfDocument pdf = new PdfDocument())
                    {
                        PdfPage pdfPage = pdf.AddPage();
                        XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                        XTextFormatter tf = new XTextFormatter(graph);
                        XFont font = new XFont("Verdana", 10, XFontStyle.Regular);
                        string line = String.Empty;

                        line = conditions;
                        tf.DrawString(line, font, XBrushes.Black, new XRect(40, 40, pdfPage.Width.Point - 80, pdfPage.Height.Point), XStringFormats.TopLeft);

                        pdf.Save(stream);
                    }
                }

                bytes = stream.ToArray();
            }

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "Vseobecne-podminky.pdf";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }

        /// <summary>
        /// TODO - co to je?
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public string GetConditions()
        {
            IPublishedContent page = Umbraco.Content(Guid.Parse("56f0e1b3-a9ac-405e-92ea-95d1bc2bd4f3"));
            string conditions = page.Value<string>("conditions");

            return conditions;
        }
    }
}