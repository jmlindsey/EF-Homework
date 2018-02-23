using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_Homework.Models;

namespace EF_Homework
{
    class Program
    {
        static void Main(string[] args)
        {
            void Print<T>(IEnumerable<T> things)
            {
                foreach (T thing in things) Console.Write(thing + " ");
                Console.WriteLine();
            }

            EfExampleEntities db = new EfExampleEntities();
            using (db)
            {
                //Print the names of all the students.
                Print(db.Students.Select(x => x.Name));

                //Print the names of all the courses.
                Print(db.Courses.Select(x => x.Name));

                //Print the number of courses that have no students enrolled.
                Console.WriteLine(db.Courses.Count(x => x.Students.Count() == 0));

                //Print the names of all the teachers.
                Print(db.Staff.Where(x => x.Teacher != null).Select(x => x.Name));

                //Remove "Mike Teacher" from the database.

                var mike = db.Teachers.First(x => x.Staff.Name == "Mike Teacher");
                mike.Courses.Select((x) => x.Teacher = null);
                db.Entry(mike.Staff).State = System.Data.Entity.EntityState.Deleted;
                db.Entry(mike).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                //Print the names of all the teachers again.
                Print(db.Staff.Where(x => x.Teacher != null).Select(x => x.Name));

                //Give every teacher a 1.5 % raise.
                var res = db.Staff.Where(x => x.Teacher != null);
                var cours = db.Courses;
                foreach (Staff staff in res)
                {
                    staff.Salary = staff.Salary + (int)(0.015 * staff.Salary);
                    db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();

                Print(db.Staff.Where(x => x.Teacher != null).Select(x => x.Salary));

                //Give every staff member who is NOT a teacher a 30 % raise.
                var res2 = db.Staff.Where(x => x.Teacher == null);

                foreach (Staff staff in res2)
                {
                    staff.Salary = staff.Salary + (int)(0.3 * staff.Salary);
                    db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                Print(db.Staff.Where(x => x.Teacher == null).Select(x => x.Salary));

                //Take John Student out of the HIST101 class.
                var john = db.Students.Single(x => x.Name == "John Student");
                var course = db.Courses.Single(x => x.Name == "Hist101");
                course.Students.Remove(john);
                john.Courses.Remove(course);
                db.Entry(john).State = System.Data.Entity.EntityState.Modified;
                db.Entry(course).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Print(john.Courses.Select(x => x.Name));

                //EXTRA(OPTIONAL) : Make a new console application and a new database.Using Entity Framework Code First, recreate the same tables and foreign key constraints that EfExample has.




                db.Dispose();
            }
            Console.ReadKey(true);
        }
    }
}
