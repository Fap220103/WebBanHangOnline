using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(string Searchtext,int? page)
        {
            var pageSize = 5;
            if(page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = db.News.OrderByDescending(p => p.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x=>x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex,pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                model.CategoryID = 3;
                db.News.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var items = db.News.Find(id);
            return View(items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
               
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                db.News.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.News.Find(id);
            if(item != null)
            {
                db.News.Remove(item);
                db.SaveChanges();
                return Json(new {success=true});
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true,isActive= item.IsActive });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult deleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if(items!=null && items.Any())
                {
                    foreach(var item in items)
                    {
                        var obj = db.News.Find(Convert.ToInt32(item));
                        db.News.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }     
            return Json(new { success = false });
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public void ExportExcel_EPPLUS()
        {

            var list = db.News.ToList();
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet Sheet = ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Tên";
            Sheet.Cells["B1"].Value = "Mô tả";
            Sheet.Cells["C1"].Value = "Chi tiet";
            Sheet.Cells["D1"].Value = "Ảnh";
            Sheet.Cells["E1"].Value = "Ngay tao";
            Sheet.Cells["F1"].Value = "Ngay sua";

            int row = 2;// dòng bắt đầu ghi dữ liệu
            foreach (var item in list)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Title;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Description;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Detail;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Image;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.CreatedDate.ToString("dd/MM/yyyy");
                Sheet.Cells[string.Format("F{0}", row)].Value = item.ModifiedDate.ToString("dd/MM/yyyy");
                
                row++;
            }
            Sheet.Cells["A:G"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Report.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
        }
        [HttpPost]
        public ActionResult ImportExcel_EPPLUS()
        {
            var fileUpload = Request.Files["ExcelFile"];
            if (fileUpload != null && fileUpload.ContentLength > 0 && Path.GetExtension(fileUpload.FileName) == ".xlsx")
            {
                List<News> newList = new List<News>();

                // Đọc dữ liệu từ tệp Excel
                using (var package = new ExcelPackage(fileUpload.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Đọc dữ liệu từ mỗi hàng và thêm tin tức vào danh sách tin tức
                        var news = new News
                        {
                            Title = worksheet.Cells[row, 1].Value?.ToString(),
                            Description = worksheet.Cells[row, 2].Value?.ToString(),
                            Detail = worksheet.Cells[row, 3].Value?.ToString(),
                            Image = worksheet.Cells[row, 4].Value?.ToString(),
                            CreatedDate = DateTime.ParseExact(worksheet.Cells[row, 5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ModifiedDate = DateTime.ParseExact(worksheet.Cells[row, 6].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            // Đọc các trường dữ liệu khác tương tự
                            CategoryID = 3,
                        };

                        newList.Add(news);
                    }
                }

                // Thêm dữ liệu vào cơ sở dữ liệu
                db.News.AddRange(newList);
                db.SaveChanges();

                // Hiển thị thông báo cho người dùng
                return Json(new { Success = true, message = "Thêm thành công" });
            }
            else
            {
                // Hiển thị thông báo lỗi nếu người dùng không tải lên tệp Excel hoặc tệp không đúng định dạng
                return Json(new { Success = false, message = "Thêm thất bại" });
            }
        }


    }
}