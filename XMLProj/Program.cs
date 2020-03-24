using CSVProj;
using LinqClassesLibrary;
using System;
using System.Linq;
using System.Xml.Linq;

namespace XMLProj
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel.xml");
            var query = from element in document.Element("Cars").Elements("Car")
                        where element.Element("Manufacturer").Value == "BMW"
                        select element.Element("Name").Value;

            foreach (var el in query)
            {
                Console.WriteLine(el);
            }

        }

        private static void CreateXml()
        {
            var ns = (XNamespace)"http://test.com/cars/2016";
            var document = new XDocument();
            var records = Helpers.ProcessFile<Car>("fuel.csv");
            var cars = new XElement("Cars",
                from record in records
                select new XElement("Car",
                        new XElement("Name", record.Carline),
                        new XElement("Manufacturer", record.Division),
                        new XElement("City", record.City)

                ));

            document.Add(cars);
            document.Save("fuel.xml");
        }
    }
}
