using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _2_лаб
{
    public class Client
    {
        public delegate void WantCar();
        public event WantCar EvWantCar;
        public void OnWantCar()
        {
            if (EvWantCar != null)
            {
                EvWantCar();
            }
        }
    }
    public interface IDomainObject { int ID { get; set; } }

    public class Car : IDomainObject { public int ID { get; set; } }

    public class Bike : IDomainObject { public int ID { get; set; } }

    public class Employee //: IDomainObject
    {
        public int ID { get; set; }
        public string Name;
        public void ClientWantCar()
        {
            OnRent();
        }
        public delegate void Rent(Employee emp, Car car);
        public event Rent RentEvent;
        public void OnRent()
        {
            if (Repository<Car>.list.Count > 0)
            {
                RentEvent(this, Repository<Car>.list[0]);
                Repository<Car>.list.RemoveAt(0);
            }
        }
    }

    public class Repository<T> where T : IDomainObject, new()
    {
        public static List<Car> list = new List<Car>();
        public static T Create(int id)
        {
            return new T() { ID = id };
        }
        public static void Rent(Employee emp, T obj)
        {
            StreamWriter stream = File.AppendText("ID5.txt");
            stream.WriteLine("Employee's name: " + emp.Name + " Car's ID: " + obj.ID);
            stream.Close();
        }  
    }

    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            Employee employee = new Employee();
            employee.Name = "Alexander";

            Car car = Repository<Car>.Create(55); // ID
            Repository<Car>.list.Add(car);
            client.EvWantCar += employee.ClientWantCar;
            employee.RentEvent += Repository<Car>.Rent;
            client.OnWantCar();
        }
    }
}