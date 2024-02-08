using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace Univercity_Dashboard
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Actions 

            Action PrintDateTimeAndUsersStatistic = () =>
            {
                Console.Clear();
                PersianCalendar pc = new PersianCalendar();
                Console.WriteLine($"{pc.GetYear(DateTime.Now)}/{pc.GetMonth(DateTime.Now)}/{pc.GetDayOfMonth(DateTime.Now)}  {pc.GetHour(DateTime.Now)}:{pc.GetMinute(DateTime.Now)}");
                using (UnivercityContext dbPrintDateTime = new UnivercityContext("UniDBConStr"))
                {
                    Console.WriteLine($"\nDashboard Statistic\tEmployee: {dbPrintDateTime.Employees.Count()}\t    Master: {dbPrintDateTime.Masters.Count()}\t   Student: {dbPrintDateTime.Students.Count()}\t    Course: {dbPrintDateTime.Courses.Count()}");
                }
                Console.WriteLine("_________________________________________________________________________________________");
            };
            Action PrintJustEnglish = () =>
            {
                PrintDateTimeAndUsersStatistic();
                Console.WriteLine("\n\tWarning!");
                Thread.Sleep(1000);
                Console.WriteLine("\tOnly type English bro <3");
                Thread.Sleep(3000);
            };

            Action<string> PrintWarningJustNumber = delegate (string Title)
            {
                PrintDateTimeAndUsersStatistic();
                Console.WriteLine("\n\tWarning!");
                Thread.Sleep(1000);
                Console.WriteLine($"\tOnly numbers are approved for {Title}!");
                Thread.Sleep(3000);
            };

            Action<string, int> PrintWarningNotFoundId = delegate (string TitleList, int id)
             {
                 PrintDateTimeAndUsersStatistic();
                 Console.WriteLine("\n\tWarning!");
                 Thread.Sleep(1000);
                 Console.WriteLine($"\tNot Found id[{id}] in {TitleList}!");
                 Thread.Sleep(3000);
             };

            #endregion

            #region Creating DataBase & Defulte item

            Console.Write("\n\n\tDatabase is loadnig...");

            using (UnivercityContext db = new UnivercityContext("UniDBConStr"))
            {
                db.Database.CreateIfNotExists();
                Console.WriteLine();
                if (!db.Roles.Any())
                {
                    db.Roles.Add(new Role(0, "manager", "admin"));
                    db.Roles.Add(new Role(1, "operator", "employee"));
                    db.Roles.Add(new Role(2, "teacher", "master"));
                    db.Roles.Add(new Role(3, "daneshjo", "student"));
                }
                if (!db.Employees.Any())
                {
                    db.Employees.Add(new Employee("admin", 7000000, "afaz", "fallah", "09901768593", "1234", db.Roles.Find(1)));
                }
                if (!db.Courses.Any())
                {
                    db.Courses.Add(new Course("C sharp", 4, new Master("it", 3000000, "nima", "zare", "09131231234", "1234", db.Roles.Find(2))));
                    db.Courses.Add(new Course("MS SQL", 6, db.Masters.Add(new Master("it", 9000000, "yasin", "abedini", "09101231234", "1234", db.Roles.Find(2)))));
                    Console.Clear();
                    Console.Write("\n\n\tDatabase created successfully!");
                }
                if (!db.Students.Any())
                {
                    db.Students.Add(new Student("it", "zahra", "mirhosseini", "09131231254", "1234", db.Roles.Find(3)));
                    db.Students.Add(new Student("it", "hamide", "dehghani", "09132557340", "1234", db.Roles.Find(3)));

                    Student student = new Student("it", "ali", "raftari", "09120012345", "009988", db.Roles.Find(3));
                    Master master = new Master("it", 7100000, "arman", "mashallahi", "09129004567", "164797", db.Roles.Find(3));
                    Course course = new Course("English", 3, master);
                    student.Courses = new List<Course> { course };
                    db.Students.Add(student);
                }
                db.SaveChanges();
                Console.WriteLine("\n\tRedirecting to login form");
                Console.Write("\tPlease wait ...");
                Thread.Sleep(4000);
            }
        #endregion

        #region login form

        Login:
            Console.Clear();
            Console.WriteLine("\nLogin page");
            Console.Write("\n\tPhonenumber  : ");
            string username = Console.ReadLine();
            string patternPhonenumber = @"^09[0-9]{9}$";
            while (!Regex.IsMatch(username, patternPhonenumber))
            {
                Console.WriteLine("\n\tIncorrect phonenumber !");
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("\nLogin page");
                Console.Write("\n\tPhonenumber  : ");
                username = Console.ReadLine();
            }
            Console.Write("\tPassword     : ");
            string password = Console.ReadLine();

            string adminName = "";

            using (UnivercityContext dbLogin = new UnivercityContext("UniDBConStr"))
            {
                var admin = dbLogin.Employees.FirstOrDefault(t => t.Phonenumber == username && t.Password == password);
                if (admin == null)
                {
                    Console.Clear();
                    Console.WriteLine("\nLogin page");
                    Console.WriteLine("\n\tIncorrect username or password");
                    Thread.Sleep(2000);
                    Console.Clear();
                    goto Login;
                }
                else if (admin.IsActiv == false)
                {
                    Console.Clear();
                    Console.WriteLine("\nLogin page");
                    Console.WriteLine("\n\tYour access is blocked");
                    Thread.Sleep(2000);
                    Console.Clear();
                    goto Login;
                }
                else
                {
                    adminName = admin.Name + " " + admin.Family;
                    goto menu;
                }
            }
        #endregion

        #region menu item

        menu:
            PrintDateTimeAndUsersStatistic();
            Console.Write("\n\t1.  View Employees List");
            Console.WriteLine("\t\t\t2.  View Students List");
            Console.Write("\t3.  View Masters   List");
            Console.WriteLine("\t\t\t4.  View Courses  List\n");

            Console.Write("\t5.  Add New Employee");
            Console.WriteLine("\t\t\t6.  Add New Student");
            Console.Write("\t7.  Add New Master");
            Console.WriteLine("\t\t\t8.  Add New Course\n");

            Console.Write("\t9.  Edit Employee Information");
            Console.WriteLine("\t\t10. Edit Student Information");
            Console.Write("\t11. Edit Master Information");
            Console.WriteLine("\t\t12. Edit Course Information");

            Console.Write("\n\t13. Remove Employee");
            Console.WriteLine("\t\t\t14. Remove Student");
            Console.Write("\t15. Remove Master");
            Console.WriteLine("\t\t\t16. Remove Course");
            Console.Write("\n\t17. Change Role users");
            Console.WriteLine("\t\t\t18. Exit The Dashboard");
            Console.Write("\n\t19. Change the background color");


            Console.Write("\n\nYour request number: ");
            int request;
            while (!int.TryParse(Console.ReadLine(), out request))
            {
                PrintWarningJustNumber("Request number");
                Console.Clear();
                PrintDateTimeAndUsersStatistic();
                goto menu;
            }
            #endregion

            switch (request)
            {
                #region veiw Employee List

                case 1:
                    PrintDateTimeAndUsersStatistic();
                    using (UnivercityContext dbViewEmployeeList = new UnivercityContext("UniDBConStr"))
                    {
                        Console.WriteLine("\nEmployee:\n");

                        List<Employee> viewEmployeeList = dbViewEmployeeList.Employees.ToList();

                        foreach (Employee employee in viewEmployeeList)
                        {
                            Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                              $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                            Console.WriteLine("\t-------------------------------------------------------------------");
                        }
                    }
                    Console.Write("\nDo you want to return to menu?");
                    Console.ReadKey();
                    goto menu;
                #endregion

                #region veiw Student List

                case 2:
                    PrintDateTimeAndUsersStatistic();
                    using (UnivercityContext dbViewStudentList = new UnivercityContext("UniDBConStr"))
                    {
                        Console.WriteLine("\nStudent:\n");
                        IEnumerable<Student> studentList = dbViewStudentList.Students.ToList();

                        foreach (Student student in studentList)
                        {
                            Console.WriteLine($"\tid: {student.UserId}\tcode: {student.StudentCode}\tname: {student.Name}\tfamily: {student.Family}\n" +
                                              $"\tphone:  {student.Phonenumber}\tdegree: {student.Degree}\tactive: {student.IsActiv}");

                            if (student.Courses.Any())
                            {
                                Console.WriteLine("\n\tCourse:");
                                foreach (Course course in student.Courses)
                                {
                                    Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t\tactive: {course.IsActive}");
                                }
                            }
                            Console.WriteLine("\t----------------------------------------------------------------");
                        }
                    }
                    Console.Write("\nDo you want to return to menu?");
                    Console.ReadKey();
                    goto menu;
                #endregion

                #region veiw Master List

                case 3:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbViewMasterList = new UnivercityContext("UniDBConStr");
                    using (dbViewMasterList)
                    {
                        Console.WriteLine("\nMaster:\n");
                        List<Master> masterList = dbViewMasterList.Masters.ToList();
                        foreach (Master master in masterList)
                        {
                            Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                              $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                            List<Course> courseMaster = dbViewMasterList.Courses.Where(t => t.Master.UserId == master.UserId).ToList();
                            for (int i = 0; i < courseMaster.Count; i++)
                            {
                                if (i == 0)
                                {
                                    Console.WriteLine("\n\tCourse:");
                                }
                                Console.WriteLine($"\tid: {courseMaster[i].CourseId}\tname: {courseMaster[i].CourseName}\tunit: {courseMaster[i].CourseUnit}\t\t\tactive: {courseMaster[i].IsActive}");
                            }
                            Console.WriteLine("\t----------------------------------------------------------------");
                        }
                    }
                    Console.Write("\nDo you want to return to menu?");
                    Console.ReadKey();
                    goto menu;
                #endregion

                #region veiw Course List

                case 4:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbViewCourseList = new UnivercityContext("UniDBConStr");
                    using (dbViewCourseList)
                    {
                        Console.WriteLine("\nCourse:\n");
                        List<Course> courseList = dbViewCourseList.Courses.ToList();

                        foreach (Course course in courseList)
                        {
                            Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                            if (course.Master != null)
                            {
                                Console.WriteLine("\tMaster:");
                                Console.WriteLine($"\tid: {course.Master.UserId}\tname: {course.Master.Name}\tfamily: {course.Master.Family}\t     active: {course.Master.IsActiv}");
                            }
                            Console.WriteLine("\t----------------------------------------------------------------");
                        }
                    }
                    Console.Write("\nDo you want to return to menu?");
                    Console.ReadKey();
                    goto menu;
                #endregion

                #region Sing up new Employee

                case 5:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbAddNewEmployee = new UnivercityContext("UniDBConStr");
                    using (dbAddNewEmployee)
                    {
                        Console.WriteLine("\nNew employee registration form");
                        Console.Write("\n\tDepartment Name: ");
                        string eAdepartment = Console.ReadLine();
                        Console.Write("\tMonthly salary : ");
                        float eASalary;
                        while (!float.TryParse(Console.ReadLine(), out eASalary))
                        {
                            PrintWarningJustNumber("Monthly salary");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nNew employee registration form");
                            Console.Write($"\n\tDepartment Name: {eAdepartment}");
                            Console.Write("\n\tMonthly salary : ");
                        }
                        Console.Write("\tFirst Name     : ");
                        string eAfirstName = Console.ReadLine();
                        Console.Write("\tLast Name      : ");
                        string eAlastName = Console.ReadLine();
                        Console.Write("\tPhonenumber    : ");
                        string eAphonenumber = Console.ReadLine();
                        while (!Regex.IsMatch(eAphonenumber, patternPhonenumber))
                        {
                            Console.WriteLine("\n\tIncorrect phonenumber !");
                            Thread.Sleep(2000);
                            Console.Write("\tPhonenumber    : ");
                            eAphonenumber = Console.ReadLine();
                        }
                        Console.Write("\tPassword       : ");
                        string eApassword = Console.ReadLine();
                        dbAddNewEmployee.Employees.Add(new Employee($"{eAdepartment}", eASalary, $"{eAfirstName}", $"{eAlastName}", $"{eAphonenumber}", $"{eApassword}", dbAddNewEmployee.Roles.Find(1)));
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{eAfirstName} {eAlastName} was registered\n");
                        dbAddNewEmployee.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Sing up new Student

                case 6:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbAddNewStudent = new UnivercityContext("UniDBConStr");
                    using (dbAddNewStudent)
                    {
                        Console.WriteLine("\nNew student registration form");
                        Console.Write("\n\tDegree Name    : ");
                        string sAdegree = Console.ReadLine();
                        Console.Write("\tFirst Name     : ");
                        string sAfirstName = Console.ReadLine();
                        Console.Write("\tLast Name      : ");
                        string sAlastName = Console.ReadLine();
                        Console.Write("\tPhonenumber    : ");
                        string sAphonenumber = Console.ReadLine();
                        while (!Regex.IsMatch(sAphonenumber, patternPhonenumber))
                        {
                            Console.WriteLine("\n\tIncorrect phonenumber !");
                            Thread.Sleep(2000);
                            Console.Write("\tPhonenumber    : ");
                            sAphonenumber = Console.ReadLine();
                        }
                        Console.Write("\tPassword       : ");
                        string sApassword = Console.ReadLine();
                        dbAddNewStudent.Students.Add(new Student($"{sAdegree}", $"{sAfirstName}", $"{sAlastName}", $"{sAphonenumber}", $"{sApassword}", dbAddNewStudent.Roles.Find(3)));
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{sAfirstName} {sAlastName} was registered\n");
                        dbAddNewStudent.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Sing up new Master

                case 7:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbAddNewMaster = new UnivercityContext("UniDBConStr");
                    using (dbAddNewMaster)
                    {
                        Console.WriteLine("\nNew master registration form");
                        Console.Write("\n\tDegree Name    : ");
                        string mAdegree = Console.ReadLine();
                        Console.Write("\tMonthly salary : ");
                        float mASalary;
                        while (!float.TryParse(Console.ReadLine(), out mASalary))
                        {
                            PrintWarningJustNumber("Monthly salary");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nNew master registration form");
                            Console.Write($"\n\tDegree Name    : {mAdegree}");
                            Console.Write("\n\tMonthly salary : ");
                        }
                        Console.Write("\tFirst Name     : ");
                        string mAfirstName = Console.ReadLine();
                        Console.Write("\tLast Name      : ");
                        string mAlastName = Console.ReadLine();
                        Console.Write("\tPhonenumber    : ");
                        string mAphonenumber = Console.ReadLine();
                        while (!Regex.IsMatch(mAphonenumber, patternPhonenumber))
                        {
                            Console.WriteLine("\n\tIncorrect phonenumber !");
                            Thread.Sleep(2000);
                            Console.Write("\tPhonenumber    : ");
                            mAphonenumber = Console.ReadLine();
                        }
                        Console.Write("\tPassword       : ");
                        string mApassword = Console.ReadLine();
                        dbAddNewMaster.Masters.Add(new Master($"{mAdegree}", mASalary, $"{mAfirstName}", $"{mAlastName}", $"{mAphonenumber}", $"{mApassword}", dbAddNewMaster.Roles.Find(2)));
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{mAfirstName} {mAlastName} was registered\n");
                        dbAddNewMaster.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Sing up new Course

                case 8:
                    PrintDateTimeAndUsersStatistic();
                    UnivercityContext dbAddNewCourse = new UnivercityContext("UniDBConStr");
                    using (dbAddNewCourse)
                    {
                        Console.WriteLine("\nNew course registration form");
                        Console.Write("\n\tCourse Name    : ");
                        string cAname = Console.ReadLine();
                        Console.Write("\tCourse unit    : ");
                        int cAunit;
                        while (!int.TryParse(Console.ReadLine(), out cAunit))
                        {
                            PrintWarningJustNumber("Course unit");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nNew course registration form");
                            Console.Write($"\n\tCourse Name    : {cAname}");
                            Console.Write("\n\tCourse unit    : ");
                        }
                    answerAddNewCourse:
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\nDoes {cAname} have a new master?");
                        Console.WriteLine("\n\t\t1. yes\n\t\t2. no");
                        Console.Write("\nYour answer number: ");
                        int answerAddNewCourse;
                        while (!int.TryParse(Console.ReadLine(), out answerAddNewCourse))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine($"\nDoes {cAname} have a new master?");
                            Console.WriteLine("\n\t\t1. yes\n\t\t2. no");
                            Console.Write("\nYour answer number: ");
                        }
                        switch (answerAddNewCourse)
                        {
                            case 1:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                Console.Write("\n\n\tDegree Name    : ");
                                string mAdegree = Console.ReadLine();
                                Console.Write("\tMonthly salary : ");
                                float mASalary;
                                while (!float.TryParse(Console.ReadLine(), out mASalary))
                                {
                                    PrintDateTimeAndUsersStatistic();
                                    PrintWarningJustNumber("Monthly salary");
                                    Thread.Sleep(2000);
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                    Console.WriteLine($"\n\n\tDegree Name    : {mAdegree}");
                                    Console.Write("\tMonthly salary : ");
                                }
                                Console.Write("\tFirst Name     : ");
                                string mAfirstName = Console.ReadLine();
                                Console.Write("\tLast Name      : ");
                                string mAlastName = Console.ReadLine();
                                Console.Write("\tPhonenumber    : ");
                                string mAphonenumber = Console.ReadLine();
                                while (!Regex.IsMatch(mAphonenumber, patternPhonenumber))
                                {
                                    Console.WriteLine("\n\tIncorrect phonenumber !");
                                    Thread.Sleep(2000);
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                    Console.Write($"\n\n\tDegree Name    : {mAdegree}");
                                    Console.Write($"\n\tMonthly salary : {mASalary}");
                                    Console.Write($"\n\tFirst Name     : {mAfirstName}");
                                    Console.Write($"\n\tLast Name      : {mAlastName}");
                                    Console.Write("\n\tPhonenumber    : ");
                                    mAphonenumber = Console.ReadLine();
                                }
                                Console.Write("\tPassword       : ");
                                string mApassword = Console.ReadLine();
                                Master masterNew = new Master($"{mAdegree}", mASalary, $"{mAfirstName}", $"{mAlastName}", $"{mAphonenumber}", $"{mApassword}", dbAddNewCourse.Roles.Find(2));
                                dbAddNewCourse.Courses.Add(new Course($"{cAname}", cAunit, masterNew));
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\t{cAname} with master {mAfirstName} {mAlastName} was registered\n");
                                goto finishAddNewCourse;

                            case 2:
                            selectMasterForAddToCourse:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\nPlease select master for course: {cAname}\n");
                                List<Master> masterList = dbAddNewCourse.Masters.ToList();
                                foreach (Master master in masterList)
                                {
                                    Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                      $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                    Console.WriteLine("\t----------------------------------------------------------------");
                                }
                                Console.Write("\nMaster id is: ");
                                int answerSelectNewCourse;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectNewCourse))
                                {
                                    PrintWarningJustNumber("Master id");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nPlease select master for course: {cAname}\n");
                                    foreach (Master master in masterList)
                                    {
                                        Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                          $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                        Console.WriteLine("\t----------------------------------------------------------------");
                                    }
                                    Console.Write("\nMaster id is: ");
                                }
                                var selectMaster = dbAddNewCourse.Masters.Find(answerSelectNewCourse);
                                if (selectMaster != null)
                                {
                                    dbAddNewCourse.Courses.Add(new Course($"{cAname}", cAunit, selectMaster));
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\t{cAname} was registered\n");
                                }
                                else
                                {
                                    PrintDateTimeAndUsersStatistic();
                                    PrintWarningNotFoundId("Master List", answerSelectNewCourse);
                                    goto selectMasterForAddToCourse;
                                }
                                goto finishAddNewCourse;

                            default:
                                goto answerAddNewCourse;
                        }
                    finishAddNewCourse:
                        dbAddNewCourse.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                        goto menu;
                    }
                #endregion

                #region Edit Info Employee

                case 9:
                employeeListForEdit:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select employee for edit\n");
                    UnivercityContext dbEditEmployee = new UnivercityContext("UniDBConStr");
                    using (dbEditEmployee)
                    {
                        List<Employee> viewEmployeeList = dbEditEmployee.Employees.ToList();
                        foreach (Employee employee in viewEmployeeList)
                        {
                            Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                              $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                            Console.WriteLine("\t-------------------------------------------------------------------");
                        }
                        Console.Write("\nEmployee id is: ");
                        int answerSelectEditEmployee;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectEditEmployee))
                        {
                            PrintWarningJustNumber("Employee id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            foreach (Employee employee in viewEmployeeList)
                            {
                                Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                                  $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                                Console.WriteLine("\t-------------------------------------------------------------------");
                            }
                            Console.Write("\nEmployee id is: ");
                        }
                        PrintDateTimeAndUsersStatistic();
                        var selectEmployeeForEdit = dbEditEmployee.Employees.Find(answerSelectEditEmployee);
                        if (selectEmployeeForEdit != null)
                        {
                            goto editEmployee;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Employee List", answerSelectEditEmployee);
                            goto employeeListForEdit;
                        }
                    editEmployee:
                        Console.WriteLine($"\nChose property for edit employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                        Console.WriteLine("\n\t\t1. Department" +
                                          "\n\t\t2. Salary" +
                                          "\n\t\t3. Name" +
                                          "\n\t\t4. Family" +
                                          "\n\t\t5. Phonenumber" +
                                          "\n\t\t6. Password" +
                                          "\n\t\t7. Access");
                        Console.Write("\nYour answer number: ");
                        int answerEditEmployee;
                        while (!int.TryParse(Console.ReadLine(), out answerEditEmployee))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine($"\nChose property for edit employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                            Console.WriteLine("\n\t\t1. Department" +
                                              "\n\t\t2. Salary" +
                                              "\n\t\t3. Name" +
                                              "\n\t\t4. Family" +
                                              "\n\t\t5. Phonenumber" +
                                              "\n\t\t6. Password" +
                                              "\n\t\t7. Access");
                            Console.Write("\nYour answer number: ");
                        }
                        PrintDateTimeAndUsersStatistic();

                        switch (answerEditEmployee)
                        {
                            case 1:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tDepartment: {selectEmployeeForEdit.Department}");
                                Console.Write("\n\tNew department: ");
                                string newDepartmentForEditEmployee = Console.ReadLine();
                                selectEmployeeForEdit.Department = newDepartmentForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe department was changed to {newDepartmentForEditEmployee} for {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            case 2:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tSalary: {selectEmployeeForEdit.Salary}");
                                Console.Write("\n\tNew Salary: ");
                                float newSalaryForEditEmployee;
                                while (!float.TryParse(Console.ReadLine(), out newSalaryForEditEmployee))
                                {
                                    PrintWarningJustNumber("Monthly salary");
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                    Console.WriteLine($"\n\tSalary: {selectEmployeeForEdit.Salary}");
                                    Console.Write("\n\tNew Salary: ");
                                }
                                selectEmployeeForEdit.Salary = newSalaryForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Salary was changed to {newSalaryForEditEmployee} for {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            case 3:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tName: {selectEmployeeForEdit.Name}");
                                Console.Write("\n\tNew name: ");
                                string newNameForEditEmployee = Console.ReadLine();
                                string name = selectEmployeeForEdit.Name;
                                selectEmployeeForEdit.Name = newNameForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Name was changed to {newNameForEditEmployee} for {name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            case 4:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tFamily: {selectEmployeeForEdit.Family}");
                                Console.Write("\n\tNew family: ");
                                string newFamilyForEditEmployee = Console.ReadLine();
                                string family = selectEmployeeForEdit.Family;
                                selectEmployeeForEdit.Family = newFamilyForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Family was changed to {newFamilyForEditEmployee} for {selectEmployeeForEdit.Name} {family}\n");
                                goto finishEditEmployee;

                            case 5:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tPhonenumber: {selectEmployeeForEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhonenumberForEditEmployee = Console.ReadLine();
                                while (!Regex.IsMatch(newPhonenumberForEditEmployee, patternPhonenumber))
                                {
                                    Console.WriteLine("\n\tIncorrect phonenumber !");
                                    Thread.Sleep(2000);
                                    Console.Write("\tPhonenumber    : ");
                                    newPhonenumberForEditEmployee = Console.ReadLine();
                                }
                                selectEmployeeForEdit.Phonenumber = newPhonenumberForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe phonenumber was changed to {newPhonenumberForEditEmployee} for {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            case 6:
                                Console.WriteLine($"\n\tEdit form for employee: {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}");
                                Console.WriteLine($"\n\tPassword: {selectEmployeeForEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForEditEmployee = Console.ReadLine();
                                selectEmployeeForEdit.Password = newPasswordForEditEmployee;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe password was changed to {newPasswordForEditEmployee} for {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            case 7:
                                if (selectEmployeeForEdit.IsActiv == true)
                                {
                                    selectEmployeeForEdit.IsActiv = false;
                                }
                                else
                                {
                                    selectEmployeeForEdit.IsActiv = true;
                                }
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe access was changed to {selectEmployeeForEdit.IsActiv} for {selectEmployeeForEdit.Name} {selectEmployeeForEdit.Family}\n");
                                goto finishEditEmployee;

                            default:
                                goto editEmployee;
                        }
                    finishEditEmployee:
                        dbEditEmployee.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Edit Info Students

                case 10:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select student for edit\n");
                    UnivercityContext dbEditStudent = new UnivercityContext("UniDBConStr");
                    using (dbEditStudent)
                    {
                        List<Student> viewStudentList = dbEditStudent.Students.ToList();
                        foreach (Student student in viewStudentList)
                        {
                            Console.WriteLine($"\tid: {student.UserId}\tname: {student.Name}\tfamily: {student.Family}\t\tdegree: {student.Degree}\n" +
                                              $"\tphone:  {student.Phonenumber}\tcode: {student.StudentCode}\t\tactive: {student.IsActiv}");
                            Console.WriteLine("\t-------------------------------------------------------------------");
                        }
                        Console.Write("\nStudent id is: ");
                        int answerSelectEditStudent;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectEditStudent))
                        {
                            PrintWarningJustNumber("Student id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nPlease select student for edit\n");
                            foreach (Student student in viewStudentList)
                            {
                                Console.WriteLine($"\tid: {student.UserId}\tname: {student.Name}\tfamily: {student.Family}\t\tdegree: {student.Degree}\n" +
                                                  $"\tphone:  {student.Phonenumber}\tcode: {student.StudentCode}\t\tactive: {student.IsActiv}");
                                Console.WriteLine("\t-------------------------------------------------------------------");
                            }
                            Console.Write("\nStudent id is: ");
                        }
                        Student selectStudentForEdit = dbEditStudent.Students.Find(answerSelectEditStudent);
                        if (selectStudentForEdit != null)
                        {
                            goto editStudent;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Student List", answerSelectEditStudent);
                            goto finishEditstudent;
                        }
                    editStudent:
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\nChose property for edit student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                        Console.WriteLine("\n\t\t1. Degree" +
                                          "\n\t\t2. Code" +
                                          "\n\t\t3. Name" +
                                          "\n\t\t4. Family" +
                                          "\n\t\t5. Phonenumber" +
                                          "\n\t\t6. Password" +
                                          "\n\t\t7. Access" +
                                          "\n\t\t8. Add course");
                        Console.Write("\nYour answer number: ");
                        int answerEditStudent;
                        while (!int.TryParse(Console.ReadLine(), out answerEditStudent))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine($"\nChose property for edit student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                            Console.WriteLine("\n\t\t1. Degree" +
                                              "\n\t\t2. Code" +
                                              "\n\t\t3. Name" +
                                              "\n\t\t4. Family" +
                                              "\n\t\t5. Phonenumber" +
                                              "\n\t\t6. Password" +
                                              "\n\t\t7. Access" +
                                              "\n\t\t8. Add course");
                            Console.Write("\nYour answer number: ");
                        }
                        PrintDateTimeAndUsersStatistic();

                        switch (answerEditStudent)
                        {
                            case 1:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tDegree: {selectStudentForEdit.Degree}");
                                Console.Write("\n\tNew degree: ");
                                string newDegreeForEditStudent = Console.ReadLine();
                                selectStudentForEdit.Degree = newDegreeForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe degree was changed to {newDegreeForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 2:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tCode: {selectStudentForEdit.StudentCode}");
                                Console.Write("\n\tNew Code: ");
                                int newCodeForEditStudent;
                                while (!int.TryParse(Console.ReadLine(), out newCodeForEditStudent))
                                {
                                    PrintWarningJustNumber("New code");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                    Console.WriteLine($"\n\tCode: {selectStudentForEdit.StudentCode}");
                                    Console.Write("\n\tNew Code: ");
                                }
                                selectStudentForEdit.StudentCode = newCodeForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Code was changed to {newCodeForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 3:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tName: {selectStudentForEdit.Name}");
                                Console.Write("\n\tNew name: ");
                                string newNameForEditStudent = Console.ReadLine();
                                selectStudentForEdit.Name = newNameForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Name was changed to {newNameForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 4:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tFamily: {selectStudentForEdit.Family}");
                                Console.Write("\n\tNew family: ");
                                string newFamilyForEditStudent = Console.ReadLine();
                                selectStudentForEdit.Family = newFamilyForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Family was changed to {newFamilyForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 5:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tPhonenumber: {selectStudentForEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhonenumberForEditStudent = Console.ReadLine();
                                while (!Regex.IsMatch(newPhonenumberForEditStudent, patternPhonenumber))
                                {
                                    Console.WriteLine("\n\tIncorrect phonenumber !");
                                    Thread.Sleep(2000);
                                    Console.Write("\tPhonenumber    : ");
                                    newPhonenumberForEditStudent = Console.ReadLine();
                                }
                                selectStudentForEdit.Phonenumber = newPhonenumberForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe phonenumber was changed to {newPhonenumberForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 6:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tPassword: {selectStudentForEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForEditStudent = Console.ReadLine();
                                selectStudentForEdit.Password = newPasswordForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe password was changed to {newPasswordForEditStudent} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 7:
                                if (selectStudentForEdit.IsActiv == true)
                                {
                                    selectStudentForEdit.IsActiv = false;
                                }
                                else
                                {
                                    selectStudentForEdit.IsActiv = true;
                                }
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe access was changed to {selectStudentForEdit.IsActiv} for {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                goto finishEditstudent;

                            case 8:
                                PrintDateTimeAndUsersStatistic();
                            addCourseForStudent:
                                Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                Console.WriteLine($"\n\tAre you adding a new Course?");
                                Console.WriteLine("\n\t\t1. Select old course" +
                                                  "\n\t\t2. Create new course");
                                Console.Write("\nYour answer number: ");
                                int answerAddCourseForStudent;
                                while (!int.TryParse(Console.ReadLine(), out answerAddCourseForStudent))
                                {
                                    PrintWarningJustNumber("Select item of menu");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}");
                                    Console.WriteLine($"\n\tAre you adding a new Course?");
                                    Console.WriteLine("\n\t\t1. Select old course" +
                                                      "\n\t\t2. Create new course");
                                    Console.Write("\nYour answer number: ");
                                }
                                PrintDateTimeAndUsersStatistic();
                                switch (answerAddCourseForStudent)
                                {
                                    case 1:
                                    courseList:
                                        Console.WriteLine($"\nPlease select a course for add to Student :  {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                        List<Course> courseList = dbEditStudent.Courses.ToList();
                                        foreach (Course course in courseList)
                                        {
                                            Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                                        }
                                        Console.Write("\nCourse id is: ");
                                        int answerSelectCourseForAddToStudentCourseList;
                                        while (!int.TryParse(Console.ReadLine(), out answerSelectCourseForAddToStudentCourseList))
                                        {
                                            PrintWarningJustNumber("course id");
                                            PrintDateTimeAndUsersStatistic();
                                            Console.WriteLine($"\nPlease select a course for add to Student :  {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                            foreach (Course course in courseList)
                                            {
                                                Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                                            }
                                            Console.Write("\nCourse id is: ");
                                        }
                                        Course selectOldCourse = dbEditStudent.Courses.Find(answerSelectCourseForAddToStudentCourseList);
                                        if (selectOldCourse != null)
                                        {
                                            selectStudentForEdit.Courses.Add(selectOldCourse);
                                        }
                                        else
                                        {
                                            PrintDateTimeAndUsersStatistic();
                                            PrintWarningNotFoundId("Course List", answerSelectCourseForAddToStudentCourseList);
                                            Console.Clear();
                                            goto courseList;
                                        }
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\t{selectOldCourse.CourseName} course added to student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                        goto finishEditstudent;

                                    case 2:
                                        Console.WriteLine($"\nNew course registration form for student: {selectStudentForEdit.Name} {selectStudentForEdit.Family} ");
                                        Console.Write("\n\tCourse Name    : ");
                                        string cAname = Console.ReadLine();
                                        Console.Write("\tCourse unit    : ");
                                        int cAunit;
                                        while (!int.TryParse(Console.ReadLine(), out cAunit))
                                        {
                                            PrintWarningJustNumber("Course unit");
                                            Console.Clear();
                                            PrintDateTimeAndUsersStatistic();
                                            Console.WriteLine("\nNew course registration form");
                                            Console.Write($"\n\tCourse Name    : {cAname}");
                                            Console.Write("\n\tCourse unit    : ");
                                        }
                                    addNewCourseForStudent:
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\nDoes {cAname} have a new master?");
                                        Console.WriteLine("\n\t\t1. yes\n\t\t2. no");
                                        Console.Write("\nYour answer number: ");
                                        int answerAddNewCourse;
                                        while (!int.TryParse(Console.ReadLine(), out answerAddNewCourse))
                                        {
                                            PrintWarningJustNumber("Select item of menu");
                                            Console.Clear();
                                            PrintDateTimeAndUsersStatistic();
                                            Console.WriteLine($"\nDoes {cAname} have a new master?");
                                            Console.WriteLine("\n\t\t1. yes\n\t\t2. no");
                                            Console.Write("\nYour answer number: ");
                                        }
                                        PrintDateTimeAndUsersStatistic();
                                        switch (answerAddNewCourse)
                                        {
                                            case 1:
                                                Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                                Console.Write("\n\n\tDegree Name    : ");
                                                string mAdegree = Console.ReadLine();
                                                Console.Write("\tMonthly salary : ");
                                                float mASalary;
                                                while (!float.TryParse(Console.ReadLine(), out mASalary))
                                                {
                                                    PrintWarningJustNumber("Monthly salary");
                                                    Console.Clear();
                                                    PrintDateTimeAndUsersStatistic();
                                                    Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                                    Console.WriteLine($"\n\n\tDegree Name    : {mAdegree}");
                                                    Console.Write("\tMonthly salary : ");
                                                }
                                                Console.Write("\tFirst Name     : ");
                                                string mAfirstName = Console.ReadLine();
                                                Console.Write("\tLast Name      : ");
                                                string mAlastName = Console.ReadLine();
                                                Console.Write("\tPhonenumber    : ");
                                                string mAphonenumber = Console.ReadLine();
                                                while (!Regex.IsMatch(mAphonenumber, patternPhonenumber))
                                                {
                                                    Console.WriteLine("\n\tIncorrect phonenumber !");
                                                    Thread.Sleep(2000);
                                                    Console.Clear();
                                                    PrintDateTimeAndUsersStatistic();
                                                    Console.WriteLine($"\nNew master registration form for course: {cAname}");
                                                    Console.Write($"\n\n\tDegree Name    : {mAdegree}");
                                                    Console.Write($"\n\tMonthly salary : {mASalary}");
                                                    Console.Write($"\n\tFirst Name     : {mAfirstName}");
                                                    Console.Write($"\n\tLast Name      : {mAlastName}");
                                                    Console.Write("\n\tPhonenumber    : ");
                                                    mAphonenumber = Console.ReadLine();
                                                }
                                                Console.Write("\tPassword       : ");
                                                string mApassword = Console.ReadLine();

                                                Course newCourse = new Course($"{cAname}", cAunit, new Master($"{mAdegree}", mASalary, $"{mAfirstName}", $"{mAlastName}", $"{mAphonenumber}", $"{mApassword}", dbEditStudent.Roles.Find(2)));
                                                dbEditStudent.Courses.Add(newCourse);
                                                selectStudentForEdit.Courses.Add(newCourse);
                                                PrintDateTimeAndUsersStatistic();
                                                Console.WriteLine($"\n\t{newCourse.CourseName} course added to student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                                goto finishEditstudent;

                                            case 2:
                                            addCourseIdForStudent:
                                                Console.WriteLine($"\nPlease select master for course: {cAname}\n");
                                                List<Master> masterList = dbEditStudent.Masters.ToList();
                                                foreach (Master master in masterList)
                                                {
                                                    Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                                      $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                                    Console.WriteLine("\t----------------------------------------------------------------");
                                                }
                                                Console.Write("\nMaster id is: ");
                                                int answerSelectNewCourse;
                                                while (!int.TryParse(Console.ReadLine(), out answerSelectNewCourse))
                                                {
                                                    PrintWarningJustNumber("Master id");
                                                    Console.Clear();
                                                    PrintDateTimeAndUsersStatistic();
                                                    foreach (Master master in masterList)
                                                    {
                                                        Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                                          $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                                        Console.WriteLine("\t----------------------------------------------------------------");
                                                    }
                                                    Console.Write("\nMaster id is: ");
                                                }
                                                var selectMaster = dbEditStudent.Masters.Find(answerSelectNewCourse);
                                                if (selectMaster != null)
                                                {
                                                    goto addNewCourseByMaster;
                                                }
                                                else
                                                {
                                                    PrintDateTimeAndUsersStatistic();
                                                    PrintWarningNotFoundId("Master List", answerSelectNewCourse);
                                                    PrintDateTimeAndUsersStatistic();
                                                    goto addCourseIdForStudent;
                                                }
                                            addNewCourseByMaster:
                                                Course newCourseByMaster = new Course($"{cAname}", cAunit, selectMaster);
                                                dbEditStudent.Courses.Add(newCourseByMaster);
                                                selectStudentForEdit.Courses.Add(newCourseByMaster);
                                                PrintDateTimeAndUsersStatistic();
                                                Console.WriteLine($"\n\t{newCourseByMaster.CourseName} course added to student: {selectStudentForEdit.Name} {selectStudentForEdit.Family}\n");
                                                goto finishEditstudent;

                                            default:
                                                goto addNewCourseForStudent;
                                        }

                                    default:
                                        goto addCourseForStudent;
                                }
                            default:
                                goto editStudent;
                        }
                    finishEditstudent:
                        dbEditStudent.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                        goto menu;
                    }
                #endregion

                #region Edit Info Master

                case 11:
                masterListForEdit:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select master for edit\n");
                    UnivercityContext dbEditMaster = new UnivercityContext("UniDBConStr");
                    using (dbEditMaster)
                    {
                        List<Master> viewMasterList = dbEditMaster.Masters.ToList();
                        foreach (Master master in viewMasterList)
                        {
                            Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tdegree: {master.Degree}\n" +
                                              $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tactive: {master.IsActiv}");
                            Console.WriteLine("\t-------------------------------------------------------------------");
                        }
                        Console.Write("\nMaster id is: ");
                        int answerSelectEditMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectEditMaster))
                        {
                            PrintWarningJustNumber("Master id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nPlease select master for edit\n");
                            foreach (Master master in viewMasterList)
                            {
                                Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tdegree: {master.Degree}\n" +
                                                  $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tactive: {master.IsActiv}");
                                Console.WriteLine("\t-------------------------------------------------------------------");
                            }
                            Console.Write("\nMaster id is: ");
                        }
                        PrintDateTimeAndUsersStatistic();
                        var selectMasterForEdit = dbEditMaster.Masters.Find(answerSelectEditMaster);
                        if (selectMasterForEdit != null)
                        {
                            goto selectMenuForEditStudent;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Master List", answerSelectEditMaster);
                            goto masterListForEdit;
                        }
                    selectMenuForEditStudent:
                        Console.WriteLine($"\nChose property for edit master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                        Console.WriteLine("\n\t\t1. Degree" +
                                          "\n\t\t2. Salary" +
                                          "\n\t\t3. Name" +
                                          "\n\t\t4. Family" +
                                          "\n\t\t5. Phonenumber" +
                                          "\n\t\t6. Password" +
                                          "\n\t\t7. Access");
                        Console.Write("\nYour answer number: ");
                        int answerEditMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerEditMaster))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine($"\nChose property for edit master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                            Console.WriteLine("\n\t\t1. Degree" +
                                              "\n\t\t2. Salary" +
                                              "\n\t\t3. Name" +
                                              "\n\t\t4. Family" +
                                              "\n\t\t5. Phonenumber" +
                                              "\n\t\t6. Password" +
                                              "\n\t\t7. Access");
                            Console.Write("\nYour answer number: ");
                        }
                        PrintDateTimeAndUsersStatistic();

                        switch (answerEditMaster)
                        {
                            case 1:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                if (selectMasterForEdit.Degree != "")
                                {
                                    Console.WriteLine($"\n\tDegree: {selectMasterForEdit.Degree}");
                                }
                                Console.Write("\n\tNew degree: ");
                                string newDegreeForEditMaster = Console.ReadLine();
                                selectMasterForEdit.Degree = newDegreeForEditMaster;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe degree was changed to {newDegreeForEditMaster} for {selectMasterForEdit.Name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            case 2:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                Console.WriteLine($"\n\tSalary: {selectMasterForEdit.Salary}");
                                Console.Write("\n\tNew Salary: ");
                                float newCodeForEditMaster;
                                while (!float.TryParse(Console.ReadLine(), out newCodeForEditMaster))
                                {
                                    PrintWarningJustNumber("Monthly salary");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                    Console.WriteLine($"\n\tSalary: {selectMasterForEdit.Salary}");
                                    Console.Write("\n\tNew Salary: ");
                                }
                                selectMasterForEdit.Salary = newCodeForEditMaster;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Salary was changed to {newCodeForEditMaster} for {selectMasterForEdit.Name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            case 3:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                Console.WriteLine($"\n\tName: {selectMasterForEdit.Name}");
                                Console.Write("\n\tNew name: ");
                                string newNameForEditMaster = Console.ReadLine();
                                string name = selectMasterForEdit.Name;
                                selectMasterForEdit.Name = newNameForEditMaster;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Name was changed to {newNameForEditMaster} for {name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            case 4:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                Console.WriteLine($"\n\tFamily: {selectMasterForEdit.Family}");
                                Console.Write("\n\tNew family: ");
                                string newFamilyForEditMaster = Console.ReadLine();
                                string family = selectMasterForEdit.Family;
                                selectMasterForEdit.Family = newFamilyForEditMaster;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Family was changed to {newFamilyForEditMaster} for {selectMasterForEdit.Name} {family}\n");
                                goto finishEditMaster;

                            case 5:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                Console.WriteLine($"\n\tPhonenumber: {selectMasterForEdit.Phonenumber}");
                                Console.Write("\n\tNew phonenumber: ");
                                string newPhonenumberForEditMaster = Console.ReadLine();
                                while (!Regex.IsMatch(newPhonenumberForEditMaster, patternPhonenumber))
                                {
                                    Console.WriteLine("\n\tIncorrect phonenumber !");
                                    Thread.Sleep(2000);
                                    Console.Write("\tPhonenumber    : ");
                                    newPhonenumberForEditMaster = Console.ReadLine();
                                }
                                selectMasterForEdit.Phonenumber = newPhonenumberForEditMaster;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe phonenumber was changed to {newPhonenumberForEditMaster} for {selectMasterForEdit.Name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            case 6:
                                Console.WriteLine($"\n\tEdit form for master: {selectMasterForEdit.Name} {selectMasterForEdit.Family}");
                                Console.WriteLine($"\n\tPassword: {selectMasterForEdit.Password}");
                                Console.Write("\n\tNew password: ");
                                string newPasswordForEditStudent = Console.ReadLine();
                                selectMasterForEdit.Password = newPasswordForEditStudent;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe password was changed to {newPasswordForEditStudent} for {selectMasterForEdit.Name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            case 7:
                                if (selectMasterForEdit.IsActiv == true)
                                {
                                    selectMasterForEdit.IsActiv = false;
                                }
                                else
                                {
                                    selectMasterForEdit.IsActiv = true;
                                }
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe access was changed to {selectMasterForEdit.IsActiv} for {selectMasterForEdit.Name} {selectMasterForEdit.Family}\n");
                                goto finishEditMaster;

                            default:
                                goto masterListForEdit;
                        }
                    finishEditMaster:
                        dbEditMaster.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Edit Info Course

                case 12:
                CourseListForEdit:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select course for edit\n");
                    UnivercityContext dbEditCourse = new UnivercityContext("UniDBConStr");
                    using (dbEditCourse)
                    {
                        List<Course> viewCourseList = dbEditCourse.Courses.ToList();
                        foreach (Course course in viewCourseList)
                        {
                            Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                            if (course.Master != null)
                            {
                                Console.WriteLine("\tMaster:");
                                Console.WriteLine($"\tid: {course.Master.UserId}\tname: {course.Master.Name}\tfamily: {course.Master.Family}\t     active: {course.Master.IsActiv}");
                            }
                            Console.WriteLine("\t-----------------------------------------------------------------");
                        }
                        Console.Write("\nCourse id is: ");
                        int answerSelectEditCourse;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectEditCourse))
                        {
                            PrintWarningJustNumber("course id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            foreach (Course course in viewCourseList)
                            {
                                Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                                Console.WriteLine("\tMaster:");
                                Console.WriteLine($"\tid: {course.Master.UserId}\tname: {course.Master.Name}\tfamily: {course.Master.Family}\t     active: {course.Master.IsActiv}");
                                Console.WriteLine("\t-----------------------------------------------------------------");
                            }
                            Console.Write("\nCourse id is: ");
                        }
                        PrintDateTimeAndUsersStatistic();
                        var selectCourseForEdit = dbEditCourse.Courses.Find(answerSelectEditCourse);
                        if (selectCourseForEdit != null)
                        {
                            goto selectItemForEditCourse;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Course List", answerSelectEditCourse);
                            goto CourseListForEdit;
                        }
                    selectItemForEditCourse:
                        Console.WriteLine($"\nChose property for edit course: {selectCourseForEdit.CourseName}");
                        Console.WriteLine("\n\t\t1. Name" +
                                          "\n\t\t2. Unit" +
                                          "\n\t\t3. Master");
                        Console.Write("\nYour answer number: ");
                        int answerEditMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerEditMaster))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine($"\nChose property for edit course: {selectCourseForEdit.CourseName}");
                            Console.WriteLine("\n\t\t1. Name" +
                                              "\n\t\t2. Unit" +
                                              "\n\t\t3. Master");
                            Console.Write("\nYour answer number: ");
                        }
                        PrintDateTimeAndUsersStatistic();

                        switch (answerEditMaster)
                        {
                            case 1:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tEdit form for course: {selectCourseForEdit.CourseName}");
                                Console.WriteLine($"\n\tName: {selectCourseForEdit.CourseName}");
                                Console.Write("\n\tNew name: ");
                                string newNameForEditCourse = Console.ReadLine();
                                selectCourseForEdit.CourseName = newNameForEditCourse;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Name was changed to {newNameForEditCourse}\n");
                                goto finishEditCourse;

                            case 2:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tEdit form for course: {selectCourseForEdit.CourseName}");
                                Console.WriteLine($"\n\tUnit: {selectCourseForEdit.CourseUnit}");
                                Console.Write("\n\tNew unit: ");
                                int newUnitForEditCourse;
                                while (!int.TryParse(Console.ReadLine(), out newUnitForEditCourse))
                                {
                                    PrintWarningJustNumber("New unit");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for course: {selectCourseForEdit.CourseName}");
                                    Console.WriteLine($"\n\tUnit: {selectCourseForEdit.CourseUnit}");
                                    Console.Write("\n\tNew unit: ");
                                }
                                selectCourseForEdit.CourseUnit = newUnitForEditCourse;
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tThe Unit was changed to {newUnitForEditCourse} for {selectCourseForEdit.CourseName}\n");
                                goto finishEditCourse;

                            case 3:
                            editMasterForCourse:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\n\tEdit form for course: {selectCourseForEdit.CourseName}");
                                if (selectCourseForEdit.Master != null)
                                {
                                    Console.WriteLine($"\n\tMaster: {selectCourseForEdit.Master.Name} {selectCourseForEdit.Master.Family}");
                                }
                                Console.WriteLine($"\n\tDoes {selectCourseForEdit.CourseName} have a new master?");
                                Console.WriteLine("\n\t\t\t1. yes\n\t\t\t2. no");
                                Console.Write("\nYour answer number: ");
                                int answerAddNewMasterForCourse;
                                while (!int.TryParse(Console.ReadLine(), out answerAddNewMasterForCourse))
                                {
                                    PrintWarningJustNumber("Select item of menu");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\n\tEdit form for course: {selectCourseForEdit.CourseName}");
                                    Console.WriteLine($"\n\tMaster: {selectCourseForEdit.Master.Name} {selectCourseForEdit.Master.Family}");
                                    Console.WriteLine($"\n\tDoes {selectCourseForEdit.CourseName} have a new master?");
                                    Console.WriteLine("\n\t\t\t1. yes\n\t\t\t2. no");
                                    Console.Write("\nYour answer number: ");
                                }
                                switch (answerAddNewMasterForCourse)
                                {
                                    case 1:
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\nNew master registration form for course: {selectCourseForEdit.CourseName}");
                                        Console.Write("\n\n\tDegree Name    : ");
                                        string cMaDegree = Console.ReadLine();
                                        Console.Write("\tMonthly salary : ");
                                        float cMaSalary;
                                        while (!float.TryParse(Console.ReadLine(), out cMaSalary))
                                        {
                                            PrintDateTimeAndUsersStatistic();
                                            PrintWarningJustNumber("Monthly salary");
                                            PrintDateTimeAndUsersStatistic();
                                            Console.WriteLine($"\nNew master registration form for course: {selectCourseForEdit.CourseName}");
                                            Console.WriteLine($"\n\n\tDegree Name    : {cMaDegree}");
                                            Console.Write("\tMonthly salary : ");
                                        }
                                        Console.Write("\tFirst Name     : ");
                                        string cMafirstName = Console.ReadLine();
                                        Console.Write("\tLast Name      : ");
                                        string cMalastName = Console.ReadLine();
                                        Console.Write("\tPhonenumber    : ");
                                        string cMaphonenumber = Console.ReadLine();
                                        while (!Regex.IsMatch(cMaphonenumber, patternPhonenumber))
                                        {
                                            Console.WriteLine("\n\tIncorrect phonenumber !");
                                            Thread.Sleep(2000);
                                            Console.Write($"\n\n\tDegree Name    : {cMaDegree}");
                                            Console.Write($"\tMonthly salary : {cMaSalary}");
                                            Console.Write($"\tFirst Name     : {cMafirstName}");
                                            Console.Write($"\tLast Name      : {cMalastName}");
                                            Console.Write("\tPhonenumber    : ");
                                            cMaphonenumber = Console.ReadLine();
                                        }
                                        Console.Write("\tPassword       : ");
                                        string cMapassword = Console.ReadLine();
                                        Master masterNew = new Master($"{cMaDegree}", cMaSalary, $"{cMafirstName}", $"{cMalastName}", $"{cMaphonenumber}", $"{cMapassword}", dbEditCourse.Roles.Find(2));
                                        dbEditCourse.Masters.Add(masterNew);
                                        selectCourseForEdit.Master = masterNew;
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tMaster of course {selectCourseForEdit.CourseName} change to {cMafirstName} {cMalastName}\n");
                                        goto finishEditCourse;

                                    case 2:
                                    selectMasterForEditCourse:
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\nPlease select master for course: {selectCourseForEdit.CourseName}\n");
                                        List<Master> masterList = dbEditCourse.Masters.ToList();
                                        foreach (Master master in masterList)
                                        {
                                            Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                              $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                            Console.WriteLine("\t----------------------------------------------------------------");
                                        }
                                        Console.Write("\nMaster id is: ");
                                        int answerSelectMasterForEditCourse;
                                        while (!int.TryParse(Console.ReadLine(), out answerSelectMasterForEditCourse))
                                        {
                                            PrintWarningJustNumber("Master id");
                                            Console.Clear();
                                            PrintDateTimeAndUsersStatistic();
                                            foreach (Master master in masterList)
                                            {
                                                Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                                  $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                                Console.WriteLine("\t----------------------------------------------------------------");
                                            }
                                            Console.Write("\nMaster id is: ");
                                        }
                                        var selectMasterForEditCourse = dbEditCourse.Masters.Find(answerSelectMasterForEditCourse);
                                        if (selectMasterForEditCourse != null)
                                        {
                                            goto editCourse;
                                        }
                                        else
                                        {
                                            PrintDateTimeAndUsersStatistic();
                                            PrintWarningNotFoundId("Master List", answerSelectMasterForEditCourse);
                                            goto selectMasterForEditCourse;
                                        }
                                    editCourse:
                                        selectCourseForEdit.Master = selectMasterForEditCourse;
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tMaster of course {selectCourseForEdit.CourseName} was changed to {selectMasterForEditCourse.Name} {selectMasterForEditCourse.Family}\n");
                                        goto finishEditCourse;

                                    default:
                                        goto editMasterForCourse;
                                }
                            default:
                                goto selectItemForEditCourse;
                        }
                    finishEditCourse:
                        dbEditCourse.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                        goto menu;
                    }
                #endregion

                #region Delete Employee

                case 13:
                removeEmployee:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select employee for remove\n");
                    using (UnivercityContext dbRemoveEmployee = new UnivercityContext("UniDBConStr"))
                    {
                        List<Employee> viewEmployeeList = dbRemoveEmployee.Employees.ToList();
                        foreach (Employee employee in viewEmployeeList)
                        {
                            Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                              $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                            Console.WriteLine("\t-------------------------------------------------------------------");
                        }
                        Console.Write("\nEmployee id is: ");
                        int answerSelectRemoveEmployee;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectRemoveEmployee))
                        {
                            PrintWarningJustNumber("Select item of menu");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            foreach (Employee employee in viewEmployeeList)
                            {
                                Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                                  $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                                Console.WriteLine("\t-------------------------------------------------------------------");
                            }
                            Console.Write("\nEmployee id is: ");
                        }
                        var removeEmployee = dbRemoveEmployee.Employees.Find(answerSelectRemoveEmployee);
                        if (removeEmployee != null)
                        {
                            goto removeEmployeeById;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Employee List", answerSelectRemoveEmployee);
                            goto removeEmployee;
                        }
                    removeEmployeeById:
                        dbRemoveEmployee.Employees.Remove(removeEmployee);
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{removeEmployee.Name} {removeEmployee.Family} was removed\n");
                        dbRemoveEmployee.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Delete Student

                case 14:
                removeStudent:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select student for remove\n");
                    using (UnivercityContext dbRemoveStudent = new UnivercityContext("UniDBConStr"))
                    {
                        IEnumerable<Student> studentList = dbRemoveStudent.Students.ToList();
                        foreach (Student student in studentList)
                        {
                            Console.WriteLine($"\tid: {student.UserId}\tcode: {student.StudentCode}\tname: {student.Name}\tfamily: {student.Family}\n" +
                                              $"\tphone:  {student.Phonenumber}\tdegree: {student.Degree}\tactive: {student.IsActiv}");
                            Console.WriteLine("\t----------------------------------------------------------------");
                        }
                        Console.Write("\nStudent id is: ");
                        int answerSelectRemoveStudent;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectRemoveStudent))
                        {
                            PrintWarningJustNumber("Student id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            foreach (Student student in studentList)
                            {
                                Console.WriteLine($"\tid: {student.UserId}\tcode: {student.StudentCode}\tname: {student.Name}\tfamily: {student.Family}\n" +
                                                  $"\tphone:  {student.Phonenumber}\tdegree: {student.Degree}\tactive: {student.IsActiv}");
                                Console.WriteLine("\t----------------------------------------------------------------");
                            }
                            Console.Write("\nStudent id is: ");
                        }
                        var removeStudent = dbRemoveStudent.Students.Find(answerSelectRemoveStudent);
                        if (removeStudent != null)
                        {
                            goto removeStudentById;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Student List", answerSelectRemoveStudent);
                            goto removeStudent;
                        }
                    removeStudentById:
                        dbRemoveStudent.Students.Remove(removeStudent);
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{removeStudent.Name} {removeStudent.Family} was removed\n");
                        dbRemoveStudent.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Delete Master

                case 15:
                removeMaster:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select master for remove\n");
                    UnivercityContext dbRemoveMaster = new UnivercityContext("UniDBConStr");
                    using (dbRemoveMaster)
                    {
                        List<Master> masterList = dbRemoveMaster.Masters.ToList();
                        foreach (Master master in masterList)
                        {
                            Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                              $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                            Console.WriteLine("\t----------------------------------------------------------------");
                        }
                        Console.Write("\nMaster id is: ");
                        int answerSelectRemoveMaster;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectRemoveMaster))
                        {
                            PrintWarningJustNumber("Master id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            foreach (Master master in masterList)
                            {
                                Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                  $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                Console.WriteLine("\t----------------------------------------------------------------");
                            }
                            Console.Write("\nMaster id is: ");
                        }
                        var removeMaster = dbRemoveMaster.Masters.Find(answerSelectRemoveMaster);
                        if (removeMaster != null)
                        {
                            goto removeMasterById;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Master List", answerSelectRemoveMaster);
                            goto removeMaster;
                        }
                    removeMasterById:
                        List<Course> removeCourseMaster = dbRemoveMaster.Courses.Where(t => t.Master.UserId == removeMaster.UserId).ToList();
                        dbRemoveMaster.Masters.Remove(removeMaster);
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{removeMaster.Name} {removeMaster.Family} was removed\n");
                        dbRemoveMaster.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }

                    goto menu;
                #endregion

                #region Delete Course

                case 16:
                removeCourse:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select course for remove\n");
                    UnivercityContext dbRemovecourse = new UnivercityContext("UniDBConStr");
                    using (dbRemovecourse)
                    {
                        Console.WriteLine("\nCourse:\n");
                        List<Course> courseList = dbRemovecourse.Courses.ToList();
                        foreach (Course course in courseList)
                        {
                            Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                        }
                        Console.Write("\nCourse id is: ");
                        int answerSelectRemoveCourse;
                        while (!int.TryParse(Console.ReadLine(), out answerSelectRemoveCourse))
                        {
                            PrintWarningJustNumber("Course id");
                            Console.Clear();
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\nPlease select course for remove\n");
                            foreach (Course course in courseList)
                            {
                                Console.WriteLine($"\tid: {course.CourseId}\tname: {course.CourseName}\tunit: {course.CourseUnit}\t   register: {course.RegisterDate}");
                            }
                            Console.Write("\nCourse id is: ");
                        }
                        var removeCourse = dbRemovecourse.Courses.Find(answerSelectRemoveCourse);
                        if (removeCourse != null)
                        {
                            goto removeCourseById;
                        }
                        else
                        {
                            PrintDateTimeAndUsersStatistic();
                            PrintWarningNotFoundId("Course List", answerSelectRemoveCourse);
                            goto removeCourse;
                        }
                    removeCourseById:
                        dbRemovecourse.Courses.Remove(removeCourse);
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine($"\n\t{removeCourse.CourseName} was removed\n");
                        dbRemovecourse.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                    }
                    goto menu;
                #endregion

                #region Change User

                case 17:
                changeRole:
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nPlease select object for change role\n");
                    Console.WriteLine("\t\t1. Employee" +
                                    "\n\t\t2. Master" +
                                    "\n\t\t3. Student ");
                    Console.Write("\nYour answer number: ");
                    int answerChangeRole;
                    while (!int.TryParse(Console.ReadLine(), out answerChangeRole))
                    {
                        PrintWarningJustNumber("Select item of menu");
                        Console.Clear();
                        PrintDateTimeAndUsersStatistic();
                        Console.WriteLine("\nPlease select object for change role\n");
                        Console.WriteLine("\t\t1. Employee" +
                                        "\n\t\t2. Master" +
                                        "\n\t\t3. Student ");
                        Console.Write("\nYour answer number: ");
                    }
                    UnivercityContext dbChangeRole = new UnivercityContext("UniDBConStr");
                    using (dbChangeRole)
                    {
                        switch (answerChangeRole)
                        {
                            #region فرم تغییر نقش کارمندان

                            case 1:
                            changeCourse:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine("\nPlease select employee for change role\n");
                                List<Employee> viewEmployeeList = dbChangeRole.Employees.ToList();
                                foreach (Employee employee in viewEmployeeList)
                                {
                                    Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                                      $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                                    Console.WriteLine("\t-------------------------------------------------------------------");
                                }
                                Console.Write("\nEmployee id is: ");
                                int answerSelectChangeEmployee;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectChangeEmployee))
                                {
                                    PrintWarningJustNumber("Employee id");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    foreach (Employee employee in viewEmployeeList)
                                    {
                                        Console.WriteLine($"\tid: {employee.UserId}\tname: {employee.Name}\tfamily: {employee.Family}\tdepartment: {employee.Department}\n" +
                                                          $"\tphone:  {employee.Phonenumber}\tsalary: {employee.Salary}\t\tactive: {employee.IsActiv}");
                                        Console.WriteLine("\t-------------------------------------------------------------------");
                                    }
                                    Console.Write("\nEmployee id is: ");
                                }
                                Employee selectEmployeeForChangeRole = dbChangeRole.Employees.Find(answerSelectChangeEmployee);

                                if (selectEmployeeForChangeRole != null)
                                {
                                    goto selectObjectForChangeEmployeeRole;
                                }
                                else
                                {
                                    PrintDateTimeAndUsersStatistic();
                                    PrintWarningNotFoundId("Employee List", answerSelectChangeEmployee);
                                    goto changeCourse;
                                }
                            selectObjectForChangeEmployeeRole:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\nPlease select object for employee: {selectEmployeeForChangeRole.Name} {selectEmployeeForChangeRole.Family}\n");
                                Console.WriteLine("\t\t1. Master" +
                                                "\n\t\t2. Student");
                                Console.Write("\nYour answer number: ");
                                int newObjectForChangeEmployee;
                                while (!int.TryParse(Console.ReadLine(), out newObjectForChangeEmployee))
                                {
                                    PrintWarningJustNumber("Select item of menu");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nPlease select object for employee: {selectEmployeeForChangeRole.Name} {selectEmployeeForChangeRole.Family}\n");
                                    Console.WriteLine("\t\t1. Master" +
                                                    "\n\t\t2. Student");
                                    Console.Write("\nYour answer number: ");
                                }
                                switch (newObjectForChangeEmployee)
                                {
                                    case 1:
                                        dbChangeRole.Masters.Add(new Master("", selectEmployeeForChangeRole.Salary, selectEmployeeForChangeRole.Name, selectEmployeeForChangeRole.Family, selectEmployeeForChangeRole.Phonenumber, selectEmployeeForChangeRole.Password, dbChangeRole.Roles.Find(2)));
                                        dbChangeRole.Employees.Remove(selectEmployeeForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectEmployeeForChangeRole.Name} {selectEmployeeForChangeRole.Family} was change to master\n");
                                        goto finishChangeRole;

                                    case 2:
                                        dbChangeRole.Students.Add(new Student("", selectEmployeeForChangeRole.Name, selectEmployeeForChangeRole.Family, selectEmployeeForChangeRole.Phonenumber, selectEmployeeForChangeRole.Password, dbChangeRole.Roles.Find(3)));
                                        dbChangeRole.Employees.Remove(selectEmployeeForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectEmployeeForChangeRole.Name} {selectEmployeeForChangeRole.Family} was change to student\n");
                                        goto finishChangeRole;

                                    default:
                                        goto selectObjectForChangeEmployeeRole;
                                }
                            #endregion

                            #region فرم تغییر نقش اساتید

                            case 2:
                            changeMaster:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine("\nPlease select master for change role\n");
                                List<Master> masterList = dbChangeRole.Masters.ToList();
                                foreach (Master master in masterList)
                                {
                                    Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                      $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                    Console.WriteLine("\t----------------------------------------------------------------");
                                }
                                Console.Write("\nMaster id is: ");
                                int answerSelectChangeMaster;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectChangeMaster))
                                {
                                    PrintWarningJustNumber("Master id");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    foreach (Master master in masterList)
                                    {
                                        Console.WriteLine($"\tid: {master.UserId}\tname: {master.Name}\tfamily: {master.Family}\tactive: {master.IsActiv}\n" +
                                                          $"\tphone:  {master.Phonenumber}\tsalary: {master.Salary}\t\tdegree: {master.Degree}");
                                        var courseMaster = dbChangeRole.Courses.Where(t => t.Master.UserId == master.UserId).ToList();
                                        Console.WriteLine("\t----------------------------------------------------------------");
                                    }
                                    Console.Write("\nMaster id is: ");
                                }
                                Master selectMasterForChangeRole = dbChangeRole.Masters.Find(answerSelectChangeMaster);
                                if (selectMasterForChangeRole != null)
                                {
                                    goto selectObjectForChangeMasterRole;
                                }
                                else
                                {
                                    PrintDateTimeAndUsersStatistic();
                                    PrintWarningNotFoundId("Master List", answerSelectChangeMaster);
                                    goto changeMaster;
                                }
                            selectObjectForChangeMasterRole:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\nPlease select object for master: {selectMasterForChangeRole.Name} {selectMasterForChangeRole.Family}\n");
                                Console.WriteLine("\t\t1. Employee" +
                                                "\n\t\t2. Student");
                                Console.Write("\nYour answer number: ");
                                int newObjectForChangeMaster;
                                while (!int.TryParse(Console.ReadLine(), out newObjectForChangeMaster))
                                {
                                    PrintWarningJustNumber("Select item of menu");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nPlease select object for master: {selectMasterForChangeRole.Name} {selectMasterForChangeRole.Family}\n");
                                    Console.WriteLine("\t\t1. Employee" +
                                                    "\n\t\t2. Student");
                                    Console.Write("\nYour answer number: ");
                                }
                                switch (newObjectForChangeMaster)
                                {
                                    case 1:
                                        dbChangeRole.Employees.Add(new Employee("", selectMasterForChangeRole.Salary, selectMasterForChangeRole.Name, selectMasterForChangeRole.Family, selectMasterForChangeRole.Phonenumber, selectMasterForChangeRole.Password, dbChangeRole.Roles.Find(1)));
                                        dbChangeRole.Masters.Remove(selectMasterForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectMasterForChangeRole.Name} {selectMasterForChangeRole.Family} was change to employee\n");
                                        goto finishChangeRole;

                                    case 2:
                                        dbChangeRole.Students.Add(new Student(selectMasterForChangeRole.Degree, selectMasterForChangeRole.Name, selectMasterForChangeRole.Family, selectMasterForChangeRole.Phonenumber, selectMasterForChangeRole.Password, dbChangeRole.Roles.Find(3)));
                                        dbChangeRole.Masters.Remove(selectMasterForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectMasterForChangeRole.Name} {selectMasterForChangeRole.Family} was change to student\n");


                                        goto finishChangeRole;
                                    default:
                                        goto selectObjectForChangeMasterRole;
                                }
                            #endregion

                            #region فرم تغییر نقش دانشجویان

                            case 3:
                            changeStudent:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine("\nPlease select student for change role\n");
                                IEnumerable<Student> studentList = dbChangeRole.Students.ToList();
                                foreach (Student student in studentList)
                                {
                                    Console.WriteLine($"\tid: {student.UserId}\tcode: {student.StudentCode}\tname: {student.Name}\tfamily: {student.Family}\n" +
                                                      $"\tphone:  {student.Phonenumber}\tdegree: {student.Degree}\tactive: {student.IsActiv}");
                                    Console.WriteLine("\t----------------------------------------------------------------");
                                }
                                Console.Write("\nstudent id is: ");
                                int answerSelectChangeStudent;
                                while (!int.TryParse(Console.ReadLine(), out answerSelectChangeStudent))
                                {
                                    PrintWarningJustNumber("student id");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    foreach (Student student in studentList)
                                    {
                                        Console.WriteLine($"\tid: {student.UserId}\tcode: {student.StudentCode}\tname: {student.Name}\tfamily: {student.Family}\n" +
                                                          $"\tphone:  {student.Phonenumber}\tdegree: {student.Degree}\tactive: {student.IsActiv}");
                                        Console.WriteLine("\t----------------------------------------------------------------");
                                    }
                                    Console.Write("\nstudent id is: ");
                                }
                                Student selectStudentForChangeRole = dbChangeRole.Students.Find(answerSelectChangeStudent);
                                if (selectStudentForChangeRole != null)
                                {
                                    goto selectObjectForChangeStudentRole;
                                }
                                else
                                {
                                    PrintDateTimeAndUsersStatistic();
                                    PrintWarningNotFoundId("Student List", answerSelectChangeStudent);
                                    goto changeStudent;
                                }
                            selectObjectForChangeStudentRole:
                                PrintDateTimeAndUsersStatistic();
                                Console.WriteLine($"\nPlease select object for student: {selectStudentForChangeRole.Name} {selectStudentForChangeRole.Family}\n");
                                Console.WriteLine("\t\t1. Employee" +
                                                "\n\t\t2. Master");
                                Console.Write("\nYour answer number: ");
                                int newObjectForChangeStudent;
                                while (!int.TryParse(Console.ReadLine(), out newObjectForChangeStudent))
                                {
                                    PrintWarningJustNumber("Select item of menu");
                                    Console.Clear();
                                    PrintDateTimeAndUsersStatistic();
                                    Console.WriteLine($"\nPlease select object for student: {selectStudentForChangeRole.Name} {selectStudentForChangeRole.Family}\n");
                                    Console.WriteLine("\t\t1. Employee" +
                                                    "\n\t\t2. Master");
                                    Console.Write("\nYour answer number: ");
                                }
                                switch (newObjectForChangeStudent)
                                {
                                    case 1:
                                        dbChangeRole.Employees.Add(new Employee("", 0, selectStudentForChangeRole.Name, selectStudentForChangeRole.Family, selectStudentForChangeRole.Phonenumber, selectStudentForChangeRole.Password, dbChangeRole.Roles.Find(1)));
                                        dbChangeRole.Students.Remove(selectStudentForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectStudentForChangeRole.Name} {selectStudentForChangeRole.Family} was change to employee\n");
                                        goto finishChangeRole;

                                    case 2:
                                        dbChangeRole.Masters.Add(new Master("", 0, selectStudentForChangeRole.Name, selectStudentForChangeRole.Family, selectStudentForChangeRole.Phonenumber, selectStudentForChangeRole.Password, dbChangeRole.Roles.Find(2)));
                                        dbChangeRole.Students.Remove(selectStudentForChangeRole);
                                        PrintDateTimeAndUsersStatistic();
                                        Console.WriteLine($"\n\tRole of {selectStudentForChangeRole.Name} {selectStudentForChangeRole.Family} was change to master\n");
                                        goto finishChangeRole;

                                    default:
                                        goto selectObjectForChangeStudentRole;
                                }
                            #endregion

                            default:
                                goto changeRole;
                        }
                    finishChangeRole:
                        dbChangeRole.SaveChanges();
                        Console.Write("\nDo you want to return to menu?");
                        Console.ReadKey();
                        goto menu;
                    }

                #endregion

                #region Exit

                case 18:
                    Console.Clear();
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine($"\n\tHave a good time {adminName}");
                    Thread.Sleep(4000);
                    Console.WriteLine($"\tBye");
                    Thread.Sleep(2000);
                    goto Login;
                #endregion

                #region Change background color

                case 19:
                    Console.Clear();
                    PrintDateTimeAndUsersStatistic();
                    Console.WriteLine("\nWhat color do you want to change?\n\n\t\t\t1. RED\t\t2. BLUE\t\t\t3. GREEN\t\t4. YELLOW\n\t\t\t5. GRAY\t\t6. WHITE\t\t7. BLACK\t\t8. MAGNETA");
                    Console.Write("\nYour answer is : ");
                    int colorChoose = Convert.ToInt32(Console.ReadLine());
                    switch (colorChoose)
                    {
                        case 1:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to red");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.Clear();
                            goto menu;
                        case 2:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to blue");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Clear();
                            goto menu;
                        case 3:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to Green");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Clear();
                            goto menu;
                        case 4:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to Yellow");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Clear();
                            goto menu;
                        case 5:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to Gray");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Clear();
                            goto menu;
                        case 6:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to White");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Clear();
                            goto menu;
                        case 7:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to Black");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            goto menu;
                        case 8:
                            PrintDateTimeAndUsersStatistic();
                            Console.WriteLine("\n\nchange backgrand to Magneta");
                            Thread.Sleep(3000);
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.Clear();
                            goto menu;
                    }
                    break;

                #endregion

                default:
                    goto menu;
            }
        }
    }
}
