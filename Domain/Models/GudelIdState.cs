using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudelIdService.Domain.Models
{
    public class GudelIdState
    {
        public int Id { get; set; }

        public Dictionary<string, string> Name { get; set; }

        public Dictionary<string, string> Description { get; set; }

        [NotMapped]
        public List<int> PossiblePreviousStateIds { get; set; }
        [NotMapped]
        public List<GudelIdState> PossiblePreviousStates { get; set; }
        [NotMapped]
        public List<int> AllowedFollowupStateIds { get; set; }
        [NotMapped]
        public List<GudelIdState> AllowedFollowupStates { get; set; }
        public IEnumerable<GudelId> GudelIds { get; set; }
        public virtual IEnumerable<ExtraFieldDefinition> ExtraFieldDefinition { get; set; }

        public GudelIdState(int id, Dictionary<string, string> name, Dictionary<string, string> description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;

            switch (Id)
            {
                case 0:
                    this.AllowedFollowupStateIds = new List<int> { GudelIdStates.ReservedId, GudelIdStates.ProducedId, GudelIdStates.AssignedId,  GudelIdStates.VoidedId };
                    this.AllowedFollowupStates = new List<GudelIdState> { GudelIdStates.Reserved, GudelIdStates.Produced, GudelIdStates.Assigned, GudelIdStates.Voided };
                    this.PossiblePreviousStateIds = new List<int>();
                    this.PossiblePreviousStates = new List<GudelIdState>();
                    break;
                case 10:
                    this.AllowedFollowupStateIds = new List<int> { GudelIdStates.ProducedId, GudelIdStates.AssignedId,  GudelIdStates.VoidedId };
                    this.AllowedFollowupStates = new List<GudelIdState>() { GudelIdStates.Produced, GudelIdStates.Assigned,  GudelIdStates.Voided };
                    this.PossiblePreviousStateIds = new List<int> { GudelIdStates.CreatedId };
                    this.PossiblePreviousStates = new List<GudelIdState>() { GudelIdStates.Created };
                    break;
                case 20:
                    this.AllowedFollowupStateIds = new List<int> { GudelIdStates.AssignedId, GudelIdStates.VoidedId };
                    this.AllowedFollowupStates = new List<GudelIdState> { GudelIdStates.Assigned, GudelIdStates.Voided };
                    this.PossiblePreviousStateIds = new List<int> { GudelIdStates.CreatedId, GudelIdStates.ReservedId };
                    this.PossiblePreviousStates = new List<GudelIdState> { GudelIdStates.Created, GudelIdStates.Reserved };
                    break;
                case 30:
                    this.AllowedFollowupStateIds = new List<int> { GudelIdStates.VoidedId };
                    this.AllowedFollowupStates = new List<GudelIdState> { GudelIdStates.Voided };
                    this.PossiblePreviousStateIds = new List<int> { GudelIdStates.CreatedId, GudelIdStates.ReservedId, GudelIdStates.ProducedId };
                    this.PossiblePreviousStates = new List<GudelIdState> { GudelIdStates.Created, GudelIdStates.Reserved, GudelIdStates.Produced };
                    break;
                case 99:
                default:
                    this.AllowedFollowupStateIds = new List<int>();
                    this.AllowedFollowupStates = new List<GudelIdState>();
                    this.PossiblePreviousStateIds = new List<int> { GudelIdStates.CreatedId, GudelIdStates.ReservedId, GudelIdStates.ProducedId, GudelIdStates.AssignedId  };
                    this.PossiblePreviousStates = new List<GudelIdState> { GudelIdStates.Created, GudelIdStates.Reserved, GudelIdStates.Produced, GudelIdStates.Assigned  };
                    break;
            }

        }
    }


    public class GudelIdStates
    {
        public const int CreatedId = 0;
        public const int ReservedId = 10;
        public const int ProducedId = 20;
        public const int AssignedId = 30;
        public const int VoidedId = 99;


        public static GudelIdState Created = new GudelIdState(CreatedId,
            new Dictionary<string, string>
            {
                { "en-US", "Created" },
                { "de-DE", "Erstellt" },
            },
            new Dictionary<string, string>
            {
                { "en-US", "Initial Status after Güdel ID is created in the global ID pool." },
                { "de-DE", "Innitialer Status nachdem eine Güdel ID global erstellt wurde." },
            }
            );

        public static GudelIdState Reserved = new GudelIdState(ReservedId,
            new Dictionary<string, string>
            {
                { "en-US", "Reserved" },
                { "de-DE", "Reserviert" },
            },
            new Dictionary<string, string>
            {
                { "en-US", "Güdel ID is reserved and transferred to a local ID pool to make sure it is available for the user even if offline." },
                { "de-DE", "Die Güdel ID ist reserviert und einem lokalen ID Pool zugeordnet. Es wird sichergestellt dass die ID auch offline verfügbar ist." },
            }
            );
        public static GudelIdState Produced = new GudelIdState(ProducedId,
            new Dictionary<string, string>
            {
                        { "en-US", "Produced" },
                        { "de-DE", "Produziert" },
            },
            new Dictionary<string, string>
            {
                        { "en-US", "Güdel ID is produced (e.g. printed on a label) and checked" },
                        { "de-DE", "Güdel ID ist produziert (z.B. auf ein Label gedruckt) und überprüft" },
            }
            );
        public static GudelIdState Assigned = new GudelIdState(AssignedId,
            new Dictionary<string, string>
            {
                        { "en-US", "Assigned" },
                        { "de-DE", "Zugewiesen" },
            },
            new Dictionary<string, string>
            {
                        { "en-US", "Güdel ID is assigned to a product and checked.\nGüdel Smart Products can be linked with Güdel ID" },
                        { "de-DE", "Güdel ID ist einem Produkt zugeordnet und geprüft. \nGüdel Smart Products sind mit einer Güdel ID verknüpft." },
            }
            );
        public static GudelIdState Voided = new GudelIdState(VoidedId,
            new Dictionary<string, string>
            {
                        { "en-US", "Void" },
                        { "de-DE", "Ungültig" },
            },
            new Dictionary<string, string>
            {
                        { "en-US", "Güdel ID is faulty and must not be used" },
                        { "de-DE", "Güdel ID ist Fehlerhaft und darf nicht genutzt werden" },
            }
            );
    }
}
