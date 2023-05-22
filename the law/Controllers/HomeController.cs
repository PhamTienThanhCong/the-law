using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using the_law.Models;
using System.Data.SqlClient;

namespace the_law.Controllers
{
    public class HomeController : Controller
    {
        private LawModel db = new LawModel();
        private string connectionString = "Data Source=DESKTOP-FBGLL1R\\MSSQLSERVER01;Initial Catalog=LAW;Integrated Security=True;";

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string mssv, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var selectQuery = "SELECT * FROM users WHERE mssv = @mssv AND password = @password";
                using (var command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@mssv", mssv);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            var username = reader.GetString(reader.GetOrdinal("username"));
                            Session["username"] = username;
                            return RedirectToAction("Index", "Admin"); // Chuyển hướng đến trang chính của ứng dụng
                        }
                    }
                }
            }

            ViewBag.ErrorMessage = "Thông tin đăng nhập không hợp lệ.";
            return View();
        }

        public async Task<ActionResult> About()
        {
            UserModel user = new UserModel();
            var userList = await user.users.ToListAsync();
            IEnumerable<the_law.Models.user> users = userList;

            return View(users);
        }

        // GET: Home
        public async Task<ActionResult> Index(string search)
        {
            var query = db.laws.AsQueryable();
            ViewBag.search = search;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(l => l.ten.Contains(search) || l.noidung.Contains(search));
            }

            var data = await query.ToListAsync();
            return View(data);
        }


        // GET: Home/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            law law = db.laws.First(l => l.id == id);

            return View(law);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
