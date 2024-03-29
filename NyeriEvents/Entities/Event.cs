﻿namespace NyeriEvents.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int TicketAmount { get; set; }
        public DateTime Date { get; set; }
        public List<User> Users { get; set; }=new List<User>();


    }
}
