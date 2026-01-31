using Fundep.ProjectService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Fundep.ProjectService.Repositories
{
    public class XmlStore<T> : IXmlStore<T>
    {
        private readonly string _filePath;
        private readonly Func<T, XElement> _toX;
        private readonly Func<XElement, T> _fromX;
        private readonly string _rootName;
        private readonly string _itemName;

        public XmlStore(string folderPath, string fileName, string rootName, string itemName, Func<T, XElement> toX, Func<XElement, T> fromX)
        {
            if (string.IsNullOrWhiteSpace(folderPath)) throw new ArgumentException("folderPath is required");
            Directory.CreateDirectory(folderPath);

            _filePath = Path.Combine(folderPath, fileName);
            _rootName = rootName;
            _itemName = itemName;
            _toX = toX;
            _fromX = fromX;
        }

        public List<T> LoadAll()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var doc = XDocument.Load(_filePath);
            var root = doc.Root;
            if (root == null || root.Name != _rootName)
                return new List<T>();

            return root.Elements(_itemName).Select(_fromX).ToList();
        }

        public void SaveAll(List<T> items)
        {
            var doc = new XDocument(new XElement(_rootName, (items ?? new List<T>()).Select(_toX)));
            doc.Save(_filePath);
        }
    }

    public static class Stores
    {
        public static IXmlStore<Coordinator> Coordinators(string appDataPath)
        {
            return new XmlStore<Coordinator>(
                appDataPath, "coordinators.xml", "Coordinators", "Coordinator",
                c => new XElement("Coordinator",
                    new XElement("Id", c.Id ?? ""),
                    new XElement("Name", c.Name ?? "")),
                x => new Coordinator
                {
                    Id = (string)x.Element("Id") ?? "",
                    Name = (string)x.Element("Name") ?? ""
                }
            );
        }

        public static IXmlStore<Project> Projects(string appDataPath)
        {
            return new XmlStore<Project>(
                appDataPath, "projects.xml", "Projects", "Project",
                p => new XElement("Project",
                    new XElement("ProjectNumber", p.ProjectNumber ?? ""),
                    new XElement("SubProjectNumber", p.SubProjectNumber ?? ""),
                    new XElement("Name", p.Name ?? ""),
                    new XElement("CoordinatorId", p.CoordinatorId ?? ""),
                    new XElement("Balance", p.Balance.ToString(System.Globalization.CultureInfo.InvariantCulture))),
                x => new Project
                {
                    ProjectNumber = (string)x.Element("ProjectNumber") ?? "",
                    SubProjectNumber = (string)x.Element("SubProjectNumber") ?? "",
                    Name = (string)x.Element("Name") ?? "",
                    CoordinatorId = (string)x.Element("CoordinatorId") ?? "",
                    Balance = ParseDecimal((string)x.Element("Balance"))
                }
            );
        }

        private static decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0m;
            decimal d;
            if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d))
                return d;
            if (decimal.TryParse(value, out d))
                return d;
            return 0m;
        }
    }
}
