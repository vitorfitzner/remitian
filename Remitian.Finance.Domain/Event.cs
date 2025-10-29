namespace Remitian.Finance.Domain
{
    public class Event
    {
        public int Id { get; private set; }
        public string Name { get; internal set; } = "";
        public DateTime Date { get; internal set; }
    }
}
