using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace applicationList
{
    class Program
    {
        static int del1 = -1, del2 = -1,up1=-1,up2=-1;
        static void Main(string[] args)
        {
            int option = 5;
            int authorOption = 6;
            List<userDetail> s = new List<userDetail>();
            List<freeBookDetail> fb = new List<freeBookDetail>();
            List<paidBookDetail> pb = new List<paidBookDetail>();
            userDetail p = new userDetail();
            readUserData(s);
            readFreeBooksDetail(fb);
            readPaidBooksDetail(pb);
            while (option != 0)
            {
                Console.Clear();
                header();
                option = loginMenu();
                if (option == 1)
                {
                    Console.Clear();
                    header();
                    Console.WriteLine("SIGN-IN MENU");
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Enter your name:");
                    p.name = Console.ReadLine();
                    Console.WriteLine("Enter Your Password:");
                    p.password = Console.ReadLine();
                    string rolechecker = signIn(p.name, p.password, s);
                    if (rolechecker == "reader" || rolechecker == "Reader" || rolechecker == "READER")
                    {
                        Console.WriteLine("Signed-Up successfully READER");
                        Console.WriteLine("Press any key to continue:");
                        Console.ReadKey();
                    }
                    else if (rolechecker == "author" || rolechecker == "Author" || rolechecker == "AUTHOR")
                    {
                        Console.WriteLine("Signed-Up successfully Author");
                        Console.WriteLine("Press any key to continue:");
                        Console.ReadKey();
                        while (authorOption != 0)
                        {
                            Console.Clear();
                            header();
                            authorOption = authorMenu();
                            if (authorOption == 1)
                            {
                                Console.Clear();
                                header();
                                fb.Add(takingInputFree(fb, p.name, pb));
                            }
                            else if (authorOption == 2)
                            {
                                Console.Clear();
                                header();
                                pb.Add(takingInputPaid(pb, p.name, fb));
                            }
                            else if (authorOption == 3)
                            {
                                Console.Clear();
                                header();
                                authorLibrary(p.name, fb, pb);
                                Console.WriteLine("Press any key to continue:");
                                Console.ReadKey();
                            }
                            else if (authorOption == 4)
                            {
                                Console.Clear();
                                header();
                                Console.WriteLine("Enter the name of book you wan't to delete:");
                                string n = Console.ReadLine();
                                bool isValid = deleteBooksChecker(p.name, n, fb, pb);
                                if (isValid == true)
                                {
                                    if (del1 != -1)
                                    {
                                        pb.RemoveAt(del1);
                                        savePaidDetailDelete(pb);
                                    }
                                    if (del2 != -1)
                                    {
                                        fb.RemoveAt(del2);
                                        saveFreeDetailDelete(fb);
                                    }
                                    Console.WriteLine("Book was deleted successfully");
                                    Console.ReadKey();
                                }
                                else if (isValid == false)
                                {
                                    Console.WriteLine("Book does not found");
                                    Console.ReadKey();
                                }
                            }
                            else if (authorOption==5)
                            {
                                Console.Clear();
                                header();
                                Console.WriteLine("Enter the name of the book you want to update:");
                                string n = Console.ReadLine();
                                bool exist = isExists(n,fb,pb);
                                if (exist == true)
                                {
                                    Console.WriteLine("Enter the new price of the book:");
                                    string newPrice = Console.ReadLine();
                                    Console.WriteLine("Enter the new category of the book:");
                                    string newCategory = Console.ReadLine();
                                    bool validCategory = categoryChecker(newCategory);
                                    bool validPrice = priceChecker(newPrice);
                                    if (validCategory == true && (validPrice == true || newPrice=="0"))
                                    {
                                        updatePaid(newPrice, newCategory, n,fb,pb,p.name);
                                        saveFreeDetailDelete(fb);
                                        savePaidDetailDelete(pb);
                                        Console.WriteLine("Book was updated successfully");
                                        Console.WriteLine("Press any key to continue");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid credentials");
                                        Console.WriteLine("Press any key to continue:");
                                        Console.ReadKey();
                                    }

                                }
                            }

                        }
                    }

                }
                else if (option == 2)
                {
                    Console.Clear();
                    header();
                    Console.WriteLine("SIGN-UP MENU");
                    Console.WriteLine("-------------------");
                    s.Add(signUp(s));
                }
            }
            if (option==0)
            {
                return;
            }
        }
        static void header()
        {
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("**   EEEEE     L         III BBBB  RRRR       A     RRRR  Y       Y  **");
            Console.WriteLine("**   E         L          I  B   B R   R     A A    R   R   Y   Y    **");
            Console.WriteLine("**   EEEEE --- L          I  BBB   RRR      A A A   RRR       Y      **");
            Console.WriteLine("**   E         L          I  B   B R  R    A     A  R  R      Y      **");
            Console.WriteLine("**   EEEEE     L L L L L III BBBB  R    R A       A R    R    Y      **");
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("***********************************************************************");
        }
        static int loginMenu()
        {
            int choice = 3;
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("----------------");
            Console.WriteLine("1. Sign-In to E-Library");
            Console.WriteLine("2. Sign-Up to E-Library");
            Console.WriteLine("0. Exit");
            Console.Write("Enter Your Choice:");
            choice = int.Parse(Console.ReadLine());
            return choice;
        }
        static userDetail signUp(List<userDetail> p)
        {

            userDetail s = new userDetail();
            bool running = true;
            while (running)
            {

                Console.WriteLine("Enter Your Name:");
                s.name = Console.ReadLine();
                Console.WriteLine("Enter Your Password:");
                s.password = Console.ReadLine();
                Console.WriteLine("Enter Your Role(Reader or Author):");
                s.role = Console.ReadLine();
                bool isValid = isValidUserName(s.name);
                bool isExists = isUserAlreadyExists(s.name, p);
                bool isRole = isValidRole(s.role);

                if (isValid == true && isExists == true && isRole == true)
                {
                    saveUsersData(s.name, s.password, s.role);
                    //signUp(name, password, role);
                    Console.WriteLine("Signed Up Successfully");
                    Console.WriteLine("Press any key to continue:");

                    Console.ReadKey();
                    running = false;

                }
                else if (isValid == false)
                {
                    Console.WriteLine("Invalid credentials!");
                    Console.WriteLine("Please don't use special character in name");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (isExists == false)
                {
                    Console.WriteLine("User already exists!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
                else if (isRole == false)
                {
                    Console.WriteLine("Invalid Role!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
            }
            return s;

        }
        static bool isValidUserName(string name)
        {
            bool flag = true;
            for (int i = 0; i < name.Length; i++)
            {
                if ((name[i] < 65 && name[i] > 32) || (name[i] > 90 && name[i] < 97) || name[i] > 122 || name[i] < 32)
                {
                    flag = false;
                }
            }
            return flag;
        }
        static bool isValidBookName(string name)
        {
            bool flag = true;
            for (int i = 0; i < name.Length; i++)
            {
                if ((name[i] < 47 && name[i] > 32) || (name[i] > 90 && name[i] < 97) || (name[i] > 57 && name[i] < 65) || name[i] > 122 || name[i] < 32)
                {
                    flag = false;
                }
            }
            return flag;
        }
        static bool isValidRole(string role)
        {
            bool flag = false;
            for (int i = 0; i < role.Length; i++)
            {
                if (role == "reader" || role == "Reader" || role == "READER" || role == "Author" || role == "AUTHOR" || role == "author")
                {
                    flag = true;
                }
            }
            return flag;
        }
        static bool isUserAlreadyExists(string name, List<userDetail> s)
        {
            bool flag = true;
            for (int i = 0; i < s.Count; i++)
            {
                if (name == s[i].name)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        static void saveUsersData(string name, string password, string role)
        {
            string path = "D:\\OOP\\applicationList\\userDetail.txt";
            StreamWriter fileVariable = new StreamWriter(path, true);
            fileVariable.WriteLine("{0},{1},{2}", name, password, role);
            fileVariable.Flush();
            fileVariable.Close();
        }
        static string signIn(string name, string Password, List<userDetail> p)
        {
            string flag = "false";
            for (int i = 0; i < p.Count; i++)
            {
                if (name == p[i].name && Password == p[i].password)
                {
                    flag = p[i].role;
                    break;
                }
            }
            return flag;
        }
        static string parseData(string words, int number)
        {
            int count = 1;
            string line = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == ',')
                {
                    count++;
                }
                else if (count == number)
                {
                    line = line + words[i];
                }
            }
            return line;
        }
        static void readUserData(List<userDetail> s)
        {
            string path = "D:\\OOP\\applicationList\\userDetail.txt";
            string record = "";

            if (File.Exists(path))
            {
                StreamReader fileVariable = new StreamReader(path);
                while ((record = fileVariable.ReadLine()) != null)
                {
                    userDetail info = new userDetail();
                    info.name = parseData(record, 1);
                    info.password = parseData(record, 2);
                    info.role = parseData(record, 3);
                    s.Add(info);
                }
                fileVariable.Close();
            }
            else
            {
                Console.WriteLine("File does not exists");
            }
        }
        static int authorMenu()
        {
            int choice;
            Console.WriteLine("Author Menu");
            Console.WriteLine("-------------------");
            Console.WriteLine("1.To add free books");
            Console.WriteLine("2.To add paid books");
            Console.WriteLine("3.View my Books");
            Console.WriteLine("4.To Delete Books");
            Console.WriteLine("5.To Update price of books");
            Console.WriteLine("0.Exit");
            Console.Write("Enter Your Option:");
            choice = int.Parse(Console.ReadLine());
            return choice;
        }
        static freeBookDetail takingInputFree(List<freeBookDetail> f, string name, List<paidBookDetail> p)
        {
            freeBookDetail s = new freeBookDetail();
            bool running = true;
            while (running)
            {
                Console.WriteLine("Enter Your Book Name:");
                s.bookNameFree = Console.ReadLine();
                Console.WriteLine("Enter Category(Science,History,Relegious or Literature):");
                s.bookCategoryFree = Console.ReadLine();
                bool validCategory = categoryChecker(s.bookCategoryFree);
                bool validName = isValidBookName(s.bookNameFree);
                bool alreadyExists = isBookAlreadyExists(s.bookNameFree, f, p);
                if (validCategory == true && validName == true && alreadyExists == true)
                {
                    s.authorFree = name;
                    saveFreeBooksDetail(name, s.bookNameFree, s.bookCategoryFree);
                    Console.WriteLine("Book added Successfully");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                    running = false;
                }
                else if (validName == false)
                {
                    Console.WriteLine("Invalid Book Name");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
                else if (alreadyExists == false)
                {
                    Console.WriteLine("Book Already Exists");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
                else if (validCategory == false)
                {
                    Console.WriteLine("Invalid Category");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
            }
            return s;
        }
        static paidBookDetail takingInputPaid(List<paidBookDetail> p, string name, List<freeBookDetail> f)
        {
            paidBookDetail s = new paidBookDetail();
            bool running = true;
            while (running)
            {
                Console.WriteLine("Enter Your Book Name:");
                s.bookNamePaid = Console.ReadLine();
                Console.WriteLine("Enter Category(Science,History,Relegious or Literature):");
                s.bookCategoryPaid = Console.ReadLine();
                Console.WriteLine("Enter your book price:");
                string price = Console.ReadLine();
                bool validCategory = categoryChecker(s.bookCategoryPaid);
                bool validName = isValidBookName(s.bookNamePaid);
                bool alreadyExists = isBookAlreadyExists(s.bookNamePaid, f, p);
                bool validPrice = priceChecker(price);
                if (validCategory == true && validName == true && alreadyExists == true && validPrice == true)
                {
                    s.bookPrice = int.Parse(price);
                    s.authorPaid = name;
                    savePaidBooksDetail(name, s.bookNamePaid, s.bookCategoryPaid, s.bookPrice);
                    Console.WriteLine("Book added Successfully");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                    running = false;
                }
                else if (validName == false)
                {
                    Console.WriteLine("Invalid Book Name");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
                else if (alreadyExists == false)
                {
                    Console.WriteLine("Book Already Exists");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
                else if (validCategory == false)
                {
                    Console.WriteLine("Invalid Category");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
                else if (validPrice == false)
                {
                    Console.WriteLine("Invalid Price");
                    Console.WriteLine("Press any key to Continue:");
                    Console.ReadKey();
                }
            }
            return s;

        }
        static bool isBookAlreadyExists(string name, List<freeBookDetail> f, List<paidBookDetail> p)
        {
            bool flag = true;

            for (int i = 0; i < p.Count; i++)
            {
                if (name == p[i].bookNamePaid)
                {
                    flag = false;
                    break;
                }
            }
            if (flag == true)
            {
                for (int i = 0; i < f.Count; i++)
                {
                    if (name == f[i].bookNameFree)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        static bool categoryChecker(string category)
        {
            bool flag = false;
            if (category == "Science" || category == "science" || category == "SCIENCE" || category == "history" ||
             category == "HISTORY" || category == "History" || category == "literature" || category == "Literature"
             || category == "LITERATURE" || category == "RELEGIOUS" || category == "Relegious" || category == "relegious")
            {
                flag = true;
            }
            return flag;
        }
        static void saveFreeBooksDetail(string author, string book, string category)
        {
            string path = "D:\\OOP\\applicationList\\freeBook.txt";
            StreamWriter fileVariable = new StreamWriter(path, true);
            fileVariable.WriteLine("{0},{1},{2}", author, book, category);
            fileVariable.Flush();
            fileVariable.Close();
        }
        static void readFreeBooksDetail(List<freeBookDetail> s)
        {
            string path = "D:\\OOP\\applicationList\\freeBook.txt";
            string record = "";

            if (File.Exists(path))
            {
                StreamReader fileVariable = new StreamReader(path);
                while ((record = fileVariable.ReadLine()) != null)
                {
                    freeBookDetail info = new freeBookDetail();
                    info.authorFree = parseData(record, 1);
                    info.bookNameFree = parseData(record, 2);
                    info.bookCategoryFree = parseData(record, 3);
                    s.Add(info);
                }
                fileVariable.Close();
            }
            else
            {
                Console.WriteLine("File does not exists");
            }

        }
        static bool priceChecker(string price)
        {
            bool flag = true;
            for (int i = 0; i < price.Length; i++)
            {
                if (price[i] > 57 || price[i] < 49)
                {
                    flag = false;
                }
            }
            return flag;
        }
        static void savePaidBooksDetail(string author, string book, string category, int price)
        {
            string path = "D:\\OOP\\applicationList\\paidBook.txt";
            StreamWriter fileVariable = new StreamWriter(path, true);
            fileVariable.WriteLine("{0},{1},{2},{3}", author, book, category, price);
            fileVariable.Flush();
            fileVariable.Close();
        }
        static void readPaidBooksDetail(List<paidBookDetail> s)
        {
            string path = "D:\\OOP\\applicationList\\paidBook.txt";
            string record = "";

            if (File.Exists(path))
            {
                StreamReader fileVariable = new StreamReader(path);
                while ((record = fileVariable.ReadLine()) != null)
                {
                    paidBookDetail info = new paidBookDetail();
                    info.authorPaid = parseData(record, 1);
                    info.bookNamePaid = parseData(record, 2);
                    info.bookCategoryPaid = parseData(record, 3);
                    info.bookPrice = int.Parse(parseData(record, 4));
                    s.Add(info);
                }
                fileVariable.Close();
            }
            else
            {
                Console.WriteLine("File does not exists");
            }

        }
        static void authorLibrary(string name, List<freeBookDetail> f, List<paidBookDetail> p)
        {
            int count = 0;
            Console.WriteLine("------My LIBRARY-----");
            Console.WriteLine("   Book Name\t\tBook Price\t\tBook Category");
            for (int i = 0; i < p.Count; i++)
            {

                if (name == p[i].authorPaid)
                {
                    Console.WriteLine("{0}.{1}\t\t{2}\t\t{3}", count + 1, p[i].bookNamePaid, p[i].bookPrice, p[i].bookCategoryPaid);
                    count++;
                }
            }
            for (int i = 0; i < f.Count; i++)
            {
                if (name == f[i].authorFree)
                {
                    Console.WriteLine("{0}.{1}\t\tFree\t\t{2}", count + 1, f[i].bookNameFree, f[i].bookCategoryFree);
                    count++;
                }
            }
            if (count == 0)
            {
                Console.WriteLine("You did not add any book");
                Console.WriteLine("Press any key to continue:");
                Console.ReadKey();
            }
        }
        static bool deleteBooksChecker(string name, string bookName, List<freeBookDetail> f, List<paidBookDetail> p)
        {
            bool flag = false;
            for (int i = 0; i < p.Count; i++)
            {
                if (name == p[i].authorPaid && bookName == p[i].bookNamePaid)
                {
                    flag = true;
                    del1 = i;
                    break;
                }
            }
            for (int i = 0; i < f.Count; i++)
            {
                if (name == f[i].authorFree && bookName == f[i].bookNameFree)
                {
                    flag = true;
                    del2 = i;
                    break;
                }
            }
            return flag;
        }
        static void savePaidDetailDelete(List<paidBookDetail> p)
        {
            string path = "D:\\OOP\\applicationList\\paidBook.txt";
            StreamWriter fileVariable = new StreamWriter(path, false);
            for (int i = 0; i < p.Count; i++)
            {
                fileVariable.WriteLine("{0},{1},{2},{3}", p[i].authorPaid, p[i].bookNamePaid, p[i].bookCategoryPaid, p[i].bookPrice);
            }

            fileVariable.Flush();
            fileVariable.Close();
        }
        static void saveFreeDetailDelete(List<freeBookDetail>f)
        {
            string path = "D:\\OOP\\applicationList\\freeBook.txt";
            StreamWriter fileVariable = new StreamWriter(path, false);
            for (int i = 0; i < f.Count; i++)
            {
                fileVariable.WriteLine("{0},{1},{2}", f[i].authorFree, f[i].bookNameFree, f[i].bookCategoryFree);
            }

            fileVariable.Flush();
            fileVariable.Close();
        }
        static bool isExists(string name,List<freeBookDetail> f,List<paidBookDetail> p)
        {
            bool flag = false;
            for (int i = 0; i < p.Count; i++)
            {
                if (name == p[i].bookNamePaid)
                {
                    flag = true;
                    up1 = i;
                    break;
                }
            }
            for (int i = 0; i < f.Count; i++)
            {
                if (name == f[i].bookNameFree)
                {
                    flag = true;
                    up2 = i;
                    break;
                }
            }
            return flag;
        }
        static void updatePaid(string newPrice, string newCategory, string name,List<freeBookDetail> f,List<paidBookDetail> p,string author)
        {
            freeBookDetail info = new freeBookDetail();
            paidBookDetail infoPaid = new paidBookDetail();
            int price = int.Parse(newPrice);
            if (up1 != -1)
            {
                if (price == 0)
                {
                    info.bookCategoryFree = newCategory;
                    info.bookNameFree = name;
                    info.authorFree = author;
                    p.RemoveAt(up1);
                    f.Add(info);
                }
                else
                {
                    p[up1].bookPrice = price;
                    p[up1].bookCategoryPaid = newCategory;
                }
            }
           if (up2 != -1)
           {
               if (price > 0)
               {
                    infoPaid.authorPaid = author;
                    infoPaid.bookNamePaid = name;
                    infoPaid.bookCategoryPaid = newCategory;
                    infoPaid.bookPrice = price;
                    f.RemoveAt(up2);
                    p.Add(infoPaid);
               }
               else
               {
                   f[up2].bookCategoryFree = newCategory;
               }
           }

        }

    }
}
