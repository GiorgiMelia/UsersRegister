using Npgsql;
using Dapper;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Company
{

    internal class Function
    {
        public static string connectionString = "Host=localhost;Username=postgres;Password=aaa;Database=Customers ";

        public static void Start()
        {
            int WorkType = ReadInput();
            DoWork(WorkType);
            FinishWork(WorkType);
        }

        private static void FinishWork(int workType)
        {
            switch (workType)
            {
                case 0:
                    Console.WriteLine("Customer Added!\n");
                    break;
                case 1:
                    Console.WriteLine("Customer Removed!\n");
                    break;
                case 2:
                    break;
                case 3:
                    Console.WriteLine("This is the customer with your ID \n");
                    break;
                case 4:
                    Console.WriteLine("Customer Updated"); ;
                    break;
                default:
                    Console.WriteLine("How did I got here");
                    break;
            }
        }

        private static void DoWork(int workType)
        {
            switch (workType)
            {
                case -1:
                    Console.WriteLine("exiting!");
                    System.Environment.Exit(1);
                    break;

                case 0:
                    AddUser();
                    break;
                case 1:
                    RemoveUser();
                    break;
                case 2:
                    AllUsers();
                    break;
                case 3:
                    GetById();
                    break;
                case 4:
                    UpdateUser();
                    break;
                default:
                    Console.WriteLine("How did I got here");
                    break;
            }
        }

        private static void UpdateUser()
        {
            using (var context = new AppDbContext())
            {
                Console.WriteLine("which user info do you want to Update?(give me Id):");
                int id = Convert.ToInt32(Console.ReadLine());
                if (CheckIdExists(id))
                {
                    var customer = context.Customers.FirstOrDefault(x => x.Id == id);
                    var info = GetInfo();
                    customer.Name = info.Item1;
                    customer.Surname = info.Item2;
                    context.SaveChanges();
                }
                else RetryUpdateRequest(id);
            }

        }

        private static void GetById()
        {
            Console.WriteLine("which user info do you want to get?(give me Id):");
            int id = Convert.ToInt32(Console.ReadLine());
            if (CheckIdExists(id))
            {
                Customer customer = GetCustomer(id);
                PrintCustomer(customer);
            }
            else RetryGetByIdRequest(id);
        }

        private static Customer GetCustomer(int id)
        {
            using (var context = new AppDbContext())
            {
                Customer customer = context.Customers.FirstOrDefault(x => x.Id == id);
                return customer;
            }
        }

        private static void AllUsers()
        {
            using (var context = new AppDbContext())
            {
                var customers = context.Customers.AsList();
                foreach (var custom in customers)
                {
                    PrintCustomer(custom);
                }
            }

        }
        private static void PrintCustomer(Customer customer)
        {
            Console.WriteLine(customer.Id + "  " + customer.Name + "  " + customer.Surname);
        }
        private static void RemoveUser()
        {
            Console.WriteLine("which user do you want to delete?(give me Id):");
            int id = Convert.ToInt32(Console.ReadLine());
            if (CheckIdExists(id))
            {
                DeleteCustomer(id);
            }
            else RetryDeleteRequest(id);

        }

        private static void DeleteCustomer(int id)
        {
            using (var context = new AppDbContext())
            {
                Customer customer = GetCustomer(id);
                context.Customers.Remove(customer);
                context.SaveChanges();

            }
        }

        private static bool CheckIdExists(int id)
        {

            using (var context = new AppDbContext())
            {

                return (context.Customers.Any(o => o.Id == id));
            }
        }
        private static void RetryGetByIdRequest(int id)
        {
            Console.WriteLine($"No Customer with id {id}! ");
            Console.WriteLine("Do you want to retry? (Y/N)");
            string read = Console.ReadLine();
            if (read == "Y") { GetById(); return; }
            else if (read == "N") Start();
            else RetryGetByIdRequest(id);
        }
        private static void RetryUpdateRequest(int id)
        {
            Console.WriteLine($"No Customer with id {id}! ");
            Console.WriteLine("Do you want to retry? (Y/N)");
            string read = Console.ReadLine();
            if (read == "Y") { UpdateUser(); return; }
            else if (read == "N") { Start(); }
            else RetryUpdateRequest(id);
        }

        private static void RetryDeleteRequest(int id)
        {
            Console.WriteLine($"No Customer with id {id}! ");
            Console.WriteLine("Do you still want to delete? (Y/N)");
            string read = Console.ReadLine();
            if (read == "Y") { RemoveUser(); return; }
            else if (read == "N") Start();
            else RetryDeleteRequest(id);
        }
        private static void AddUser()
        {
            SaveUser(CreateUser(GetInfo()));

        }

        private static void SaveUser(Customer customer)
        {
            using (var context = new AppDbContext())
            {

                context.Customers.Add(customer);
                context.SaveChanges();
            }

        }

        private static string? GetUserInfo(Customer customer)
        {
            return customer.Name + " " + customer.Surname + " " + customer.Id;
        }

        private static Customer CreateUser(Tuple<string, string> tuple)
        {
            return new Customer(tuple.Item1, tuple.Item2);
        }

        private static Tuple<string, string> GetInfo()
        {
            Console.WriteLine("Input Name : ");
            string name = Console.ReadLine();
            Console.WriteLine("Input Surname : ");
            string surname = Console.ReadLine();
            if (name == null) name = "";
            if (surname == null) surname = "";
            Tuple<string?, string?> tuple = Tuple.Create(name, surname);
            return tuple;
        }

        private static int ReadInput()
        {
            AskForInput();
            int input = Convert.ToInt32(Console.ReadLine());
            if (CheckInput(input)) return input;
            else return RetryInput(input);



        }

        private static int RetryInput(int input)
        {
            Console.WriteLine("Wrong input!\n" + input + " is not valid code");
            return ReadInput();
        }

        private static bool CheckInput(int input)
        {
            return input <= 4 && input >= -1;
        }

        private static void AskForInput()
        {
            Console.Write("input -1 to exit program,\n 0 - add user,\n 1 - remove user,\n 2 - get all users,\n 3 - get user by Id,\n 4 - Update Customer : ");
        }
    }
}
