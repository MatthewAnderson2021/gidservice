namespace GudelIdService.Domain.Dto
{
    public class GudelIdTypeDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public GudelIdTypeDto() { }

        public GudelIdTypeDto(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }


    }
}
