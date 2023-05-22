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

    public class AdminController : Controller
    {
        private LawModel db = new LawModel();
        private string connectionString = "Data Source=DESKTOP-FBGLL1R\\MSSQLSERVER01;Initial Catalog=LAW;Integrated Security=True;";

        // GET: Home
        public async Task<ActionResult> Index(string search)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.username = Session["username"];
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
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.username = Session["username"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            law law = db.laws.First(l => l.id == id);
            return View(law);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.username = Session["username"];
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "dieu,ten,noidung")] law law)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Lấy id cuối cùng
                    var lastIdQuery = "SELECT TOP 1 id FROM laws ORDER BY id DESC";
                    using (var lastIdCommand = new SqlCommand(lastIdQuery, connection))
                    {
                        int? lastId = await lastIdCommand.ExecuteScalarAsync() as int?;
                        law.id = (lastId.HasValue ? lastId.Value : 0) + 1;
                    }

                    // Thêm dữ liệu vào bảng laws
                    var insertQuery = "INSERT INTO laws (id, dieu, ten, noidung) VALUES (@id, @dieu, @ten, @noidung)";
                    using (var insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@id", law.id);
                        insertCommand.Parameters.AddWithValue("@dieu", law.dieu);
                        insertCommand.Parameters.AddWithValue("@ten", law.ten);
                        insertCommand.Parameters.AddWithValue("@noidung", law.noidung);
                        await insertCommand.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction("Details", new { id = law.id });
            }

            return View(law);
        }


        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.username = Session["username"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            law law = db.laws.First(l => l.id == id);
            
            return View(law);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,dieu,ten,noidung")] law law)
        {
            if (ModelState.IsValid)
            {
                db.Entry(law).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = law.id });

            }
            return View(law);
        }

        // GET: Home/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.username = Session["username"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            law law = await db.laws.FindAsync(id);
            if (law == null)
            {
                return HttpNotFound();
            }
            return View(law);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            law law = await db.laws.FindAsync(id);
            db.laws.Remove(law);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
