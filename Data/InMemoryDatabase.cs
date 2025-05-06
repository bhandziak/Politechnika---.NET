using EventRegisterProject.Models;

namespace EventRegisterProject.Data
{
    public static class InMemoryDatabase
    {
        public static List<Event> Events { get; } = new();
        public static List<Participant> Participants { get; } = new();
    }
}
