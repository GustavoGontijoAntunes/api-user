namespace app.Domain.Models.Excel
{
    public class Excel
    {
        public string MymeType { get => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }

        public Excel(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }

        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}