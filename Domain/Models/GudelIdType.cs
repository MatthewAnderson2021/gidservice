using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GudelIdService.Domain.Models
{
    public class GudelIdType
    {
        public int Id { get; set; }
        [Required]
        public Dictionary<string, string> Name { get; set; }
        public Dictionary<string, string> Description { get; set; }

        public GudelIdType() { }

        public GudelIdType(int id, Dictionary<string, string> name, Dictionary<string, string> description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public ICollection<GudelId> GudelIds { get; set; }

    }

    public class GudelIdTypes
    {
        public const int SmartproductId = 1;
        public const int UserId = 2;
        public const int ProductionassetId = 3;
        public const int InfrastructureassetId = 4;
        public const int DevelopmentId = 5;

        public static GudelIdType smartproduct = new GudelIdType(SmartproductId,
            new Dictionary<string, string>
            {
                {"en-US", "SmartProduct"},
                {"de-DE", "SmartProduct"},
            },
            new Dictionary<string, string>
            {
                {"en-US", "Güdel ID of a smart product by Güdel"},
                {"de-DE", "Güdel ID eines Smart Product von Güdel"},
            }
        );

        public static GudelIdType user = new GudelIdType(UserId,
            new Dictionary<string, string>
            {
                { "en-US", "User" },
                { "de-DE", "User" },
            },
            new Dictionary<string, string>
            {
                { "en-US", "Güdel ID of a human being, user of Güdel ID System" },
                { "de-DE", "Güdel ID eines menschlichen Benutzers des Güdel ID System" },
            }
            );

        public static GudelIdType productionasset = new GudelIdType(ProductionassetId,
           new Dictionary<string, string>
           {
                { "en-US", "ProductionAsset" },
                { "de-DE", "ProductionAsset" },
           },
           new Dictionary<string, string>
           {
                { "en-US", "Güdel ID of an asset which could be used to assign Güdel IDs to assets" },
                { "de-DE", "Güdel ID eines assets welches dazu genutzt werden kann Güdel IDs zuzuordnen" },
           }
           );

        public static GudelIdType infrastructureasset = new GudelIdType(InfrastructureassetId,
           new Dictionary<string, string>
           {
                { "en-US", "InfrastructureAsset" },
                { "de-DE", "InfrastructureAsset" },
           },
           new Dictionary<string, string>
           {
                { "en-US", "Güdel ID of IT infrastructure assets" },
                { "de-DE", "Güdel ID eines IT Infrastruktur Assets" },
           }
           );

        public static GudelIdType developmentid = new GudelIdType(DevelopmentId,
           new Dictionary<string, string>
           {
                { "en-US", "DevelopmentID" },
                { "de-DE", "DevelopmentID" },
           },
           new Dictionary<string, string>
           {
                { "en-US", "Güdel ID for Güdel ID System development and testing" },
                { "de-DE", "Güdel ID zur Entwicklung und Testing des Güdel ID System" },
           }
           );

    }
}
