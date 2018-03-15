using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class ToDoManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void EndToDo(int? kanryoToDoId)
        {
            if (kanryoToDoId != null)
            {
                ToDo ToDoData = db.ToDos.Find(kanryoToDoId);
                ToDoData.KanryoDate = DateTime.Now;
                db.Entry(ToDoData).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void ReturnToDo(int? returnToDoId)
        {
            if (returnToDoId != null)
            {
                ToDo ToDoData = db.ToDos.Find(returnToDoId);
                ToDoData.KanryoDate = null;
                db.Entry(ToDoData).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<MikanryoToDoView> GetToDoList()
        {
            var toDo = (from a in db.ToDos
                        from b in db.Users.Where(x => x.Id == a.TantoshaId).DefaultIfEmpty()
                        where a.KanryoDate == null
                        orderby a.ShimekiriDate
                        select new MikanryoToDoView
                        {
                            Id = a.Id,
                            Title = a.Title,
                            TantoshaName = b.AccountName,
                            ShimekiriDate = a.ShimekiriDate
                        });

            return toDo.ToList();
        }

        public List<KanryoToDoView> GetKanryoToDoList(DateTime minDate)
        {
            var toDo = (from a in db.ToDos
                        from b in db.Users.Where(x => x.Id == a.TantoshaId).DefaultIfEmpty()
                        where a.KanryoDate != null
                        where a.KanryoDate.Value >= minDate
                        orderby a.KanryoDate descending
                        select new KanryoToDoView
                        {
                            Id = a.Id,
                            Title = a.Title,
                            TantoshaName = b.AccountName,
                            KanryoDate = a.KanryoDate
                        });

            return toDo.ToList();
        }

    }
}