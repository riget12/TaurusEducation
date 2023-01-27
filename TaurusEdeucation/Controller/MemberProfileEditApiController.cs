using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using TaurusEdeucation.config;
using TaurusEdeucation.Models;
using TaurusEdeucation.Types;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace TaurusEdeucation.Controller
{
    [Route("api/MemberProfileEditApi")]
    public class MemberProfileEditApiController : UmbracoApiController
    {
        /// <summary>
        /// vyplní overviewmodel s hodnotami současně připojeného membera
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public MemberProfileEditModel PopulateProfileModel()
        {
            MemberProfileEditModel model = new MemberProfileEditModel();
            var currentMember = Members.GetCurrentMember();

            if (currentMember == null)
            {
                return null;
            }
            model.Id = currentMember.Id.ToString();
            model.FirstName = currentMember.GetProperty("firstName").GetValue().ToString();
            model.SurName = currentMember.GetProperty("surName").GetValue().ToString();
            model.City = currentMember.GetProperty("city").GetValue().ToString();
            model.Street = currentMember.GetProperty("street").GetValue().ToString();
            model.Phone = currentMember.GetProperty("phone").GetValue().ToString();
            model.Email = currentMember.GetProperty("email").GetValue().ToString();


            return model;
        }

        [HttpGet]
        public string GetProfileImage(int lectorId)
        {
            IMemberService memberService = Services.MemberService;
            IMember lector = memberService.GetById(lectorId);

            string imageSrc = lector.GetValue("photoBigImage").ToString();
            return imageSrc;
        }

        [HttpGet]
        public string GetThumbnailImage(int lectorId)
        {
            IMemberService memberService = Services.MemberService;
            IMember lector = memberService.GetById(lectorId);

            object thumbnail = lector.GetValue("photoSmallImage");

            string thumbnailSrc = thumbnail == null ? "" : thumbnail.ToString();
            return thumbnailSrc;
        }

        [HttpPost]
        public ActionResult SaveNewProfileImage()
        {
            try
            {
                int lectorId = int.Parse(System.Web.HttpContext.Current.Request.Params["lectorId"]);
                string thumbnailImage = System.Web.HttpContext.Current.Request.Params["thumbnailProfileImage"];
                string imageName = System.Web.HttpContext.Current.Request.Params["imageName"];
                int mediaRootId = AppSettings.ProfileImagesMediaId;
                string mediaType = Constants.Conventions.MediaTypes.Image;

                thumbnailImage = thumbnailImage.Substring(thumbnailImage.IndexOf("base64,"));
                thumbnailImage = thumbnailImage.Replace("base64,", "");

                byte[] bytes = Convert.FromBase64String(thumbnailImage);
                IMediaService mediaService = Services.MediaService;
                IMedia media = mediaService.CreateMedia(imageName, mediaRootId, mediaType);
                Stream stream = new MemoryStream(bytes);
                media.SetValue(Services.ContentTypeBaseServices, "umbracoFile", imageName, stream);
                mediaService.Save(media);

                int newMediaId = media.Id;

                IMemberService memberService = Services.MemberService;
                IMember lector = memberService.GetById(lectorId);

                string jsonString = media.Properties.First(x => x.Alias == "umbracoFile").Values.First().EditedValue.ToString();
                dynamic data = JObject.Parse(jsonString);
                string src = data.src;

                int oldMediaId = 0;
                if (lector.GetValue("photoImageId") != null)
                {
                    if (int.TryParse(lector.GetValue("photoImageId").ToString(), out oldMediaId))
                    {
                        media = mediaService.GetById(oldMediaId);
                        mediaService.Delete(media);
                    }
                }

                lector.SetValue("photoSmallImage", src);
                lector.SetValue("photoImageId", newMediaId);

                memberService.Save(lector);
            }
            catch (Exception ex)
            {
                Logger.Error(GetType(), ex);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Create()
        {
            string result = "";

            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                try
                {
                    System.Web.HttpPostedFile image = System.Web.HttpContext.Current.Request.Files["file"];
                    string lectorId = System.Web.HttpContext.Current.Request.Params["lectorId"];
                    string filenameWithoutExtension = $"{lectorId}_{Path.GetFileNameWithoutExtension(image.FileName)}";
                    string extension = Path.GetExtension(image.FileName);

                    string[] allowedExtensions = new[] { ".png", ".jpg", ".jpeg", "tiff", "tif", "bmp" };

                    if (!allowedExtensions.Contains(extension))
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed, "Nepodporovaná příloha");
                    }

                    string filename = $"{filenameWithoutExtension}{extension}";

                    using (var srcImage = Image.FromStream(image.InputStream))
                    using (var newImage = new Bitmap(100, 100))
                    using (var graphics = Graphics.FromImage(newImage))
                    using (var stream = new MemoryStream())
                    {
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new Rectangle(0, 0, 100, 100));
                        newImage.Save(stream, ImageFormat.Png);
                        string thumbnailName = $"{filenameWithoutExtension}-thumbnail{Path.GetExtension(filename)}";

                        byte[] byte_image = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(byte_image, 0, byte_image.Length);

                        result = $"{{\"ImageData\":\"{Convert.ToBase64String(byte_image)}\",\"Extension\":\"{extension}\",\"ImageName\":\"{thumbnailName}\"}}";
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error(GetType(), ex);
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
                }
            }

            return new JsonHttpStatusResult(result, System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public void HideMember(int lectorId, bool hide)
        {
            IMemberService memberService = Services.MemberService;
            IMember lector = memberService.GetById(lectorId);

            lector.SetValue("isHidden", hide);
        }

        [HttpGet]
        public JsonResult CheckPoznamka()
        {
            var currentMember = Members.GetCurrentMember();

            var poznamka = currentMember.GetProperty("poznamka").GetValue().ToString();
            if (String.IsNullOrEmpty(poznamka))
            {
                return null;
            }

            bool isReaded = (bool)currentMember.GetProperty("poznamkaIsRead").GetValue();

            if (isReaded)
            {
                return null;
            }

            return new JsonResult()
            {
                Data = JsonConvert.SerializeObject(poznamka),
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        public void ReadPoznamka()
        {
            var currentMember = Members.GetCurrentMember();
            var id = currentMember.Id;

            IMemberService memberService = Services.MemberService;
            IMember lector = memberService.GetById(id);

            lector.SetValue("poznamkaIsRead", true);

            memberService.Save(lector);
        }

        [HttpPost]
        public void DeleteLector()
        {
            var currentMember = Members.GetCurrentMember();
            var id = currentMember.Id;

            IMemberService memberService = Services.MemberService;
            IMember lector = memberService.GetById(id);

            string email = lector.Email;

            memberService.Delete(lector);

            using (var con = new SqlConnection(ConnectionStrings.UmbracoDbDSN))
            {
                con.Open();

                string sql = "insert into LectoDeletedList (Email) values (@email)";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.Add(new SqlParameter("@email", email));

                command.ExecuteNonQuery();
            }

            FormsAuthentication.SignOut();
            return;
        }
    }
}