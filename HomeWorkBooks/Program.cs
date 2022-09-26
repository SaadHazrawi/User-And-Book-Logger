using BookAccessDataBase;
using BookDomin;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HomeWorkBooks
{
    internal class Program
    {
        public static StreamWriter streamWriter { get; set; } = new StreamWriter("logger.txt");
        static void Main(string[] args)
        {
            InsertUser();
            #region addbook
            //using (var context = new BookAccess())
            //{
            //    List<PrintsBook> prints = new List<PrintsBook>
            //    {
            //        new PrintsBook() { LibName = "Big Print" },
            //        new PrintsBook() { LibName = "Big Print" },
            //        new PrintsBook() { LibName = "Big Print" },
            //        new PrintsBook() { LibName = "Big Print" },
            //        new PrintsBook() { LibName = "Big Print" },

            //    };
            //    Book book = new Book()
            //    {
            //        Name = "Learn AI",
            //        PrintsBooks = prints
            //    };
            //    context.Books.Add(book);
            //    context.SaveChanges();
            //}
            #endregion
            #region Showmybook
            //using (var showdata = new BookAccess())
            //{
            //    var databook = showdata.Books
            //        .Include(p => p.PrintsBooks)
            //        .ToList();
            //    foreach (var book in databook)
            //    {
            //        Console.WriteLine("id-->:" + book.Id + " Name-->:" + book.Name);
            //        foreach (var PrintBook in book.PrintsBooks)
            //        {
            //            Console.WriteLine("LibName-->" + PrintBook.LibName);
            //        }
            //    }

            //}
            #endregion
            #region Login

            Console.Write("Enter Your UserName:");
            string username = Console.ReadLine();
            Console.Write("Enter Your Password:");
            string password = Console.ReadLine();
            (bool isCorect, int Count) = CheckDataUser(username, password);
            Console.WriteLine((isCorect ? $"Hello Mr:{username}"
                : "The username Or password is not Found") + "\n" +
                "Count the book in DataBase:" + Count);
            Console.WriteLine("End Insert");

            streamWriter.Write((isCorect ? "The user " + username + " login for system \n"
                : "Not login"));

            #endregion
            #region ListTheCommit
            if (isCorect)
            {
                Console.WriteLine(
                    "Press 1 for Show Dtilas book and for more details for count \n" +
                    "press 2 for show data one book for Choise him\n" +
                    "Press 3 for delete book \n" +
                    "press 4 for Edit Price and address book");
                string inputcommit = Console.ReadLine();
                streamWriter.WriteLine(username + " Press : " + inputcommit + "\n");

                if (inputcommit.Equals("1"))
                {
                    var dataBookalldatabase = ShowBookShortData();
                    foreach (var book in dataBookalldatabase)
                    {
                        var countbook = book.PrintsBooks.Count();
                        Console.WriteLine("Id Book-->:" + book.Id +
                            "| Name Book-->" + book.Name +
                            "| Count for this book in DB:" + countbook);

                    }
                    using (var countthebook = new BookAccess())
                    {
                        Console.WriteLine("The All Books In DataBase is About-->:"
                            + countthebook.Prints.Count());
                    }
                    streamWriter.WriteLine(username + " Press : " + inputcommit + " show all data \n");
                    streamWriter.Close();

                }
                else if (inputcommit.Equals("2"))
                {
                    Console.Write("Enter The ID Book");
                    int id = int.Parse(Console.ReadLine());
                    streamWriter.WriteLine(username + " show the book : " + id + "\n");
                    GetDataOneBook(id);
                    streamWriter.Close();
                }
                else if (inputcommit.Equals("3"))
                {
                    Console.Write("Enter The ID Book for remove ");
                    int id = int.Parse(Console.ReadLine());
                    streamWriter.WriteLine(username + " Delete the book : " + id + "\n");
                    DeleteBook(id);

                    streamWriter.Close();
                }
                else if (inputcommit.Equals("4"))
                {
                    Console.Write("Enter The ID Book for Edit name and price ");
                    int id = int.Parse(Console.ReadLine());
                    streamWriter.WriteLine(username + " Edit the book : " + id);
                    EditBook(id);

                    streamWriter.Close();
                }
                else
                {
                    Console.WriteLine("The Comind is not Found please Enter The Coriect Comind");
                    streamWriter.WriteLine(username + " the prees num is not found in system " + "\n");
                    streamWriter.Close();
                }
            }
            #endregion
        }
        #region Method
        #region Checked data user
        private static (bool, int) CheckDataUser(string username, string password)
        {
            int conutbooks = 0;
            bool Iscorectdata = false;
            using (var checkdatauser = new BookAccess())
            {
                var datafromDB = checkdatauser.Users
                    .Select(d => new { d.UserName, d.Password }).ToList();
                foreach (var user in datafromDB)
                {
                    if (user.UserName.Equals(username) && user.Password.Equals(password))
                    {
                        Iscorectdata = true;
                        conutbooks = checkdatauser.Prints.Count();
                        break;
                    }
                }
            }
            return (Iscorectdata, conutbooks);
        }
        #endregion
        #region InsertUser
        private static void InsertUser()
        {
            using (var checkeduser = new BookAccess())
            {
                var include = checkeduser.Users.Any();
                if (include)
                {
                    Console.WriteLine("The User In Data is Found");
                }
                else
                {
                    string pathfile = File.ReadAllText("user.json");
                    var defuser = JsonSerializer.Deserialize<List<User>>(pathfile);
                    using (var context = new BookAccess())
                    {
                        context.Users.AddRange(defuser);
                        context.SaveChanges();
                    }
                    using (var showuser = new BookAccess())
                    {
                        var ob = showuser.Users
                            .ToList();
                        foreach (var user in ob)
                            Console.WriteLine("Username:" + user.UserName + " password:" + user.Password);
                    }
                }
            }
        }
        #endregion
        #region showDataallBook
        private static List<Book> ShowBookShortData()
        {
            using (var context = new BookAccess())
            {
                return context.Books
                    .Include(pb => pb.PrintsBooks)
                    .ToList();
            }
        }
        #endregion
        #region GetDataOneBook
        private static void GetDataOneBook(int Id)
        {
            using (var context = new BookAccess())
            {
                var IsFound = context.Books.FirstOrDefault(i => i.Id == Id);
                if (IsFound == null)
                {
                    Console.WriteLine("This book is not Found");
                    streamWriter.WriteLine("The Book is not Found in DataBase");
                }
                else
                {
                    var ShowBookFromId = context.Books
                            .Include(pb => pb.PrintsBooks)
                            .Where(i => i.Id == Id)
                            .ToList();
                    foreach (var databook in ShowBookFromId)
                    {
                        var moredata = databook.PrintsBooks.Count();
                        Console.WriteLine("Name This Book ID IS-->:" + databook.Name
                            + " :the count off Clone in data base for this book is: " + moredata);
                    }

                }
            }
        }
        #endregion
        #region Delete
        private static void DeleteBook(int id)
        {
            using (var context = new BookAccess())
            {
                var IsFound = context.Books.FirstOrDefault(i => i.Id == id);
                if (IsFound == null)
                {
                    Console.WriteLine("This book is not Found");
                    streamWriter.WriteLine("The Book is not Found in DataBase");
                }
                else
                {
                    var del = context.Books
                        .Include(pb => pb.PrintsBooks)
                        .Where(i => i.Id == id).FirstOrDefault();
                    context.Books.Remove(del);
                    context.SaveChanges();
                    streamWriter.WriteLine("Corict Delete");
                }
            }
        }

        #endregion
        #region Edit one book
        private static void EditBook(int id){
            
            using (var context = new BookAccess())
            {
                var IsFound = context.Books.FirstOrDefault(i => i.Id == id);
                if (IsFound == null)
                    Console.WriteLine("This book is not Found");
                else
                {
                    foreach (var book in context.Books.Where(i => i.Id == id).ToList())
                    {
                        Console.WriteLine("The name book for this id is" +
                            "--> " + " ( " + book.Name + " ) " + "the Price is :" +
                            "--> " + book.Price);
                        streamWriter.WriteLine("Old name :" + book.Name + "\n Old Price :" + book.Price);
                    }
                    Console.Write("Enter the new name:");
                    string newname = Console.ReadLine();
                    Console.Write("Enter the new Price:");
                    double newprice = Convert.ToDouble(Console.ReadLine());
                    IsFound.Name = newname;
                    IsFound.Price = newprice;
                    context.SaveChanges();
                    streamWriter.WriteLine("new name the book :" + newname + "\n new Price :" + newprice);
                }
            }
        }
        #endregion
        #endregion
    }
}
